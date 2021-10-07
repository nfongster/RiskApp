using System;
using System.Collections.Generic;
using RiskEngine;
using System.Linq;

namespace RiskApp
{
    class ConsoleApp
    {
        public static string[] Faction_Array = new string[] { "RED", "BLUE", "GREEN", "YELLOW", "CYAN", "MAGENTA" };
        public static string filePath = @"C:\Users\Nick\Desktop\Risk\";
        public static int TurnCount = 0;

        public static void Main(string[] args)
        {
            Console.ResetColor();
            Console.Write("Welcome to Risk.\nPlease specify the number of factions (2-6): ");
            int num_factions = Int32.Parse(Console.ReadLine());
            Console.WriteLine("\nIn this game there will be {0} players on the board.", num_factions);

            // Populate string array of factions
            string[] factions = new string[num_factions];
            for (int i = 0; i < num_factions; i++)
            {
                factions[i] = Faction_Array[i];
            }

            // Parse continent file to get continent territories and bonuses
            Dictionary<string, string[]> continentToTerritories = new Dictionary<string, string[]>();
            Dictionary<string, int> continentToScore = new Dictionary<string, int>();
            CommonFunctions.ParseContinentFile(filePath, ref continentToTerritories, ref continentToScore);

            // Create board, assign territories, and place armies (randomly or otherwise)
            RiskBoard board = new RiskBoard(continentToTerritories, CommonFunctions.ParseMapFile(filePath), factions);
            Console.Write("Do you wish to randomly place armies? ");
            string random = Console.ReadLine();
            if (random.ToLower() == "y")
            {
                Console.WriteLine("This game will randomly assign territories and randomly place armies.\n");
                board.RandomAssignment();
                board.RandomArmyPlacement();
            }
            else
            {
                Console.WriteLine("Players will alternate claiming territory and placing armies.\n");
                while (board.GetTotalReserves() > 0)
                {
                    foreach (var player in board.GetFactions())
                    {
                        PrintMap(board);
                        Console.Write("Player {0}, choose territory: ", player.FactionName);
                        string territory = Console.ReadLine();
                        board.PlayerArmyPlacement(player, board.GetTerritoryNodeByName(territory));
                    }
                }
            }
            
            Console.WriteLine();
            PrintMap(board);
            bool gameActive = true;

            while (gameActive)  // TODO: better loop condition
            {
                TerritoryNode offensiveNode = null;
                TerritoryNode defensiveNode = null;

                foreach (var player in board.GetFactions())
                {
                    bool continueTurn = true;
                    if (TurnCount > 0)
                    {
                        ArmyPlacement(board, player, continentToScore);
                    }

                    while (continueTurn)
                    {
                        CommonFunctions.DeclareAttack(board, player, ref offensiveNode, ref defensiveNode);
                        while (!offensiveNode.IsAdjacentTo(defensiveNode) || offensiveNode.OccupyingFaction.FactionName == defensiveNode.OccupyingFaction.FactionName)
                        {
                            Console.WriteLine("Either these nodes are not adjacent, or they belong to the same faction.  Please choose another set of nodes.");
                            CommonFunctions.DeclareAttack(board, player, ref offensiveNode, ref defensiveNode);
                        }

                        // Attacker chooses number of die (num armies must be > num die chosen)
                        Console.Write("Player {0}, choose number of die to roll (up to {1}): ", 
                            player.FactionName, 
                            CommonFunctions.GetMaxDiceToRoll(offensiveNode.GetArmyCount(), CommonFunctions.FactionState.Attacker));
                        int numRedDice = Int32.Parse(Console.ReadLine());

                        // Defender chooses number of die (either 1 or 2)
                        Console.Write("Player {0}, choose number of die to roll (up to {1}): ", 
                            defensiveNode.OccupyingFaction.FactionName, 
                            CommonFunctions.GetMaxDiceToRoll(defensiveNode.GetArmyCount(), CommonFunctions.FactionState.Defender));
                        int numWhiteDice = Int32.Parse(Console.ReadLine());

                        List<Dice> redDice = new List<Dice>();
                        List<Dice> whiteDice = new List<Dice>();
                        CommonFunctions.RollAllDie(ref redDice, ref whiteDice, numRedDice, numWhiteDice);

                        // Die are rolled and results are shown
                        Console.WriteLine("Player {0} rolls {1} red dice and player {2} rolls {3} white dice.", 
                            player.FactionName, numRedDice, defensiveNode.OccupyingFaction.FactionName, numWhiteDice);
                        Console.Write("Results for Player {0}: ", player.FactionName);
                        PrintDice(redDice);
                        Console.Write("Results for Player {0}: ", defensiveNode.OccupyingFaction.FactionName);
                        PrintDice(whiteDice);
                        CommonFunctions.CompareDiceRoll(redDice, whiteDice, offensiveNode, defensiveNode);
                        PrintMap(board);

                        Console.Write("Roll again (Y/N)? ");
                        string response = Console.ReadLine();
                        if (response.ToLower() == "n")
                        {
                            continueTurn = false;
                            // Receive a card under appropriate conditions
                            if (player.NumConqueredTerritoriesCurrentTurn > 0)
                            {
                                if (board.CardStack.Count == 0)
                                {
                                    Console.WriteLine("Replenishing and reshuffling card stack...");
                                    board.CardStack = new Stack<Card>(CommonFunctions.Shuffle(board.DiscardStack.ToList()));
                                    board.DiscardStack.Clear();
                                }
                                player.DrawCard(board);
                                Console.WriteLine("Player draws the following card for conquering new territory: {0}", player.GetCardList()[player.GetCardList().Count - 1].type.ToString());
                            }
                            player.NumConqueredTerritoriesCurrentTurn = 0;

                            // Fortify position - you only get to do this once
                            Console.Write("Fortify position (move armies)? ");
                            string fortify = Console.ReadLine();
                            if (fortify.ToLower() == "y")
                            {
                                Console.Write("Territory from which to remove armies: ");
                                string territorySource = Console.ReadLine();
                                TerritoryNode nodeSource = board.GetTerritoryNodeByName(territorySource);
                                Console.Write("Territory to which armies will move: ");
                                string territoryDestination = Console.ReadLine();
                                TerritoryNode nodeDestination = board.GetTerritoryNodeByName(territoryDestination);
                                Console.Write("Number of armies to move: ");
                                int migratingArmies = Int32.Parse(Console.ReadLine());
                                while (migratingArmies >= nodeSource.GetArmyCount())
                                {
                                    Console.WriteLine("Number of armies to move must be < total armies currently occupying it.");
                                    Console.Write("Number of armies to move: ");
                                    migratingArmies = Int32.Parse(Console.ReadLine());
                                }
                                nodeSource.RemoveArmies(migratingArmies);
                                nodeDestination.AddArmies(migratingArmies);
                                PrintMap(board);
                            }
                        }
                    }
                }
                TurnCount++;
                if (board.GetActiveFactionCount() == 1)
                {
                    Console.WriteLine("Game over - player x is the winner.");
                }
            }
        }

