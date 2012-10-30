namespace Centipede
{
    partial class VariableDisplayControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.VarNameLabel = new System.Windows.Forms.Label();
            this.VarValueTextBox = new System.Windows.Forms.TextBox();
            this.VarTypeLabel = new System.Windows.Forms.Label();
            this.VarUnitComboBox = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // VarNameLabel
            // 
            this.VarNameLabel.AutoSize = true;
            this.VarNameLabel.Location = new System.Drawing.Point(3, 6);
            this.VarNameLabel.Name = "VarNameLabel";
            this.VarNameLabel.Size = new System.Drawing.Size(39, 13);
            this.VarNameLabel.TabIndex = 0;
            this.VarNameLabel.Text = "(name)";
            // 
            // VarValueTextBox
            // 
            this.VarValueTextBox.Location = new System.Drawing.Point(91, 4);
            this.VarValueTextBox.Name = "VarValueTextBox";
            this.VarValueTextBox.Size = new System.Drawing.Size(100, 20);
            this.VarValueTextBox.TabIndex = 1;
            // 
            // VarTypeLabel
            // 
            this.VarTypeLabel.AutoSize = true;
            this.VarTypeLabel.Location = new System.Drawing.Point(48, 6);
            this.VarTypeLabel.Name = "VarTypeLabel";
            this.VarTypeLabel.Size = new System.Drawing.Size(33, 13);
            this.VarTypeLabel.TabIndex = 0;
            this.VarTypeLabel.Text = "(type)";
            // 
            // VarUnitComboBox
            // 
            this.VarUnitComboBox.FormattingEnabled = true;
            this.VarUnitComboBox.Location = new System.Drawing.Point(197, 3);
            this.VarUnitComboBox.Name = "VarUnitComboBox";
            this.VarUnitComboBox.Size = new System.Drawing.Size(121, 21);
            this.VarUnitComboBox.TabIndex = 2;
            // 
            // VariableDisplayControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.Controls.Add(this.VarUnitComboBox);
            this.Controls.Add(this.VarValueTextBox);
            this.Controls.Add(this.VarTypeLabel);
            this.Controls.Add(this.VarNameLabel);
            this.Name = "VariableDisplayControl";
            this.Size = new System.Drawing.Size(321, 27);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label VarNameLabel;
        private System.Windows.Forms.TextBox VarValueTextBox;
        private System.Windows.Forms.Label VarTypeLabel;
        private System.Windows.Forms.ComboBox VarUnitComboBox;
    }
}
