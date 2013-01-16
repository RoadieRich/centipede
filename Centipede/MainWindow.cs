using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Policy;
using System.Text;
using System.Windows.Forms;
using Centipede.Actions;
using Centipede.Properties;


namespace Centipede
{
    public partial class MainWindow : Form
    {
        public MainWindow()
        {
            InitializeComponent();

            Instance = this;

        }

        private Boolean ErrorHandler(ActionException e, out Action nextAction)
        {
            var messageBuilder = new StringBuilder();

            if (e.ErrorAction != null)
            {
                UpdateHandlerDone(e.ErrorAction);
                var adc = e.ErrorAction.Tag as ActionDisplayControl;
                if (adc != null && adc.InvokeRequired)
                {
                    adc.Invoke(_setActionDisplayState, adc, ActionState.Error, e.Message);
                }
                else
                {
                    _setActionDisplayState(adc, ActionState.Error, e.Message);
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
                Resources.MainWindow_ErrorHandler_Error,
                MessageBoxButtons.AbortRetryIgnore,
                MessageBoxIcon.Exclamation
            );

            if (e.ErrorAction == null)
            {
                nextAction = null;
                return false;
            }
            switch (result)
            {
            case DialogResult.Abort:
                nextAction = null;
                return false;
            case DialogResult.Retry:
                nextAction = e.ErrorAction;
                return true;
            case DialogResult.Ignore:
                nextAction = e.ErrorAction.GetNext();
                return true;
            default:
                nextAction = null;
                return false;
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
                    row.Value = v.Value;
                    //row.SetModified();
                }
                else
                {
                    _dataSet.Variables.AddVariablesRow(v.Key, v.Value, 0);
                }
            }
            var adc = currentAction.Tag as ActionDisplayControl;
            if (adc.InvokeRequired)
            {
                adc.Invoke(_setActionDisplayState, adc, ActionState.Completed, "Completed");
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
            MessageBox.Show(message, Resources.MainWindow_CompletedHandler_Finished, MessageBoxButtons.OK, icon);
        }

        private void ItemActivate(object sender, EventArgs e)
        {
            var sendingListView = sender as ListView;

            var sendingActionFactory = sendingListView.SelectedItems[0] as ActionFactory;

            Program.AddAction(sendingActionFactory.Generate());
        }

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

        private readonly SetStateDeligate _setActionDisplayState = SetActionDisplayedState;

        private void MainWindow_Load(object sender, EventArgs e)
        {
            UIActListBox.LargeImageList.Images.Add(@"generic", Resources.generic);

            var af = new ActionFactory("Demo Action", typeof(DemoAction));

            UIActListBox.Items.Add(af);

            af = new ActionFactory("Get Filename", typeof(GetFileNameAction));

            UIActListBox.Items.Add(af);

            af = new ActionFactory("Show Messagebox", typeof(ShowMessageBox));

            UIActListBox.Items.Add(af);

            af = new ActionFactory("Branch", typeof(BranchAction));

            FlowControlListBox.Items.Add(af);

            af = new ActionFactory("Variable", typeof(VariableAction)); 

            FlowControlListBox.Items.Add(af);

            GetActionPlugins();


            VarDataGridView.DataSource = _dataSet.Variables;
            _dataSet.Variables.VariablesRowChanged += Variables_VariablesRowChanged;
            _dataSet.Variables.RowDeleted += DataStore_Variables_RowDeleted;

            foreach (RowStyle s in ActionContainer.RowStyles)
            {
                s.Height = 20f;
                s.SizeType = SizeType.AutoSize;
            }

            Program.ActionCompleted += UpdateHandlerDone;
            Program.BeforeAction += Program_BeforeAction;
            Program.JobCompleted += CompletedHandler;
            Program.ActionErrorOccurred += ErrorHandler;
            Program.ActionAdded += Program_ActionAdded;
            Program.ActionRemoved += Program_ActionRemoved;
            Program.AfterLoad += Program_AfterLoad;




            if (Program.JobFileName.Length <= 0)
                LoadJob();
            Dirty = false;
        }

        private void LoadJob()
        {
            var startWindow = new GetJob();
            startWindow.ShowDialog();

            switch (startWindow.Result)
            {
                case GetJobResult.Open:
                    String fileName = startWindow.GetJobFileName();

                    Program.LoadJob(fileName);
                    break;

                case GetJobResult.New:
                    Program.JobFileName = "";
                    Program.JobName = "";
                    Program.Clear();
                    break;
            }
        }

        private void GetActionPlugins()
        {
            var di = new DirectoryInfo(Path.Combine(Application.StartupPath, Settings.Default.PluginFolder));

            var dlls = di.EnumerateFiles("*.dll", SearchOption.AllDirectories);

            foreach (FileInfo fi in dlls)
            {
                var evidence = new Evidence();
                var appDir = new ApplicationDirectory(Assembly.GetEntryAssembly().CodeBase);
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
                    
                    if (pluginType.GetCustomAttributes(typeof(ActionCategoryAttribute), true).Any())
                    {
                        AddToActionTab(pluginType);
                    }
                }

            }
        }

