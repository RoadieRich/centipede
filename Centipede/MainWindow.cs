using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
//using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Policy;
using System.Text;
using System.Windows.Forms;
using Centipede.Actions;


namespace Centipede
{


    public partial class MainWindow : Form
    {
        public MainWindow()
        {
            InitializeComponent();

            Instance = this;

        }

        private Boolean ErrorHandler(ActionException e, ref Action nextAction)
        {
            StringBuilder messageBuilder = new StringBuilder();

            if (e.ErrorAction != null)
            {
                UpdateHandlerDone(e.ErrorAction);
                ActionDisplayControl adc = e.ErrorAction.Tag as ActionDisplayControl;
                if (adc.InvokeRequired)
                {
                    adc.Invoke(SetActionDisplayState, adc, ActionState.Error, e.Message);
                }
                else
                {
                    SetActionDisplayState(adc, ActionState.Error, e.Message);
                }

                messageBuilder.AppendLine("Error occurred in ");
                messageBuilder.Append(e.ErrorAction.Name).Append(" (");
                messageBuilder.Append(e.ErrorAction.Comment).AppendLine(")");
                messageBuilder.AppendLine();
                messageBuilder.AppendLine("Message was:");
                messageBuilder.AppendLine(e.Message);

            }
            else
            {
                messageBuilder.AppendLine("Error:");
                messageBuilder.AppendLine();
                messageBuilder.AppendLine(e.Message);
            }

            DialogResult result = MessageBox.Show(
                messageBuilder.ToString(),
                "Error",
                MessageBoxButtons.AbortRetryIgnore,
                MessageBoxIcon.Exclamation
            );

            if (e.ErrorAction == null)
            {
                nextAction = null;
                return false;
            }
            else
            {
                switch (result)
                {
                    case System.Windows.Forms.DialogResult.Abort:
                        nextAction = null;
                        return false;
                    case System.Windows.Forms.DialogResult.Retry:
                        nextAction = e.ErrorAction;
                        return true;
                    case System.Windows.Forms.DialogResult.Ignore:
                        nextAction = e.ErrorAction.GetNext();
                        return true;
                    default:
                        nextAction = null;
                        return false;
                }
            }
        }

        private void UpdateHandlerDone(Action currentAction)
        {
            foreach (KeyValuePair<String, Object> v in Program.Variables.ToArray())
            {
                if (v.Key.StartsWith("_"))
                {
                    continue;
                }
                JobDataSet.VariablesRow row = _dataSet.Variables.FindByName(v.Key);
                if (row != null)
                {
                    if (row.Value != v.Value)
                    {
                        row.Value = v.Value;
                        //row.SetModified();
                    }
                }
                else
                {
                    _dataSet.Variables.AddVariablesRow(v.Key, v.Value, 0);
                }
            }
            ActionDisplayControl adc = currentAction.Tag as ActionDisplayControl;
            if (adc.InvokeRequired)
            {
                adc.Invoke(SetActionDisplayState, adc, ActionState.Completed, "Completed");
            }
            else
            {
                (adc).State = ActionState.Completed;
            }

            //this is supposed to be a percentage, but this works.
            backgroundWorker1.ReportProgress(currentAction.Complexity);

        }

        private void CompletedHandler(Boolean success)
        {
            String message;
            MessageBoxIcon icon;
            if (success)
            {
                message = "Job finished successfully.";
                icon = MessageBoxIcon.Information;
            }
            else
            {
                message = "Job did not finish.";
                icon = MessageBoxIcon.Error;
            }
            MessageBox.Show(message, "Finished", MessageBoxButtons.OK, icon);
        }

        private void ItemActivate(object sender, EventArgs e)
        {
            ListView sendingListView = sender as ListView;

            ActionFactory sendingActionFactory = sendingListView.SelectedItems[0] as ActionFactory;

            Program.AddAction(sendingActionFactory.Generate());
        }

        private JobDataSet _dataSet = new JobDataSet();
        private delegate void SetStateDeligate(ActionDisplayControl adc, ActionState state, String message);

        private static void SetActionDisplayedState(ActionDisplayControl adc, ActionState state, String message)
        {
            lock (adc)
            {
                adc.State = state;
                if (message != "")
                {
                    adc.StatusMessage = message;
                }
            }
        }

        private SetStateDeligate SetActionDisplayState = new SetStateDeligate(SetActionDisplayedState);

