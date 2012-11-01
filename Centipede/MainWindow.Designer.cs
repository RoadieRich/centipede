namespace Centipede
{
    partial class MainWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
            this.ActionsVarsTabControl = new System.Windows.Forms.TabControl();
            this.ActionsTab = new System.Windows.Forms.TabPage();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.jobActionListBox = new Centipede.JobActionListBox();
            this.AddActionTabs = new System.Windows.Forms.TabControl();
            this.UIActTab = new System.Windows.Forms.TabPage();
            this.UIActListBox = new System.Windows.Forms.ListBox();
            this.FlowContActTab = new System.Windows.Forms.TabPage();
            this.FlowContListBox = new System.Windows.Forms.ListBox();
            this.ExcelActTab = new System.Windows.Forms.TabPage();
            this.ExcelActListBox = new System.Windows.Forms.ListBox();
            this.MathCadActTab = new System.Windows.Forms.TabPage();
            this.MathCadListBox = new System.Windows.Forms.ListBox();
            this.SolidWorksActTab = new System.Windows.Forms.TabPage();
            this.SolidWorksListBox = new System.Windows.Forms.ListBox();
            this.OtherActTab = new System.Windows.Forms.TabPage();
            this.OtherActListBox = new System.Windows.Forms.ListBox();
            this.VarsTab = new System.Windows.Forms.TabPage();
            this.VarDataGridView = new System.Windows.Forms.DataGridView();
            this.VarsContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.VarMenuUndo = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.VarMenuCut = new System.Windows.Forms.ToolStripMenuItem();
            this.VarMenuCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.VarMenuPaste = new System.Windows.Forms.ToolStripMenuItem();
            this.VarMenuDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.VarMenuSelectAll = new System.Windows.Forms.ToolStripMenuItem();
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
            this.LoadBtn = new System.Windows.Forms.Button();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.programBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.updateTimer = new System.Windows.Forms.Timer(this.components);
            this.ActionsVarsTabControl.SuspendLayout();
            this.ActionsTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.AddActionTabs.SuspendLayout();
            this.UIActTab.SuspendLayout();
            this.FlowContActTab.SuspendLayout();
            this.ExcelActTab.SuspendLayout();
            this.MathCadActTab.SuspendLayout();
            this.SolidWorksActTab.SuspendLayout();
            this.OtherActTab.SuspendLayout();
            this.VarsTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.VarDataGridView)).BeginInit();
            this.VarsContextMenu.SuspendLayout();
            this.ActionContextMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.programBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // ActionsVarsTabControl
            // 
            this.ActionsVarsTabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ActionsVarsTabControl.Controls.Add(this.ActionsTab);
            this.ActionsVarsTabControl.Controls.Add(this.VarsTab);
            this.ActionsVarsTabControl.Location = new System.Drawing.Point(12, 12);
            this.ActionsVarsTabControl.Name = "ActionsVarsTabControl";
            this.ActionsVarsTabControl.SelectedIndex = 0;
            this.ActionsVarsTabControl.Size = new System.Drawing.Size(455, 471);
            this.ActionsVarsTabControl.TabIndex = 0;
            // 
            // ActionsTab
            // 
            this.ActionsTab.Controls.Add(this.splitContainer1);
            this.ActionsTab.Location = new System.Drawing.Point(4, 22);
            this.ActionsTab.Name = "ActionsTab";
            this.ActionsTab.Padding = new System.Windows.Forms.Padding(3);
            this.ActionsTab.Size = new System.Drawing.Size(447, 445);
            this.ActionsTab.TabIndex = 0;
            this.ActionsTab.Text = "Actions";
            this.ActionsTab.UseVisualStyleBackColor = true;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.jobActionListBox);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.AddActionTabs);
            this.splitContainer1.Size = new System.Drawing.Size(447, 445);
            this.splitContainer1.SplitterDistance = 285;
            this.splitContainer1.TabIndex = 0;
            // 
            // jobActionListBox
            // 
            this.jobActionListBox.FormattingEnabled = true;
            this.jobActionListBox.Location = new System.Drawing.Point(7, 7);
            this.jobActionListBox.Name = "jobActionListBox";
            this.jobActionListBox.Size = new System.Drawing.Size(434, 277);
            this.jobActionListBox.TabIndex = 0;
            // 
            // AddActionTabs
            // 
            this.AddActionTabs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.AddActionTabs.Controls.Add(this.UIActTab);
            this.AddActionTabs.Controls.Add(this.FlowContActTab);
            this.AddActionTabs.Controls.Add(this.ExcelActTab);
            this.AddActionTabs.Controls.Add(this.MathCadActTab);
            this.AddActionTabs.Controls.Add(this.SolidWorksActTab);
            this.AddActionTabs.Controls.Add(this.OtherActTab);
            this.AddActionTabs.Location = new System.Drawing.Point(3, 3);
            this.AddActionTabs.Multiline = true;
            this.AddActionTabs.Name = "AddActionTabs";
            this.AddActionTabs.SelectedIndex = 0;
            this.AddActionTabs.Size = new System.Drawing.Size(444, 150);
            this.AddActionTabs.TabIndex = 3;
            // 
            // UIActTab
            // 
            this.UIActTab.Controls.Add(this.UIActListBox);
            this.UIActTab.Location = new System.Drawing.Point(4, 22);
            this.UIActTab.Name = "UIActTab";
            this.UIActTab.Padding = new System.Windows.Forms.Padding(3);
            this.UIActTab.Size = new System.Drawing.Size(436, 124);
            this.UIActTab.TabIndex = 0;
            this.UIActTab.Text = "UI";
            this.UIActTab.UseVisualStyleBackColor = true;
            // 
            // UIActListBox
            // 
            this.UIActListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.UIActListBox.FormattingEnabled = true;
            this.UIActListBox.Location = new System.Drawing.Point(7, 7);
            this.UIActListBox.Name = "UIActListBox";
            this.UIActListBox.Size = new System.Drawing.Size(426, 108);
            this.UIActListBox.TabIndex = 0;
            this.UIActListBox.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.ActListBox_Dbl_Click);
            // 
            // FlowContActTab
            // 
            this.FlowContActTab.AllowDrop = true;
            this.FlowContActTab.Controls.Add(this.FlowContListBox);
            this.FlowContActTab.Location = new System.Drawing.Point(4, 22);
            this.FlowContActTab.Name = "FlowContActTab";
            this.FlowContActTab.Padding = new System.Windows.Forms.Padding(3);
            this.FlowContActTab.Size = new System.Drawing.Size(436, 124);
            this.FlowContActTab.TabIndex = 1;
            this.FlowContActTab.Text = "Flow Control & Variables";
            this.FlowContActTab.UseVisualStyleBackColor = true;
            // 
            // FlowContListBox
            // 
            this.FlowContListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.FlowContListBox.FormattingEnabled = true;
            this.FlowContListBox.Location = new System.Drawing.Point(6, 6);
            this.FlowContListBox.Name = "FlowContListBox";
            this.FlowContListBox.Size = new System.Drawing.Size(427, 108);
            this.FlowContListBox.TabIndex = 1;
            this.FlowContListBox.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.ActListBox_Dbl_Click);
            // 
            // ExcelActTab
            // 
            this.ExcelActTab.Controls.Add(this.ExcelActListBox);
            this.ExcelActTab.Location = new System.Drawing.Point(4, 22);
            this.ExcelActTab.Name = "ExcelActTab";
            this.ExcelActTab.Padding = new System.Windows.Forms.Padding(3);
            this.ExcelActTab.Size = new System.Drawing.Size(436, 124);
            this.ExcelActTab.TabIndex = 2;
            this.ExcelActTab.Text = "Excel";
            this.ExcelActTab.UseVisualStyleBackColor = true;
            // 
            // ExcelActListBox
            // 
            this.ExcelActListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ExcelActListBox.FormattingEnabled = true;
            this.ExcelActListBox.Location = new System.Drawing.Point(6, 6);
            this.ExcelActListBox.Name = "ExcelActListBox";
            this.ExcelActListBox.Size = new System.Drawing.Size(424, 108);
            this.ExcelActListBox.TabIndex = 1;
            this.ExcelActListBox.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.ActListBox_Dbl_Click);
            // 
            // MathCadActTab
            // 
            this.MathCadActTab.Controls.Add(this.MathCadListBox);
            this.MathCadActTab.Location = new System.Drawing.Point(4, 22);
            this.MathCadActTab.Name = "MathCadActTab";
            this.MathCadActTab.Size = new System.Drawing.Size(436, 124);
            this.MathCadActTab.TabIndex = 3;
            this.MathCadActTab.Text = "MathCAD";
            this.MathCadActTab.UseVisualStyleBackColor = true;
            // 
            // MathCadListBox
            // 
            this.MathCadListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.MathCadListBox.FormattingEnabled = true;
            this.MathCadListBox.Location = new System.Drawing.Point(6, 6);
            this.MathCadListBox.Name = "MathCadListBox";
            this.MathCadListBox.Size = new System.Drawing.Size(424, 108);
            this.MathCadListBox.TabIndex = 2;
            this.MathCadListBox.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.ActListBox_Dbl_Click);
            // 
            // SolidWorksActTab
            // 
            this.SolidWorksActTab.Controls.Add(this.SolidWorksListBox);
            this.SolidWorksActTab.Location = new System.Drawing.Point(4, 22);
            this.SolidWorksActTab.Name = "SolidWorksActTab";
            this.SolidWorksActTab.Size = new System.Drawing.Size(436, 124);
            this.SolidWorksActTab.TabIndex = 4;
            this.SolidWorksActTab.Text = "SolidWorks";
            this.SolidWorksActTab.UseVisualStyleBackColor = true;
            // 
            // SolidWorksListBox
            // 
            this.SolidWorksListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.SolidWorksListBox.FormattingEnabled = true;
            this.SolidWorksListBox.Location = new System.Drawing.Point(6, 6);
            this.SolidWorksListBox.Name = "SolidWorksListBox";
            this.SolidWorksListBox.Size = new System.Drawing.Size(424, 108);
            this.SolidWorksListBox.TabIndex = 3;
            this.SolidWorksListBox.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.ActListBox_Dbl_Click);
            // 
            // OtherActTab
            // 
            this.OtherActTab.Controls.Add(this.OtherActListBox);
            this.OtherActTab.Location = new System.Drawing.Point(4, 22);
            this.OtherActTab.Name = "OtherActTab";
            this.OtherActTab.Size = new System.Drawing.Size(436, 124);
            this.OtherActTab.TabIndex = 5;
            this.OtherActTab.Text = "Other Actions";
            this.OtherActTab.UseVisualStyleBackColor = true;
            // 
            // OtherActListBox
            // 
            this.OtherActListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.OtherActListBox.FormattingEnabled = true;
            this.OtherActListBox.Location = new System.Drawing.Point(6, 6);
            this.OtherActListBox.Name = "OtherActListBox";
            this.OtherActListBox.Size = new System.Drawing.Size(424, 108);
            this.OtherActListBox.TabIndex = 3;
            this.OtherActListBox.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.ActListBox_Dbl_Click);
            // 
            // VarsTab
            // 
            this.VarsTab.Controls.Add(this.VarDataGridView);
            this.VarsTab.Location = new System.Drawing.Point(4, 22);
            this.VarsTab.Name = "VarsTab";
            this.VarsTab.Padding = new System.Windows.Forms.Padding(3);
            this.VarsTab.Size = new System.Drawing.Size(447, 445);
            this.VarsTab.TabIndex = 1;
            this.VarsTab.Text = "Variables";
            this.VarsTab.UseVisualStyleBackColor = true;
            // 
            // VarDataGridView
            // 
            this.VarDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.VarDataGridView.ContextMenuStrip = this.VarsContextMenu;
            this.VarDataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.VarDataGridView.Location = new System.Drawing.Point(3, 3);
            this.VarDataGridView.Name = "VarDataGridView";
            this.VarDataGridView.Size = new System.Drawing.Size(441, 439);
            this.VarDataGridView.TabIndex = 7;
            this.VarDataGridView.VirtualMode = true;
            
            // 
            // VarsContextMenu
            // 
            this.VarsContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.VarMenuUndo,
            this.toolStripSeparator5,
            this.VarMenuCut,
            this.VarMenuCopy,
            this.VarMenuPaste,
            this.VarMenuDelete,
            this.toolStripSeparator6,
            this.VarMenuSelectAll});
            this.VarsContextMenu.Name = "ActionContextMenu";
            this.VarsContextMenu.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.VarsContextMenu.ShowImageMargin = false;
            this.VarsContextMenu.Size = new System.Drawing.Size(143, 148);
            // 
            // VarMenuUndo
            // 
            this.VarMenuUndo.Enabled = false;
            this.VarMenuUndo.Name = "VarMenuUndo";
            this.VarMenuUndo.ShortcutKeyDisplayString = "Ctrl+Z";
            this.VarMenuUndo.Size = new System.Drawing.Size(142, 22);
            this.VarMenuUndo.Text = "&Undo";
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(139, 6);
            // 
            // VarMenuCut
            // 
            this.VarMenuCut.Name = "VarMenuCut";
            this.VarMenuCut.ShortcutKeyDisplayString = "Ctrl+X";
            this.VarMenuCut.Size = new System.Drawing.Size(142, 22);
            this.VarMenuCut.Text = "Cu&t";
            // 
            // VarMenuCopy
            // 
            this.VarMenuCopy.Name = "VarMenuCopy";
            this.VarMenuCopy.ShortcutKeyDisplayString = "Ctrl+C";
            this.VarMenuCopy.Size = new System.Drawing.Size(142, 22);
            this.VarMenuCopy.Text = "&Copy";
            // 
            // VarMenuPaste
            // 
            this.VarMenuPaste.Name = "VarMenuPaste";
            this.VarMenuPaste.ShortcutKeyDisplayString = "Ctrl+V";
            this.VarMenuPaste.Size = new System.Drawing.Size(142, 22);
            this.VarMenuPaste.Text = "&Paste";
            // 
            // VarMenuDelete
            // 
            this.VarMenuDelete.Name = "VarMenuDelete";
            this.VarMenuDelete.ShortcutKeyDisplayString = "Del";
            this.VarMenuDelete.Size = new System.Drawing.Size(142, 22);
            this.VarMenuDelete.Text = "&Delete";
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(139, 6);
            // 
            // VarMenuSelectAll
            // 
            this.VarMenuSelectAll.Name = "VarMenuSelectAll";
            this.VarMenuSelectAll.ShortcutKeyDisplayString = "Ctrl+A";
            this.VarMenuSelectAll.Size = new System.Drawing.Size(142, 22);
            this.VarMenuSelectAll.Text = "Select &All";
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
            this.ActionContextMenu.Size = new System.Drawing.Size(143, 198);
            // 
            // ActMenuMoveUp
            // 
            this.ActMenuMoveUp.Name = "ActMenuMoveUp";
            this.ActMenuMoveUp.ShortcutKeyDisplayString = "[";
            this.ActMenuMoveUp.Size = new System.Drawing.Size(142, 22);
            this.ActMenuMoveUp.Text = "Move Up";
            // 
            // ActMenuMoveDown
            // 
            this.ActMenuMoveDown.Name = "ActMenuMoveDown";
            this.ActMenuMoveDown.ShortcutKeyDisplayString = "]";
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
            this.ActMenuUndo.ShortcutKeyDisplayString = "Ctrl+Z";
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
            this.ActMenuCut.Name = "ActMenuCut";
            this.ActMenuCut.ShortcutKeyDisplayString = "Ctrl+X";
            this.ActMenuCut.Size = new System.Drawing.Size(142, 22);
            this.ActMenuCut.Text = "Cu&t";
            // 
            // ActMenuCopy
            // 
            this.ActMenuCopy.Name = "ActMenuCopy";
            this.ActMenuCopy.ShortcutKeyDisplayString = "Ctrl+C";
            this.ActMenuCopy.Size = new System.Drawing.Size(142, 22);
            this.ActMenuCopy.Text = "&Copy";
            // 
            // ActMenuPaste
            // 
            this.ActMenuPaste.Name = "ActMenuPaste";
            this.ActMenuPaste.ShortcutKeyDisplayString = "Ctrl+V";
            this.ActMenuPaste.Size = new System.Drawing.Size(142, 22);
            this.ActMenuPaste.Text = "&Paste";
            // 
            // ActMenuDelete
            // 
            this.ActMenuDelete.Name = "ActMenuDelete";
            this.ActMenuDelete.ShortcutKeyDisplayString = "Del";
            this.ActMenuDelete.Size = new System.Drawing.Size(142, 22);
            this.ActMenuDelete.Text = "&Delete";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(139, 6);
            // 
            // ActMenuSelectAll
            // 
            this.ActMenuSelectAll.Name = "ActMenuSelectAll";
            this.ActMenuSelectAll.ShortcutKeyDisplayString = "Ctrl+A";
            this.ActMenuSelectAll.Size = new System.Drawing.Size(142, 22);
            this.ActMenuSelectAll.Text = "Select &All";
            // 
            // LoadBtn
            // 
            this.LoadBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.LoadBtn.Location = new System.Drawing.Point(473, 12);
            this.LoadBtn.Name = "LoadBtn";
            this.LoadBtn.Size = new System.Drawing.Size(75, 23);
            this.LoadBtn.TabIndex = 1;
            this.LoadBtn.Text = "&Load";
            this.LoadBtn.UseVisualStyleBackColor = true;
            // 
            // progressBar1
            // 
            this.progressBar1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar1.Location = new System.Drawing.Point(12, 489);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(455, 23);
            this.progressBar1.TabIndex = 2;
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(473, 489);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 3;
            this.button1.Text = "&Run";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button2.Location = new System.Drawing.Point(473, 42);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 4;
            this.button2.Text = "&Save";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // programBindingSource
            // 
            this.programBindingSource.DataSource = typeof(Centipede.Program);
            // 
            // updateTimer
            // 
            this.updateTimer.Enabled = true;
            this.updateTimer.Tick += new System.EventHandler(this.updateTimer_Tick);
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(560, 524);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.LoadBtn);
            this.Controls.Add(this.ActionsVarsTabControl);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainWindow";
            this.Text = "Centipede";
            this.Load += new System.EventHandler(this.MainWindow_Load);
            this.ActionsVarsTabControl.ResumeLayout(false);
            this.ActionsTab.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.AddActionTabs.ResumeLayout(false);
            this.UIActTab.ResumeLayout(false);
            this.FlowContActTab.ResumeLayout(false);
            this.ExcelActTab.ResumeLayout(false);
            this.MathCadActTab.ResumeLayout(false);
            this.SolidWorksActTab.ResumeLayout(false);
            this.OtherActTab.ResumeLayout(false);
            this.VarsTab.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.VarDataGridView)).EndInit();
            this.VarsContextMenu.ResumeLayout(false);
            this.ActionContextMenu.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.programBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl ActionsVarsTabControl;
        private System.Windows.Forms.TabPage ActionsTab;
        private System.Windows.Forms.TabPage VarsTab;
        private System.Windows.Forms.Button LoadBtn;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TabControl AddActionTabs;
        private System.Windows.Forms.TabPage UIActTab;
        private System.Windows.Forms.ListBox UIActListBox;
        private System.Windows.Forms.TabPage FlowContActTab;
        private System.Windows.Forms.TabPage ExcelActTab;
        private System.Windows.Forms.TabPage MathCadActTab;
        private System.Windows.Forms.TabPage SolidWorksActTab;
        private System.Windows.Forms.TabPage OtherActTab;
        private System.Windows.Forms.ListBox FlowContListBox;
        private System.Windows.Forms.ListBox ExcelActListBox;
        private System.Windows.Forms.ListBox MathCadListBox;
        private System.Windows.Forms.ListBox SolidWorksListBox;
        private System.Windows.Forms.ListBox OtherActListBox;
        private System.Windows.Forms.ContextMenuStrip ActionContextMenu;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem ActMenuCut;
        private System.Windows.Forms.ToolStripMenuItem ActMenuCopy;
        private System.Windows.Forms.ToolStripMenuItem ActMenuPaste;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem ActMenuDelete;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem ActMenuUndo;
        private System.Windows.Forms.ToolStripMenuItem ActMenuMoveUp;
        private System.Windows.Forms.ToolStripMenuItem ActMenuMoveDown;
        private System.Windows.Forms.ToolStripMenuItem ActMenuSelectAll;
        private System.Windows.Forms.ContextMenuStrip VarsContextMenu;
        private System.Windows.Forms.ToolStripMenuItem VarMenuUndo;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripMenuItem VarMenuCut;
        private System.Windows.Forms.ToolStripMenuItem VarMenuCopy;
        private System.Windows.Forms.ToolStripMenuItem VarMenuPaste;
        private System.Windows.Forms.ToolStripMenuItem VarMenuDelete;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripMenuItem VarMenuSelectAll;
        private JobActionListBox jobActionListBox;
        private System.Windows.Forms.DataGridView VarDataGridView;
        private System.Windows.Forms.BindingSource programBindingSource;
        private System.Windows.Forms.Timer updateTimer;
    }
}