        private void AddToActionTab(Type pluginType)
        {

            Assembly asm = pluginType.Assembly;
            Object[] customAttributes = pluginType.GetCustomAttributes(typeof(ActionCategoryAttribute), true);

            var catAttribute = customAttributes[0] as ActionCategoryAttribute;

            TabPage tabPage = AddActionTabs.TabPages.Cast<TabPage>()
                                           .FirstOrDefault(tp => tp.Text == catAttribute.category);

            ListView catListView;
            if (tabPage != null)
            {
                catListView = tabPage.Tag as ListView;
            }
            else
            {
                catListView = GenerateNewTabPage(catAttribute.category);
            }


            var af = new ActionFactory(catAttribute, pluginType);

            if (catAttribute.iconName != "")
            {
                var t = asm.GetType(pluginType.Namespace + ".Properties.Resources");
                if (t != null)
                {
                    var icon = t.GetProperty(catAttribute.iconName, typeof(System.Drawing.Icon)).GetValue(t, null) as System.Drawing.Icon;
                    if (icon != null)
                    {
                        catListView.LargeImageList.Images.Add(pluginType.Name, icon);
                        af.ImageKey = pluginType.Name;
                    }
                }
            }

            catListView.Items.Add(af);
        }

        void Program_AfterLoad(object sender, EventArgs e)
        {
            Dirty = false;
            Text = Program.JobName;
        }

        private new String Text
        {
            set
            {
                String dirtyMarker = "";
                if (Dirty)
                {
                    dirtyMarker = " *";
                }
                base.Text = String.Format("Centipede - {0}{1}", new object[] { value, dirtyMarker });
            }
        }

        void Program_ActionRemoved(Action action)
        {
            ActionDisplayControl adc = ActionContainer.Controls
                                                      .Cast<ActionDisplayControl>()
                                                      .FirstOrDefault(a => a.ThisAction == action);
            if (adc != null)
            {
                ActionContainer.Controls.Remove(adc);
                Dirty = true;
            }
        }

        private ListView GenerateNewTabPage(String category)
        {
            var lv = new ListView { LargeImageList = ActionIcons, SmallImageList = ActionIcons };

            var tabPage = new TabPage(category);
            tabPage.Controls.Add(lv);
            lv.Dock = DockStyle.Fill;
            tabPage.Tag = lv;
            lv.ItemDrag += BeginDrag;
            lv.ItemActivate += ItemActivate;
            AddActionTabs.TabPages.Add(tabPage);


            return lv;
        }

