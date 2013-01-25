namespace Centipede
{
    partial class MainWindow
    {
        private System.Windows.Forms.TabControl ActionsVarsTabControl;
        private System.Windows.Forms.TabPage ActionsTab;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TabControl AddActionTabs;
        private System.Windows.Forms.ImageList ActionIcons;
        private System.ComponentModel.IContainer components;
        private System.Windows.Forms.ImageList OtherActIcons;
        private System.Windows.Forms.TabPage VarsTab;
        private System.Windows.Forms.DataGridView VarDataGridView;
        private System.Windows.Forms.ContextMenuStrip VarsContextMenu;
        private System.Windows.Forms.ToolStripMenuItem VarMenuUndo;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripMenuItem VarMenuCut;
        private System.Windows.Forms.ToolStripMenuItem VarMenuCopy;
        private System.Windows.Forms.ToolStripMenuItem VarMenuPaste;
        private System.Windows.Forms.ToolStripMenuItem VarMenuDelete;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripMenuItem VarMenuSelectAll;
        private System.Windows.Forms.Button LoadBtn;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Button RunButton;
        private System.Windows.Forms.Button SaveButton;
        private System.Windows.Forms.Timer updateTimer;
        private System.Windows.Forms.NotifyIcon notifyIcon;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;

        #region Windows Form Designer generated code
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.ActionContainer = new System.Windows.Forms.TableLayoutPanel();
            this.AddActionTabs = new System.Windows.Forms.TabControl();
            this.UIActTab = new System.Windows.Forms.TabPage();
            this.UIActListBox = new System.Windows.Forms.ListView();
            this.ActionIcons = new System.Windows.Forms.ImageList(this.components);
            this.ActionsVarsTabControl = new System.Windows.Forms.TabControl();
            this.ActionsTab = new System.Windows.Forms.TabPage();
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
            this.OtherActIcons = new System.Windows.Forms.ImageList(this.components);
            this.LoadBtn = new System.Windows.Forms.Button();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.RunButton = new System.Windows.Forms.Button();
            this.SaveButton = new System.Windows.Forms.Button();
            this.updateTimer = new System.Windows.Forms.Timer(this.components);
            this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.jobDataSet1 = new Centipede.JobDataSet();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.GetFileNameDialogue = new System.Windows.Forms.OpenFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.AddActionTabs.SuspendLayout();
            this.UIActTab.SuspendLayout();
            this.ActionsVarsTabControl.SuspendLayout();
            this.ActionsTab.SuspendLayout();
            this.VarsTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.VarDataGridView)).BeginInit();
            this.VarsContextMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.jobDataSet1)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            resources.ApplyResources(this.splitContainer1, "splitContainer1");
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.ActionContainer);
            resources.ApplyResources(this.splitContainer1.Panel1, "splitContainer1.Panel1");
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.BackColor = System.Drawing.SystemColors.Control;
            this.splitContainer1.Panel2.Controls.Add(this.AddActionTabs);
            // 
            // ActionContainer
            // 
            this.ActionContainer.AllowDrop = true;
            resources.ApplyResources(this.ActionContainer, "ActionContainer");
            this.ActionContainer.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.ActionContainer.Name = "ActionContainer";
            this.ActionContainer.DragDrop += new System.Windows.Forms.DragEventHandler(this.ActionContainer_DragDrop);
            this.ActionContainer.DragEnter += new System.Windows.Forms.DragEventHandler(this.ActionContainer_DragEnter);
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
            // ActionsVarsTabControl
            // 
            resources.ApplyResources(this.ActionsVarsTabControl, "ActionsVarsTabControl");
            this.ActionsVarsTabControl.Controls.Add(this.ActionsTab);
            this.ActionsVarsTabControl.Controls.Add(this.VarsTab);
            this.ActionsVarsTabControl.HotTrack = true;
            this.ActionsVarsTabControl.Name = "ActionsVarsTabControl";
            this.ActionsVarsTabControl.SelectedIndex = 0;
            // 
            // ActionsTab
            // 
            this.ActionsTab.BackColor = System.Drawing.SystemColors.Window;
            this.ActionsTab.Controls.Add(this.splitContainer1);
            resources.ApplyResources(this.ActionsTab, "ActionsTab");
            this.ActionsTab.Name = "ActionsTab";
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
            this.VarDataGridView.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.VarDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.VarDataGridView.ContextMenuStrip = this.VarsContextMenu;
            resources.ApplyResources(this.VarDataGridView, "VarDataGridView");
            this.VarDataGridView.Name = "VarDataGridView";
            this.VarDataGridView.CellContextMenuStripNeeded += new System.Windows.Forms.DataGridViewCellContextMenuStripNeededEventHandler(this.VarDataGridView_CellContextMenuStripNeeded);
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
            // OtherActIcons
            // 
            this.OtherActIcons.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("OtherActIcons.ImageStream")));
            this.OtherActIcons.TransparentColor = System.Drawing.Color.Transparent;
            this.OtherActIcons.Images.SetKeyName(0, "pycon.ico");
            // 
            // LoadBtn
            // 
            resources.ApplyResources(this.LoadBtn, "LoadBtn");
            this.LoadBtn.Name = "LoadBtn";
            this.LoadBtn.UseVisualStyleBackColor = true;
            this.LoadBtn.Click += new System.EventHandler(this.LoadBtn_Click);
            // 
            // progressBar1
            // 
            resources.ApplyResources(this.progressBar1, "progressBar1");
            this.progressBar1.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            // 
            // RunButton
            // 
            resources.ApplyResources(this.RunButton, "RunButton");
            this.RunButton.Name = "RunButton";
            this.RunButton.UseVisualStyleBackColor = true;
            this.RunButton.Click += new System.EventHandler(this.RunButton_Click);
            // 
            // SaveButton
            // 
            resources.ApplyResources(this.SaveButton, "SaveButton");
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.UseVisualStyleBackColor = true;
            this.SaveButton.Click += new System.EventHandler(this.SaveButton_Click);
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
            // jobDataSet1
            // 
            this.jobDataSet1.DataSetName = "VarDataSet";
            this.jobDataSet1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
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
            // MainWindow
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.SaveButton);
            this.Controls.Add(this.RunButton);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.LoadBtn);
            this.Controls.Add(this.ActionsVarsTabControl);
            this.KeyPreview = true;
            this.Name = "MainWindow";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.Load += new System.EventHandler(this.MainWindow_Load);
            this.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.MainWindow_PreviewKeyDown);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.AddActionTabs.ResumeLayout(false);
            this.UIActTab.ResumeLayout(false);
            this.ActionsVarsTabControl.ResumeLayout(false);
            this.ActionsTab.ResumeLayout(false);
            this.VarsTab.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.VarDataGridView)).EndInit();
            this.VarsContextMenu.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.jobDataSet1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private JobDataSet jobDataSet1;
        private System.Windows.Forms.TableLayoutPanel ActionContainer;
        private System.Windows.Forms.TabPage UIActTab;
        private System.Windows.Forms.ListView UIActListBox;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        internal System.Windows.Forms.OpenFileDialog GetFileNameDialogue;
    }
}

