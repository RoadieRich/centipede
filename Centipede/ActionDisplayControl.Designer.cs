namespace Centipede
{
    partial class ActionDisplayControl
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
            this.NameLabel = new System.Windows.Forms.Label();
            this.ExpandButton = new System.Windows.Forms.Button();
            this.AttributeTable = new System.Windows.Forms.TableLayoutPanel();
            this.SuspendLayout();
            // 
            // NameLabel
            // 
            this.NameLabel.AutoSize = true;
            this.NameLabel.Location = new System.Drawing.Point(25, 5);
            this.NameLabel.Name = "NameLabel";
            this.NameLabel.Size = new System.Drawing.Size(35, 13);
            this.NameLabel.TabIndex = 0;
            this.NameLabel.Text = "label1";
            // 
            // ExpandButton
            // 
            this.ExpandButton.AutoSize = true;
            this.ExpandButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ExpandButton.Location = new System.Drawing.Point(-1, 0);
            this.ExpandButton.Margin = new System.Windows.Forms.Padding(0);
            this.ExpandButton.Name = "ExpandButton";
            this.ExpandButton.Size = new System.Drawing.Size(23, 23);
            this.ExpandButton.TabIndex = 1;
            this.ExpandButton.Text = "+";
            this.ExpandButton.UseVisualStyleBackColor = true;
            this.ExpandButton.Click += new System.EventHandler(this.ExpandButton_Click);
            // 
            // AttributeTable
            // 
            this.AttributeTable.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.AttributeTable.AutoSize = true;
            this.AttributeTable.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.AttributeTable.ColumnCount = 2;
            this.AttributeTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.AttributeTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.AttributeTable.Location = new System.Drawing.Point(-2, 27);
            this.AttributeTable.Name = "AttributeTable";
            this.AttributeTable.RowCount = 1;
            this.AttributeTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.AttributeTable.Size = new System.Drawing.Size(0, 20);
            this.AttributeTable.TabIndex = 2;
            this.AttributeTable.Visible = false;
            // 
            // ActionDisplayControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.Controls.Add(this.AttributeTable);
            this.Controls.Add(this.ExpandButton);
            this.Controls.Add(this.NameLabel);
            this.Name = "ActionDisplayControl";
            this.Size = new System.Drawing.Size(63, 49);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label NameLabel;
        private System.Windows.Forms.Button ExpandButton;
        private System.Windows.Forms.TableLayoutPanel AttributeTable;


    }
}