        void Program_BeforeAction(Action action)
        {
            var adc = action.Tag as ActionDisplayControl;
            if (adc.InvokeRequired)
            {
                adc.Invoke(_setActionDisplayState, adc, ActionState.Running, "Running");
            }
            else
            {
                adc.State = ActionState.Running;
            }
        }

        void Program_ActionAdded(Action action, int index)
        {
            Type actionType = action.GetType();
            var actionAttribute = actionType.GetCustomAttributes(true)[0] as ActionCategoryAttribute;
            ActionDisplayControl adc;
            if (actionAttribute != null && actionAttribute.displayControl != null)
            {
                ConstructorInfo constructor = actionType.Assembly.GetType(string.Format("{0}.{1}",
                                                                                        action.GetType().Namespace,
                                                                                        actionAttribute.displayControl))
                                                        .GetConstructor(new[] { typeof (Action) });

                adc = constructor.Invoke(new object[] { action }) as ActionDisplayControl;
            }
            else
            {
                adc = new ActionDisplayControl(action);
            }
            adc.Deleted += adc_Deleted;
            adc.DragEnter += ActionContainer_DragEnter;
            adc.DragDrop += ActionContainer_DragDrop;
            ActionContainer.Controls.Add(adc, 0, index);
            ActionContainer.SetRow(adc, index);
            Dirty = true;
        }

        void adc_Deleted(object sender, CentipedeEventArgs e)
        {
            var adc = sender as ActionDisplayControl;
            Program.RemoveAction(adc.ThisAction);
        }

        void DataStore_Variables_RowDeleted(object sender, DataRowChangeEventArgs e)
        {
            IEnumerable<string> varsToDelete = from kvp in Program.Variables
                                       where !_dataSet.Variables.Rows.Contains(kvp.Key)
                                       select kvp.Key
                                       into k
                                       where !k.StartsWith("_")
                                       select k;

            // ReSharper disable PossibleMultipleEnumeration
            if (varsToDelete.Any())
            {
                Program.Variables.Remove(varsToDelete.First());
            }
            // ReSharper restore PossibleMultipleEnumeration
        }

        void Variables_VariablesRowChanged(object sender, JobDataSet.VariablesRowChangeEvent e)
        {
            JobDataSet.VariablesRow row = e.Row;

            if (!Program.Variables.ContainsKey(row.Name))
            {
                var it = from kvp in Program.Variables
                         where !_dataSet.Variables.Rows.Contains(kvp.Key)
                         select kvp.Key
                             into k
                             where !k.StartsWith("_")
                             select k;

                foreach (String key in it)
                {
                    Program.Variables.Remove(key);
                    break;
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
            var view = sender as DataGridView;
            view.ClearSelection();
            view.Rows[e.RowIndex].Selected = true;
            e.ContextMenuStrip = VarsContextMenu;
            e.ContextMenuStrip.Show();
        }

        private void LoadBtn_Click(object sender, EventArgs e)
        {
            if (Dirty)
            {
                DialogResult result = MessageBox.Show(this,
                                                      Resources.MainWindow_LoadBtn_Click_Save_changes_,
                                                      Resources.MainWindow_LoadBtn_Click_Unsaved_Changes,
                                                      MessageBoxButtons.YesNoCancel);
                switch (result)
                {
                    case DialogResult.Yes:
                        SaveJob();
                        break;
                    case DialogResult.No:
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
                var askJobTitle = new AskJobTitle();
                DialogResult result = askJobTitle.ShowDialog(this);
                if (result == DialogResult.Cancel)
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
            Dirty = false;
        }

        public static MainWindow Instance;
        private bool _dirty;
        private readonly JobDataSet _dataSet = new JobDataSet();

        private bool Dirty
        {
            get
            {
                return _dirty;
            }
            set
            {
                _dirty = value;
                Text = Program.JobName;
            }
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            //It's not really a percentage, but it serves the purpose
            progressBar1.Increment(e.ProgressPercentage);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            Program.Dispose(disposing);
        }
    }
}
