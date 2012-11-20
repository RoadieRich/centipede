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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ActionDisplayControl));
            this.NameLabel = new System.Windows.Forms.Label();
            this.ExpandButton = new System.Windows.Forms.Button();
            this.AttributeTable = new System.Windows.Forms.TableLayoutPanel();
            this.StatusIcons = new System.Windows.Forms.ImageList(this.components);
            this.StatusIconBox = new System.Windows.Forms.PictureBox();
            this.ActionContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ActMenuMoveUp = new System.Windows.Forms.ToolStripMenuItem();
            this.ActMenuMoveDown = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.ActMenuUndo = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.ActMenuCut = new System.Windows.Forms.ToolStripMenuItem();
            this.ActMenuCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.ActMenuPaste = new System.Windows.Forms.ToolStripMenuItem();
            this.ActMenuDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.ActMenuSelectAll = new System.Windows.Forms.ToolStripMenuItem();
            this.CommentTextBox = new System.Windows.Forms.TextBox();
            this.StatusTooltip = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.StatusIconBox)).BeginInit();
            this.ActionContextMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // NameLabel
            // 
            this.NameLabel.AutoSize = true;
            this.NameLabel.BackColor = System.Drawing.Color.Transparent;
            this.NameLabel.Location = new System.Drawing.Point(32, 10);
            this.NameLabel.Name = "NameLabel";
            this.NameLabel.Size = new System.Drawing.Size(35, 13);
            this.NameLabel.TabIndex = 0;
            this.NameLabel.Text = "label1";
            // 
            // ExpandButton
            // 
            this.ExpandButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ExpandButton.Location = new System.Drawing.Point(3, 3);
            this.ExpandButton.Margin = new System.Windows.Forms.Padding(0);
            this.ExpandButton.Name = "ExpandButton";
            this.ExpandButton.Size = new System.Drawing.Size(26, 26);
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
            this.AttributeTable.BackColor = System.Drawing.Color.Transparent;
            this.AttributeTable.ColumnCount = 2;
            this.AttributeTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.AttributeTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.AttributeTable.Location = new System.Drawing.Point(4, 34);
            this.AttributeTable.MinimumSize = new System.Drawing.Size(0, 28);
            this.AttributeTable.Name = "AttributeTable";
            this.AttributeTable.RowCount = 1;
            this.AttributeTable.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.AttributeTable.Size = new System.Drawing.Size(132, 28);
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
            this.StatusIconBox.Location = new System.Drawing.Point(110, 3);
            this.StatusIconBox.Name = "StatusIconBox";
            this.StatusIconBox.Size = new System.Drawing.Size(26, 26);
            this.StatusIconBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.StatusIconBox.TabIndex = 3;
            this.StatusIconBox.TabStop = false;
            // 
            // ActionContextMenu
            // 
            this.ActionContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ActMenuMoveUp,
            this.ActMenuMoveDown,
            this.toolStripSeparator3,
            this.ActMenuUndo,
            this.toolStripSeparator1,
            this.ActMenuCut,
            this.ActMenuCopy,
            this.ActMenuPaste,
            this.ActMenuDelete,
            this.toolStripSeparator2,
            this.ActMenuSelectAll});
            this.ActionContextMenu.Name = "ActionContextMenu";
            this.ActionContextMenu.ShowImageMargin = false;
            this.ActionContextMenu.Size = new System.Drawing.Size(143, 220);
            // 
            // ActMenuMoveUp
            // 
            this.ActMenuMoveUp.Enabled = false;
            this.ActMenuMoveUp.Name = "ActMenuMoveUp";
            this.ActMenuMoveUp.ShortcutKeyDisplayString = "[";
            this.ActMenuMoveUp.Size = new System.Drawing.Size(142, 22);
            this.ActMenuMoveUp.Text = "Move Up";
            // 
            // ActMenuMoveDown
            // 
            this.ActMenuMoveDown.Enabled = false;
            this.ActMenuMoveDown.Name = "ActMenuMoveDown";
            this.ActMenuMoveDown.Size = new System.Drawing.Size(142, 22);
            this.ActMenuMoveDown.Text = "Move Down";
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(139, 6);
            // 
            // ActMenuUndo
            // 
            this.ActMenuUndo.Enabled = false;
            this.ActMenuUndo.Name = "ActMenuUndo";
            this.ActMenuUndo.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Z)));
            this.ActMenuUndo.Size = new System.Drawing.Size(142, 22);
            this.ActMenuUndo.Text = "&Undo";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(139, 6);
            // 
            // ActMenuCut
            // 
            this.ActMenuCut.Enabled = false;
            this.ActMenuCut.Name = "ActMenuCut";
            this.ActMenuCut.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.X)));
            this.ActMenuCut.Size = new System.Drawing.Size(142, 22);
            this.ActMenuCut.Text = "Cu&t";
            // 
            // ActMenuCopy
            // 
            this.ActMenuCopy.BackColor = System.Drawing.Color.Transparent;
            this.ActMenuCopy.Enabled = false;
            this.ActMenuCopy.Name = "ActMenuCopy";
            this.ActMenuCopy.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
            this.ActMenuCopy.Size = new System.Drawing.Size(142, 22);
            this.ActMenuCopy.Text = "&Copy";
            // 
            // ActMenuPaste
            // 
            this.ActMenuPaste.Enabled = false;
            this.ActMenuPaste.Name = "ActMenuPaste";
            this.ActMenuPaste.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.V)));
            this.ActMenuPaste.Size = new System.Drawing.Size(142, 22);
            this.ActMenuPaste.Text = "&Paste";
            // 
            // ActMenuDelete
            // 
            this.ActMenuDelete.Name = "ActMenuDelete";
            this.ActMenuDelete.ShortcutKeyDisplayString = "";
            this.ActMenuDelete.Size = new System.Drawing.Size(142, 22);
            this.ActMenuDelete.Text = "&Delete";
            this.ActMenuDelete.Click += new System.EventHandler(this.ActMenuDelete_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(139, 6);
            // 
            // ActMenuSelectAll
            // 
            this.ActMenuSelectAll.Enabled = false;
            this.ActMenuSelectAll.Name = "ActMenuSelectAll";
            this.ActMenuSelectAll.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.A)));
            this.ActMenuSelectAll.Size = new System.Drawing.Size(142, 22);
            this.ActMenuSelectAll.Text = "Select &All";
            // 
            // CommentTextBox
            // 
            this.CommentTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.CommentTextBox.Location = new System.Drawing.Point(104, 7);
            this.CommentTextBox.Name = "CommentTextBox";
            this.CommentTextBox.Size = new System.Drawing.Size(0, 20);
            this.CommentTextBox.TabIndex = 4;
            this.CommentTextBox.TextChanged += new System.EventHandler(this.CommentTextBox_TextChanged);
            // 
            // ActionDisplayControl
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.ContextMenuStrip = this.ActionContextMenu;
            this.Controls.Add(this.StatusIconBox);
            this.Controls.Add(this.CommentTextBox);
            this.Controls.Add(this.AttributeTable);
            this.Controls.Add(this.ExpandButton);
            this.Controls.Add(this.NameLabel);
            this.Name = "ActionDisplayControl";
            this.Size = new System.Drawing.Size(139, 65);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.ActionDisplayControl_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.ActionDisplayControl_DragEnter);
            ((System.ComponentModel.ISupportInitialize)(this.StatusIconBox)).EndInit();
            this.ActionContextMenu.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label NameLabel;
        private System.Windows.Forms.Button ExpandButton;
        private System.Windows.Forms.TableLayoutPanel AttributeTable;
        private System.Windows.Forms.ImageList StatusIcons;
        private System.Windows.Forms.PictureBox StatusIconBox;
        private System.Windows.Forms.ContextMenuStrip ActionContextMenu;
        private System.Windows.Forms.ToolStripMenuItem ActMenuMoveUp;
        private System.Windows.Forms.ToolStripMenuItem ActMenuMoveDown;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem ActMenuUndo;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem ActMenuCut;
        private System.Windows.Forms.ToolStripMenuItem ActMenuCopy;
        private System.Windows.Forms.ToolStripMenuItem ActMenuPaste;
        private System.Windows.Forms.ToolStripMenuItem ActMenuDelete;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem ActMenuSelectAll;
        private System.Windows.Forms.TextBox CommentTextBox;
        private System.Windows.Forms.ToolTip StatusTooltip;


    }
}
