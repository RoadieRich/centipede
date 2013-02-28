


namespace Centipede
{
    partial class MainWindow
    {
        private System.Windows.Forms.ImageList ActionIcons;
        private System.ComponentModel.IContainer components;
        private System.Windows.Forms.ImageList OtherActIcons;
        private System.Windows.Forms.ContextMenuStrip VarsContextMenu;
        private System.Windows.Forms.ToolStripMenuItem VarMenuUndo;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripMenuItem VarMenuCut;
        private System.Windows.Forms.ToolStripMenuItem VarMenuCopy;
        private System.Windows.Forms.ToolStripMenuItem VarMenuPaste;
        private System.Windows.Forms.ToolStripMenuItem VarMenuDelete;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripMenuItem VarMenuSelectAll;
        private System.Windows.Forms.Timer updateTimer;
        private System.Windows.Forms.NotifyIcon notifyIcon;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;

        #region Windows Form Designer generated code
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
            this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
            this.SplitContainer1 = new System.Windows.Forms.SplitContainer();
            this.SplitContainer2 = new System.Windows.Forms.SplitContainer();
            this.toolStripContainer4 = new System.Windows.Forms.ToolStripContainer();
            this.WebBrowser = new System.Windows.Forms.WebBrowser();
            this.NavigationToolbar = new System.Windows.Forms.ToolStrip();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton3 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.SplitContainer3 = new System.Windows.Forms.SplitContainer();
            this.ActionContainer = new System.Windows.Forms.TableLayoutPanel();
            this.VarsContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.VarMenuUndo = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.VarMenuCut = new System.Windows.Forms.ToolStripMenuItem();
            this.VarMenuCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.VarMenuPaste = new System.Windows.Forms.ToolStripMenuItem();
            this.VarMenuDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.VarMenuSelectAll = new System.Windows.Forms.ToolStripMenuItem();
            this.AddActionTabs = new System.Windows.Forms.TabControl();
            this.UIActTab = new System.Windows.Forms.TabPage();
            this.UIActListBox = new System.Windows.Forms.ListView();
            this.ActionIcons = new System.Windows.Forms.ImageList(this.components);
            this.RunTabs = new System.Windows.Forms.TabControl();
            this.OutputTab = new System.Windows.Forms.TabPage();
            this.toolStripContainer3 = new System.Windows.Forms.ToolStripContainer();
            this.MessageDataGridView = new System.Windows.Forms.DataGridView();
            this.timestampDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.messageDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.actionDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.levelDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.messagesBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this._dataSet = new Centipede.JobDataSet();
            this.MessageFilterToolStrip = new System.Windows.Forms.ToolStrip();
            this.toolStripButton4 = new System.Windows.Forms.ToolStripButton();
            this.VarsTab = new System.Windows.Forms.TabPage();
            this.VarDataGridView = new System.Windows.Forms.DataGridView();
            this.RunButton = new System.Windows.Forms.Button();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.saveToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.printToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.printPreviewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.undoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.redoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
            this.cutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pasteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator9 = new System.Windows.Forms.ToolStripSeparator();
            this.selectAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.runToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.runToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.abortToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.contentsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.indexToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.searchToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator10 = new System.Windows.Forms.ToolStripSeparator();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.FavouritesMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.FavouritesToolStripMenuPlaceholder = new System.Windows.Forms.ToolStripMenuItem();
            this.FaveMenuSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.addCurrentToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.EditFavouritesMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripContainer2 = new System.Windows.Forms.ToolStripContainer();
            this.OtherActIcons = new System.Windows.Forms.ImageList(this.components);
            this.updateTimer = new System.Windows.Forms.Timer(this.components);
            this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.GetFileNameDialogue = new System.Windows.Forms.OpenFileDialog();
            this.OpenFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.BottomToolStripPanel = new System.Windows.Forms.ToolStripPanel();
            this.TopToolStripPanel = new System.Windows.Forms.ToolStripPanel();
            this.RightToolStripPanel = new System.Windows.Forms.ToolStripPanel();
            this.LeftToolStripPanel = new System.Windows.Forms.ToolStripPanel();
            this.ContentPanel = new System.Windows.Forms.ToolStripContentPanel();
            this.miniToolStrip = new System.Windows.Forms.MenuStrip();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.favouriteJobs1 = new Centipede.FavouriteJobs();
            this.toolStripContainer1.ContentPanel.SuspendLayout();
            this.toolStripContainer1.TopToolStripPanel.SuspendLayout();
            this.toolStripContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SplitContainer1)).BeginInit();
            this.SplitContainer1.Panel1.SuspendLayout();
            this.SplitContainer1.Panel2.SuspendLayout();
            this.SplitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SplitContainer2)).BeginInit();
            this.SplitContainer2.Panel1.SuspendLayout();
            this.SplitContainer2.Panel2.SuspendLayout();
            this.SplitContainer2.SuspendLayout();
            this.toolStripContainer4.ContentPanel.SuspendLayout();
            this.toolStripContainer4.TopToolStripPanel.SuspendLayout();
            this.toolStripContainer4.SuspendLayout();
            this.NavigationToolbar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SplitContainer3)).BeginInit();
            this.SplitContainer3.Panel1.SuspendLayout();
            this.SplitContainer3.Panel2.SuspendLayout();
            this.SplitContainer3.SuspendLayout();
            this.VarsContextMenu.SuspendLayout();
            this.AddActionTabs.SuspendLayout();
            this.UIActTab.SuspendLayout();
            this.RunTabs.SuspendLayout();
            this.OutputTab.SuspendLayout();
            this.toolStripContainer3.ContentPanel.SuspendLayout();
            this.toolStripContainer3.TopToolStripPanel.SuspendLayout();
            this.toolStripContainer3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MessageDataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.messagesBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._dataSet)).BeginInit();
            this.MessageFilterToolStrip.SuspendLayout();
            this.VarsTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.VarDataGridView)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.toolStripContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.favouriteJobs1)).BeginInit();
            this.SuspendLayout();
            // 
            // toolStripContainer1
            // 
            this.toolStripContainer1.BottomToolStripPanelVisible = false;
            // 
            // toolStripContainer1.ContentPanel
            // 
            this.toolStripContainer1.ContentPanel.Controls.Add(this.SplitContainer1);
            resources.ApplyResources(this.toolStripContainer1.ContentPanel, "toolStripContainer1.ContentPanel");
            resources.ApplyResources(this.toolStripContainer1, "toolStripContainer1");
            this.toolStripContainer1.LeftToolStripPanelVisible = false;
            this.toolStripContainer1.Name = "toolStripContainer1";
            this.toolStripContainer1.RightToolStripPanelVisible = false;
            // 
            // toolStripContainer1.TopToolStripPanel
            // 
            this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.menuStrip1);
            // 
            // SplitContainer1
            // 
            this.SplitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.SplitContainer1, "SplitContainer1");
            this.SplitContainer1.Name = "SplitContainer1";
            // 
            // SplitContainer1.Panel1
            // 
            this.SplitContainer1.Panel1.Controls.Add(this.SplitContainer2);
            // 
            // SplitContainer1.Panel2
            // 
            this.SplitContainer1.Panel2.Controls.Add(this.RunTabs);
            this.SplitContainer1.Panel2.Controls.Add(this.RunButton);
            this.SplitContainer1.Panel2.Controls.Add(this.progressBar1);
            this.SplitContainer1.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.SplitContainer1_SplitterMoved);
            // 
            // SplitContainer2
            // 
            resources.ApplyResources(this.SplitContainer2, "SplitContainer2");
            this.SplitContainer2.Name = "SplitContainer2";
            // 
            // SplitContainer2.Panel1
            // 
            this.SplitContainer2.Panel1.Controls.Add(this.toolStripContainer4);
            resources.ApplyResources(this.SplitContainer2.Panel1, "SplitContainer2.Panel1");
            // 
            // SplitContainer2.Panel2
            // 
            this.SplitContainer2.Panel2.BackColor = System.Drawing.SystemColors.Control;
            this.SplitContainer2.Panel2.Controls.Add(this.SplitContainer3);
            // 
            // toolStripContainer4
            // 
            this.toolStripContainer4.BottomToolStripPanelVisible = false;
            // 
            // toolStripContainer4.ContentPanel
            // 
            this.toolStripContainer4.ContentPanel.Controls.Add(this.WebBrowser);
            resources.ApplyResources(this.toolStripContainer4.ContentPanel, "toolStripContainer4.ContentPanel");
            resources.ApplyResources(this.toolStripContainer4, "toolStripContainer4");
            this.toolStripContainer4.LeftToolStripPanelVisible = false;
            this.toolStripContainer4.Name = "toolStripContainer4";
            this.toolStripContainer4.RightToolStripPanelVisible = false;
            // 
            // toolStripContainer4.TopToolStripPanel
            // 
            this.toolStripContainer4.TopToolStripPanel.Controls.Add(this.NavigationToolbar);
            // 
            // WebBrowser
            // 
            resources.ApplyResources(this.WebBrowser, "WebBrowser");
            this.WebBrowser.MinimumSize = new System.Drawing.Size(20, 20);
            this.WebBrowser.Name = "WebBrowser";
            this.WebBrowser.Navigating += new System.Windows.Forms.WebBrowserNavigatingEventHandler(this.WebBrowser_Navigating);
            // 
            // NavigationToolbar
            // 
            resources.ApplyResources(this.NavigationToolbar, "NavigationToolbar");
            this.NavigationToolbar.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.NavigationToolbar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton1,
            this.toolStripButton3,
            this.toolStripButton2});
            this.NavigationToolbar.Name = "NavigationToolbar";
            this.NavigationToolbar.Stretch = true;
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.toolStripButton1, "toolStripButton1");
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // toolStripButton3
            // 
            this.toolStripButton3.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.toolStripButton3, "toolStripButton3");
            this.toolStripButton3.Name = "toolStripButton3";
            this.toolStripButton3.Click += new System.EventHandler(this.toolStripButton3_Click);
            // 
            // toolStripButton2
            // 
            this.toolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.toolStripButton2, "toolStripButton2");
            this.toolStripButton2.Name = "toolStripButton2";
            this.toolStripButton2.Click += new System.EventHandler(this.toolStripButton2_Click);
            // 
            // SplitContainer3
            // 
            this.SplitContainer3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.SplitContainer3, "SplitContainer3");
            this.SplitContainer3.Name = "SplitContainer3";
            // 
            // SplitContainer3.Panel1
            // 
            this.SplitContainer3.Panel1.Controls.Add(this.ActionContainer);
            // 
            // SplitContainer3.Panel2
            // 
            this.SplitContainer3.Panel2.Controls.Add(this.AddActionTabs);
            // 
            // ActionContainer
            // 
            this.ActionContainer.AllowDrop = true;
            resources.ApplyResources(this.ActionContainer, "ActionContainer");
            this.ActionContainer.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.ActionContainer.ContextMenuStrip = this.VarsContextMenu;
            this.ActionContainer.Name = "ActionContainer";
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
            resources.ApplyResources(this.VarsContextMenu, "VarsContextMenu");
            // 
            // VarMenuUndo
            // 
            resources.ApplyResources(this.VarMenuUndo, "VarMenuUndo");
            this.VarMenuUndo.Name = "VarMenuUndo";
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            resources.ApplyResources(this.toolStripSeparator5, "toolStripSeparator5");
            // 
            // VarMenuCut
            // 
            this.VarMenuCut.Name = "VarMenuCut";
            resources.ApplyResources(this.VarMenuCut, "VarMenuCut");
            // 
            // VarMenuCopy
            // 
            this.VarMenuCopy.Name = "VarMenuCopy";
            resources.ApplyResources(this.VarMenuCopy, "VarMenuCopy");
            // 
            // VarMenuPaste
            // 
            this.VarMenuPaste.Name = "VarMenuPaste";
            resources.ApplyResources(this.VarMenuPaste, "VarMenuPaste");
            // 
            // VarMenuDelete
            // 
            this.VarMenuDelete.Name = "VarMenuDelete";
            resources.ApplyResources(this.VarMenuDelete, "VarMenuDelete");
            this.VarMenuDelete.Click += new System.EventHandler(this.VarMenuDelete_Click);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            resources.ApplyResources(this.toolStripSeparator6, "toolStripSeparator6");
            // 
            // VarMenuSelectAll
            // 
            this.VarMenuSelectAll.Name = "VarMenuSelectAll";
            resources.ApplyResources(this.VarMenuSelectAll, "VarMenuSelectAll");
            // 
            // AddActionTabs
            // 
            this.AddActionTabs.Controls.Add(this.UIActTab);
            resources.ApplyResources(this.AddActionTabs, "AddActionTabs");
            this.AddActionTabs.HotTrack = true;
            this.AddActionTabs.Multiline = true;
            this.AddActionTabs.Name = "AddActionTabs";
            this.AddActionTabs.SelectedIndex = 0;
            // 
            // UIActTab
            // 
            this.UIActTab.BackColor = System.Drawing.SystemColors.Window;
            this.UIActTab.Controls.Add(this.UIActListBox);
            resources.ApplyResources(this.UIActTab, "UIActTab");
            this.UIActTab.Name = "UIActTab";
            // 
            // UIActListBox
            // 
            resources.ApplyResources(this.UIActListBox, "UIActListBox");
            this.UIActListBox.LargeImageList = this.ActionIcons;
            this.UIActListBox.Name = "UIActListBox";
            this.UIActListBox.SmallImageList = this.ActionIcons;
            this.UIActListBox.UseCompatibleStateImageBehavior = false;
            this.UIActListBox.ItemActivate += new System.EventHandler(this.ItemActivate);
            this.UIActListBox.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.BeginDrag);
            // 
            // ActionIcons
            // 
            this.ActionIcons.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ActionIcons.ImageStream")));
            this.ActionIcons.TransparentColor = System.Drawing.Color.Transparent;
            this.ActionIcons.Images.SetKeyName(0, "pycon.ico");
            this.ActionIcons.Images.SetKeyName(1, "If.ico");
            // 
            // RunTabs
            // 
            resources.ApplyResources(this.RunTabs, "RunTabs");
            this.RunTabs.Controls.Add(this.OutputTab);
            this.RunTabs.Controls.Add(this.VarsTab);
            this.RunTabs.Name = "RunTabs";
            this.RunTabs.SelectedIndex = 0;
            // 
            // OutputTab
            // 
            this.OutputTab.Controls.Add(this.toolStripContainer3);
            resources.ApplyResources(this.OutputTab, "OutputTab");
            this.OutputTab.Name = "OutputTab";
            this.OutputTab.UseVisualStyleBackColor = true;
            // 
            // toolStripContainer3
            // 
            this.toolStripContainer3.BottomToolStripPanelVisible = false;
            // 
            // toolStripContainer3.ContentPanel
            // 
            this.toolStripContainer3.ContentPanel.Controls.Add(this.MessageDataGridView);
            resources.ApplyResources(this.toolStripContainer3.ContentPanel, "toolStripContainer3.ContentPanel");
            resources.ApplyResources(this.toolStripContainer3, "toolStripContainer3");
            this.toolStripContainer3.LeftToolStripPanelVisible = false;
            this.toolStripContainer3.Name = "toolStripContainer3";
            this.toolStripContainer3.RightToolStripPanelVisible = false;
            // 
            // toolStripContainer3.TopToolStripPanel
            // 
            this.toolStripContainer3.TopToolStripPanel.Controls.Add(this.MessageFilterToolStrip);
            // 
            // MessageDataGridView
            // 
            this.MessageDataGridView.AllowUserToAddRows = false;
            this.MessageDataGridView.AllowUserToDeleteRows = false;
            this.MessageDataGridView.AllowUserToOrderColumns = true;
            this.MessageDataGridView.AutoGenerateColumns = false;
            this.MessageDataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.MessageDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.MessageDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.timestampDataGridViewTextBoxColumn,
            this.messageDataGridViewTextBoxColumn,
            this.actionDataGridViewTextBoxColumn,
            this.levelDataGridViewTextBoxColumn});
            this.MessageDataGridView.DataSource = this.messagesBindingSource;
            resources.ApplyResources(this.MessageDataGridView, "MessageDataGridView");
            this.MessageDataGridView.Name = "MessageDataGridView";
            this.MessageDataGridView.RowHeadersVisible = false;
            this.MessageDataGridView.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.MessageDataGridView_RowsAdded);
            // 
            // timestampDataGridViewTextBoxColumn
            // 
            this.timestampDataGridViewTextBoxColumn.DataPropertyName = "Timestamp";
            resources.ApplyResources(this.timestampDataGridViewTextBoxColumn, "timestampDataGridViewTextBoxColumn");
            this.timestampDataGridViewTextBoxColumn.Name = "timestampDataGridViewTextBoxColumn";
            // 
            // messageDataGridViewTextBoxColumn
            // 
            this.messageDataGridViewTextBoxColumn.DataPropertyName = "Message";
            resources.ApplyResources(this.messageDataGridViewTextBoxColumn, "messageDataGridViewTextBoxColumn");
            this.messageDataGridViewTextBoxColumn.Name = "messageDataGridViewTextBoxColumn";
            // 
            // actionDataGridViewTextBoxColumn
            // 
            this.actionDataGridViewTextBoxColumn.DataPropertyName = "Action";
            resources.ApplyResources(this.actionDataGridViewTextBoxColumn, "actionDataGridViewTextBoxColumn");
            this.actionDataGridViewTextBoxColumn.Name = "actionDataGridViewTextBoxColumn";
            // 
            // levelDataGridViewTextBoxColumn
            // 
            this.levelDataGridViewTextBoxColumn.DataPropertyName = "Level";
            resources.ApplyResources(this.levelDataGridViewTextBoxColumn, "levelDataGridViewTextBoxColumn");
            this.levelDataGridViewTextBoxColumn.Name = "levelDataGridViewTextBoxColumn";
            // 
            // messagesBindingSource
            // 
            this.messagesBindingSource.DataMember = "Messages";
            this.messagesBindingSource.DataSource = this._dataSet;
            this.messagesBindingSource.Filter = "Visible";
            // 
            // _dataSet
            // 
            this._dataSet.DataSetName = "JobDataSet";
            this._dataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // MessageFilterToolStrip
            // 
            resources.ApplyResources(this.MessageFilterToolStrip, "MessageFilterToolStrip");
            this.MessageFilterToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton4});
            this.MessageFilterToolStrip.Name = "MessageFilterToolStrip";
            // 
            // toolStripButton4
            // 
            this.toolStripButton4.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripButton4.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            resources.ApplyResources(this.toolStripButton4, "toolStripButton4");
            this.toolStripButton4.Name = "toolStripButton4";
            this.toolStripButton4.Click += new System.EventHandler(this.toolStripButton4_Click);
            // 
            // VarsTab
            // 
            this.VarsTab.BackColor = System.Drawing.SystemColors.Window;
            this.VarsTab.Controls.Add(this.VarDataGridView);
            resources.ApplyResources(this.VarsTab, "VarsTab");
            this.VarsTab.Name = "VarsTab";
            // 
            // VarDataGridView
            // 
            this.VarDataGridView.AllowUserToAddRows = false;
            this.VarDataGridView.AllowUserToDeleteRows = false;
            this.VarDataGridView.AllowUserToResizeRows = false;
            this.VarDataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.VarDataGridView.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.VarDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.VarDataGridView.ColumnHeadersVisible = false;
            resources.ApplyResources(this.VarDataGridView, "VarDataGridView");
            this.VarDataGridView.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.VarDataGridView.Name = "VarDataGridView";
            this.VarDataGridView.ReadOnly = true;
            this.VarDataGridView.RowHeadersVisible = false;
            // 
            // RunButton
            // 
            resources.ApplyResources(this.RunButton, "RunButton");
            this.RunButton.Name = "RunButton";
            this.RunButton.UseVisualStyleBackColor = true;
            this.RunButton.Click += new System.EventHandler(this.RunButton_Click);
            // 
            // progressBar1
            // 
            resources.ApplyResources(this.progressBar1, "progressBar1");
            this.progressBar1.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.progressBar1.Name = "progressBar1";
            // 
            // menuStrip1
            // 
            resources.ApplyResources(this.menuStrip1, "menuStrip1");
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem1,
            this.editToolStripMenuItem,
            this.runToolStripMenuItem,
            this.helpToolStripMenuItem1,
            this.FavouritesMenu});
            this.menuStrip1.Name = "menuStrip1";
            // 
            // fileToolStripMenuItem1
            // 
            this.fileToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem1,
            this.openToolStripMenuItem,
            this.toolStripSeparator,
            this.saveToolStripMenuItem1,
            this.saveAsToolStripMenuItem1,
            this.toolStripSeparator4,
            this.printToolStripMenuItem1,
            this.printPreviewToolStripMenuItem,
            this.toolStripSeparator7,
            this.toolStripMenuItem1,
            this.toolStripSeparator1,
            this.exitToolStripMenuItem1});
            this.fileToolStripMenuItem1.Name = "fileToolStripMenuItem1";
            resources.ApplyResources(this.fileToolStripMenuItem1, "fileToolStripMenuItem1");
            // 
            // newToolStripMenuItem1
            // 
            resources.ApplyResources(this.newToolStripMenuItem1, "newToolStripMenuItem1");
            this.newToolStripMenuItem1.Name = "newToolStripMenuItem1";
            this.newToolStripMenuItem1.Click += new System.EventHandler(this.newToolStripMenuItem1_Click);
            // 
            // openToolStripMenuItem
            // 
            resources.ApplyResources(this.openToolStripMenuItem, "openToolStripMenuItem");
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // toolStripSeparator
            // 
            this.toolStripSeparator.Name = "toolStripSeparator";
            resources.ApplyResources(this.toolStripSeparator, "toolStripSeparator");
            // 
            // saveToolStripMenuItem1
            // 
            resources.ApplyResources(this.saveToolStripMenuItem1, "saveToolStripMenuItem1");
            this.saveToolStripMenuItem1.Name = "saveToolStripMenuItem1";
            this.saveToolStripMenuItem1.Click += new System.EventHandler(this.saveToolStripMenuItem1_Click);
            // 
            // saveAsToolStripMenuItem1
            // 
            this.saveAsToolStripMenuItem1.Name = "saveAsToolStripMenuItem1";
            resources.ApplyResources(this.saveAsToolStripMenuItem1, "saveAsToolStripMenuItem1");
            this.saveAsToolStripMenuItem1.Click += new System.EventHandler(this.saveAsToolStripMenuItem1_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            resources.ApplyResources(this.toolStripSeparator4, "toolStripSeparator4");
            // 
            // printToolStripMenuItem1
            // 
            resources.ApplyResources(this.printToolStripMenuItem1, "printToolStripMenuItem1");
            this.printToolStripMenuItem1.Name = "printToolStripMenuItem1";
            // 
            // printPreviewToolStripMenuItem
            // 
            resources.ApplyResources(this.printPreviewToolStripMenuItem, "printPreviewToolStripMenuItem");
            this.printPreviewToolStripMenuItem.Name = "printPreviewToolStripMenuItem";
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            resources.ApplyResources(this.toolStripSeparator7, "toolStripSeparator7");
            // 
            // toolStripMenuItem1
            // 
            resources.ApplyResources(this.toolStripMenuItem1, "toolStripMenuItem1");
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Click += new System.EventHandler(this.toolStripMenuItem1_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
            // 
            // exitToolStripMenuItem1
            // 
            this.exitToolStripMenuItem1.Name = "exitToolStripMenuItem1";
            resources.ApplyResources(this.exitToolStripMenuItem1, "exitToolStripMenuItem1");
            this.exitToolStripMenuItem1.Click += new System.EventHandler(this.exitToolStripMenuItem1_Click);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.undoToolStripMenuItem,
            this.redoToolStripMenuItem,
            this.toolStripSeparator8,
            this.cutToolStripMenuItem,
            this.copyToolStripMenuItem,
            this.pasteToolStripMenuItem,
            this.toolStripSeparator9,
            this.selectAllToolStripMenuItem});
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            resources.ApplyResources(this.editToolStripMenuItem, "editToolStripMenuItem");
            // 
            // undoToolStripMenuItem
            // 
            this.undoToolStripMenuItem.Name = "undoToolStripMenuItem";
            resources.ApplyResources(this.undoToolStripMenuItem, "undoToolStripMenuItem");
            // 
            // redoToolStripMenuItem
            // 
            this.redoToolStripMenuItem.Name = "redoToolStripMenuItem";
            resources.ApplyResources(this.redoToolStripMenuItem, "redoToolStripMenuItem");
            // 
            // toolStripSeparator8
            // 
            this.toolStripSeparator8.Name = "toolStripSeparator8";
            resources.ApplyResources(this.toolStripSeparator8, "toolStripSeparator8");
            // 
            // cutToolStripMenuItem
            // 
            resources.ApplyResources(this.cutToolStripMenuItem, "cutToolStripMenuItem");
            this.cutToolStripMenuItem.Name = "cutToolStripMenuItem";
            // 
            // copyToolStripMenuItem
            // 
            resources.ApplyResources(this.copyToolStripMenuItem, "copyToolStripMenuItem");
            this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            // 
            // pasteToolStripMenuItem
            // 
            resources.ApplyResources(this.pasteToolStripMenuItem, "pasteToolStripMenuItem");
            this.pasteToolStripMenuItem.Name = "pasteToolStripMenuItem";
            // 
            // toolStripSeparator9
            // 
            this.toolStripSeparator9.Name = "toolStripSeparator9";
            resources.ApplyResources(this.toolStripSeparator9, "toolStripSeparator9");
            // 
            // selectAllToolStripMenuItem
            // 
            this.selectAllToolStripMenuItem.Name = "selectAllToolStripMenuItem";
            resources.ApplyResources(this.selectAllToolStripMenuItem, "selectAllToolStripMenuItem");
            // 
            // runToolStripMenuItem
            // 
            this.runToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.runToolStripMenuItem1,
            this.abortToolStripMenuItem});
            this.runToolStripMenuItem.Name = "runToolStripMenuItem";
            resources.ApplyResources(this.runToolStripMenuItem, "runToolStripMenuItem");
            // 
            // runToolStripMenuItem1
            // 
            this.runToolStripMenuItem1.Image = global::Centipede.Properties.Resources.StatusAnnotation_Run;
            this.runToolStripMenuItem1.Name = "runToolStripMenuItem1";
            resources.ApplyResources(this.runToolStripMenuItem1, "runToolStripMenuItem1");
            this.runToolStripMenuItem1.Click += new System.EventHandler(this.RunButton_Click);
            // 
            // abortToolStripMenuItem
            // 
            this.abortToolStripMenuItem.Name = "abortToolStripMenuItem";
            resources.ApplyResources(this.abortToolStripMenuItem, "abortToolStripMenuItem");
            // 
            // helpToolStripMenuItem1
            // 
            this.helpToolStripMenuItem1.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.helpToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.contentsToolStripMenuItem,
            this.indexToolStripMenuItem,
            this.searchToolStripMenuItem,
            this.toolStripSeparator10,
            this.aboutToolStripMenuItem});
            resources.ApplyResources(this.helpToolStripMenuItem1, "helpToolStripMenuItem1");
            this.helpToolStripMenuItem1.Name = "helpToolStripMenuItem1";
            // 
            // contentsToolStripMenuItem
            // 
            this.contentsToolStripMenuItem.Name = "contentsToolStripMenuItem";
            resources.ApplyResources(this.contentsToolStripMenuItem, "contentsToolStripMenuItem");
            // 
            // indexToolStripMenuItem
            // 
            this.indexToolStripMenuItem.Name = "indexToolStripMenuItem";
            resources.ApplyResources(this.indexToolStripMenuItem, "indexToolStripMenuItem");
            // 
            // searchToolStripMenuItem
            // 
            this.searchToolStripMenuItem.Name = "searchToolStripMenuItem";
            resources.ApplyResources(this.searchToolStripMenuItem, "searchToolStripMenuItem");
            // 
            // toolStripSeparator10
            // 
            this.toolStripSeparator10.Name = "toolStripSeparator10";
            resources.ApplyResources(this.toolStripSeparator10, "toolStripSeparator10");
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            resources.ApplyResources(this.aboutToolStripMenuItem, "aboutToolStripMenuItem");
            // 
            // FavouritesMenu
            // 
            this.FavouritesMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.FavouritesToolStripMenuPlaceholder,
            this.FaveMenuSeparator,
            this.addCurrentToolStripMenuItem,
            this.EditFavouritesMenuItem});
            this.FavouritesMenu.Name = "FavouritesMenu";
            resources.ApplyResources(this.FavouritesMenu, "FavouritesMenu");
            // 
            // FavouritesToolStripMenuPlaceholder
            // 
            resources.ApplyResources(this.FavouritesToolStripMenuPlaceholder, "FavouritesToolStripMenuPlaceholder");
            this.FavouritesToolStripMenuPlaceholder.Name = "FavouritesToolStripMenuPlaceholder";
            // 
            // FaveMenuSeparator
            // 
            this.FaveMenuSeparator.Name = "FaveMenuSeparator";
            resources.ApplyResources(this.FaveMenuSeparator, "FaveMenuSeparator");
            // 
            // addCurrentToolStripMenuItem
            // 
            this.addCurrentToolStripMenuItem.Name = "addCurrentToolStripMenuItem";
            resources.ApplyResources(this.addCurrentToolStripMenuItem, "addCurrentToolStripMenuItem");
            this.addCurrentToolStripMenuItem.Click += new System.EventHandler(this.addCurrentToolStripMenuItem_Click);
            // 
            // EditFavouritesMenuItem
            // 
            this.EditFavouritesMenuItem.Name = "EditFavouritesMenuItem";
            resources.ApplyResources(this.EditFavouritesMenuItem, "EditFavouritesMenuItem");
            this.EditFavouritesMenuItem.Click += new System.EventHandler(this.EditFavouritesMenuItem_Click);
            // 
            // toolStripContainer2
            // 
            this.toolStripContainer2.BottomToolStripPanelVisible = false;
            // 
            // toolStripContainer2.ContentPanel
            // 
            resources.ApplyResources(this.toolStripContainer2.ContentPanel, "toolStripContainer2.ContentPanel");
            resources.ApplyResources(this.toolStripContainer2, "toolStripContainer2");
            this.toolStripContainer2.Name = "toolStripContainer2";
            // 
            // OtherActIcons
            // 
            this.OtherActIcons.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("OtherActIcons.ImageStream")));
            this.OtherActIcons.TransparentColor = System.Drawing.Color.Transparent;
            this.OtherActIcons.Images.SetKeyName(0, "pycon.ico");
            // 
            // updateTimer
            // 
            this.updateTimer.Enabled = true;
            // 
            // notifyIcon
            // 
            this.notifyIcon.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            resources.ApplyResources(this.notifyIcon, "notifyIcon");
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.WorkerReportsProgress = true;
            this.backgroundWorker1.WorkerSupportsCancellation = true;
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.backgroundWorker1.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker1_ProgressChanged);
            // 
            // saveFileDialog1
            // 
            resources.ApplyResources(this.saveFileDialog1, "saveFileDialog1");
            this.saveFileDialog1.FileOk += new System.ComponentModel.CancelEventHandler(this.saveFileDialog1_FileOk);
            // 
            // GetFileNameDialogue
            // 
            this.GetFileNameDialogue.AddExtension = false;
            resources.ApplyResources(this.GetFileNameDialogue, "GetFileNameDialogue");
            // 
            // OpenFileDialog
            // 
            this.OpenFileDialog.DefaultExt = "100p";
            resources.ApplyResources(this.OpenFileDialog, "OpenFileDialog");
            // 
            // BottomToolStripPanel
            // 
            resources.ApplyResources(this.BottomToolStripPanel, "BottomToolStripPanel");
            this.BottomToolStripPanel.Name = "BottomToolStripPanel";
            this.BottomToolStripPanel.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.BottomToolStripPanel.RowMargin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            // 
            // TopToolStripPanel
            // 
            resources.ApplyResources(this.TopToolStripPanel, "TopToolStripPanel");
            this.TopToolStripPanel.Name = "TopToolStripPanel";
            this.TopToolStripPanel.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.TopToolStripPanel.RowMargin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            // 
            // RightToolStripPanel
            // 
            resources.ApplyResources(this.RightToolStripPanel, "RightToolStripPanel");
            this.RightToolStripPanel.Name = "RightToolStripPanel";
            this.RightToolStripPanel.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.RightToolStripPanel.RowMargin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            // 
            // LeftToolStripPanel
            // 
            resources.ApplyResources(this.LeftToolStripPanel, "LeftToolStripPanel");
            this.LeftToolStripPanel.Name = "LeftToolStripPanel";
            this.LeftToolStripPanel.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.LeftToolStripPanel.RowMargin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            // 
            // ContentPanel
            // 
            resources.ApplyResources(this.ContentPanel, "ContentPanel");
            // 
            // miniToolStrip
            // 
            resources.ApplyResources(this.miniToolStrip, "miniToolStrip");
            this.miniToolStrip.Name = "miniToolStrip";
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.DataPropertyName = "Action";
            resources.ApplyResources(this.dataGridViewTextBoxColumn1, "dataGridViewTextBoxColumn1");
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            // 
            // favouriteJobs1
            // 
            this.favouriteJobs1.DataSetName = "FavouriteJobs";
            this.favouriteJobs1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // MainWindow
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.toolStripContainer1);
            this.Controls.Add(this.toolStripContainer2);
            this.KeyPreview = true;
            this.MainMenuStrip = this.miniToolStrip;
            this.Name = "MainWindow";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainWindow_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainWindow_FormClosed);
            this.Load += new System.EventHandler(this.MainWindow_Load);
            this.LocationChanged += new System.EventHandler(this.MainWindow_LocationChanged);
            this.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.MainWindow_PreviewKeyDown);
            this.Resize += new System.EventHandler(this.MainWindow_Resize);
            this.toolStripContainer1.ContentPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.PerformLayout();
            this.toolStripContainer1.ResumeLayout(false);
            this.toolStripContainer1.PerformLayout();
            this.SplitContainer1.Panel1.ResumeLayout(false);
            this.SplitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.SplitContainer1)).EndInit();
            this.SplitContainer1.ResumeLayout(false);
            this.SplitContainer2.Panel1.ResumeLayout(false);
            this.SplitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.SplitContainer2)).EndInit();
            this.SplitContainer2.ResumeLayout(false);
            this.toolStripContainer4.ContentPanel.ResumeLayout(false);
            this.toolStripContainer4.TopToolStripPanel.ResumeLayout(false);
            this.toolStripContainer4.TopToolStripPanel.PerformLayout();
            this.toolStripContainer4.ResumeLayout(false);
            this.toolStripContainer4.PerformLayout();
            this.NavigationToolbar.ResumeLayout(false);
            this.NavigationToolbar.PerformLayout();
            this.SplitContainer3.Panel1.ResumeLayout(false);
            this.SplitContainer3.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.SplitContainer3)).EndInit();
            this.SplitContainer3.ResumeLayout(false);
            this.VarsContextMenu.ResumeLayout(false);
            this.AddActionTabs.ResumeLayout(false);
            this.UIActTab.ResumeLayout(false);
            this.RunTabs.ResumeLayout(false);
            this.OutputTab.ResumeLayout(false);
            this.toolStripContainer3.ContentPanel.ResumeLayout(false);
            this.toolStripContainer3.TopToolStripPanel.ResumeLayout(false);
            this.toolStripContainer3.TopToolStripPanel.PerformLayout();
            this.toolStripContainer3.ResumeLayout(false);
            this.toolStripContainer3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MessageDataGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.messagesBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._dataSet)).EndInit();
            this.MessageFilterToolStrip.ResumeLayout(false);
            this.MessageFilterToolStrip.PerformLayout();
            this.VarsTab.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.VarDataGridView)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.toolStripContainer2.ResumeLayout(false);
            this.toolStripContainer2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.favouriteJobs1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        internal System.Windows.Forms.OpenFileDialog GetFileNameDialogue;
        private System.Windows.Forms.OpenFileDialog OpenFileDialog;
        internal FavouriteJobs favouriteJobs1;
        private System.Windows.Forms.BindingSource messagesBindingSource;
        private System.Windows.Forms.ToolStripContainer toolStripContainer1;
        private System.Windows.Forms.SplitContainer SplitContainer1;
        private System.Windows.Forms.TabControl RunTabs;
        private System.Windows.Forms.TabPage OutputTab;
        private System.Windows.Forms.ToolStripContainer toolStripContainer3;
        private System.Windows.Forms.DataGridView MessageDataGridView;
        private System.Windows.Forms.TabPage VarsTab;
        private System.Windows.Forms.DataGridView VarDataGridView;
        private System.Windows.Forms.Button RunButton;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem printToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem printPreviewToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem undoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem redoToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator8;
        private System.Windows.Forms.ToolStripMenuItem cutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pasteToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator9;
        private System.Windows.Forms.ToolStripMenuItem selectAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem runToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem runToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem abortToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem contentsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem indexToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem searchToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator10;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem FavouritesMenu;
        private System.Windows.Forms.ToolStripMenuItem FavouritesToolStripMenuPlaceholder;
        private System.Windows.Forms.ToolStripSeparator FaveMenuSeparator;
        private System.Windows.Forms.ToolStripMenuItem EditFavouritesMenuItem;
        private System.Windows.Forms.MenuStrip miniToolStrip;
        private System.Windows.Forms.ToolStripContainer toolStripContainer2;
        private System.Windows.Forms.ToolStrip MessageFilterToolStrip;
        private System.Windows.Forms.SplitContainer SplitContainer3;
        private System.Windows.Forms.TableLayoutPanel ActionContainer;
        private System.Windows.Forms.TabControl AddActionTabs;
        private System.Windows.Forms.TabPage UIActTab;
        private System.Windows.Forms.ListView UIActListBox;
        private System.Windows.Forms.ToolStripContainer toolStripContainer4;
        private System.Windows.Forms.WebBrowser WebBrowser;
        private System.Windows.Forms.ToolStrip NavigationToolbar;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.ToolStripButton toolStripButton3;
        private System.Windows.Forms.ToolStripButton toolStripButton2;
        private System.Windows.Forms.ToolStripPanel BottomToolStripPanel;
        private System.Windows.Forms.ToolStripPanel TopToolStripPanel;
        private System.Windows.Forms.ToolStripPanel RightToolStripPanel;
        private System.Windows.Forms.ToolStripPanel LeftToolStripPanel;
        private System.Windows.Forms.ToolStripContentPanel ContentPanel;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.SplitContainer SplitContainer2;
        private System.Windows.Forms.ToolStripMenuItem addCurrentToolStripMenuItem;
        private JobDataSet _dataSet;
        private System.Windows.Forms.DataGridViewTextBoxColumn timestampDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn messageDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn actionDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn levelDataGridViewTextBoxColumn;
        private System.Windows.Forms.ToolStripButton toolStripButton4;
    }
}

