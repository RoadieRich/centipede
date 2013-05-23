namespace Centipede
{
    partial class EditFavourites
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EditFavourites));
            this.MoveUpButton = new System.Windows.Forms.Button();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.AddButton = new System.Windows.Forms.Button();
            this.RemoveButton = new System.Windows.Forms.Button();
            this.MoveDownButton = new System.Windows.Forms.Button();
            this.OKBtn = new System.Windows.Forms.Button();
            this.CancelBtn = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.FavouriteJobsGridView = new System.Windows.Forms.DataGridView();
            this.favouritesBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.favouriteJobs = new Centipede.FavouriteJobs();
            ((System.ComponentModel.ISupportInitialize)(this.FavouriteJobsGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.favouritesBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.favouriteJobs)).BeginInit();
            this.SuspendLayout();
            // 
            // MoveUpButton
            // 
            this.MoveUpButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.MoveUpButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.MoveUpButton.Enabled = false;
            this.MoveUpButton.ImageIndex = 0;
            this.MoveUpButton.ImageList = this.imageList1;
            this.MoveUpButton.Location = new System.Drawing.Point(304, 12);
            this.MoveUpButton.Name = "MoveUpButton";
            this.MoveUpButton.Size = new System.Drawing.Size(43, 42);
            this.MoveUpButton.TabIndex = 1;
            this.MoveUpButton.UseVisualStyleBackColor = true;
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Magenta;
            this.imageList1.Images.SetKeyName(0, "BuilderDialog_moveup.bmp");
            this.imageList1.Images.SetKeyName(1, "BuilderDialog_add.bmp");
            this.imageList1.Images.SetKeyName(2, "BuilderDialog_delete.bmp");
            this.imageList1.Images.SetKeyName(3, "BuilderDialog_movedown.bmp");
            this.imageList1.Images.SetKeyName(4, "base_plus_sign_32.png");
            // 
            // AddButton
            // 
            this.AddButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.AddButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AddButton.ImageList = this.imageList1;
            this.AddButton.Location = new System.Drawing.Point(304, 60);
            this.AddButton.Name = "AddButton";
            this.AddButton.Size = new System.Drawing.Size(43, 42);
            this.AddButton.TabIndex = 1;
            this.AddButton.Text = "+";
            this.AddButton.UseVisualStyleBackColor = true;
            this.AddButton.Click += new System.EventHandler(this.AddButton_Click);
            // 
            // RemoveButton
            // 
            this.RemoveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.RemoveButton.ImageIndex = 2;
            this.RemoveButton.ImageList = this.imageList1;
            this.RemoveButton.Location = new System.Drawing.Point(304, 108);
            this.RemoveButton.Name = "RemoveButton";
            this.RemoveButton.Size = new System.Drawing.Size(43, 42);
            this.RemoveButton.TabIndex = 1;
            this.RemoveButton.UseVisualStyleBackColor = true;
            this.RemoveButton.Click += new System.EventHandler(this.RemoveButton_Click);
            // 
            // MoveDownButton
            // 
            this.MoveDownButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.MoveDownButton.Enabled = false;
            this.MoveDownButton.ImageIndex = 3;
            this.MoveDownButton.ImageList = this.imageList1;
            this.MoveDownButton.Location = new System.Drawing.Point(304, 156);
            this.MoveDownButton.Name = "MoveDownButton";
            this.MoveDownButton.Size = new System.Drawing.Size(43, 42);
            this.MoveDownButton.TabIndex = 1;
            this.MoveDownButton.UseVisualStyleBackColor = true;
            // 
            // OKBtn
            // 
            this.OKBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.OKBtn.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.OKBtn.Location = new System.Drawing.Point(191, 304);
            this.OKBtn.Name = "OKBtn";
            this.OKBtn.Size = new System.Drawing.Size(75, 23);
            this.OKBtn.TabIndex = 2;
            this.OKBtn.Text = "Ok";
            this.OKBtn.UseVisualStyleBackColor = true;
            // 
            // CancelBtn
            // 
            this.CancelBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.CancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CancelBtn.Location = new System.Drawing.Point(272, 304);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(75, 23);
            this.CancelBtn.TabIndex = 3;
            this.CancelBtn.Text = "Cancel";
            this.CancelBtn.UseVisualStyleBackColor = true;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.DefaultExt = "100p";
            this.openFileDialog1.FileName = "openFileDialog1";
            this.openFileDialog1.Filter = "*.100p|*.100p";
            this.openFileDialog1.Multiselect = true;
            // 
            // FavouriteJobsGridView
            // 
            this.FavouriteJobsGridView.AllowUserToAddRows = false;
            this.FavouriteJobsGridView.AllowUserToDeleteRows = false;
            this.FavouriteJobsGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.FavouriteJobsGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.FavouriteJobsGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.FavouriteJobsGridView.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.FavouriteJobsGridView.Location = new System.Drawing.Point(13, 13);
            this.FavouriteJobsGridView.Name = "FavouriteJobsGridView";
            this.FavouriteJobsGridView.ReadOnly = true;
            this.FavouriteJobsGridView.RowHeadersVisible = false;
            this.FavouriteJobsGridView.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.FavouriteJobsGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.FavouriteJobsGridView.ShowEditingIcon = false;
            this.FavouriteJobsGridView.Size = new System.Drawing.Size(285, 285);
            this.FavouriteJobsGridView.TabIndex = 4;
            // 
            // favouritesBindingSource
            // 
            this.favouritesBindingSource.DataMember = "Favourites";
            // 
            // favouriteJobs
            // 
            this.favouriteJobs.DataSetName = "FavouriteJobs";
            this.favouriteJobs.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // EditFavourites
            // 
            this.AcceptButton = this.OKBtn;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.CancelBtn;
            this.ClientSize = new System.Drawing.Size(359, 339);
            this.Controls.Add(this.FavouriteJobsGridView);
            this.Controls.Add(this.CancelBtn);
            this.Controls.Add(this.OKBtn);
            this.Controls.Add(this.MoveDownButton);
            this.Controls.Add(this.RemoveButton);
            this.Controls.Add(this.AddButton);
            this.Controls.Add(this.MoveUpButton);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "EditFavourites";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "EditFavourites";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.EditFavourites_FormClosed);
            this.Load += new System.EventHandler(this.EditFavourites_Load);
            ((System.ComponentModel.ISupportInitialize)(this.FavouriteJobsGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.favouritesBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.favouriteJobs)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button MoveUpButton;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.Button AddButton;
        private System.Windows.Forms.Button RemoveButton;
        private System.Windows.Forms.Button MoveDownButton;
        private System.Windows.Forms.Button OKBtn;
        private System.Windows.Forms.Button CancelBtn;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.DataGridView FavouriteJobsGridView;
        private System.Windows.Forms.BindingSource favouritesBindingSource;
        private FavouriteJobs favouriteJobs;
    }
}