        public delegate void ShowMessageBoxDelegate(String message);
        public void ShowMessageBox(string message)
        {
            System.Windows.Forms.MessageBox.Show(message);
        }

        private void MainWindow_Load(object sender, EventArgs e)
        {
            UIActListBox.LargeImageList.Images.Add("generic", Properties.Resources.generic);

            ActionFactory af = new ActionFactory("Demo Action", typeof(DemoAction));
            af.ImageKey = "generic";

            UIActListBox.Items.Add(af);

            af = new ActionFactory("Get Filename", typeof(GetFileNameAction));
            af.ImageKey = "generic";

            UIActListBox.Items.Add(af);

            af = new ActionFactory("Show Messagebox", typeof(ShowMessageBox));
            af.ImageKey = "generic";

            UIActListBox.Items.Add(af);

            GetActionPlugins();


            VarDataGridView.DataSource = _dataSet.Variables;
            _dataSet.Variables.VariablesRowChanged += new JobDataSet.VariablesRowChangeEventHandler(Variables_VariablesRowChanged);
            _dataSet.Variables.RowDeleted += new DataRowChangeEventHandler(Variables_RowDeleted);

            foreach (RowStyle s in ActionContainer.RowStyles)
            {
                s.Height = 20f;
                s.SizeType = SizeType.AutoSize;
            }

            Program.ActionCompleted += new Program.ActionUpdateCallback(UpdateHandlerDone);
            Program.BeforeAction += new Program.ActionUpdateCallback(Program_BeforeAction);
            Program.JobCompleted += new Program.CompletedHandler(CompletedHandler);
            Program.ActionErrorOccurred += new Program.ErrorHandler(ErrorHandler);
            Program.ActionAdded += new Program.AddActionCallback(Program_ActionAdded);
            Program.ActionRemoved += new Program.ActionRemovedHandler(Program_ActionRemoved);
            Program.AfterLoad += new Program.AfterLoadEventHandler(Program_AfterLoad);




            if (Program.JobFileName.Length <= 0)
                LoadJob();
            this.Dirty = false;
        }

        private void LoadJob()
        {
            GetJob startWindow = new GetJob();
            startWindow.ShowDialog();

            switch (startWindow.Result)
            {
                case GetJobResult.Open:
                    String fileName = startWindow.getJobFileName();

                    Program.LoadJob(fileName);
                    break;

                case GetJobResult.New:
                    Program.JobFileName = "";
                    Program.JobName = "";
                    Program.Clear();
                    break;
                default:
                    break;
            }
        }

        private void GetActionPlugins()
        {
            DirectoryInfo di = new DirectoryInfo(Path.Combine(Application.StartupPath, Properties.Settings.Default.PluginFolder));

            var dlls = di.EnumerateFiles("*.dll", SearchOption.AllDirectories);

            foreach (FileInfo fi in dlls)
            {
                Evidence evidence = new Evidence();
                ApplicationDirectory appDir = new ApplicationDirectory(Assembly.GetEntryAssembly().CodeBase);
                evidence.AddAssemblyEvidence(appDir);
                Assembly asm;
                try
                {
                    asm = Assembly.LoadFrom(fi.FullName);
                }
                catch (BadImageFormatException)
                {
                    continue;
                }
                Type[] pluginsInFile = asm.GetExportedTypes();
                foreach (Type pluginType in pluginsInFile)
                {
                    if (pluginType.IsAbstract)
                    {
                        continue;
                    }
                    Object[] customAttributes = pluginType.GetCustomAttributes(typeof(Centipede.ActionCategoryAttribute), true);
                    if (customAttributes.Count() > 0)
                    {
                        ActionCategoryAttribute catAttribute = customAttributes[0] as ActionCategoryAttribute;

                        TabPage tabPage = null;
                        foreach (TabPage _tabPage in AddActionTabs.TabPages)
                        {
                            if (_tabPage.Text == catAttribute.category)
                            {
                                tabPage = _tabPage;
                                break;
                            }
                        }

                        ListView catListView;
                        if (tabPage != null)
                        {
                            catListView = tabPage.Tag as ListView;
                        }
                        else
                        {
                            catListView = GenerateNewTabPage(catAttribute.category);
                        }


                        ActionFactory af = new ActionFactory(catAttribute, pluginType);

                        if (catAttribute.iconName != "")
                        {
                            String[] names = asm.GetManifestResourceNames();
                            var t = asm.GetType(pluginType.Namespace + ".Properties.Resources");
                            System.Drawing.Icon icon;
                            if (t != null)
                            {

                                try
                                {
                                    icon = t.GetProperty(catAttribute.iconName, typeof(System.Drawing.Icon)).GetValue(t, null) as System.Drawing.Icon;
                                    catListView.LargeImageList.Images.Add(pluginType.Name, icon);
                                    af.ImageKey = pluginType.Name;
                                }
                                catch (Exception)
                                {
                                    af.ImageKey = "generic";
                                }
                            }
                            else
                            {
                                af.ImageKey = "generic";
                            }


                        }

                        catListView.Items.Add(af);
                    }
                }

            }
        }

