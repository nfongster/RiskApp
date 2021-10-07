namespace RiskUI
{
    partial class MoveArmiesUI
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
            this.moveArmiesLabel = new System.Windows.Forms.Label();
            this.numArmiesSelector = new System.Windows.Forms.NumericUpDown();
            this.numArmiesMoveOKButton = new System.Windows.Forms.Button();
            this.numArmiesMoveCancelButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.numArmiesSelector)).BeginInit();
            this.SuspendLayout();
            // 
            // moveArmiesLabel
            // 
            this.moveArmiesLabel.AutoSize = true;
            this.moveArmiesLabel.Location = new System.Drawing.Point(33, 101);
            this.moveArmiesLabel.Name = "moveArmiesLabel";
            this.moveArmiesLabel.Size = new System.Drawing.Size(295, 32);
            this.moveArmiesLabel.TabIndex = 0;
            this.moveArmiesLabel.Text = "Numer of armies to move:";
            // 
            // numArmiesSelector
            // 
            this.numArmiesSelector.Location = new System.Drawing.Point(354, 99);
            this.numArmiesSelector.Name = "numArmiesSelector";
            this.numArmiesSelector.Size = new System.Drawing.Size(109, 39);
            this.numArmiesSelector.TabIndex = 1;
            this.numArmiesSelector.ValueChanged += new System.EventHandler(this.numArmiesSelector_ValueChanged);
            // 
            // numArmiesMoveOKButton
            // 
            this.numArmiesMoveOKButton.Location = new System.Drawing.Point(49, 190);
            this.numArmiesMoveOKButton.Name = "numArmiesMoveOKButton";
            this.numArmiesMoveOKButton.Size = new System.Drawing.Size(150, 46);
            this.numArmiesMoveOKButton.TabIndex = 2;
            this.numArmiesMoveOKButton.Text = "OK";
            this.numArmiesMoveOKButton.UseVisualStyleBackColor = true;
            this.numArmiesMoveOKButton.Click += new System.EventHandler(this.numArmiesMoveOKButton_Click);
            // 
            // numArmiesMoveCancelButton
            // 
            this.numArmiesMoveCancelButton.Location = new System.Drawing.Point(287, 190);
            this.numArmiesMoveCancelButton.Name = "numArmiesMoveCancelButton";
            this.numArmiesMoveCancelButton.Size = new System.Drawing.Size(150, 46);
            this.numArmiesMoveCancelButton.TabIndex = 2;
            this.numArmiesMoveCancelButton.Text = "Cancel";
            this.numArmiesMoveCancelButton.UseVisualStyleBackColor = true;
            this.numArmiesMoveCancelButton.Click += new System.EventHandler(this.numArmiesMoveCancelButton_Click);
            // 
            // MoveArmiesUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(13F, 32F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(497, 268);
            this.Controls.Add(this.numArmiesMoveCancelButton);
            this.Controls.Add(this.numArmiesMoveOKButton);
            this.Controls.Add(this.numArmiesSelector);
            this.Controls.Add(this.moveArmiesLabel);
            this.Name = "MoveArmiesUI";
            this.Text = "MoveArmiesUI";
            ((System.ComponentModel.ISupportInitialize)(this.numArmiesSelector)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label moveArmiesLabel;
        private System.Windows.Forms.NumericUpDown numArmiesSelector;
        private System.Windows.Forms.Button numArmiesMoveOKButton;
        private System.Windows.Forms.Button numArmiesMoveCancelButton;
    }
}