using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RiskUI;
using RiskEngine;

namespace RiskGUI
{
    public partial class BoardUI : Form
    {
        public static string[] Faction_Array = new string[] { "RED", "BLUE", "GREEN", "YELLOW", "CYAN", "MAGENTA" };
        public static string filePath = @"C:\Users\Nick\Desktop\Risk\";
        public static int TurnCount = 0;
        public RiskBoard board;
        public int numRedDice, numWhiteDice;
        public TerritoryNode InvaderNode, DefenderNode;
        public List<Faction> factionObjs;
        public int currentFactionIndex;
        public int numTurns;
        public Dictionary<string, string[]> continentToTerritories;
        public Dictionary<string, int> continentToScore;

        public BoardUI()
        {
            InitializeComponent();
            MessageBox.Show("Welcome to Risk.  DEGBUG: This game will take place between 2 factions, armies will be randomly placed.");
            int num_factions = 2;
            this.numTurns = 0;

            // Populate string array of factions
            string[] factions = new string[num_factions];
            for (int i = 0; i < num_factions; i++)
            {
                factions[i] = Faction_Array[i];
            }

            // Parse continent file to get continent territories and bonuses
            /*
            Dictionary<string, string[]> continentToTerritories = new Dictionary<string, string[]>();
            Dictionary<string, int> continentToScore = new Dictionary<string, int>();
            CommonFunctions.ParseContinentFile(filePath, ref continentToTerritories, ref continentToScore);*/
            this.continentToTerritories = new Dictionary<string, string[]>();
            this.continentToScore = new Dictionary<string, int>();
            CommonFunctions.ParseContinentFile(filePath, ref continentToTerritories, ref continentToScore);

            // Create board, assign territories, and place armies (randomly or otherwise)
            board = new RiskBoard(continentToTerritories, CommonFunctions.ParseMapFile(filePath), factions);
            board.RandomAssignment();
            board.RandomArmyPlacement();
            this.factionObjs = board.GetFactions();
            this.currentFactionIndex = 0;
            bool gameActive = true;

            #region Initial Game Layout
            // Populate gameboard with armies
            // NORTH AMERICA
            buttonAK.Text = board.GetTerritoryNodeByName("Alaska").GetArmyCount().ToString();
            buttonAK.BackColor = GetColorByNodeFaction(board.GetTerritoryNodeByName("Alaska"));
            buttonAL.Text = board.GetTerritoryNodeByName("Alberta").GetArmyCount().ToString();
            buttonAL.BackColor = GetColorByNodeFaction(board.GetTerritoryNodeByName("Alberta"));
            buttonCA.Text = board.GetTerritoryNodeByName("Central America").GetArmyCount().ToString();
            buttonCA.BackColor = GetColorByNodeFaction(board.GetTerritoryNodeByName("Central America"));
            buttonEU.Text = board.GetTerritoryNodeByName("Eastern US").GetArmyCount().ToString();
            buttonEU.BackColor = GetColorByNodeFaction(board.GetTerritoryNodeByName("Eastern US"));
            buttonGL.Text = board.GetTerritoryNodeByName("Greenland").GetArmyCount().ToString();
            buttonGL.BackColor = GetColorByNodeFaction(board.GetTerritoryNodeByName("Greenland"));
            buttonNT.Text = board.GetTerritoryNodeByName("Northwest Territory").GetArmyCount().ToString();
            buttonNT.BackColor = GetColorByNodeFaction(board.GetTerritoryNodeByName("Northwest Territory"));
            buttonON.Text = board.GetTerritoryNodeByName("Ontario").GetArmyCount().ToString();
            buttonON.BackColor = GetColorByNodeFaction(board.GetTerritoryNodeByName("Ontario"));
            buttonQU.Text = board.GetTerritoryNodeByName("Quebec").GetArmyCount().ToString();
            buttonQU.BackColor = GetColorByNodeFaction(board.GetTerritoryNodeByName("Quebec"));
            buttonWU.Text = board.GetTerritoryNodeByName("Western US").GetArmyCount().ToString();
            buttonWU.BackColor = GetColorByNodeFaction(board.GetTerritoryNodeByName("Western US"));

            // SOUTH AMERICA
            buttonAR.Text = board.GetTerritoryNodeByName("Argentina").GetArmyCount().ToString();
            buttonAR.BackColor = GetColorByNodeFaction(board.GetTerritoryNodeByName("Argentina"));
            buttonBR.Text = board.GetTerritoryNodeByName("Brazil").GetArmyCount().ToString();
            buttonBR.BackColor = GetColorByNodeFaction(board.GetTerritoryNodeByName("Brazil"));
            buttonPU.Text = board.GetTerritoryNodeByName("Peru").GetArmyCount().ToString();
            buttonPU.BackColor = GetColorByNodeFaction(board.GetTerritoryNodeByName("Peru"));
            buttonVZ.Text = board.GetTerritoryNodeByName("Venezuela").GetArmyCount().ToString();
            buttonVZ.BackColor = GetColorByNodeFaction(board.GetTerritoryNodeByName("Venezuela"));

            // EUROPE
            buttonGB.Text = board.GetTerritoryNodeByName("Great Britain").GetArmyCount().ToString();
            buttonGB.BackColor = GetColorByNodeFaction(board.GetTerritoryNodeByName("Great Britain"));
            buttonIC.Text = board.GetTerritoryNodeByName("Iceland").GetArmyCount().ToString();
            buttonIC.BackColor = GetColorByNodeFaction(board.GetTerritoryNodeByName("Iceland"));
            buttonNE.Text = board.GetTerritoryNodeByName("Northern Europe").GetArmyCount().ToString();
            buttonNE.BackColor = GetColorByNodeFaction(board.GetTerritoryNodeByName("Northern Europe"));
            buttonSC.Text = board.GetTerritoryNodeByName("Scandinavia").GetArmyCount().ToString();
            buttonSC.BackColor = GetColorByNodeFaction(board.GetTerritoryNodeByName("Scandinavia"));
            buttonSE.Text = board.GetTerritoryNodeByName("Southern Europe").GetArmyCount().ToString();
            buttonSE.BackColor = GetColorByNodeFaction(board.GetTerritoryNodeByName("Southern Europe"));
            buttonRS.Text = board.GetTerritoryNodeByName("Russia").GetArmyCount().ToString();
            buttonRS.BackColor = GetColorByNodeFaction(board.GetTerritoryNodeByName("Russia"));
            buttonWE.Text = board.GetTerritoryNodeByName("Western Europe").GetArmyCount().ToString();
            buttonWE.BackColor = GetColorByNodeFaction(board.GetTerritoryNodeByName("Western Europe"));

            // AFRICA
            buttonCO.Text = board.GetTerritoryNodeByName("Congo").GetArmyCount().ToString();
            buttonCO.BackColor = GetColorByNodeFaction(board.GetTerritoryNodeByName("Congo"));
            buttonEA.Text = board.GetTerritoryNodeByName("East Africa").GetArmyCount().ToString();
            buttonEA.BackColor = GetColorByNodeFaction(board.GetTerritoryNodeByName("East Africa"));
            buttonEG.Text = board.GetTerritoryNodeByName("Egypt").GetArmyCount().ToString();
            buttonEG.BackColor = GetColorByNodeFaction(board.GetTerritoryNodeByName("Egypt"));
            buttonMD.Text = board.GetTerritoryNodeByName("Madagascar").GetArmyCount().ToString();
            buttonMD.BackColor = GetColorByNodeFaction(board.GetTerritoryNodeByName("Madagascar"));
            buttonNA.Text = board.GetTerritoryNodeByName("North Africa").GetArmyCount().ToString();
            buttonNA.BackColor = GetColorByNodeFaction(board.GetTerritoryNodeByName("North Africa"));
            buttonSA.Text = board.GetTerritoryNodeByName("South Africa").GetArmyCount().ToString();
            buttonSA.BackColor = GetColorByNodeFaction(board.GetTerritoryNodeByName("South Africa"));

            // ASIA
            buttonAF.Text = board.GetTerritoryNodeByName("Afghanistan").GetArmyCount().ToString();
            buttonAF.BackColor = GetColorByNodeFaction(board.GetTerritoryNodeByName("Afghanistan"));
            buttonCH.Text = board.GetTerritoryNodeByName("China").GetArmyCount().ToString();
            buttonCH.BackColor = GetColorByNodeFaction(board.GetTerritoryNodeByName("China"));
            buttonIN.Text = board.GetTerritoryNodeByName("India").GetArmyCount().ToString();
            buttonIN.BackColor = GetColorByNodeFaction(board.GetTerritoryNodeByName("India"));
            buttonIR.Text = board.GetTerritoryNodeByName("Irkutsk").GetArmyCount().ToString();
            buttonIR.BackColor = GetColorByNodeFaction(board.GetTerritoryNodeByName("Irkutsk"));
            buttonJA.Text = board.GetTerritoryNodeByName("Japan").GetArmyCount().ToString();
            buttonJA.BackColor = GetColorByNodeFaction(board.GetTerritoryNodeByName("Japan"));
            buttonKM.Text = board.GetTerritoryNodeByName("Kamchatka").GetArmyCount().ToString();
            buttonKM.BackColor = GetColorByNodeFaction(board.GetTerritoryNodeByName("Kamchatka"));
            buttonME.Text = board.GetTerritoryNodeByName("Middle East").GetArmyCount().ToString();
            buttonME.BackColor = GetColorByNodeFaction(board.GetTerritoryNodeByName("Middle East"));
            buttonMO.Text = board.GetTerritoryNodeByName("Mongolia").GetArmyCount().ToString();
            buttonMO.BackColor = GetColorByNodeFaction(board.GetTerritoryNodeByName("Mongolia"));
            buttonSI.Text = board.GetTerritoryNodeByName("Siam").GetArmyCount().ToString();
            buttonSI.BackColor = GetColorByNodeFaction(board.GetTerritoryNodeByName("Siam"));
            buttonSB.Text = board.GetTerritoryNodeByName("Siberia").GetArmyCount().ToString();
            buttonSB.BackColor = GetColorByNodeFaction(board.GetTerritoryNodeByName("Siberia"));
            buttonUR.Text = board.GetTerritoryNodeByName("Ural").GetArmyCount().ToString();
            buttonUR.BackColor = GetColorByNodeFaction(board.GetTerritoryNodeByName("Ural"));
            buttonYA.Text = board.GetTerritoryNodeByName("Yakutsk").GetArmyCount().ToString();
            buttonYA.BackColor = GetColorByNodeFaction(board.GetTerritoryNodeByName("Yakutsk"));

            // AUSTRALIA
            buttonES.Text = board.GetTerritoryNodeByName("Eastern Australia").GetArmyCount().ToString();
            buttonES.BackColor = GetColorByNodeFaction(board.GetTerritoryNodeByName("Eastern Australia"));
            buttonIS.Text = board.GetTerritoryNodeByName("Indonesia").GetArmyCount().ToString();
            buttonIS.BackColor = GetColorByNodeFaction(board.GetTerritoryNodeByName("Indonesia"));
            buttonNG.Text = board.GetTerritoryNodeByName("New Guinea").GetArmyCount().ToString();
            buttonNG.BackColor = GetColorByNodeFaction(board.GetTerritoryNodeByName("New Guinea"));
            buttonWS.Text = board.GetTerritoryNodeByName("Western Australia").GetArmyCount().ToString();
            buttonWS.BackColor = GetColorByNodeFaction(board.GetTerritoryNodeByName("Western Australia"));

            #endregion

            endTurnButton.Enabled = true;
            RollDiceButton.Enabled = false;
            quitDiceRollButton.Enabled = false;
            currentPlayer.Text = this.factionObjs[0].FactionName;

            #region Add event handlers
            // North America
            buttonAK.Click += TerritoryButton_Click;
            buttonNT.Click += TerritoryButton_Click;
            buttonAL.Click += TerritoryButton_Click;
            buttonON.Click += TerritoryButton_Click;
            buttonQU.Click += TerritoryButton_Click;
            buttonGL.Click += TerritoryButton_Click;
            buttonWU.Click += TerritoryButton_Click;
            buttonEU.Click += TerritoryButton_Click;
            buttonCA.Click += TerritoryButton_Click;
            // South America
            buttonVZ.Click += TerritoryButton_Click;
            buttonPU.Click += TerritoryButton_Click;
            buttonBR.Click += TerritoryButton_Click;
            buttonAR.Click += TerritoryButton_Click;
            // Europe
            buttonIC.Click += TerritoryButton_Click;
            buttonSC.Click += TerritoryButton_Click;
            buttonGB.Click += TerritoryButton_Click;
            buttonWE.Click += TerritoryButton_Click;
            buttonNE.Click += TerritoryButton_Click;
            buttonSE.Click += TerritoryButton_Click;
            buttonRS.Click += TerritoryButton_Click;
            // Africa
            buttonEG.Click += TerritoryButton_Click;
            buttonNA.Click += TerritoryButton_Click;
            buttonEA.Click += TerritoryButton_Click;
            buttonCO.Click += TerritoryButton_Click;
            buttonMD.Click += TerritoryButton_Click;
            buttonSA.Click += TerritoryButton_Click;
            // Asia
            buttonUR.Click += TerritoryButton_Click;
            buttonSB.Click += TerritoryButton_Click;
            buttonYA.Click += TerritoryButton_Click;
            buttonKM.Click += TerritoryButton_Click;
            buttonIR.Click += TerritoryButton_Click;
            buttonAF.Click += TerritoryButton_Click;
            buttonME.Click += TerritoryButton_Click;
            buttonIN.Click += TerritoryButton_Click;
            buttonCH.Click += TerritoryButton_Click;
            buttonMO.Click += TerritoryButton_Click;
            buttonJA.Click += TerritoryButton_Click;
            buttonSI.Click += TerritoryButton_Click;
            // Australia
            buttonIS.Click += TerritoryButton_Click;
            buttonWS.Click += TerritoryButton_Click;
            buttonES.Click += TerritoryButton_Click;
            buttonNG.Click += TerritoryButton_Click;
            #endregion

            // Enable all RED faction squares, and disable all non-RED faction squares
            ToggleSquares(board, this.factionObjs[0], false);
            for (int i = 1; i < this.factionObjs.Count; i++)
            {
                ToggleSquares(board, this.factionObjs[i], true);
            }
        }