        void Program_AfterLoad(object sender, EventArgs e)
        {
            this.Dirty = false;
            this.Text = Program.JobName;
        }

        public new String Text
        {
            get
            {
                return base.Text;
            }
            set
            {
                String dirtyMarker = "";
                if (this.Dirty)
                {
                    dirtyMarker = " *";
                }
                base.Text = String.Format("Centipede - {0}{1}", new object[] { value, dirtyMarker });
            }
        }

        void Program_ActionRemoved(Action action)
        {
            ActionDisplayControl adc = null;
            foreach (ActionDisplayControl a in ActionContainer.Controls)
            {
                if (a.ThisAction == action)
                {
                    adc = a;
                    break;
                }
            }
            ActionContainer.Controls.Remove(adc);
            this.Dirty = true;
        }

        private ListView GenerateNewTabPage(String category)
        {
            ListView lv = new ListView();

            lv.LargeImageList = ActionIcons;
            lv.SmallImageList = ActionIcons;
            TabPage tabPage = new TabPage(category);
            tabPage.Controls.Add(lv);
            lv.Dock = DockStyle.Fill;
            tabPage.Tag = lv;
            lv.ItemDrag += new ItemDragEventHandler(BeginDrag);
            lv.ItemActivate += new EventHandler(ItemActivate);
            AddActionTabs.TabPages.Add(tabPage);


            return lv;
        }

        void Program_BeforeAction(Action action)
        {
            ActionDisplayControl adc = action.Tag as ActionDisplayControl;
            if (adc.InvokeRequired)
            {
                adc.Invoke(SetActionDisplayState, adc, ActionState.Running, "Running");
            }
            else
            {
                adc.State = ActionState.Running;
            }
        }

        void Program_ActionAdded(Action action, int index)
        {
            Type actionType = action.GetType();
            ActionCategoryAttribute actionAttribute = actionType.GetCustomAttributes(true)[0] as ActionCategoryAttribute;
            ActionDisplayControl adc;
            if (actionAttribute.displayControl != null)
            {
                ConstructorInfo constructor = actionType.Assembly.GetType(action.GetType().Namespace + "." + actionAttribute.displayControl).GetConstructor(new Type[] { typeof(Action) });
                adc = constructor.Invoke(new object[] { action }) as ActionDisplayControl;
            }
            else
            {
                adc = new ActionDisplayControl(action);
            }
            adc.Deleted += new ActionDisplayControl.DeletedEventHandler(adc_Deleted);
            adc.DragEnter += new DragEventHandler(ActionContainer_DragEnter);
            adc.DragDrop += new DragEventHandler(ActionContainer_DragDrop);
            ActionContainer.Controls.Add(adc, 0, index);
            ActionContainer.SetRow(adc, index);
            this.Dirty = true;
        }

        void adc_Deleted(object sender, CentipedeEventArgs e)
        {
            ActionDisplayControl adc = sender as ActionDisplayControl;
            Program.RemoveAction(adc.ThisAction);
        }

        void Variables_RowDeleted(object sender, DataRowChangeEventArgs e)
        {
            foreach (String key in (from kvp in Program.Variables
                                    where _dataSet.Variables.Rows.Contains(kvp.Key)
                                    select kvp.Key))
            {
                if (key != "console")
                {
                    Program.Variables.Remove(key);
                    break;
                }
            }

        }

