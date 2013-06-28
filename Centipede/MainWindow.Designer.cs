


namespace Centipede
{
    partial class MainWindow
    {
        private System.Windows.Forms.ImageList ActionIcons;
        private System.ComponentModel.IContainer components;
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
        private System.ComponentModel.BackgroundWorker BackgroundWorker;

        #region Windows Form Designer generated code
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.MainToolStripContainer = new System.Windows.Forms.ToolStripContainer();
            this.SplitContainer1 = new System.Windows.Forms.SplitContainer();
            this.SplitContainer2 = new System.Windows.Forms.SplitContainer();
            this.NaviagtionToolStripContainer = new System.Windows.Forms.ToolStripContainer();
            this.WebBrowser = new System.Windows.Forms.WebBrowser();
            this.NavigationToolbar = new System.Windows.Forms.ToolStrip();
            this.NavigationBackButton = new System.Windows.Forms.ToolStripButton();
            this.NavigationForwardButton = new System.Windows.Forms.ToolStripButton();
            this.NavigationRefresh = new System.Windows.Forms.ToolStripButton();
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
            this.RunTabs = new System.Windows.Forms.TabControl();
            this.OutputTab = new System.Windows.Forms.TabPage();
            this.OutputToolStripContainer = new System.Windows.Forms.ToolStripContainer();
            this.MessageDataGridView = new System.Windows.Forms.DataGridView();
            this.timestampDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.messageDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.levelDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.actionDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.messagesBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this._dataSet = new Centipede.JobDataSet();
            this.MessageFilterToolStrip = new System.Windows.Forms.ToolStrip();
            this.OutputClear = new System.Windows.Forms.ToolStripButton();
            this.VarsTab = new System.Windows.Forms.TabPage();
            this.VarDataGridView = new System.Windows.Forms.DataGridView();
            this.RunButton = new System.Windows.Forms.Button();
            this.ProgressBar = new System.Windows.Forms.ProgressBar();
            this.MainMenuStrip = new System.Windows.Forms.MenuStrip();
            this.FileMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.FileNewMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.FileOpenMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.FileMenuSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.FileSaveMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.FileSaveAsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.FileMenuSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.FilePrintMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.FilePrintPreviewMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.FileMenuSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.FilePropertiesMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.FileMenuSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.FileExitMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.RunMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.RunRunMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.RunAbortMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.RunMenuSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.RunResetJobMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.RunStepThroughMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.FavouritesMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.FavouritesToolStripMenuPlaceholder = new System.Windows.Forms.ToolStripMenuItem();
            this.FavoutiesMenuSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.FavouritesAddCurrentMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.FavouritesEditFavouritesMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.HelpMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.HelpContentsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.HelpIndexMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.HelpSearchMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.HelpMenuSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.HelpPythonReferenceooMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.visitGetSatisfactionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.HelpMenuSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.HelpAboutMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.MiniToolStrip = new System.Windows.Forms.MenuStrip();
            this.ActionIcons = new System.Windows.Forms.ImageList(this.components);
            this.updateTimer = new System.Windows.Forms.Timer(this.components);
            this.BackgroundWorker = new System.ComponentModel.BackgroundWorker();
            this.SaveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.GetFileNameDialogue = new System.Windows.Forms.OpenFileDialog();
            this.OpenFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.BottomToolStripPanel = new System.Windows.Forms.ToolStripPanel();
            this.TopToolStripPanel = new System.Windows.Forms.ToolStripPanel();
            this.RightToolStripPanel = new System.Windows.Forms.ToolStripPanel();
            this.LeftToolStripPanel = new System.Windows.Forms.ToolStripPanel();
            this.ContentPanel = new System.Windows.Forms.ToolStripContentPanel();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FavouriteJobs = new Centipede.FavouriteJobs();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MainToolStripContainer.ContentPanel.SuspendLayout();
            this.MainToolStripContainer.TopToolStripPanel.SuspendLayout();
            this.MainToolStripContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SplitContainer1)).BeginInit();
            this.SplitContainer1.Panel1.SuspendLayout();
            this.SplitContainer1.Panel2.SuspendLayout();
            this.SplitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SplitContainer2)).BeginInit();
            this.SplitContainer2.Panel1.SuspendLayout();
            this.SplitContainer2.Panel2.SuspendLayout();
            this.SplitContainer2.SuspendLayout();
            this.NaviagtionToolStripContainer.ContentPanel.SuspendLayout();
            this.NaviagtionToolStripContainer.TopToolStripPanel.SuspendLayout();
            this.NaviagtionToolStripContainer.SuspendLayout();
            this.NavigationToolbar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SplitContainer3)).BeginInit();
            this.SplitContainer3.Panel1.SuspendLayout();
            this.SplitContainer3.Panel2.SuspendLayout();
            this.SplitContainer3.SuspendLayout();
            this.VarsContextMenu.SuspendLayout();
            this.RunTabs.SuspendLayout();
            this.OutputTab.SuspendLayout();
            this.OutputToolStripContainer.ContentPanel.SuspendLayout();
            this.OutputToolStripContainer.TopToolStripPanel.SuspendLayout();
            this.OutputToolStripContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MessageDataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.messagesBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._dataSet)).BeginInit();
            this.MessageFilterToolStrip.SuspendLayout();
            this.VarsTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.VarDataGridView)).BeginInit();
            this.MainMenuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.FavouriteJobs)).BeginInit();
            this.SuspendLayout();
            // 
            // MainToolStripContainer
            // 
            this.MainToolStripContainer.BottomToolStripPanelVisible = false;
            // 
            // MainToolStripContainer.ContentPanel
            // 
            this.MainToolStripContainer.ContentPanel.Controls.Add(this.SplitContainer1);
            resources.ApplyResources(this.MainToolStripContainer.ContentPanel, "MainToolStripContainer.ContentPanel");
            resources.ApplyResources(this.MainToolStripContainer, "MainToolStripContainer");
            this.MainToolStripContainer.LeftToolStripPanelVisible = false;
            this.MainToolStripContainer.Name = "MainToolStripContainer";
            this.MainToolStripContainer.RightToolStripPanelVisible = false;
            // 
            // MainToolStripContainer.TopToolStripPanel
            // 
            this.MainToolStripContainer.TopToolStripPanel.Controls.Add(this.MainMenuStrip);
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
            this.SplitContainer1.Panel2.Controls.Add(this.ProgressBar);
            // 
            // SplitContainer2
            // 
            resources.ApplyResources(this.SplitContainer2, "SplitContainer2");
            this.SplitContainer2.Name = "SplitContainer2";
            // 
            // SplitContainer2.Panel1
            // 
            this.SplitContainer2.Panel1.Controls.Add(this.NaviagtionToolStripContainer);
            resources.ApplyResources(this.SplitContainer2.Panel1, "SplitContainer2.Panel1");
            // 
            // SplitContainer2.Panel2
            // 
            this.SplitContainer2.Panel2.BackColor = System.Drawing.SystemColors.Control;
            this.SplitContainer2.Panel2.Controls.Add(this.SplitContainer3);
            // 
            // NaviagtionToolStripContainer
            // 
            this.NaviagtionToolStripContainer.BottomToolStripPanelVisible = false;
            // 
            // NaviagtionToolStripContainer.ContentPanel
            // 
            this.NaviagtionToolStripContainer.ContentPanel.Controls.Add(this.WebBrowser);
            resources.ApplyResources(this.NaviagtionToolStripContainer.ContentPanel, "NaviagtionToolStripContainer.ContentPanel");
            resources.ApplyResources(this.NaviagtionToolStripContainer, "NaviagtionToolStripContainer");
            this.NaviagtionToolStripContainer.LeftToolStripPanelVisible = false;
            this.NaviagtionToolStripContainer.Name = "NaviagtionToolStripContainer";
            this.NaviagtionToolStripContainer.RightToolStripPanelVisible = false;
            // 
            // NaviagtionToolStripContainer.TopToolStripPanel
            // 
            this.NaviagtionToolStripContainer.TopToolStripPanel.Controls.Add(this.NavigationToolbar);
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
            this.NavigationBackButton,
            this.NavigationForwardButton,
            this.NavigationRefresh});
            this.NavigationToolbar.Name = "NavigationToolbar";
            this.NavigationToolbar.Stretch = true;
            // 
            // NavigationBackButton
            // 
            this.NavigationBackButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.NavigationBackButton, "NavigationBackButton");
            this.NavigationBackButton.Name = "NavigationBackButton";
            this.NavigationBackButton.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // NavigationForwardButton
            // 
            this.NavigationForwardButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.NavigationForwardButton.Name = "NavigationForwardButton";
            resources.ApplyResources(this.NavigationForwardButton, "NavigationForwardButton");
            this.NavigationForwardButton.Click += new System.EventHandler(this.toolStripButton3_Click);
            // 
            // NavigationRefresh
            // 
            this.NavigationRefresh.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.NavigationRefresh, "NavigationRefresh");
            this.NavigationRefresh.Name = "NavigationRefresh";
            this.NavigationRefresh.Click += new System.EventHandler(this.toolStripButton2_Click);
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
            this.ActionContainer.DragDrop += new System.Windows.Forms.DragEventHandler(this.ActionContainer_DragDrop);
            this.ActionContainer.DragEnter += new System.Windows.Forms.DragEventHandler(this.ActionContainer_DragEnter);
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
            resources.ApplyResources(this.AddActionTabs, "AddActionTabs");
            this.AddActionTabs.HotTrack = true;
            this.AddActionTabs.Multiline = true;
            this.AddActionTabs.Name = "AddActionTabs";
            this.AddActionTabs.SelectedIndex = 0;
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
            this.OutputTab.Controls.Add(this.OutputToolStripContainer);
            resources.ApplyResources(this.OutputTab, "OutputTab");
            this.OutputTab.Name = "OutputTab";
            this.OutputTab.UseVisualStyleBackColor = true;
            // 
            // OutputToolStripContainer
            // 
            this.OutputToolStripContainer.BottomToolStripPanelVisible = false;
            // 
            // OutputToolStripContainer.ContentPanel
            // 
            this.OutputToolStripContainer.ContentPanel.Controls.Add(this.MessageDataGridView);
            resources.ApplyResources(this.OutputToolStripContainer.ContentPanel, "OutputToolStripContainer.ContentPanel");
            resources.ApplyResources(this.OutputToolStripContainer, "OutputToolStripContainer");
            this.OutputToolStripContainer.LeftToolStripPanelVisible = false;
            this.OutputToolStripContainer.Name = "OutputToolStripContainer";
            this.OutputToolStripContainer.RightToolStripPanelVisible = false;
            // 
            // OutputToolStripContainer.TopToolStripPanel
            // 
            this.OutputToolStripContainer.TopToolStripPanel.Controls.Add(this.MessageFilterToolStrip);
            // 
            // MessageDataGridView
            // 
            this.MessageDataGridView.AllowUserToAddRows = false;
            this.MessageDataGridView.AllowUserToDeleteRows = false;
            this.MessageDataGridView.AllowUserToOrderColumns = true;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.MessageDataGridView.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.MessageDataGridView.AutoGenerateColumns = false;
            this.MessageDataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.MessageDataGridView.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.MessageDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.MessageDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.timestampDataGridViewTextBoxColumn,
            this.messageDataGridViewTextBoxColumn,
            this.levelDataGridViewTextBoxColumn,
            this.actionDataGridViewTextBoxColumn});
            this.MessageDataGridView.DataSource = this.messagesBindingSource;
            resources.ApplyResources(this.MessageDataGridView, "MessageDataGridView");
            this.MessageDataGridView.Name = "MessageDataGridView";
            this.MessageDataGridView.RowHeadersVisible = false;
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
            // levelDataGridViewTextBoxColumn
            // 
            this.levelDataGridViewTextBoxColumn.DataPropertyName = "Level";
            resources.ApplyResources(this.levelDataGridViewTextBoxColumn, "levelDataGridViewTextBoxColumn");
            this.levelDataGridViewTextBoxColumn.Name = "levelDataGridViewTextBoxColumn";
            // 
            // actionDataGridViewTextBoxColumn
            // 
            this.actionDataGridViewTextBoxColumn.DataPropertyName = "Action";
            resources.ApplyResources(this.actionDataGridViewTextBoxColumn, "actionDataGridViewTextBoxColumn");
            this.actionDataGridViewTextBoxColumn.Name = "actionDataGridViewTextBoxColumn";
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
            this.OutputClear});
            this.MessageFilterToolStrip.Name = "MessageFilterToolStrip";
            // 
            // OutputClear
            // 
            this.OutputClear.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.OutputClear.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            resources.ApplyResources(this.OutputClear, "OutputClear");
            this.OutputClear.Name = "OutputClear";
            this.OutputClear.Click += new System.EventHandler(this.toolStripButton4_Click);
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
            // ProgressBar
            // 
            resources.ApplyResources(this.ProgressBar, "ProgressBar");
            this.ProgressBar.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.ProgressBar.Name = "ProgressBar";
            // 
            // MainMenuStrip
            // 
            resources.ApplyResources(this.MainMenuStrip, "MainMenuStrip");
            this.MainMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.FileMenuItem,
            this.RunMenu,
            this.FavouritesMenu,
            this.HelpMenuItem});
            this.MainMenuStrip.Name = "MainMenuStrip";
            // 
            // FileMenuItem
            // 
            this.FileMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.FileNewMenuItem,
            this.FileOpenMenuItem,
            this.FileMenuSeparator1,
            this.FileSaveMenuItem,
            this.FileSaveAsMenuItem,
            this.FileMenuSeparator2,
            this.FilePrintMenuItem,
            this.FilePrintPreviewMenuItem,
            this.FileMenuSeparator3,
            this.FilePropertiesMenuItem,
            this.FileMenuSeparator4,
            this.FileExitMenuItem});
            this.FileMenuItem.Name = "FileMenuItem";
            resources.ApplyResources(this.FileMenuItem, "FileMenuItem");
            // 
            // FileNewMenuItem
            // 
            resources.ApplyResources(this.FileNewMenuItem, "FileNewMenuItem");
            this.FileNewMenuItem.Name = "FileNewMenuItem";
            this.FileNewMenuItem.Click += new System.EventHandler(this.newToolStripMenuItem1_Click);
            // 
            // FileOpenMenuItem
            // 
            resources.ApplyResources(this.FileOpenMenuItem, "FileOpenMenuItem");
            this.FileOpenMenuItem.Name = "FileOpenMenuItem";
            this.FileOpenMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // FileMenuSeparator1
            // 
            this.FileMenuSeparator1.Name = "FileMenuSeparator1";
            resources.ApplyResources(this.FileMenuSeparator1, "FileMenuSeparator1");
            // 
            // FileSaveMenuItem
            // 
            resources.ApplyResources(this.FileSaveMenuItem, "FileSaveMenuItem");
            this.FileSaveMenuItem.Name = "FileSaveMenuItem";
            this.FileSaveMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem1_Click);
            // 
            // FileSaveAsMenuItem
            // 
            this.FileSaveAsMenuItem.Name = "FileSaveAsMenuItem";
            resources.ApplyResources(this.FileSaveAsMenuItem, "FileSaveAsMenuItem");
            this.FileSaveAsMenuItem.Click += new System.EventHandler(this.saveAsToolStripMenuItem1_Click);
            // 
            // FileMenuSeparator2
            // 
            this.FileMenuSeparator2.Name = "FileMenuSeparator2";
            resources.ApplyResources(this.FileMenuSeparator2, "FileMenuSeparator2");
            // 
            // FilePrintMenuItem
            // 
            resources.ApplyResources(this.FilePrintMenuItem, "FilePrintMenuItem");
            this.FilePrintMenuItem.Name = "FilePrintMenuItem";
            // 
            // FilePrintPreviewMenuItem
            // 
            resources.ApplyResources(this.FilePrintPreviewMenuItem, "FilePrintPreviewMenuItem");
            this.FilePrintPreviewMenuItem.Name = "FilePrintPreviewMenuItem";
            // 
            // FileMenuSeparator3
            // 
            this.FileMenuSeparator3.Name = "FileMenuSeparator3";
            resources.ApplyResources(this.FileMenuSeparator3, "FileMenuSeparator3");
            // 
            // FilePropertiesMenuItem
            // 
            resources.ApplyResources(this.FilePropertiesMenuItem, "FilePropertiesMenuItem");
            this.FilePropertiesMenuItem.Name = "FilePropertiesMenuItem";
            this.FilePropertiesMenuItem.Click += new System.EventHandler(this.toolStripMenuItem1_Click);
            // 
            // FileMenuSeparator4
            // 
            this.FileMenuSeparator4.Name = "FileMenuSeparator4";
            resources.ApplyResources(this.FileMenuSeparator4, "FileMenuSeparator4");
            // 
            // FileExitMenuItem
            // 
            this.FileExitMenuItem.Name = "FileExitMenuItem";
            resources.ApplyResources(this.FileExitMenuItem, "FileExitMenuItem");
            this.FileExitMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem1_Click);
            // 
            // RunMenu
            // 
            this.RunMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.RunRunMenuItem,
            this.RunAbortMenuItem,
            this.RunMenuSeparator1,
            this.RunResetJobMenuItem,
            this.RunStepThroughMenuItem});
            this.RunMenu.Name = "RunMenu";
            resources.ApplyResources(this.RunMenu, "RunMenu");
            // 
            // RunRunMenuItem
            // 
            this.RunRunMenuItem.Image = global::Centipede.Properties.Resources.StatusAnnotation_Run;
            this.RunRunMenuItem.Name = "RunRunMenuItem";
            resources.ApplyResources(this.RunRunMenuItem, "RunRunMenuItem");
            this.RunRunMenuItem.Click += new System.EventHandler(this.RunButton_Click);
            // 
            // RunAbortMenuItem
            // 
            this.RunAbortMenuItem.Name = "RunAbortMenuItem";
            resources.ApplyResources(this.RunAbortMenuItem, "RunAbortMenuItem");
            this.RunAbortMenuItem.Click += new System.EventHandler(this.abortToolStripMenuItem_Click);
            // 
            // RunMenuSeparator1
            // 
            this.RunMenuSeparator1.Name = "RunMenuSeparator1";
            resources.ApplyResources(this.RunMenuSeparator1, "RunMenuSeparator1");
            // 
            // RunResetJobMenuItem
            // 
            this.RunResetJobMenuItem.Name = "RunResetJobMenuItem";
            resources.ApplyResources(this.RunResetJobMenuItem, "RunResetJobMenuItem");
            this.RunResetJobMenuItem.Click += new System.EventHandler(this.resetJobToolStripMenuItem_Click);
            // 
            // RunStepThroughMenuItem
            // 
            this.RunStepThroughMenuItem.Name = "RunStepThroughMenuItem";
            resources.ApplyResources(this.RunStepThroughMenuItem, "RunStepThroughMenuItem");
            this.RunStepThroughMenuItem.Click += new System.EventHandler(this.stepThroughToolStripMenuItem_Click);
            // 
            // FavouritesMenu
            // 
            this.FavouritesMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.FavouritesToolStripMenuPlaceholder,
            this.FavoutiesMenuSeparator,
            this.FavouritesAddCurrentMenuItem,
            this.FavouritesEditFavouritesMenuItem});
            this.FavouritesMenu.Name = "FavouritesMenu";
            resources.ApplyResources(this.FavouritesMenu, "FavouritesMenu");
            // 
            // FavouritesToolStripMenuPlaceholder
            // 
            resources.ApplyResources(this.FavouritesToolStripMenuPlaceholder, "FavouritesToolStripMenuPlaceholder");
            this.FavouritesToolStripMenuPlaceholder.Name = "FavouritesToolStripMenuPlaceholder";
            // 
            // FavoutiesMenuSeparator
            // 
            this.FavoutiesMenuSeparator.Name = "FavoutiesMenuSeparator";
            resources.ApplyResources(this.FavoutiesMenuSeparator, "FavoutiesMenuSeparator");
            // 
            // FavouritesAddCurrentMenuItem
            // 
            this.FavouritesAddCurrentMenuItem.Name = "FavouritesAddCurrentMenuItem";
            resources.ApplyResources(this.FavouritesAddCurrentMenuItem, "FavouritesAddCurrentMenuItem");
            this.FavouritesAddCurrentMenuItem.Click += new System.EventHandler(this.addCurrentToolStripMenuItem_Click);
            // 
            // FavouritesEditFavouritesMenuItem
            // 
            this.FavouritesEditFavouritesMenuItem.Name = "FavouritesEditFavouritesMenuItem";
            resources.ApplyResources(this.FavouritesEditFavouritesMenuItem, "FavouritesEditFavouritesMenuItem");
            this.FavouritesEditFavouritesMenuItem.Click += new System.EventHandler(this.EditFavouritesMenuItem_Click);
            // 
            // HelpMenuItem
            // 
            this.HelpMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.HelpContentsMenuItem,
            this.HelpIndexMenuItem,
            this.HelpSearchMenuItem,
            this.HelpMenuSeparator1,
            this.HelpPythonReferenceooMenuItem,
            this.visitGetSatisfactionToolStripMenuItem,
            this.HelpMenuSeparator2,
            this.HelpAboutMenuItem});
            this.HelpMenuItem.Name = "HelpMenuItem";
            resources.ApplyResources(this.HelpMenuItem, "HelpMenuItem");
            // 
            // HelpContentsMenuItem
            // 
            resources.ApplyResources(this.HelpContentsMenuItem, "HelpContentsMenuItem");
            this.HelpContentsMenuItem.Name = "HelpContentsMenuItem";
            // 
            // HelpIndexMenuItem
            // 
            resources.ApplyResources(this.HelpIndexMenuItem, "HelpIndexMenuItem");
            this.HelpIndexMenuItem.Name = "HelpIndexMenuItem";
            // 
            // HelpSearchMenuItem
            // 
            resources.ApplyResources(this.HelpSearchMenuItem, "HelpSearchMenuItem");
            this.HelpSearchMenuItem.Name = "HelpSearchMenuItem";
            // 
            // HelpMenuSeparator1
            // 
            this.HelpMenuSeparator1.Name = "HelpMenuSeparator1";
            resources.ApplyResources(this.HelpMenuSeparator1, "HelpMenuSeparator1");
            // 
            // HelpPythonReferenceooMenuItem
            // 
            this.HelpPythonReferenceooMenuItem.Name = "HelpPythonReferenceooMenuItem";
            resources.ApplyResources(this.HelpPythonReferenceooMenuItem, "HelpPythonReferenceooMenuItem");
            this.HelpPythonReferenceooMenuItem.Click += new System.EventHandler(this.pythonReferenceToolStripMenuItem_Click);
            // 
            // visitGetSatisfactionToolStripMenuItem
            // 
            this.visitGetSatisfactionToolStripMenuItem.Name = "visitGetSatisfactionToolStripMenuItem";
            resources.ApplyResources(this.visitGetSatisfactionToolStripMenuItem, "visitGetSatisfactionToolStripMenuItem");
            this.visitGetSatisfactionToolStripMenuItem.Click += new System.EventHandler(this.visitGetSatisfactionToolStripMenuItem_Click);
            // 
            // HelpMenuSeparator2
            // 
            this.HelpMenuSeparator2.Name = "HelpMenuSeparator2";
            resources.ApplyResources(this.HelpMenuSeparator2, "HelpMenuSeparator2");
            // 
            // HelpAboutMenuItem
            // 
            this.HelpAboutMenuItem.Name = "HelpAboutMenuItem";
            resources.ApplyResources(this.HelpAboutMenuItem, "HelpAboutMenuItem");
            this.HelpAboutMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // MiniToolStrip
            // 
            resources.ApplyResources(this.MiniToolStrip, "MiniToolStrip");
            this.MiniToolStrip.Name = "MiniToolStrip";
            // 
            // ActionIcons
            // 
            this.ActionIcons.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ActionIcons.ImageStream")));
            this.ActionIcons.TransparentColor = System.Drawing.Color.Transparent;
            this.ActionIcons.Images.SetKeyName(0, "pycon.ico");
            this.ActionIcons.Images.SetKeyName(1, "If.ico");
            // 
            // updateTimer
            // 
            this.updateTimer.Enabled = true;
            // 
            // BackgroundWorker
            // 
            this.BackgroundWorker.WorkerReportsProgress = true;
            this.BackgroundWorker.WorkerSupportsCancellation = true;
            this.BackgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.BackgroundWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker1_ProgressChanged);
            // 
            // SaveFileDialog
            // 
            resources.ApplyResources(this.SaveFileDialog, "SaveFileDialog");
            this.SaveFileDialog.FileOk += new System.ComponentModel.CancelEventHandler(this.saveFileDialog1_FileOk);
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
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.DataPropertyName = "Action";
            resources.ApplyResources(this.dataGridViewTextBoxColumn1, "dataGridViewTextBoxColumn1");
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            // 
            // FavouriteJobs
            // 
            this.FavouriteJobs.DataSetName = "FavouriteJobs";
            this.FavouriteJobs.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.DataPropertyName = "Action";
            resources.ApplyResources(this.dataGridViewTextBoxColumn2, "dataGridViewTextBoxColumn2");
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            // 
            // MainWindow
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.MainToolStripContainer);
            this.KeyPreview = true;
            this.Name = "MainWindow";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainWindow_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainWindow_FormClosed);
            this.Load += new System.EventHandler(this.MainWindow_Load);
            this.LocationChanged += new System.EventHandler(this.MainWindow_LocationChanged);
            this.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.MainWindow_PreviewKeyDown);
            this.Resize += new System.EventHandler(this.MainWindow_Resize);
            this.MainToolStripContainer.ContentPanel.ResumeLayout(false);
            this.MainToolStripContainer.TopToolStripPanel.ResumeLayout(false);
            this.MainToolStripContainer.TopToolStripPanel.PerformLayout();
            this.MainToolStripContainer.ResumeLayout(false);
            this.MainToolStripContainer.PerformLayout();
            this.SplitContainer1.Panel1.ResumeLayout(false);
            this.SplitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.SplitContainer1)).EndInit();
            this.SplitContainer1.ResumeLayout(false);
            this.SplitContainer2.Panel1.ResumeLayout(false);
            this.SplitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.SplitContainer2)).EndInit();
            this.SplitContainer2.ResumeLayout(false);
            this.NaviagtionToolStripContainer.ContentPanel.ResumeLayout(false);
            this.NaviagtionToolStripContainer.TopToolStripPanel.ResumeLayout(false);
            this.NaviagtionToolStripContainer.TopToolStripPanel.PerformLayout();
            this.NaviagtionToolStripContainer.ResumeLayout(false);
            this.NaviagtionToolStripContainer.PerformLayout();
            this.NavigationToolbar.ResumeLayout(false);
            this.NavigationToolbar.PerformLayout();
            this.SplitContainer3.Panel1.ResumeLayout(false);
            this.SplitContainer3.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.SplitContainer3)).EndInit();
            this.SplitContainer3.ResumeLayout(false);
            this.VarsContextMenu.ResumeLayout(false);
            this.RunTabs.ResumeLayout(false);
            this.OutputTab.ResumeLayout(false);
            this.OutputToolStripContainer.ContentPanel.ResumeLayout(false);
            this.OutputToolStripContainer.TopToolStripPanel.ResumeLayout(false);
            this.OutputToolStripContainer.TopToolStripPanel.PerformLayout();
            this.OutputToolStripContainer.ResumeLayout(false);
            this.OutputToolStripContainer.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MessageDataGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.messagesBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._dataSet)).EndInit();
            this.MessageFilterToolStrip.ResumeLayout(false);
            this.MessageFilterToolStrip.PerformLayout();
            this.VarsTab.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.VarDataGridView)).EndInit();
            this.MainMenuStrip.ResumeLayout(false);
            this.MainMenuStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.FavouriteJobs)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SaveFileDialog SaveFileDialog;
        internal System.Windows.Forms.OpenFileDialog GetFileNameDialogue;
        private System.Windows.Forms.OpenFileDialog OpenFileDialog;
        internal FavouriteJobs FavouriteJobs;
        private System.Windows.Forms.BindingSource messagesBindingSource;
        private System.Windows.Forms.ToolStripContainer MainToolStripContainer;
        private System.Windows.Forms.SplitContainer SplitContainer1;
        private System.Windows.Forms.TabControl RunTabs;
        private System.Windows.Forms.TabPage OutputTab;
        private System.Windows.Forms.ToolStripContainer OutputToolStripContainer;
        private System.Windows.Forms.DataGridView MessageDataGridView;
        private System.Windows.Forms.TabPage VarsTab;
        private System.Windows.Forms.DataGridView VarDataGridView;
        private System.Windows.Forms.Button RunButton;
        private System.Windows.Forms.ProgressBar ProgressBar;
        private System.Windows.Forms.MenuStrip MainMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem FileMenuItem;
        private System.Windows.Forms.ToolStripMenuItem FileNewMenuItem;
        private System.Windows.Forms.ToolStripMenuItem FileOpenMenuItem;
        private System.Windows.Forms.ToolStripSeparator FileMenuSeparator1;
        private System.Windows.Forms.ToolStripMenuItem FileSaveMenuItem;
        private System.Windows.Forms.ToolStripMenuItem FileSaveAsMenuItem;
        private System.Windows.Forms.ToolStripSeparator FileMenuSeparator2;
        private System.Windows.Forms.ToolStripMenuItem FilePrintMenuItem;
        private System.Windows.Forms.ToolStripMenuItem FilePrintPreviewMenuItem;
        private System.Windows.Forms.ToolStripSeparator FileMenuSeparator3;
        private System.Windows.Forms.ToolStripMenuItem FilePropertiesMenuItem;
        private System.Windows.Forms.ToolStripSeparator FileMenuSeparator4;
        private System.Windows.Forms.ToolStripMenuItem FileExitMenuItem;
        private System.Windows.Forms.ToolStripMenuItem RunMenu;
        private System.Windows.Forms.ToolStripMenuItem RunRunMenuItem;
        private System.Windows.Forms.ToolStripMenuItem RunAbortMenuItem;
        private System.Windows.Forms.ToolStripMenuItem HelpMenuItem;
        private System.Windows.Forms.ToolStripMenuItem HelpContentsMenuItem;
        private System.Windows.Forms.ToolStripMenuItem HelpIndexMenuItem;
        private System.Windows.Forms.ToolStripMenuItem HelpSearchMenuItem;
        private System.Windows.Forms.ToolStripSeparator HelpMenuSeparator2;
        private System.Windows.Forms.ToolStripMenuItem HelpAboutMenuItem;
        private System.Windows.Forms.ToolStripMenuItem FavouritesMenu;
        private System.Windows.Forms.ToolStripMenuItem FavouritesToolStripMenuPlaceholder;
        private System.Windows.Forms.ToolStripSeparator FavoutiesMenuSeparator;
        private System.Windows.Forms.ToolStripMenuItem FavouritesEditFavouritesMenuItem;
        private System.Windows.Forms.MenuStrip MiniToolStrip;
        private System.Windows.Forms.ToolStrip MessageFilterToolStrip;
        private System.Windows.Forms.SplitContainer SplitContainer3;
        private System.Windows.Forms.TableLayoutPanel ActionContainer;
        private System.Windows.Forms.ToolStripContainer NaviagtionToolStripContainer;
        private System.Windows.Forms.WebBrowser WebBrowser;
        private System.Windows.Forms.ToolStrip NavigationToolbar;
        private System.Windows.Forms.ToolStripButton NavigationBackButton;
        private System.Windows.Forms.ToolStripButton NavigationForwardButton;
        private System.Windows.Forms.ToolStripButton NavigationRefresh;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.SplitContainer SplitContainer2;
        private System.Windows.Forms.ToolStripMenuItem FavouritesAddCurrentMenuItem;
        private JobDataSet _dataSet;
        private System.Windows.Forms.DataGridViewTextBoxColumn timestampDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn messageDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn actionDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn levelDataGridViewTextBoxColumn;
        private System.Windows.Forms.ToolStripButton OutputClear;
        private System.Windows.Forms.TabControl AddActionTabs;
        private System.Windows.Forms.ToolStripSeparator HelpMenuSeparator1;
        private System.Windows.Forms.ToolStripMenuItem visitGetSatisfactionToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator RunMenuSeparator1;
        private System.Windows.Forms.ToolStripMenuItem RunStepThroughMenuItem;
        private System.Windows.Forms.ToolStripMenuItem RunResetJobMenuItem;
        private System.Windows.Forms.ToolStripMenuItem HelpPythonReferenceooMenuItem;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.ToolStripPanel BottomToolStripPanel;
        private System.Windows.Forms.ToolStripPanel TopToolStripPanel;
        private System.Windows.Forms.ToolStripPanel RightToolStripPanel;
        private System.Windows.Forms.ToolStripPanel LeftToolStripPanel;
        private System.Windows.Forms.ToolStripContentPanel ContentPanel;
    }
}

