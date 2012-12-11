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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GetJob));
            this.FavouritesListbox = new System.Windows.Forms.ListBox();
            this.LoadButton = new System.Windows.Forms.Button();
            this.BrowseButton = new System.Windows.Forms.Button();
            this.NewButton = new System.Windows.Forms.Button();
            this.BrowseLoadDialogue = new System.Windows.Forms.OpenFileDialog();
            this.OtherButton = new System.Windows.Forms.Button();
            this.OtherOpenDialogue = new System.Windows.Forms.OpenFileDialog();
            this.SuspendLayout();
            // 
            // FavouritesListbox
            // 
            resources.ApplyResources(this.FavouritesListbox, "FavouritesListbox");
            this.FavouritesListbox.DisplayMember = "Text";
            this.FavouritesListbox.Name = "FavouritesListbox";
            this.FavouritesListbox.SelectedIndexChanged += new System.EventHandler(this.FavouritesListbox_SelectedIndexChanged);
            // 
            // LoadButton
            // 
            resources.ApplyResources(this.LoadButton, "LoadButton");
            this.LoadButton.DialogResult = System.Windows.Forms.DialogResult.Yes;
            this.LoadButton.Name = "LoadButton";
            this.LoadButton.UseVisualStyleBackColor = true;
            this.LoadButton.Click += new System.EventHandler(this.LoadButton_Click);
            // 
            // BrowseButton
            // 
            resources.ApplyResources(this.BrowseButton, "BrowseButton");
            this.BrowseButton.Name = "BrowseButton";
            this.BrowseButton.UseVisualStyleBackColor = true;
            this.BrowseButton.Click += new System.EventHandler(this.BrowseButton_Click);
            // 
            // NewButton
            // 
            resources.ApplyResources(this.NewButton, "NewButton");
            this.NewButton.Name = "NewButton";
            this.NewButton.UseVisualStyleBackColor = true;
            this.NewButton.Click += new System.EventHandler(this.NewButton_Click);
            // 
            // BrowseLoadDialogue
            // 
            this.BrowseLoadDialogue.DefaultExt = "100p";
            resources.ApplyResources(this.BrowseLoadDialogue, "BrowseLoadDialogue");
            this.BrowseLoadDialogue.FileOk += new System.ComponentModel.CancelEventHandler(this.BrowseLoadDialogue_FileOk);
            // 
            // OtherButton
            // 
            resources.ApplyResources(this.OtherButton, "OtherButton");
            this.OtherButton.Name = "OtherButton";
            this.OtherButton.UseVisualStyleBackColor = true;
            this.OtherButton.Click += new System.EventHandler(this.OtherButton_Click);
            // 
            // OtherOpenDialogue
            // 
            this.OtherOpenDialogue.DefaultExt = "100p";
            resources.ApplyResources(this.OtherOpenDialogue, "OtherOpenDialogue");
            this.OtherOpenDialogue.FileOk += new System.ComponentModel.CancelEventHandler(this.OtherOpenDialogue_FileOk);
            // 
            // GetJob
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.NewButton);
            this.Controls.Add(this.OtherButton);
            this.Controls.Add(this.BrowseButton);
            this.Controls.Add(this.LoadButton);
            this.Controls.Add(this.FavouritesListbox);
            this.Name = "GetJob";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.GetJob_FormClosing);
            this.Load += new System.EventHandler(this.GetJob_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox FavouritesListbox;
        private System.Windows.Forms.Button LoadButton;
        private System.Windows.Forms.Button BrowseButton;
        private System.Windows.Forms.Button NewButton;
        private System.Windows.Forms.OpenFileDialog BrowseLoadDialogue;
        private System.Windows.Forms.Button OtherButton;
        private System.Windows.Forms.OpenFileDialog OtherOpenDialogue;
    }
}