        #region Miscellaneous Functions
        private static Color GetColorByNodeFaction(TerritoryNode node)
        {
            switch (node.OccupyingFaction.FactionName)
            {
                case "RED":
                    return Color.Red;
                case "BLUE":
                    return Color.Blue;
                case "GREEN":
                    return Color.Green;
                case "CYAN":
                    return Color.Cyan;
                case "YELLOW":
                    return Color.Yellow;
                default:  // Magenta
                    return Color.Magenta;
            }
        }

        private static Color GetColorByFaction(Faction faction)
        {
            switch (faction.FactionName)
            {
                case "RED":
                    return Color.Red;
                case "BLUE":
                    return Color.Blue;
                case "GREEN":
                    return Color.Green;
                case "CYAN":
                    return Color.Cyan;
                case "YELLOW":
                    return Color.Yellow;
                default:  // Magenta
                    return Color.Magenta;
            }
        }

        /// <summary>
        /// Disables or enables all squares associated with the given faction
        /// </summary>
        /// <param name="faction"></param>
        private void ToggleSquares(RiskBoard board, Faction faction, bool disable)
        {
            string name = faction.FactionName;
            foreach (Control c in this.Controls)
            {
                if (c.GetType() == typeof(System.Windows.Forms.Button) && c.BackColor == GetColorByFaction(faction))
                {
                    if (disable)
                    {
                        c.Enabled = false;
                    }
                    else
                    {
                        c.Enabled = true;
                    }
                }
            }
        }

