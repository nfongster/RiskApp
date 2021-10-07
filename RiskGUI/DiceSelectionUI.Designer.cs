namespace RiskUI
{
    partial class DiceSelectionUI
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.diceChoiceLabel = new System.Windows.Forms.Label();
            this.redDiceLabel = new System.Windows.Forms.Label();
            this.whiteDiceLabel = new System.Windows.Forms.Label();
            this.redDiceNumeric = new System.Windows.Forms.NumericUpDown();
            this.whiteDiceNumeric = new System.Windows.Forms.NumericUpDown();
            this.diceselectOKButton = new System.Windows.Forms.Button();
            this.diceselectCancelButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.redDiceNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.whiteDiceNumeric)).BeginInit();
            this.SuspendLayout();
            // 
            // diceChoiceLabel
            // 
            this.diceChoiceLabel.AutoSize = true;
            this.diceChoiceLabel.Location = new System.Drawing.Point(53, 40);
            this.diceChoiceLabel.Name = "diceChoiceLabel";
            this.diceChoiceLabel.Size = new System.Drawing.Size(265, 32);
            this.diceChoiceLabel.TabIndex = 0;
            this.diceChoiceLabel.Text = "Choose number of dice";
            // 
            // redDiceLabel
            // 
            this.redDiceLabel.AutoSize = true;
            this.redDiceLabel.Location = new System.Drawing.Point(53, 132);
            this.redDiceLabel.Name = "redDiceLabel";
            this.redDiceLabel.Size = new System.Drawing.Size(213, 32);
            this.redDiceLabel.TabIndex = 1;
            this.redDiceLabel.Text = "Attacker (Red dice)";
            // 
            // whiteDiceLabel
            // 
            this.whiteDiceLabel.AutoSize = true;
            this.whiteDiceLabel.Location = new System.Drawing.Point(461, 132);
            this.whiteDiceLabel.Name = "whiteDiceLabel";
            this.whiteDiceLabel.Size = new System.Drawing.Size(249, 32);
            this.whiteDiceLabel.TabIndex = 1;
            this.whiteDiceLabel.Text = "Defender (White dice)";
            // 
            // redDiceNumeric
            // 
            this.redDiceNumeric.Location = new System.Drawing.Point(53, 198);
            this.redDiceNumeric.Name = "redDiceNumeric";
            this.redDiceNumeric.Size = new System.Drawing.Size(108, 39);
            this.redDiceNumeric.TabIndex = 2;
            this.redDiceNumeric.ValueChanged += new System.EventHandler(this.redDiceNumeric_ValueChanged);
            // 
            // whiteDiceNumeric
            // 
            this.whiteDiceNumeric.Location = new System.Drawing.Point(461, 198);
            this.whiteDiceNumeric.Name = "whiteDiceNumeric";
            this.whiteDiceNumeric.Size = new System.Drawing.Size(108, 39);
            this.whiteDiceNumeric.TabIndex = 2;
            this.whiteDiceNumeric.ValueChanged += new System.EventHandler(this.whiteDiceNumeric_ValueChanged);
            // 
            // diceselectOKButton
            // 
            this.diceselectOKButton.Location = new System.Drawing.Point(62, 292);
            this.diceselectOKButton.Name = "diceselectOKButton";
            this.diceselectOKButton.Size = new System.Drawing.Size(150, 46);
            this.diceselectOKButton.TabIndex = 3;
            this.diceselectOKButton.Text = "OK";
            this.diceselectOKButton.UseVisualStyleBackColor = true;
            this.diceselectOKButton.Click += new System.EventHandler(this.diceselectOKButton_Click);
            // 
            // diceselectCancelButton
            // 
            this.diceselectCancelButton.Location = new System.Drawing.Point(461, 292);
            this.diceselectCancelButton.Name = "diceselectCancelButton";
            this.diceselectCancelButton.Size = new System.Drawing.Size(150, 46);
            this.diceselectCancelButton.TabIndex = 3;
            this.diceselectCancelButton.Text = "Cancel";
            this.diceselectCancelButton.UseVisualStyleBackColor = true;
            this.diceselectCancelButton.Click += new System.EventHandler(this.diceselectCancelButton_Click);
            // 
            // DiceSelectionUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(13F, 32F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(803, 359);
            this.Controls.Add(this.diceselectCancelButton);
            this.Controls.Add(this.diceselectOKButton);
            this.Controls.Add(this.whiteDiceNumeric);
            this.Controls.Add(this.redDiceNumeric);
            this.Controls.Add(this.whiteDiceLabel);
            this.Controls.Add(this.redDiceLabel);
            this.Controls.Add(this.diceChoiceLabel);
            this.Name = "DiceSelectionUI";
            this.Text = "DiceSelectionUI";
            ((System.ComponentModel.ISupportInitialize)(this.redDiceNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.whiteDiceNumeric)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label diceChoiceLabel;
        private System.Windows.Forms.Label redDiceLabel;
        private System.Windows.Forms.Label whiteDiceLabel;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.NumericUpDown numericUpDown2;
        //private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button diceselectOKButton;
        private System.Windows.Forms.NumericUpDown whiteDiceNumeric;
        private System.Windows.Forms.NumericUpDown redDiceNumeric;
        private System.Windows.Forms.Button diceselectCancelButton;
    }
}