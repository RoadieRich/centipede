namespace Centipede
{
    partial class GetJob
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
            this.FavouritesListbox = new System.Windows.Forms.ListBox();
            this.LoadButton = new System.Windows.Forms.Button();
            this.BrowseButton = new System.Windows.Forms.Button();
            this.NewButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // FavouritesListbox
            // 
            this.FavouritesListbox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.FavouritesListbox.FormattingEnabled = true;
            this.FavouritesListbox.Location = new System.Drawing.Point(13, 13);
            this.FavouritesListbox.Name = "FavouritesListbox";
            this.FavouritesListbox.Size = new System.Drawing.Size(535, 342);
            this.FavouritesListbox.TabIndex = 0;
            // 
            // LoadButton
            // 
            this.LoadButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.LoadButton.DialogResult = System.Windows.Forms.DialogResult.Yes;
            this.LoadButton.Location = new System.Drawing.Point(12, 359);
            this.LoadButton.Name = "LoadButton";
            this.LoadButton.Size = new System.Drawing.Size(75, 23);
            this.LoadButton.TabIndex = 1;
            this.LoadButton.Text = "&Load";
            this.LoadButton.UseVisualStyleBackColor = true;
            // 
            // BrowseButton
            // 
            this.BrowseButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.BrowseButton.Enabled = false;
            this.BrowseButton.Location = new System.Drawing.Point(93, 359);
            this.BrowseButton.Name = "BrowseButton";
            this.BrowseButton.Size = new System.Drawing.Size(75, 23);
            this.BrowseButton.TabIndex = 2;
            this.BrowseButton.Text = "&Browse...";
            this.BrowseButton.UseVisualStyleBackColor = true;
            // 
            // NewButton
            // 
            this.NewButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.NewButton.DialogResult = System.Windows.Forms.DialogResult.No;
            this.NewButton.Location = new System.Drawing.Point(473, 359);
            this.NewButton.Name = "NewButton";
            this.NewButton.Size = new System.Drawing.Size(75, 23);
            this.NewButton.TabIndex = 3;
            this.NewButton.Text = "&New";
            this.NewButton.UseVisualStyleBackColor = true;
            // 
            // GetJob
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(560, 394);
            this.Controls.Add(this.NewButton);
            this.Controls.Add(this.BrowseButton);
            this.Controls.Add(this.LoadButton);
            this.Controls.Add(this.FavouritesListbox);
            this.Name = "GetJob";
            this.Text = "Centipede";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox FavouritesListbox;
        private System.Windows.Forms.Button LoadButton;
        private System.Windows.Forms.Button BrowseButton;
        private System.Windows.Forms.Button NewButton;
    }
}