        /// <summary>
        /// Returns territory name associated with the 2-letter code
        /// </summary>
        /// <param name="buttonName"></param>
        /// <returns></returns>
        private string TwoLetterCodeToTerritoryName(string buttonName)
        {
            // Parse
            var stringArray = buttonName.ToArray();
            string codeString = new string(new char[] { stringArray[stringArray.Length - 2], stringArray[stringArray.Length - 1] });
            // Case switch
            switch (codeString)
            {
                // North America
                case "AK": return "Alaska";
                case "NT": return "Northwest Territory";
                case "AL": return "Alberta";
                case "ON": return "Ontario";
                case "QU": return "Quebec";
                case "GL": return "Greenland";
                case "WU": return "Western US";
                case "EU": return "Eastern US";
                case "CA": return "Central America";
                // South America
                case "VZ": return "Venezuela";
                case "BR": return "Brazil";
                case "PU": return "Peru";
                case "AR": return "Argentina";
                // Europe
                case "IC": return "Iceland";
                case "SC": return "Scandinavia";
                case "RS": return "Russia";
                case "NE": return "Northern Europe";
                case "GB": return "Great Britain";
                case "WE": return "Western Europe";
                case "SE": return "Southern Europe";
                // Africa
                case "EG": return "Egypt";
                case "NA": return "North Africa";
                case "EA": return "East Africa";
                case "CO": return "Congo";
                case "MD": return "Madagascar";
                case "SA": return "South Africa";
                // Asia
                case "UR": return "Ural";
                case "SB": return "Siberia";
                case "YA": return "Yakutsk";
                case "IR": return "Irkutsk";
                case "KM": return "Kamchatka";
                case "AF": return "Afghanistan";
                case "MO": return "Mongolia";
                case "ME": return "Middle East";
                case "CH": return "China";
                case "JA": return "Japan";
                case "IN": return "India";
                case "SI": return "Siam";
                // Australia
                case "WS": return "Western Australia";
                case "ES": return "Eastern Australia";
                case "IS": return "Indonesia";
                case "NG": return "New Guinea";
                default: return null;
            }
        }

