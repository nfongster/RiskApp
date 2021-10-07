using System;
using System.Collections.Generic;
using System.Linq;

namespace RiskEngine
{
    public class RiskBoard
    {
        private List<TerritoryNode> TerritoryNodeList;
        private List<Faction> FactionList;
        public Dictionary<string, List<TerritoryNode>> ContinentList;
        public Stack<Card> CardStack;
        public Stack<Card> DiscardStack;
        private int CardBonus { get; set; }
        
        /// <summary>
        /// Initializes an empty RiskBoard object with 0 armies and no faction assignments
        /// </summary>
        /// <param name="territoryMap">List of territory nodes on the board</param>
        public RiskBoard(Dictionary<string, string[]> territoryMap)
        {
            // Build list of nodes
            this.TerritoryNodeList = new List<TerritoryNode>();
            foreach (string territory in territoryMap.Keys)
            {
                this.TerritoryNodeList.Add(new TerritoryNode(territory));
            }

            // For each node, add list of pointers to adjacent nodes
            foreach (TerritoryNode node in TerritoryNodeList)
            {
                string[] adjacentTerritories = territoryMap[node.TerritoryName];
                foreach (TerritoryNode testNode in TerritoryNodeList)  // TODO: reduce search redundancy
                {
                    if (adjacentTerritories.Contains(testNode.TerritoryName))
                    {
                        node.AddAdjacentNode(testNode);
                    }
                }
            }

            // Initialize an empty FactionList
            this.FactionList = new List<Faction>();

            // Initialize a full card stack - place appropriate number of each type, then shuffle
            this.CardStack = new Stack<Card>();
            int numWild = 2;
            int numOther = 14;
            for (int i = 0; i < numWild; i++)
            {
                this.CardStack.Push(new Card(CommonFunctions.CardType.Wild));
            }
            for (int i = 0; i < numOther; i++)
            {
                this.CardStack.Push(new Card(CommonFunctions.CardType.Artillery));
                this.CardStack.Push(new Card(CommonFunctions.CardType.Cavalry));
                this.CardStack.Push(new Card(CommonFunctions.CardType.Infantry));
            }
            List<Card> tempList = this.CardStack.ToList();
            tempList = CommonFunctions.Shuffle(tempList);
            this.CardStack = new Stack<Card>(tempList);

            // Initialize an empty discard pile
            this.DiscardStack = new Stack<Card>();

            // Initial card bonus is set
            this.CardBonus = CommonFunctions.CARD_BONUSES[0];
        }

        /// <summary>
        /// Initializes an empty RiskBoard object with 0 armies as well as with faction assignments
        /// </summary>
        /// <param name="territoryMap"></param>
        /// <param name="factions"></param>
        public RiskBoard(Dictionary<string, string[]> territoryMap, string[] factions) : this(territoryMap)
        {
            foreach (string name in factions)
            {
                this.FactionList.Add(new Faction(name));
            }
        }

        /// <summary>
        /// Initializes an empty RiskBoard object with 0 armies, faction assignments, and continent labels to territories
        /// </summary>
        /// <param name="continentMap"></param>
        /// <param name="territoryMap"></param>
        /// <param name="factions"></param>
        public RiskBoard(Dictionary<string, string[]> continentMap, Dictionary<string, string[]> territoryMap, string[] factions) : this(territoryMap, factions)
        {
            this.ContinentList = new Dictionary<string, List<TerritoryNode>>();
            foreach (string continent in continentMap.Keys)
            {
                var territories = continentMap[continent];
                List<TerritoryNode> nodeList = new List<TerritoryNode>();
                foreach (string territory in territories)
                {
                    nodeList.Add(this.GetTerritoryNodeByName(territory));
                    // Assign each node to its respective continent
                    foreach (var node in this.TerritoryNodeList)
                    {
                        if (node.TerritoryName == territory)
                        {
                            node.continent = (CommonFunctions.Continent)Enum.Parse(typeof(CommonFunctions.Continent), continent);
                        }
                    }
                }
                // Populate ContinentList field
                this.ContinentList.Add(continent, nodeList);
            }
        }

        /// <summary>
        /// Get total number of reserves across all factions
        /// </summary>
        /// <returns></returns>
        public int GetTotalReserves()
        {
            int total = 0;
            foreach (var faction in this.FactionList)
            {
                total += faction.GetReservesCount();
            }
            return total;
        }       

