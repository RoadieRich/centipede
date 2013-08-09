using System.Windows.Forms;


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
            this.NameLabel = new System.Windows.Forms.Label();
            this.ExpandButton = new System.Windows.Forms.Button();
            this.AttributeTable = new System.Windows.Forms.TableLayoutPanel();
            this.StatusIcons = new System.Windows.Forms.ImageList(this.components);
            this.StatusIconBox = new System.Windows.Forms.PictureBox();
            this.ActionContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ActMenuDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.CommentTextBox = new System.Windows.Forms.TextBox();
            this.StatusTooltip = new System.Windows.Forms.ToolTip(this.components);
            this.ArgumentTooltips = new System.Windows.Forms.ToolTip(this.components);
            this.ActionIcon = new System.Windows.Forms.PictureBox();
            this.TopPanel = new System.Windows.Forms.Panel();
            this.BottomPanel = new System.Windows.Forms.Panel();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.moveUpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.moveDownToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.StatusIconBox)).BeginInit();
            this.ActionContextMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ActionIcon)).BeginInit();
            this.TopPanel.SuspendLayout();
            this.BottomPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // NameLabel
            // 
            this.NameLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.NameLabel.AutoEllipsis = true;
            this.NameLabel.BackColor = System.Drawing.Color.Transparent;
            this.NameLabel.Location = new System.Drawing.Point(24, 3);
            this.NameLabel.Name = "NameLabel";
            this.NameLabel.Size = new System.Drawing.Size(169, 20);
            this.NameLabel.TabIndex = 0;
            this.NameLabel.Text = "<NAME>";
            // 
            // ExpandButton
            // 
            this.ExpandButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ExpandButton.Location = new System.Drawing.Point(4, 26);
            this.ExpandButton.Margin = new System.Windows.Forms.Padding(0);
            this.ExpandButton.Name = "ExpandButton";
            this.ExpandButton.Size = new System.Drawing.Size(20, 20);
            this.ExpandButton.TabIndex = 1;
            this.ExpandButton.Text = "+";
            this.ExpandButton.UseVisualStyleBackColor = true;
            this.ExpandButton.Click += new System.EventHandler(this.ExpandButton_Click);
            // 
            // AttributeTable
            // 
            this.AttributeTable.AutoSize = true;
            this.AttributeTable.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
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
            this.AttributeTable.Size = new System.Drawing.Size(250, 24);
            this.AttributeTable.TabIndex = 2;
            this.AttributeTable.Visible = false;
            // 
            // StatusIcons
            // 
            this.StatusIcons.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("StatusIcons.ImageStream")));
            this.StatusIcons.TransparentColor = System.Drawing.Color.Transparent;
            this.StatusIcons.Images.SetKeyName(0, "Run.png");
            this.StatusIcons.Images.SetKeyName(1, "OK.png");
            this.StatusIcons.Images.SetKeyName(2, "Error.png");
            // 
            // StatusIconBox
            // 
            this.StatusIconBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.StatusIconBox.BackColor = System.Drawing.Color.Transparent;
            this.StatusIconBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.StatusIconBox.Location = new System.Drawing.Point(199, 3);
            this.StatusIconBox.Name = "StatusIconBox";
            this.StatusIconBox.Size = new System.Drawing.Size(48, 48);
            this.StatusIconBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.StatusIconBox.TabIndex = 3;
            this.StatusIconBox.TabStop = false;
            // 
            // ActionContextMenu
            // 
            this.ActionContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ActMenuDelete,
            this.toolStripSeparator1,
            this.moveUpToolStripMenuItem,
            this.moveDownToolStripMenuItem});
            this.ActionContextMenu.Name = "ActionContextMenu";
            this.ActionContextMenu.ShowImageMargin = false;
            this.ActionContextMenu.Size = new System.Drawing.Size(128, 98);
            // 
            // ActMenuDelete
            // 
            this.ActMenuDelete.Name = "ActMenuDelete";
            this.ActMenuDelete.ShortcutKeyDisplayString = "";
            this.ActMenuDelete.Size = new System.Drawing.Size(127, 22);
            this.ActMenuDelete.Text = "&Delete";
            this.ActMenuDelete.Click += new System.EventHandler(this.ActMenuDelete_Click);
            // 
            // CommentTextBox
            // 
            this.CommentTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.CommentTextBox.Location = new System.Drawing.Point(27, 27);
            this.CommentTextBox.Name = "CommentTextBox";
            this.CommentTextBox.Size = new System.Drawing.Size(166, 20);
            this.CommentTextBox.TabIndex = 4;
            this.CommentTextBox.WordWrap = false;
            this.CommentTextBox.TextChanged += new System.EventHandler(this.CommentTextBox_TextChanged);
            // 
            // ActionIcon
            // 
            this.ActionIcon.Location = new System.Drawing.Point(4, 3);
            this.ActionIcon.Name = "ActionIcon";
            this.ActionIcon.Size = new System.Drawing.Size(20, 20);
            this.ActionIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.ActionIcon.TabIndex = 5;
            this.ActionIcon.TabStop = false;
            // 
            // TopPanel
            // 
            this.TopPanel.AutoSize = true;
            this.TopPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.TopPanel.Controls.Add(this.CommentTextBox);
            this.TopPanel.Controls.Add(this.StatusIconBox);
            this.TopPanel.Controls.Add(this.ExpandButton);
            this.TopPanel.Controls.Add(this.NameLabel);
            this.TopPanel.Controls.Add(this.ActionIcon);
            this.TopPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.TopPanel.Location = new System.Drawing.Point(0, 3);
            this.TopPanel.Name = "TopPanel";
            this.TopPanel.Size = new System.Drawing.Size(250, 54);
            this.TopPanel.TabIndex = 7;
            // 
            // BottomPanel
            // 
            this.BottomPanel.AutoSize = true;
            this.BottomPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BottomPanel.Controls.Add(this.AttributeTable);
            this.BottomPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BottomPanel.Location = new System.Drawing.Point(0, 57);
            this.BottomPanel.Name = "BottomPanel";
            this.BottomPanel.Size = new System.Drawing.Size(250, 24);
            this.BottomPanel.TabIndex = 0;
            // 
            // splitter1
            // 
            this.splitter1.Dock = System.Windows.Forms.DockStyle.Top;
            this.splitter1.Location = new System.Drawing.Point(0, 0);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(250, 3);
            this.splitter1.TabIndex = 8;
            this.splitter1.TabStop = false;
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(124, 6);
            // 
            // moveUpToolStripMenuItem
            // 
            this.moveUpToolStripMenuItem.Name = "moveUpToolStripMenuItem";
            this.moveUpToolStripMenuItem.Size = new System.Drawing.Size(127, 22);
            this.moveUpToolStripMenuItem.Text = "Move Up";
            this.moveUpToolStripMenuItem.Click += new System.EventHandler(this.moveUpToolStripMenuItem_Click);
            // 
            // moveDownToolStripMenuItem
            // 
            this.moveDownToolStripMenuItem.Name = "moveDownToolStripMenuItem";
            this.moveDownToolStripMenuItem.Size = new System.Drawing.Size(127, 22);
            this.moveDownToolStripMenuItem.Text = "Move Down";
            this.moveDownToolStripMenuItem.Click += new System.EventHandler(this.moveDownToolStripMenuItem_Click);
            // 
            // ActionDisplayControl
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ContextMenuStrip = this.ActionContextMenu;
            this.Controls.Add(this.BottomPanel);
            this.Controls.Add(this.TopPanel);
            this.Controls.Add(this.splitter1);
            this.MinimumSize = new System.Drawing.Size(250, 4);
            this.Name = "ActionDisplayControl";
            this.Size = new System.Drawing.Size(250, 81);
            this.Load += new System.EventHandler(this.ActionDisplayControl_Load);
            ((System.ComponentModel.ISupportInitialize)(this.StatusIconBox)).EndInit();
            this.ActionContextMenu.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ActionIcon)).EndInit();
            this.TopPanel.ResumeLayout(false);
            this.TopPanel.PerformLayout();
            this.BottomPanel.ResumeLayout(false);
            this.BottomPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip ActionContextMenu;
        private System.Windows.Forms.ToolStripMenuItem ActMenuDelete;
        private System.Windows.Forms.ToolTip StatusTooltip;
        private System.Windows.Forms.ToolTip ArgumentTooltips;

        /// <summary>
        /// 
        /// </summary>
        protected System.Windows.Forms.TableLayoutPanel AttributeTable;
        /// <summary>
        /// 
        /// </summary>
        protected Label NameLabel;
        private PictureBox ActionIcon;
        private PictureBox StatusIconBox;
        private ImageList StatusIcons;
        private Button ExpandButton;
        private Panel TopPanel;
        private Panel BottomPanel;
        private Splitter splitter1;
        protected internal TextBox CommentTextBox;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripMenuItem moveUpToolStripMenuItem;
        private ToolStripMenuItem moveDownToolStripMenuItem;
    }
}
