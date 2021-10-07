using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace RiskUI
{
    public partial class DiceSelectionUI : Form
    {
        public int numWhiteDice;
        public int numRedDice;
        public DiceSelectionUI()
        {
            InitializeComponent();

            // TODO: change max values according to num armies
            this.redDiceNumeric.Minimum = 1;
            this.redDiceNumeric.Maximum = 3;
            this.whiteDiceNumeric.Minimum = 1;
            this.whiteDiceNumeric.Maximum = 2;
        }

        private void diceselectOKButton_Click(object sender, EventArgs e)
        {
            // Do something
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void diceselectCancelButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void redDiceNumeric_ValueChanged(object sender, EventArgs e)
        {
            this.numRedDice = (int)redDiceNumeric.Value;
        }

        private void whiteDiceNumeric_ValueChanged(object sender, EventArgs e)
        {
            this.numWhiteDice = (int)whiteDiceNumeric.Value;
        }
    }
}
