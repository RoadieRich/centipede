namespace Centipede
{
    partial class JobPropertyForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.InfoUrlTextbox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.AuthorTextbox = new System.Windows.Forms.TextBox();
            this.ContactTextbox = new System.Windows.Forms.TextBox();
            this.BrowseButton = new System.Windows.Forms.Button();
            this.OKBtn = new System.Windows.Forms.Button();
            this.CancelBtn = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.JobNameTextbox = new System.Windows.Forms.TextBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.SetDefaultButton = new System.Windows.Forms.Button();
            this.EmailLink = new System.Windows.Forms.LinkLabel();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 35);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Info Url";
            // 
            // InfoUrlTextbox
            // 
            this.InfoUrlTextbox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.InfoUrlTextbox.Location = new System.Drawing.Point(73, 32);
            this.InfoUrlTextbox.Name = "InfoUrlTextbox";
            this.InfoUrlTextbox.Size = new System.Drawing.Size(301, 20);
            this.InfoUrlTextbox.TabIndex = 1;
            this.InfoUrlTextbox.TextChanged += new System.EventHandler(this.TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 61);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(38, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Author";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 87);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(44, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Contact";
            // 
            // AuthorTextbox
            // 
            this.AuthorTextbox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.AuthorTextbox.Location = new System.Drawing.Point(73, 58);
            this.AuthorTextbox.Name = "AuthorTextbox";
            this.AuthorTextbox.Size = new System.Drawing.Size(382, 20);
            this.AuthorTextbox.TabIndex = 2;
            this.AuthorTextbox.TextChanged += new System.EventHandler(this.TextChanged);
            // 
            // ContactTextbox
            // 
            this.ContactTextbox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ContactTextbox.Location = new System.Drawing.Point(73, 84);
            this.ContactTextbox.Name = "ContactTextbox";
            this.ContactTextbox.Size = new System.Drawing.Size(301, 20);
            this.ContactTextbox.TabIndex = 3;
            this.ContactTextbox.TextChanged += new System.EventHandler(this.TextChanged);
            // 
            // BrowseButton
            // 
            this.BrowseButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BrowseButton.Location = new System.Drawing.Point(380, 30);
            this.BrowseButton.Name = "BrowseButton";
            this.BrowseButton.Size = new System.Drawing.Size(75, 23);
            this.BrowseButton.TabIndex = 6;
            this.BrowseButton.Text = "Browse...";
            this.BrowseButton.UseVisualStyleBackColor = true;
            this.BrowseButton.Click += new System.EventHandler(this.BrowseButton_Click);
            // 
            // OKBtn
            // 
            this.OKBtn.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.OKBtn.Location = new System.Drawing.Point(155, 117);
            this.OKBtn.Name = "OKBtn";
            this.OKBtn.Size = new System.Drawing.Size(75, 23);
            this.OKBtn.TabIndex = 4;
            this.OKBtn.Text = "OK";
            this.OKBtn.UseVisualStyleBackColor = true;
            this.OKBtn.Click += new System.EventHandler(this.OKBtn_Click);
            // 
            // CancelBtn
            // 
            this.CancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CancelBtn.Location = new System.Drawing.Point(236, 117);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(75, 23);
            this.CancelBtn.TabIndex = 5;
            this.CancelBtn.Text = "Cancel";
            this.CancelBtn.UseVisualStyleBackColor = true;
            this.CancelBtn.Click += new System.EventHandler(this.CancelBtn_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 9);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(55, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "Job Name";
            // 
            // JobNameTextbox
            // 
            this.JobNameTextbox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.JobNameTextbox.Location = new System.Drawing.Point(73, 6);
            this.JobNameTextbox.Name = "JobNameTextbox";
            this.JobNameTextbox.Size = new System.Drawing.Size(382, 20);
            this.JobNameTextbox.TabIndex = 0;
            this.JobNameTextbox.TextChanged += new System.EventHandler(this.TextChanged);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.DefaultExt = "htm";
            this.openFileDialog1.Filter = "HTML (*.htm; *.html)|*.htm;*.html|All Files|*.*";
            this.openFileDialog1.FileOk += new System.ComponentModel.CancelEventHandler(this.openFileDialog1_FileOk);
            // 
            // SetDefaultButton
            // 
            this.SetDefaultButton.Location = new System.Drawing.Point(379, 117);
            this.SetDefaultButton.Name = "SetDefaultButton";
            this.SetDefaultButton.Size = new System.Drawing.Size(75, 23);
            this.SetDefaultButton.TabIndex = 7;
            this.SetDefaultButton.Text = "Set Defaults";
            this.SetDefaultButton.UseVisualStyleBackColor = true;
            this.SetDefaultButton.Click += new System.EventHandler(this.button1_Click);
            // 
            // EmailLink
            // 
            this.EmailLink.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.EmailLink.AutoSize = true;
            this.EmailLink.Location = new System.Drawing.Point(381, 86);
            this.EmailLink.Name = "EmailLink";
            this.EmailLink.Size = new System.Drawing.Size(32, 13);
            this.EmailLink.TabIndex = 10;
            this.EmailLink.TabStop = true;
            this.EmailLink.Text = "Email";
            this.EmailLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.EmailLink_LinkClicked);
            // 
            // JobPropertyForm
            // 
            this.AcceptButton = this.OKBtn;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.CancelBtn;
            this.ClientSize = new System.Drawing.Size(467, 152);
            this.Controls.Add(this.EmailLink);
            this.Controls.Add(this.SetDefaultButton);
            this.Controls.Add(this.CancelBtn);
            this.Controls.Add(this.OKBtn);
            this.Controls.Add(this.BrowseButton);
            this.Controls.Add(this.JobNameTextbox);
            this.Controls.Add(this.ContactTextbox);
            this.Controls.Add(this.AuthorTextbox);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.InfoUrlTextbox);
            this.Controls.Add(this.label1);
            this.Name = "JobPropertyForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Properties";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox InfoUrlTextbox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox AuthorTextbox;
        private System.Windows.Forms.TextBox ContactTextbox;
        private System.Windows.Forms.Button BrowseButton;
        private System.Windows.Forms.Button OKBtn;
        private System.Windows.Forms.Button CancelBtn;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox JobNameTextbox;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button SetDefaultButton;
        private System.Windows.Forms.LinkLabel EmailLink;
    }
}