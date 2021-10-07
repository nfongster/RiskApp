using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace RiskEngine
{
    public class CommonFunctions
    {
        /// This is a library of common functions not necessarily tied to any individual object,
        /// such as the board, a player, etc.
        public static int RESERVE_COUNT = 35;  // Number of reserves.  TODO: change based on num. players
        public static int MIN_ARMIES_TO_ATTACK = 2;  // Min number of armies needed on a given territory in order to launch attack
        public static int[] CARD_BONUSES = { 4, 6, 8, 10, 12, 15 };  // Card bonuses for each set traded in.  After 15, increases by 5 each time

        public enum FactionState
        {
            Attacker,
            Defender
        }

        public enum Continent
        {
            NorthAmerica,
            SouthAmerica,
            Europe,
            Africa,
            Asia,
            Australia
        }

        public enum CardType
        {
            Infantry,
            Cavalry,
            Artillery,
            Wild
        }

        /// <summary>
        /// Returns a mapping of territory names to their adjacent territories based on a text file.  The text
        /// file should adhere to the following format:
        /// 
        /// TerritoryA:AdjacentTerritoryA1,AdjacentTerritoryA2,...,AdjacentTerritoryAK\n
        /// TerritoryB:AdjacentTerritoryB1,AdjacentTerritoryB2,...,AdjacentTerritoryBL\n
        /// ...
        /// TerritoryN:AdjacentTerritoryN1,AdjacentTerritoryN2,...,AdjacentTerritoryNM
        /// 
        /// This also assumes there is a file that maps territories to continents.
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static Dictionary<string, string[]> ParseMapFile(string filePath)
        {
            Dictionary<string, string[]> territoryMap = new Dictionary<string, string[]>();
            using (var sr = new StreamReader(filePath + "Risk_Map.txt"))
            {
                string node;
                while ((node = sr.ReadLine()) != null)
                {
                    var nodeInfo = node.Split(':');
                    var nodeName = nodeInfo[0];
                    var adjacentNodes = nodeInfo[1].Split(',');
                    territoryMap.Add(nodeName, adjacentNodes);
                }
            }
            return territoryMap;
        }

        public static void ParseContinentFile(string filePath, ref Dictionary<string, string[]> continentToTerritories, ref Dictionary<string, int> continentToScore)
        {
            using (var sr = new StreamReader(filePath + "Risk_Continents.txt"))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    var lineSplit = line.Split(':');
                    var continentAndScore = lineSplit[0].Split('_');
                    var continentTerritories = lineSplit[1].Split(',');
                    string continent = continentAndScore[0];
                    int score = Int32.Parse(continentAndScore[1]);
                    continentToTerritories.Add(continent, continentTerritories);
                    continentToScore.Add(continent, score);
                }
            }
        }

        /// <summary>
        /// Compares roll of the dice, depleting armies from attacker and defender nodes accordingly (Command Line Only)
        /// </summary>
        /// <param name="attackerDice"></param>
        /// <param name="defenderDice"></param>
        /// <returns></returns>
        public static void CompareDiceRoll(List<Dice> attackerDice, List<Dice> defenderDice,
                                    TerritoryNode attackerNode, TerritoryNode defenderNode)
        {
            if (attackerNode.GetArmyCount() < MIN_ARMIES_TO_ATTACK)
            {
                Console.WriteLine(String.Format("You need at least {0} armies on this territory to launch an attack.", MIN_ARMIES_TO_ATTACK));
            }
            if (attackerNode.GetArmyCount() >= MIN_ARMIES_TO_ATTACK && defenderNode.GetArmyCount() > 0)
            {
                // Compare first pair of die
                int[] attackerVals = GetMaxDiceValues(attackerDice);
                int[] defenderVals = GetMaxDiceValues(defenderDice);
                if (attackerVals[0] > defenderVals[0])
                {
                    defenderNode.RemoveArmies(1);
                }
                else
                {
                    attackerNode.RemoveArmies(1);
                }
                // Compare second pair of die, if both players had rolled at least 2 dice each
                if (attackerNode.GetArmyCount() >= MIN_ARMIES_TO_ATTACK && defenderNode.GetArmyCount() > 0 && attackerVals[1] > 0 && defenderVals[1] > 0)
                {
                    if (attackerVals[1] > defenderVals[1])
                    {
                        defenderNode.RemoveArmies(1);
                    }
                    else
                    {
                        attackerNode.RemoveArmies(1);
                    }
                    if (defenderNode.GetArmyCount() == 0)
                    {
                        TransferArmies(attackerNode, defenderNode);
                    }
                }
                else if (defenderNode.GetArmyCount() == 0)
                {
                    TransferArmies(attackerNode, defenderNode);
                }
            }
        }

        /// <summary>
        /// Gets the maximum dice values in the list.  Second element is -1 if only 1 dice is rolled.
        /// </summary>
        /// <param name="dice"></param>
        /// <returns></returns>
        public static int[] GetMaxDiceValues(List<Dice> dice)
        {
            int[] max2 = new int[] { -1, -1 };
            foreach (var die in dice)
            {
                if (die.PeekDice() > max2[0])
                {
                    max2[0] = die.PeekDice();
                }
            }

            // Get second-highest dice if applicable
            if (dice.Count > 1)
            {
                foreach (var die in dice)
                {
                    if (die.PeekDice() > max2[1] && die.PeekDice() <= max2[0])
                    {
                        max2[1] = die.PeekDice();
                    }
                }
            }

            return max2;
        }

        /// <summary>
        /// Rolls all dice for both players.
        /// </summary>
        /// <param name="redDice"></param>
        /// <param name="whiteDice"></param>
        /// <param name="numRed"></param>
        /// <param name="numWhite"></param>
        public static void RollAllDie(ref List<Dice> redDice, ref List<Dice> whiteDice, int numRed, int numWhite)
        {
            for (int i = 0; i < numRed; i++) 
            { 
                redDice.Add(new Dice()); 
            }
            for (int i = 0; i < numWhite; i++) 
            { 
                whiteDice.Add(new Dice());
            }
            foreach (var d in redDice)
            {
                d.Roll();
            }
            foreach (var d in whiteDice) 
            {
                d.Roll();
            }
        }

        /// <summary>
        /// Returns the maximum number of dice that can be rolled
        /// </summary>
        /// <param name="armies"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public static int GetMaxDiceToRoll(int armies, FactionState state)
        {
            if (state == FactionState.Attacker)
            {
                if (armies > 3) return 3;
                else if (armies == 3) return 2;
                else return 1;
            }
            else
            {
                if (armies >= 2) return 2;
                else return 1;
            }
        }

        /// <summary>
        /// Transfers a user-input number of armies from the attacker node to the defender node (Console Only)
        /// </summary>
        /// <param name="attackerNode"></param>
        /// <param name="defenderNode"></param>
        private static void TransferArmies(TerritoryNode attackerNode, TerritoryNode defenderNode)
        {
            //Console.Write("Attacker won new territory! Choose number of armies to move: ");
            int movingArmies = Int32.Parse(Console.ReadLine());
            while (movingArmies < 1 || movingArmies >= attackerNode.GetArmyCount())
            {
                Console.WriteLine(String.Format("Number of armies to move must be at least 1 and no more than {0}.", attackerNode.GetArmyCount() - 1));
                Console.Write("Please choose a different value: ");
                movingArmies = Int32.Parse(Console.ReadLine());
            }
            attackerNode.RemoveArmies(movingArmies);
            defenderNode.AddArmies(movingArmies);
            attackerNode.OccupyingFaction.OccupyTerritory(defenderNode);
            attackerNode.OccupyingFaction.NumConqueredTerritoriesCurrentTurn += 1;
        }

        /// <summary>
        /// Assign offensive and defensive nodes
        /// </summary>
        /// <param name="board"></param>
        /// <param name="player"></param>
        /// <param name="offensiveNode"></param>
        /// <param name="defensiveNode"></param>
        public static void DeclareAttack(RiskBoard board, Faction player, ref TerritoryNode offensiveNode, ref TerritoryNode defensiveNode)
        {
            Console.Write(String.Format("Player {0}, choose attacking territory: ", player.FactionName));
            offensiveNode = board.GetTerritoryNodeByName(Console.ReadLine());
            Console.Write(String.Format("Player {0}, choose territory to attack: ", player.FactionName));
            defensiveNode = board.GetTerritoryNodeByName(Console.ReadLine());
        }

        /// <summary>
        /// Shuffles a list using the Fisher-Yates algorithm (https://en.wikipedia.org/wiki/Fisher%E2%80%93Yates_shuffle)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static List<T> Shuffle<T>(List<T> list)
        {
            Random r = new Random();
            for (int i = list.Count - 1; i > 0; i--)
            {
                int j = r.Next(i + 1);
                var temp = list[i];
                list[i] = list[j];
                list[j] = temp;
            }
            return list;
        }
    }
}