        /// <summary>
        /// Adds a new player (faction) to the board
        /// </summary>
        /// <param name="name"></param>
        public void AddFaction(string name)
        {
            this.FactionList.Add(new Faction(name));
        }

        /// <summary>
        /// Returns a list of all players (factions) currently on the board
        /// </summary>
        /// <returns></returns>
        public List<Faction> GetFactions()
        {
            return this.FactionList;
        }

        /// <summary>
        /// Gets a string array of all territory names on the board
        /// </summary>
        /// <returns></returns>
        public string[] GetTerritoryNames()
        {
            string[] TerritoryNames = new string[TerritoryNodeList.Count];
            for (int i = 0; i < TerritoryNodeList.Count; i++)
            {
                TerritoryNames[i] = TerritoryNodeList[i].TerritoryName;
            }
            return TerritoryNames;
        }

        /// <summary>
        /// Gets the list of all territory nodes on the board
        /// </summary>
        /// <returns></returns>
        public List<TerritoryNode> GetTerritoryNodes()
        {
            return this.TerritoryNodeList;
        }

        /// <summary>
        /// Clears the board of all factions and armies
        /// </summary>
        public void ClearBoard()
        {
            foreach (TerritoryNode node in this.TerritoryNodeList)
            {
                node.OccupyingFaction = null;
                node.ClearArmies();
            }
        }

        /// <summary>
        /// Players alternate distributing armies at beginning of game
        /// </summary>
        public void PlayerArmyPlacement(Faction player, TerritoryNode node)
        {
            player.PlaceArmies(node, 1);
            if (node.OccupyingFaction == null)
            {
                node.OccupyingFaction = player;
            }
        }

        /// <summary>
        /// Randomly assigns nodes to existing factions
        /// </summary>
        /// <param name="FactionList"></param>
        public void RandomAssignment()
        {
            // Create list of available territories from which factions alternate sampling
            Random r = new Random();
            if (this.FactionList.Count > 0)
            {
                List<string> names = GetTerritoryNames().ToList();
                while (names.Count > 0)
                {
                    foreach (Faction player in this.FactionList)
                    {
                        if (names.Count == 0) break;
                        int i_rand = r.Next(names.Count);
                        var node = this.TerritoryNodeList.Find(x => x.TerritoryName == names[i_rand]);
                        player.FactionTerritories.Add(node);
                        node.OccupyingFaction = player;  // Each node also points to faction
                        player.PlaceArmies(node, 1);
                        names.RemoveAt(i_rand);
                    }
                }  
            }
            else
            {
                // Log this
                Console.WriteLine("Factions have not yet been initialized.  Please initialize some factions.");
            }
        }

        /// <summary>
        /// Randomly distribute armies at the beginning of the game
        /// </summary>
        public void RandomArmyPlacement()
        {
            // TODO: exception handling if there are no factions
            // TODO: can't do this in the middle of the game?
            int totalReserves = this.GetTotalReserves();
            Random r = new Random();
            while (totalReserves > 0)
            {
                foreach (var player in this.FactionList)
                {
                    player.PlaceArmies(player.FactionTerritories[r.Next(player.FactionTerritories.Count)], 1);
                    totalReserves = this.GetTotalReserves();
                }
            }
        }

        /// <summary>
        /// Returns the TerritoryNode corresponding to the input name.
        /// </summary>
        /// <param name="board"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public TerritoryNode GetTerritoryNodeByName(string name)
        {
            foreach (TerritoryNode node in this.TerritoryNodeList)
            {
                if (node.TerritoryName == name)
                {
                    return node;
                }
            }
            return null;  // TODO: handle this better
        }

        public int GetActiveFactionCount()
        {
            int count = 0;
            foreach (var player in this.FactionList)
            {
                if (player.IsActive())
                {
                    count++;
                }
            }
            return count;
        }

        /// <summary>
        /// Gets current bonus army count for turning in appropriate set of 3 cards
        /// </summary>
        /// <returns></returns>
        public int GetCurrentCardBonus()
        {
            return this.CardBonus;
        }

        /// <summary>
        /// Sets current bonus army count for turning in appropriate set of 3 cards
        /// </summary>
        /// <param name="value"></param>
        public void SetCurrentCardBonus(int value)
        {
            this.CardBonus = value;
        }
    }
}