        /// <summary>
        /// Prints the face values of all die in the list of dice
        /// </summary>
        /// <param name="dice"></param>
        private static void PrintDice(List<Dice> dice)
        {
            Console.Write("[");
            foreach(var die in dice)
            {
                Console.Write(die.PeekDice().ToString() + " ");
            }
            Console.WriteLine("]");
        }

        /// <summary>
        /// Game logic for army acquisition and placement at the beginning of turn
        /// </summary>
        /// <param name="board"></param>
        /// <param name="player"></param>
        private static void ArmyPlacement(RiskBoard board, Faction player, Dictionary<string, int> continentScores)
        {
            // 1. Total territory control bonus
            int receivedArmies = player.FactionTerritories.Count / 3;
            receivedArmies = (receivedArmies >= 3 ? receivedArmies : 3);
            
            // 2. Continent bonus
            foreach (var continent in continentScores.Keys)
            {
                // if player controls all nodes in continent, add appropriate score to total
                bool continentControl = true;
                foreach (var node in board.ContinentList[continent])
                {                    
                    if (!player.FactionTerritories.Contains(node))
                    {
                        continentControl = false;
                        break;
                    }
                }

                if (continentControl)
                {
                    receivedArmies += continentScores[continent];
                }
            }

            // 3. Card bonus
            if (player.GetCardList().Count >= 5)
            {
                Console.WriteLine("Player has 5+ cards, please select 3 cards to turn in for a bonus.");
                Console.Write("Available cards: ");
                foreach (var card in player.GetCardList())
                {
                    Console.Write(card.type.ToString() + " ");
                }
                Console.WriteLine();
                bool threeSelected = false;
                List<Card> threeCards = new List<Card>();
                while (!threeSelected)
                {
                    for (int i = 1; i <= 3; i++)
                    {
                        Console.Write("Choose card #{0}: ", i);
                        string cardType = Console.ReadLine();
                        threeCards.Add(new Card((CommonFunctions.CardType)Enum.Parse(typeof(CommonFunctions.CardType), cardType)));
                    }
                    // Check if all 3 cards are the same, or if at least 1 is wild, or if they're all different
                    if (!threeCards.Any(x => x != threeCards[0]) || threeCards.Any(x => x.type == CommonFunctions.CardType.Wild) || threeCards.Distinct().Count() == threeCards.Count)
                    {
                        int currentBonus = board.GetCurrentCardBonus();
                        Console.WriteLine("Player receives {0} additional armies.", currentBonus);
                        receivedArmies += currentBonus;
                        // update card bonus
                        if (currentBonus >= CommonFunctions.CARD_BONUSES[CommonFunctions.CARD_BONUSES.Count() - 1])
                        {
                            board.SetCurrentCardBonus(currentBonus + 5);
                        }
                        else
                        {
                            board.SetCurrentCardBonus(CommonFunctions.CARD_BONUSES[Array.IndexOf(CommonFunctions.CARD_BONUSES, currentBonus) + 1]);
                        }
                        foreach (var card in threeCards)
                        {
                            player.GetCardList().Remove(card);
                            board.DiscardStack.Push(card);
                        }
                        threeSelected = true;
                    }
                    else
                    {
                        Console.WriteLine("Inappropriate card combination, please try again.");
                        threeCards.Clear();
                    }
                }
            }
            else
            {
                // Optional - keep looping and asking for 3 cards, otherwise exit
            }

            player.SetReservesCount(receivedArmies);
            Console.WriteLine(String.Format("Player {0} receives {1} armies.", player.FactionName, receivedArmies));
            Console.WriteLine("Please place armies in desired locations.");
            while (player.GetReservesCount() > 0)
            {
                Console.Write(String.Format("Choose territory ({0}/{1} reserves remaining): ", player.GetReservesCount(), receivedArmies));
                string territory = Console.ReadLine();
                Console.Write(String.Format("Choose number of armies to place ({0}/{1} reserves remaining): ", player.GetReservesCount(), receivedArmies));
                int reinforcements = Int32.Parse(Console.ReadLine());
                Console.WriteLine(String.Format("Placing {0} army(ies) on {1}", reinforcements, territory));
                TerritoryNode node = board.GetTerritoryNodeByName(territory);
                player.SetReservesCount(player.GetReservesCount() - reinforcements);
                node.AddArmies(reinforcements);
                PrintMap(board);
            }
        }