        /// <summary>
        /// Returns territory button associated with the 2-letter code, or full territory name
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        private Button TerritoryNameToButton(string code)
        {
            switch (code)
            {
                // North America
                case "AK": case "Alaska": return buttonAK;
                case "NT": case "Northwest Territory": return buttonNT;
                case "AL": case "Alberta": return buttonAL;
                case "ON": case "Ontario": return buttonON;
                case "QU": case "Quebec": return buttonQU;
                case "GL": case "Greenland": return buttonGL;
                case "WU": case "Western US": return buttonWU;
                case "EU": case "Eastern US": return buttonEU;
                case "CA": case "Central America": return buttonCA;
                // South America
                case "VZ": case "Venezuela": return buttonVZ;
                case "BR": case "Brazil": return buttonBR;
                case "PU": case "Peru": return buttonPU;
                case "AR": case "Argentina": return buttonAR;
                // Europe
                case "IC": case "Iceland": return buttonIC;
                case "SC": case "Scandinavia": return buttonSC;
                case "RS": case "Russia": return buttonRS;
                case "NE": case "Northern Europe": return buttonNE;
                case "GB": case "Great Britain": return buttonGB;
                case "WE": case "Western Europe": return buttonWE;
                case "SE": case "Southern Europe": return buttonSE;
                // Africa
                case "EG": case "Egypt": return buttonEG;
                case "NA": case "North Africa": return buttonNA;
                case "EA": case "East Africa": return buttonEA;
                case "CO": case "Congo": return buttonCO;
                case "MD": case "Madagascar": return buttonMD;
                case "SA": case "South Africa": return buttonSA;
                // Asia
                case "UR": case "Ural": return buttonUR;
                case "SB": case "Siberia": return buttonSB;
                case "YA": case "Yakutsk": return buttonYA;
                case "IR": case "Irkutsk": return buttonIR;
                case "KM": case "Kamchatka": return buttonKM;
                case "AF": case "Afghanistan": return buttonAF;
                case "MO": case "Mongolia": return buttonMO;
                case "ME": case "Middle East": return buttonME;
                case "CH": case "China": return buttonCH;
                case "JA": case "Japan": return buttonJA;
                case "IN": case "India": return buttonIN;
                case "SI": case "Siam": return buttonSI;
                // Australia
                case "WS": case "Western Australia": return buttonWS;
                case "ES": case "Eastern Australia": return buttonES;
                case "IS": case "Indonesia": return buttonIS;
                case "NG": case "New Guinea": return buttonNG;
                default: return null;
            }
        }

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
                //PrintMap(board);
            }
        }

        private void showDice(Dice d, PictureBox pb, CommonFunctions.FactionState state)
        {
            string impath = filePath + @"Risk_UnitTesting\RiskGUI\";
            if (state == CommonFunctions.FactionState.Attacker)
            {
                switch (d.PeekDice())
                {
                    case 1: pb.Image = Image.FromFile(impath + "redDie1.png"); break;
                    case 2: pb.Image = Image.FromFile(impath + "redDie2.png"); break;
                    case 3: pb.Image = Image.FromFile(impath + "redDie3.png"); break;
                    case 4: pb.Image = Image.FromFile(impath + "redDie4.png"); break;
                    case 5: pb.Image = Image.FromFile(impath + "redDie5.png"); break;
                    case 6: pb.Image = Image.FromFile(impath + "redDie6.png"); break;
                }
            }
            else
            {
                switch (d.PeekDice())
                {
                    case 1: pb.Image = Image.FromFile(impath + "whiteDie1.png"); break;
                    case 2: pb.Image = Image.FromFile(impath + "whiteDie2.png"); break;
                    case 3: pb.Image = Image.FromFile(impath + "whiteDie3.png"); break;
                    case 4: pb.Image = Image.FromFile(impath + "whiteDie4.png"); break;
                    case 5: pb.Image = Image.FromFile(impath + "whiteDie5.png"); break;
                    case 6: pb.Image = Image.FromFile(impath + "whiteDie6.png"); break;
                }
            }
            
        }

        /// <summary>
        /// Compares roll of the dice, depleting armies from attacker and defender nodes accordingly (RiskUI Only)
        /// </summary>
        /// <param name="attackerDice"></param>
        /// <param name="defenderDice"></param>
        /// <returns></returns>
        public void CompareDiceRollUI(List<Dice> attackerDice, List<Dice> defenderDice,
                                    TerritoryNode attackerNode, TerritoryNode defenderNode)
        {
            if (this.InvaderNode.GetArmyCount() >= CommonFunctions.MIN_ARMIES_TO_ATTACK && this.DefenderNode.GetArmyCount() > 0)
            {
                // Compare first pair of die
                int[] attackerVals = CommonFunctions.GetMaxDiceValues(attackerDice);
                int[] defenderVals = CommonFunctions.GetMaxDiceValues(defenderDice);
                Button buttonDefender = TerritoryNameToButton(defenderNode.TerritoryName);
                Button buttonAttacker = TerritoryNameToButton(attackerNode.TerritoryName);
                if (attackerVals[0] > defenderVals[0])
                {
                    defenderNode.RemoveArmies(1);
                    buttonDefender.Text = (Int32.Parse(buttonDefender.Text) - 1).ToString();
                }
                else
                {
                    attackerNode.RemoveArmies(1);
                    buttonAttacker.Text = (Int32.Parse(buttonAttacker.Text) - 1).ToString();
                }
                // Compare second pair of die, if both players had rolled at least 2 dice each
                if (attackerNode.GetArmyCount() >= CommonFunctions.MIN_ARMIES_TO_ATTACK && defenderNode.GetArmyCount() > 0 && attackerVals[1] > 0 && defenderVals[1] > 0)
                {
                    if (attackerVals[1] > defenderVals[1])
                    {
                        defenderNode.RemoveArmies(1);
                        buttonDefender.Text = (Int32.Parse(buttonDefender.Text) - 1).ToString();
                    }
                    else
                    {
                        attackerNode.RemoveArmies(1);
                        buttonAttacker.Text = (Int32.Parse(buttonAttacker.Text) - 1).ToString();
                    }
                    if (defenderNode.GetArmyCount() == 0)
                    {
                        TransferArmiesUI(attackerNode, defenderNode);
                    }
                }
                else if (defenderNode.GetArmyCount() == 0)
                {
                    TransferArmiesUI(attackerNode, defenderNode);
                }
                if (attackerNode.GetArmyCount() == 1)
                {
                    MessageBox.Show("Invading forces have been reduced to 1 army.  Please select a different territory from which to invade, or end turn.");
                    ToggleSquares(board, attackerNode.OccupyingFaction, false);
                    ToggleSquares(board, defenderNode.OccupyingFaction, true);
                }
            }
        }

        /// <summary>
        /// Transfers a user-input number of armies from the attacker node to the defender node.
        /// </summary>
        /// <param name="attackerNode"></param>
        /// <param name="defenderNode"></param>
        private void TransferArmiesUI(TerritoryNode attackerNode, TerritoryNode defenderNode)
        {
            MessageBox.Show("Attacker won new territory! Choose number of armies to move.");
            var moveArmiesUI = new MoveArmiesUI();
            int movingArmies = moveArmiesUI.numArmies;
            while (movingArmies < 1 || movingArmies >= attackerNode.GetArmyCount())
            {
                MessageBox.Show(String.Format("Number of armies to move must be at least 1 and no more than {0}.  Please choose a different value.", attackerNode.GetArmyCount() - 1));
                moveArmiesUI = new MoveArmiesUI();
                if (moveArmiesUI.ShowDialog() == DialogResult.OK)
                {
                    movingArmies = moveArmiesUI.numArmies;
                }
            }
            attackerNode.RemoveArmies(movingArmies);
            defenderNode.AddArmies(movingArmies);
            var defeatedFaction = defenderNode.OccupyingFaction;
            attackerNode.OccupyingFaction.OccupyTerritory(defenderNode);
            attackerNode.OccupyingFaction.NumConqueredTerritoriesCurrentTurn += 1;

            // Update UI and reset invader/defender nodes
            Button buttonConquered = TerritoryNameToButton(defenderNode.TerritoryName);
            buttonConquered.Text = (Int32.Parse(buttonConquered.Text) + movingArmies).ToString();
            buttonConquered.BackColor = GetColorByNodeFaction(attackerNode);
            Button buttonAttacker = TerritoryNameToButton(attackerNode.TerritoryName);
            buttonAttacker.Text = (Int32.Parse(buttonAttacker.Text) - movingArmies).ToString();
            ToggleSquares(board, this.InvaderNode.OccupyingFaction, false);
            ToggleSquares(board, defeatedFaction, true);
            this.InvaderNode = null;
            this.DefenderNode = null;
            buttonConquered.Click -= EnemyTerritoryButton_Click;
            buttonConquered.Click += TerritoryButton_Click;           
        }

        #endregion

        #region Event Handlers
        private void TerritoryButton_Click(object sender, EventArgs e)
        {
            string num_armies = (sender as Button).Text;
            if (Int32.Parse(num_armies) < 2)
            {
                MessageBox.Show("You need at least 2 armies to launch an attack from this province.");
            }
            else
            {
                var node = board.GetTerritoryNodeByName(TwoLetterCodeToTerritoryName((sender as Button).Name));
                this.InvaderNode = node;
                var adjNodes = node.GetAdjacentNodes();
                string faction = node.OccupyingFaction.FactionName;
                bool frontier = false;
                foreach (var x in adjNodes)
                {
                    if (x.OccupyingFaction.FactionName != faction) // if just one adj territory is different faction
                    {
                        frontier = true;
                        break;
                    }
                }
                if (frontier)  // i.e,, node is frontier if it borders at least one node of different faction
                {
                    MessageBox.Show("Choose territory to invade.");
                    // If so, disable all button of this territory, enable all buttons adjacent to this territory
                    ToggleSquares(board, node.OccupyingFaction, true);
                    foreach (var x in adjNodes)
                    {
                        if (x.OccupyingFaction.FactionName != faction)
                        {
                            TerritoryNameToButton(x.TerritoryName).Enabled = true;
                            // Replace event handler with new one
                            TerritoryNameToButton(x.TerritoryName).Click -= TerritoryButton_Click;
                            TerritoryNameToButton(x.TerritoryName).Click += EnemyTerritoryButton_Click;
                        }
                    }
                }
                else
                {
                    MessageBox.Show("All adjacent provinces belong to current faction - please choose another province.");
                }
            }
        }

        private void EnemyTerritoryButton_Click(object sender, EventArgs e)
        {
            this.DefenderNode = board.GetTerritoryNodeByName(TwoLetterCodeToTerritoryName((sender as Button).Name));
            var diceForm = new DiceSelectionUI();
            if (diceForm.ShowDialog() == DialogResult.OK)  // this line is what shows the form.  showdialog itself returns a result
            {
                MessageBox.Show("Roll the dice.  Re-click territory to change number of dice.");
                this.numRedDice = diceForm.numRedDice;
                this.numWhiteDice = diceForm.numWhiteDice;
            }
            RollDiceButton.Enabled = true;
        }

        private void endTurnButton_Click(object sender, EventArgs e)
        {
            // reset nodes
            this.InvaderNode = null;
            this.DefenderNode = null;
            // go to next faction
            this.currentFactionIndex += 1;
            if (this.currentFactionIndex >= this.factionObjs.Count) this.currentFactionIndex = 0;
            // Enable all current faction squares, and disable all non-current faction squares
            ToggleSquares(board, this.factionObjs[this.currentFactionIndex], false);
            for (int i = 0; i < this.factionObjs.Count; i++)
            {
                if (i != this.currentFactionIndex) ToggleSquares(board, this.factionObjs[i], true);
            }
            // display new faction title
            MessageBox.Show(String.Format("Ending turn for {0}.  Next player is {1}.", currentPlayer.Text, this.factionObjs[this.currentFactionIndex].FactionName));
            currentPlayer.Text = this.factionObjs[this.currentFactionIndex].FactionName;

            // grant bonus
            ArmyPlacement(board, this.factionObjs[this.currentFactionIndex], this.continentToScore);
        }

        private void quitDiceRollButton_Click(object sender, EventArgs e)
        {
            //
        }

        private void RollDiceButton_Click(object sender, EventArgs e)
        {
            List<Dice> redDice = new List<Dice>();
            List<Dice> whiteDice = new List<Dice>();
            CommonFunctions.RollAllDie(ref redDice, ref whiteDice, numRedDice, numWhiteDice);
            // Display appropriate dice on BoardUI based on redDice and whiteDice results
            showDice(redDice[0], this.redDice1, CommonFunctions.FactionState.Attacker);
            if (numRedDice < 3)
            {
                this.redDice3.Image = null;
                if (numRedDice == 1)
                {
                    this.redDice2.Image = null;
                }
                else
                {
                    showDice(redDice[1], this.redDice2, CommonFunctions.FactionState.Attacker);
                }
            }
            else
            {
                showDice(redDice[1], this.redDice2, CommonFunctions.FactionState.Attacker);
                showDice(redDice[2], this.redDice3, CommonFunctions.FactionState.Attacker);
            }
            showDice(whiteDice[0], this.whiteDice1, CommonFunctions.FactionState.Defender);
            if (numWhiteDice < 2)
            {
                this.whiteDice2.Image = null;
            }
            else
            {
                showDice(whiteDice[1], this.whiteDice2, CommonFunctions.FactionState.Defender);
            }

            // Display appropriate number of losses for each faction
            CompareDiceRollUI(redDice, whiteDice, this.InvaderNode, this.DefenderNode);
        }

        private void TradeCardsButton_Click(object sender, EventArgs e)
        {
            var tradeCardsForm = new CardSelectionUI();
            //this.Enabled = false;
            tradeCardsForm.Show();
            //this.Enabled = true;  // need to appropriately disable main form
        }

        #endregion
    }
}
