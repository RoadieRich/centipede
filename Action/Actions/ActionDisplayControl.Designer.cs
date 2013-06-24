using System.Windows.Forms;

// 24 June 2013 Fixed the strange adc growing issues, by placing controls onto panels instead of directly on the form.
// but broke the python adc :-(

namespace Centipede.Actions
{
    public partial class ActionDisplayControl
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
                ThisAction.Dispose();
            }
            base.Dispose(disposing);
            
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        protected void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ActionDisplayControl));
            this.StatusIcons = new System.Windows.Forms.ImageList(this.components);
            this.ActionContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ActMenuDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.StatusTooltip = new System.Windows.Forms.ToolTip(this.components);
            this.ArgumentTooltips = new System.Windows.Forms.ToolTip(this.components);
            this.panel1 = new System.Windows.Forms.Panel();
            this.NameLabel = new System.Windows.Forms.Label();
            this.ActionIcon = new System.Windows.Forms.PictureBox();
            this.ExpandButton = new System.Windows.Forms.Button();
            this.CommentTextBox = new System.Windows.Forms.TextBox();
            this.StatusIconBox = new System.Windows.Forms.PictureBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.AttributeTable = new System.Windows.Forms.TableLayoutPanel();
            this.ActionContextMenu.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ActionIcon)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.StatusIconBox)).BeginInit();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // StatusIcons
            // 
            this.StatusIcons.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("StatusIcons.ImageStream")));
            this.StatusIcons.TransparentColor = System.Drawing.Color.Transparent;
            this.StatusIcons.Images.SetKeyName(0, "Run.png");
            this.StatusIcons.Images.SetKeyName(1, "OK.png");
            this.StatusIcons.Images.SetKeyName(2, "Error.png");
            // 
            // ActionContextMenu
            // 
            this.ActionContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ActMenuDelete});
            this.ActionContextMenu.Name = "ActionContextMenu";
            this.ActionContextMenu.ShowImageMargin = false;
            this.ActionContextMenu.Size = new System.Drawing.Size(128, 48);
            // 
            // ActMenuDelete
            // 
            this.ActMenuDelete.Name = "ActMenuDelete";
            this.ActMenuDelete.ShortcutKeyDisplayString = "";
            this.ActMenuDelete.Size = new System.Drawing.Size(142, 22);
            this.ActMenuDelete.Text = "&Delete";
            this.ActMenuDelete.Click += new System.EventHandler(this.ActMenuDelete_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.StatusIconBox);
            this.panel1.Controls.Add(this.CommentTextBox);
            this.panel1.Controls.Add(this.ExpandButton);
            this.panel1.Controls.Add(this.ActionIcon);
            this.panel1.Controls.Add(this.NameLabel);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(561, 56);
            this.panel1.TabIndex = 6;
            // 
            // NameLabel
            // 
            this.NameLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.NameLabel.AutoEllipsis = true;
            this.NameLabel.BackColor = System.Drawing.Color.Transparent;
            this.NameLabel.Location = new System.Drawing.Point(30, 7);
            this.NameLabel.Name = "NameLabel";
            this.NameLabel.Size = new System.Drawing.Size(474, 16);
            this.NameLabel.TabIndex = 1;
            this.NameLabel.Text = "<NAME>";
            // 
            // ActionIcon
            // 
            this.ActionIcon.Location = new System.Drawing.Point(4, 3);
            this.ActionIcon.Name = "ActionIcon";
            this.ActionIcon.Size = new System.Drawing.Size(20, 20);
            this.ActionIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.ActionIcon.TabIndex = 6;
            this.ActionIcon.TabStop = false;
            // 
            // ExpandButton
            // 
            this.ExpandButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ExpandButton.Location = new System.Drawing.Point(4, 26);
            this.ExpandButton.Margin = new System.Windows.Forms.Padding(0);
            this.ExpandButton.Name = "ExpandButton";
            this.ExpandButton.Size = new System.Drawing.Size(20, 20);
            this.ExpandButton.TabIndex = 7;
            this.ExpandButton.Text = "+";
            this.ExpandButton.UseVisualStyleBackColor = true;
            this.ExpandButton.Click += new System.EventHandler(this.ExpandButton_Click);
            // 
            // CommentTextBox
            // 
            this.CommentTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.CommentTextBox.Location = new System.Drawing.Point(33, 27);
            this.CommentTextBox.Name = "CommentTextBox";
            this.CommentTextBox.Size = new System.Drawing.Size(471, 20);
            this.CommentTextBox.TabIndex = 8;
            this.CommentTextBox.WordWrap = false;
            // 
            // StatusIconBox
            // 
            this.StatusIconBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.StatusIconBox.BackColor = System.Drawing.Color.Transparent;
            this.StatusIconBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.StatusIconBox.Location = new System.Drawing.Point(510, 3);
            this.StatusIconBox.Name = "StatusIconBox";
            this.StatusIconBox.Size = new System.Drawing.Size(48, 48);
            this.StatusIconBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.StatusIconBox.TabIndex = 9;
            this.StatusIconBox.TabStop = false;
            // 
            // panel2
            // 
            this.panel2.AutoSize = true;
            this.panel2.Controls.Add(this.AttributeTable);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 56);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(561, 10);
            this.panel2.TabIndex = 7;
            // 
            // AttributeTable
            // 
            this.AttributeTable.AutoSize = true;
            this.AttributeTable.BackColor = System.Drawing.Color.Transparent;
            this.AttributeTable.ColumnCount = 2;
            this.AttributeTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.AttributeTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.AttributeTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.AttributeTable.Location = new System.Drawing.Point(0, 0);
            this.AttributeTable.MinimumSize = new System.Drawing.Size(10, 10);
            this.AttributeTable.Name = "AttributeTable";
            this.AttributeTable.RowCount = 1;
            this.AttributeTable.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.AttributeTable.Size = new System.Drawing.Size(561, 10);
            this.AttributeTable.TabIndex = 3;
            this.AttributeTable.Visible = false;
            // 
            // ActionDisplayControl
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.ContextMenuStrip = this.ActionContextMenu;
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "ActionDisplayControl";
            this.Size = new System.Drawing.Size(561, 66);
            this.Load += new System.EventHandler(this.ActionDisplayControl_Load);
            this.ActionContextMenu.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ActionIcon)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.StatusIconBox)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip ActionContextMenu;
        private System.Windows.Forms.ToolStripMenuItem ActMenuDelete;
        private System.Windows.Forms.ToolTip StatusTooltip;
        private System.Windows.Forms.ToolTip ArgumentTooltips;
        private ImageList StatusIcons;
        private Panel panel1;
        private PictureBox StatusIconBox;
        private TextBox CommentTextBox;
        private Button ExpandButton;
        private PictureBox ActionIcon;
        protected Label NameLabel;
        private Panel panel2;
        protected TableLayoutPanel AttributeTable;
    }
}
