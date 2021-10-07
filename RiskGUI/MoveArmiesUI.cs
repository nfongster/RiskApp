using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace RiskUI
{
    public partial class MoveArmiesUI : Form
    {
        public int numArmies;
        public MoveArmiesUI()
        {
            InitializeComponent();
            this.numArmies = (int)this.numArmiesSelector.Value;
        }

        private void numArmiesMoveOKButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void numArmiesMoveCancelButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void numArmiesSelector_ValueChanged(object sender, EventArgs e)
        {
            this.numArmies = (int)this.numArmiesSelector.Value;
        }
    }
}
