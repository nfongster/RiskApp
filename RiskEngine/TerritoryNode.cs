using System;
using System.Collections.Generic;
using System.Text;

namespace RiskEngine
{
    public class TerritoryNode
    {
        private HashSet<TerritoryNode> AdjacentNodeSet { get; set; }
        public string TerritoryName { get; }
        public Faction OccupyingFaction { get; set; }
        private int NumArmies;
        public CommonFunctions.Continent continent { get; set; }

        /// <summary>
        /// Creates a new TerritoryNode object with the given name, and 0 armies
        /// </summary>
        /// <param name="name">Name of territory</param>
        public TerritoryNode(string name)
        {
            this.AdjacentNodeSet = new HashSet<TerritoryNode>();
            this.TerritoryName = name;
            this.NumArmies = 0;
        }

        /// <summary>
        /// Adds a TerritoryNode to the Adjacent Node set
        /// </summary>
        /// <param name="adjacentNode"></param>
        public void AddAdjacentNode(TerritoryNode adjacentNode)
        {
            this.AdjacentNodeSet.Add(adjacentNode);
        }

        /// <summary>
        /// Returns the number of armies currently occupying the node
        /// </summary>
        /// <returns></returns>
        public int GetArmyCount()
        {
            return this.NumArmies;
        }

        /// <summary>
        /// Adds the specified number of armies to the node
        /// </summary>
        /// <param name="num"></param>
        public void AddArmies(int num)
        {
            this.NumArmies += num;
        }

        /// <summary>
        /// Removes the specified number of armies from the node
        /// </summary>
        /// <param name="num"></param>
        public void RemoveArmies(int num)
        {
            this.NumArmies -= num;
        }

        /// <summary>
        /// Removs all armies from the node
        /// </summary>
        public void ClearArmies()
        {
            this.NumArmies = 0;
        }

        /// <summary>
        /// Returns a boolean indicating whether this node is adjacent to the parameter node.
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public bool IsAdjacentTo(TerritoryNode node)
        {
            if (this.AdjacentNodeSet.Contains(node))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Returns a hash set of all nodes adjacent to this territory.
        /// </summary>
        /// <returns></returns>
        public HashSet<TerritoryNode> GetAdjacentNodes()
        {
            return this.AdjacentNodeSet;
        }
    }
}