        void Variables_VariablesRowChanged(object sender, JobDataSet.VariablesRowChangeEvent e)
        {
            JobDataSet.VariablesDataTable table = sender as JobDataSet.VariablesDataTable;
            JobDataSet.VariablesRow row = e.Row as JobDataSet.VariablesRow;

            if (!Program.Variables.ContainsKey(row.Name))
            {
                var it = from kvp in Program.Variables
                         where !_dataSet.Variables.Rows.Contains(kvp.Key)
                         select kvp.Key;

                foreach (String key in it)
                {
                    if (key != "console")
                    {
                        Program.Variables.Remove(key);
                        break;
                    }
                }

            }

            Program.Variables[row.Name] = row.Value;

        }

        private void VarMenuDelete_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow r in VarDataGridView.SelectedRows)
            {
                _dataSet.Variables.RemoveVariablesRow((r.DataBoundItem as DataRowView).Row as JobDataSet.VariablesRow);
            }
        }

        private void VarDataGridView_CellContextMenuStripNeeded(object sender, DataGridViewCellContextMenuStripNeededEventArgs e)
        {
            DataGridView view = sender as DataGridView;
            view.ClearSelection();
            view.Rows[e.RowIndex].Selected = true;
            e.ContextMenuStrip = VarsContextMenu;
            e.ContextMenuStrip.Show();
        }

        private void LoadBtn_Click(object sender, EventArgs e)
        {
            if (this.Dirty)
            {
                DialogResult result = MessageBox.Show(this, "Save changes?", "Unsaved Changes", MessageBoxButtons.YesNoCancel);
                switch (result)
                {
                    case System.Windows.Forms.DialogResult.Yes:
                        SaveJob();
                        break;
                    case System.Windows.Forms.DialogResult.No:
                        // do nothing
                        break;
                    default:
                        return;
                }
            }

            LoadJob();
        }

        private void RunButton_Click(object sender, EventArgs e)
        {
            foreach (ActionDisplayControl adc in ActionContainer.Controls)
            {
                adc.State = ActionState.None;
            }
            progressBar1.Value = 0;
            progressBar1.Maximum = Program.JobComplexity;
            backgroundWorker1.RunWorkerAsync();
        }

        [STAThread]
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            Program.RunJob();
        }

        private void FlowControlToolbar_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void copyToolStripButton_Click(object sender, EventArgs e)
        {
            ToolStripButton btn = sender as ToolStripButton;
        }

        private void MainWindow_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyData == Keys.F5)
            {
                RunButton.PerformClick();
            }
        }

        private void ActionContainer_DragDrop(object sender, DragEventArgs e)
        {
            Int32 index = -1;
            if (sender is ActionDisplayControl)
            {
                index = ActionContainer.GetPositionFromControl(sender as ActionDisplayControl).Row;
            }
            var s = e.Data.GetFormats();
            var data = e.Data.GetData("WindowsForms10PersistentObject");
            Program.AddAction((data as ActionFactory).Generate(), index);

        }

        private void BeginDrag(object sender, ItemDragEventArgs e)
        {

            DoDragDrop(e.Item, DragDropEffects.Move);
        }

        private void ActionContainer_DragEnter(object sender, DragEventArgs e)
        {
            //TODO: Check type in here?
            e.Effect = DragDropEffects.Move;
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            SaveJob();
        }

        private void SaveJob()
        {
            if (Program.JobName == "")
            {
                AskJobTitle askJobTitle = new AskJobTitle();
                DialogResult result = askJobTitle.ShowDialog(this);
                if (result == System.Windows.Forms.DialogResult.Cancel)
                {
                    return;
                }
            }
            saveFileDialog1.FileName = Program.JobFileName;
            saveFileDialog1.ShowDialog(this);
        }

        private void saveFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            Program.SaveJob(saveFileDialog1.FileName);
            this.Dirty = false;
        }

        public static MainWindow Instance;
        private bool _dirty;
        private bool Dirty
        {
            get
            {
                return _dirty;
            }
            set
            {
                _dirty = value;
                this.Text = Program.JobName;
            }
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            //It's not really a percentage, but it serves the purpose
            this.progressBar1.Increment(e.ProgressPercentage);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int a = 0;
            a += 1;
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            Program.Dispose(disposing);
        }
    }
    class GuiConsole
    {
        public void write(string message)
        {
            Program.mainForm.Invoke(new MainWindow.ShowMessageBoxDelegate(MainWindow.Instance.ShowMessageBox), new object[] { message });
            MessageBox.Show(Program.mainForm, message, "Python Output");
        }
    }
}