        /// <summary>
        /// Prints the total number of armies for each faction
        /// </summary>
        /// <param name="board"></param>
        private static void PrintFactionStrength(RiskBoard board)
        {
            foreach (var faction in board.GetFactions())
            {
                Console.WriteLine(String.Format("Faction {0} has {1} armies.", faction.FactionName, faction.GetNumberActiveArmies()));
            }
        }

        /// <summary>
        /// Writes the number of armies per territory, and occupying faction of each
        /// territory, to the console (old)
        /// </summary>
        public static void PrintArmyDistribution(RiskBoard board)
        {
            foreach (var node in board.GetTerritoryNodes())
            {
                Console.WriteLine(String.Format(@"{0}: Faction={1}, Army Count={2}", 
                    node.TerritoryName, node.OccupyingFaction.FactionName, node.GetArmyCount()));
            }
            Console.WriteLine();
        }

        /// <summary>
        /// Prints world map with current faction territory and army counts
        /// </summary>
        /// <param name="board"></param>
        private static void PrintMap(RiskBoard board)
        {
            Console.WriteLine(@"///////////////////////////////////////////////////////////////////////////////////////////////");
            Console.Write(@"                     --");
            PrintColoredText(board.GetTerritoryNodeByName("Greenland"), "GL");
            Console.WriteLine("-------");

            Console.WriteLine(@"                    / / |          \");

            Console.Write("[KM]-");
            PrintColoredText(board.GetTerritoryNodeByName("Alaska"), "AK");
            Console.Write("----");
            PrintColoredText(board.GetTerritoryNodeByName("Northwest Territory"), "NT");
            Console.Write("- /  |          ");
            PrintColoredText(board.GetTerritoryNodeByName("Iceland"), "IC");
            Console.Write("----");
            PrintColoredText(board.GetTerritoryNodeByName("Scandinavia"), "SC");
            Console.Write("--       ");
            PrintColoredText(board.GetTerritoryNodeByName("Ural"), "UR");
            Console.Write("---");
            PrintColoredText(board.GetTerritoryNodeByName("Siberia"), "SB");
            Console.Write("---");
            PrintColoredText(board.GetTerritoryNodeByName("Yakutsk"), "YA");
            Console.Write("---");
            PrintColoredText(board.GetTerritoryNodeByName("Kamchatka"), "KM");
            Console.WriteLine("-[AK]");

            Console.Write(@"     \        \     /   |            \     /  |    \     /  | \     |\^-----^");
            PrintColoredText(board.GetTerritoryNodeByName("Irkutsk"), "IR");
            Console.WriteLine("^  |");

            Console.Write("      ");
            PrintColoredText(board.GetTerritoryNodeByName("Alberta"), "AL");
            Console.Write("----");
            PrintColoredText(board.GetTerritoryNodeByName("Ontario"), "ON");
            Console.Write("--");
            PrintColoredText(board.GetTerritoryNodeByName("Quebec"), "QU");
            Console.Write("           ");
            PrintColoredText(board.GetTerritoryNodeByName("Great Britain"), "GB");
            Console.Write("-");
            PrintColoredText(board.GetTerritoryNodeByName("Northern Europe"), "NE");
            Console.Write("---");
            PrintColoredText(board.GetTerritoryNodeByName("Russia"), "RS");
            Console.Write("   |  ---  |  ");
            PrintColoredText(board.GetTerritoryNodeByName("Mongolia"), "MO");
            Console.WriteLine("---^     |");

            Console.WriteLine(@"      \    /   \     /                \    /  |     /|   \  |     \ |  /   \         |");

            Console.Write("      ");
            PrintColoredText(board.GetTerritoryNodeByName("Western US"), "WU");
            Console.Write("----");
            PrintColoredText(board.GetTerritoryNodeByName("Eastern US"), "EU");
            Console.Write("-                 ");
            PrintColoredText(board.GetTerritoryNodeByName("Western Europe"), "WE");
            Console.Write("--");
            PrintColoredText(board.GetTerritoryNodeByName("Southern Europe"), "SE");
            Console.Write("-- |    ");
            PrintColoredText(board.GetTerritoryNodeByName("Afghanistan"), "AF");
            Console.Write("----");
            PrintColoredText(board.GetTerritoryNodeByName("China"), "CN");
            Console.Write("   ");
            PrintColoredText(board.GetTerritoryNodeByName("Japan"), "JA");
            Console.WriteLine("------");

            Console.WriteLine(@"         \     /                        |     |   \  |    /  \    /  \");

            Console.Write("          ");
            PrintColoredText(board.GetTerritoryNodeByName("Central America"), "CA");
            Console.Write("                         |  ");
            PrintColoredText(board.GetTerritoryNodeByName("Egypt"), "EG");
            Console.Write("--");
            PrintColoredText(board.GetTerritoryNodeByName("Middle East"), "ME");
            Console.Write("------");
            PrintColoredText(board.GetTerritoryNodeByName("India"), "IN");
            Console.Write("---");
            PrintColoredText(board.GetTerritoryNodeByName("Siam"), "SI");
            Console.WriteLine();

            Console.WriteLine(@"              \                         | /   \  /                    \");

            Console.Write("               ");
            PrintColoredText(board.GetTerritoryNodeByName("Venezuela"), "VZ");
            Console.Write("                    ");
            PrintColoredText(board.GetTerritoryNodeByName("North Africa"), "NA");
            Console.Write("--");
            PrintColoredText(board.GetTerritoryNodeByName("East Africa"), "EA");
            Console.Write("                 ");
            PrintColoredText(board.GetTerritoryNodeByName("Indonesia"), "IS");
            Console.Write("---");
            PrintColoredText(board.GetTerritoryNodeByName("New Guinea"), "NG");
            Console.WriteLine();

            Console.WriteLine(@"              /    \                  /   \   /    \                   \   /  \");

            Console.Write("           ");
            PrintColoredText(board.GetTerritoryNodeByName("Peru"), "PU");
            Console.Write("---");
            PrintColoredText(board.GetTerritoryNodeByName("Brazil"), "BR");
            Console.Write("--------------     ");
            PrintColoredText(board.GetTerritoryNodeByName("Congo"), "CO");
            Console.Write("    ");
            PrintColoredText(board.GetTerritoryNodeByName("Madagascar"), "MD");
            Console.Write("             ");
            PrintColoredText(board.GetTerritoryNodeByName("Western Australia"), "WA");
            Console.Write("---");
            PrintColoredText(board.GetTerritoryNodeByName("Eastern Australia"), "EA");
            Console.WriteLine();

            Console.WriteLine(@"               \   /                         |     /");

            Console.Write("               ");
            PrintColoredText(board.GetTerritoryNodeByName("Argentina"), "AR");
            Console.Write("                       ");
            PrintColoredText(board.GetTerritoryNodeByName("South Africa"), "SA");
            Console.WriteLine("---");

            Console.WriteLine(@"///////////////////////////////////////////////////////////////////////////////////////////////");
        }

        /// <summary>
        /// Prints a territory node along with its army count, colorized according to occupying faction
        /// </summary>
        /// <param name="node"></param>
        /// <param name="abbreviation"></param>
        private static void PrintColoredText(TerritoryNode node, string abbreviation)
        {
            if (node.OccupyingFaction == null)
            {
                Console.Write(abbreviation + "=00");
            }
            else
            {
                switch (node.OccupyingFaction.FactionName)
                {
                    case "RED":
                        Console.ForegroundColor = ConsoleColor.Red;
                        break;
                    case "BLUE":
                        Console.ForegroundColor = ConsoleColor.Blue;
                        break;
                    case "GREEN":
                        Console.ForegroundColor = ConsoleColor.Green;
                        break;
                    case "CYAN":
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        break;
                    case "YELLOW":
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        break;
                    default:  // Magenta
                        Console.ForegroundColor = ConsoleColor.Magenta;
                        break;
                }
                if (10F / (float)node.GetArmyCount() <= 1.0)
                {
                    Console.Write(abbreviation + "=" + node.GetArmyCount().ToString());
                }
                else
                {
                    Console.Write(abbreviation + "=0" + node.GetArmyCount().ToString());
                }
                Console.ResetColor();
            }            
        }   
    }
}
