using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.IO;
using RiskEngine;
using System;

namespace Risk_UnitTesting
{
    [TestClass]
    public class RiskTests
    {
        public static string Faction_1 = "RED";
        public static string Faction_2 = "BLUE";
        //public static string filePath = @"C:\Users\Nick\Desktop\Risk\MicroBoard_Map.txt";
        public static string filePath = @"C:\Users\Nick\Desktop\Risk\Risk_Map.txt";

        [TestMethod]
        public void CreateBoard_AddFactions()
        {
            RiskBoard board = new RiskBoard(CommonFunctions.ParseMapFile(filePath));
            board.AddFaction(Faction_1);
            board.AddFaction(Faction_2);
            board.ClearBoard();
        }

        [TestMethod]
        public void CreateBoardWithFactions()
        {
            RiskBoard board = new RiskBoard(CommonFunctions.ParseMapFile(filePath), new string[] { Faction_1, Faction_2 });
            board.ClearBoard();
        }

        [TestMethod]
        public void RandomFactionAssignments()
        {
            RiskBoard board = new RiskBoard(CommonFunctions.ParseMapFile(filePath), new string[] { Faction_1, Faction_2 });
            board.RandomAssignment();
        }

        [TestMethod]
        public void RandomBoardSetup()
        {
            // Create board and get player objects
            RiskBoard board = new RiskBoard(CommonFunctions.ParseMapFile(filePath), new string[] { Faction_1, Faction_2 });
            board.RandomAssignment();
            List<Faction> players = board.GetFactions();

            // Deplete total reserve count
            int totalReserves = board.GetTotalReserves();
            Random r = new Random();
            while (totalReserves > 0)
            {
                foreach (var player in players)
                {
                    player.PlaceArmies(player.FactionTerritories[r.Next(player.FactionTerritories.Count)], 1);
                    totalReserves = board.GetTotalReserves();
                }
            }
        }

        [TestMethod]
        public void SimulateTurn()
        {
            RiskBoard board = new RiskBoard(CommonFunctions.ParseMapFile(filePath), new string[] { Faction_1, Faction_2 });
            board.RandomAssignment();
            List<Faction> players = board.GetFactions();
            //var rollResults = players[0].RollDice();
        }

        // Other stuff to test:
        // 2. Simulate single dice roll and turn (inc. choosing to move armies)
        // 6. Create console app for 2+ real players to play (take turns picking territory)
        // 4. Compute probability of winning a particular battle
        // x. Simulate random game
        // 5. Simulate intelligent game that applies #4 (hard bc includes how many armies should I move?)
        // 7. Create game based on actual Risk board

        // GUI:
        // 1. Create actual Risk node map
        // 2. Create buttons to play game before rendering any visual maps
        // 3. Create fields storing game state
        // 4. Render visual map (hard!)
        // 5. Create mouse-over property list of each territory (inc. probability of winning)
        // 6. Try adding a map of Japan (shogun)

        // Advanced:
        // 1. Create continents (bonuses for controlling them)
        // 2. Players get territory cards (tokens) they can exchange for armies
        // 3. Package everything (inc. instructions) into installer
        // 4. Store game history, settings in an AppData folder
        // 5. Use something else like JSON to read in custom maps, and render custom maps
        // 6. Output statistics of random simulations to Python plots, do some machine learning?
        // 7. Add sounds to button clicks, other events like winning a battle
        // 8. Enable LAN or Wifi battles
        // 9. HTML-like helpfile for API(?)
    }
}
