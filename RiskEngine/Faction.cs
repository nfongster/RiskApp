using System;
using System.Collections.Generic;
using System.Text;

namespace RiskEngine
{
    public class Faction
    {
        public List<TerritoryNode> FactionTerritories { get; set; }
        public string FactionName { get; }
        private int ArmyReserves { get; set; }
        private bool Active { get; set; }
        private List<Card> FactionCardList { get; set; }
        public int NumConqueredTerritoriesCurrentTurn { get; set; }

        public Faction(string name)
        {
            this.FactionName = name;
            this.FactionTerritories = new List<TerritoryNode>();
            this.ArmyReserves = CommonFunctions.RESERVE_COUNT;
            this.Active = true;
            this.FactionCardList = new List<Card>();
            this.NumConqueredTerritoriesCurrentTurn = 0;
        }

        /// <summary>
        /// Returns this faction's total number of armies currently active on the board.
        /// </summary>
        /// <returns></returns>
        public int GetNumberActiveArmies()
        {
            int total = 0;
            foreach (TerritoryNode node in this.FactionTerritories)
            {
                total += node.GetArmyCount();
            }
            // Eliminate faction if no longer in game
            if (total == 0)
            {
                this.Active = false;
            }
            return total;
        }

        /// <summary>
        /// Removes specified number of armies from reserve pool of Faction, and places them into
        /// specified Territory Node's army count
        /// </summary>
        /// <param name="node"></param>
        /// <param name="numArmies"></param>
        public void PlaceArmies(TerritoryNode node, int numArmies)
        {
            // Remove from reserve pool
            // Place army on node
            this.ArmyReserves -= numArmies;
            node.AddArmies(numArmies);
        }

        /// <summary>
        /// Returns this faction's number of army reserves not yet placed on the board
        /// </summary>
        /// <returns></returns>
        public int GetReservesCount()
        {
            return this.ArmyReserves;
        }

        /// <summary>
        /// Sets this faction's total reserve count (armies to be placed on board)
        /// </summary>
        /// <param name="count"></param>
        public void SetReservesCount(int count)
        {
            this.ArmyReserves = count;
        }


        /// <summary>
        /// Adds the given node to this faction's territory list, and removes it from previous owner's list
        /// </summary>
        public void OccupyTerritory(TerritoryNode node)
        {
            Faction oldFaction = node.OccupyingFaction;
            node.OccupyingFaction = this;
            this.FactionTerritories.Add(node);
            oldFaction.FactionTerritories.Remove(node);
        }

        /// <summary>
        /// Returns whether or not the given faction is still active
        /// </summary>
        /// <returns></returns>
        public bool IsActive()
        {
            return this.Active;
        }

        /// <summary>
        /// Draws a card from the board's stack, places into player's hand
        /// </summary>
        /// <param name="board"></param>
        public void DrawCard(RiskBoard board)
        {
            this.FactionCardList.Add(board.CardStack.Pop());
        }
        
        /// <summary>
        /// Returns a list of all cards in the player's hand
        /// </summary>
        /// <returns></returns>
        public List<Card> GetCardList()
        {
            return this.FactionCardList;
        }
    }
}
