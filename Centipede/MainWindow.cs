using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using System.Text;
using System.IO;
using System.Reflection;

namespace Centipede
{


    public partial class MainWindow : Form
    {
        public MainWindow()
        {
            InitializeComponent();

            if (Program.JobFileName == "")
            {
                GetJob startWindow = new GetJob();
                DialogResult loadJob = startWindow.ShowDialog();

                switch (loadJob)
                {
                    case DialogResult.Yes:
                        Program.JobFileName = startWindow.getJobFileName();
                        Program.LoadJob();
                        break;
                    case DialogResult.No:
                        Program.JobFileName = "new";
                        Program.JobName = "New";
                        break;
                    default:
                        Close();
                        break;
                }

                startWindow.Dispose();
            }

            this.Text = "Centipede 0.1 " + Program.JobName;

            Program.Variables.Add("_console", new GuiConsole());

            //ActionFactory fact = new PythonActionFactory();
            //fact.ImageIndex = 0;
            //OtherActListBox.Items.Add(fact);

            //fact = new PythonBranchActionFactory();
            //fact.ImageIndex = 0;
            //FlowContListBox.Items.Add(fact);

            //fact = new BranchActionFactory();
            //fact.ImageIndex = 1;
            //FlowContListBox.Items.Add(fact);

            //fact = new DemoActionFactory();
            //OtherActListBox.Items.Add(fact);




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

                ActionDisplayControl adc = currentAction.Tag as ActionDisplayControl;
                if (adc.InvokeRequired)
                {
                    adc.Invoke(SetActionDisplayState, adc, ActionState.Completed, "Completed");
                }
                else
                {
                    (adc).State = ActionState.Completed;
                }

                _progress += 10;

                backgroundWorker1.ReportProgress(_progress);
            }
        }

        private Int32 _progress = 0;

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


        private void MainWindow_Load(object sender, EventArgs e)
        {

            DirectoryInfo di = new DirectoryInfo(Path.Combine(Application.StartupPath, Properties.Settings.Default.PluginFolder));

            var dlls = di.EnumerateFiles("*.dll", SearchOption.AllDirectories);

            foreach (FileInfo fi in dlls)
            {
                Assembly asm = Assembly.LoadFile(fi.FullName);
                Type[] pluginsInFile = asm.GetExportedTypes();
                foreach (Type pluginType in pluginsInFile)
                {
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


                        catListView.Items.Add(new ActionFactory(catAttribute, pluginType));
                    }
                }

            }


            VarDataGridView.DataSource = _dataSet.Variables;
            _dataSet.Variables.VariablesRowChanged += new JobDataSet.VariablesRowChangeEventHandler(Variables_VariablesRowChanged);
            _dataSet.Variables.RowDeleted += new DataRowChangeEventHandler(Variables_RowDeleted);

            //SetActionDisplayState = new SetStateDeligate((adc, state) => adc.State = state);

            //    void SetState(ActionDisplayControl adc, ActionState state)
            //{
            //    adc.State = state;
            //}


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

            Program.SetupTestActions(Program.ActionsToTest.All);

            //backgroundWorker1.RunWorkerAsync();

        }

        void AddActionTabs_DoubleClick(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private ListView GenerateNewTabPage(String category)
        {
            ListView lv = new ListView();
            ImageList il = new ImageList();
            lv.LargeImageList = il;
            lv.SmallImageList = il;
            TabPage tabPage = new TabPage(category);
            tabPage.Controls.Add(lv);
            lv.Dock = DockStyle.Fill;
            tabPage.Tag = lv;
            lv.ItemDrag += new ItemDragEventHandler(BeginDrag);
            lv.ItemActivate += new EventHandler(ItemActivate);
            AddActionTabs.TabPages.Add(tabPage);


            return lv;
        }

        void tabPage_MouseDoubleClick(object sender, EventArgs e)
        {
            throw new NotImplementedException();
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
            ActionDisplayControl adc = new ActionDisplayControl(action);
            ActionContainer.Controls.Add(adc, 0, index);
            ActionContainer.SetRow(adc, index);
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
            Program.Variables.ToArray();
        }

        private void RunButton_Click(object sender, EventArgs e)
        {
            foreach (ActionDisplayControl adc in ActionContainer.Controls)
            {
                adc.State = ActionState.None;
            }

            backgroundWorker1.RunWorkerAsync();
        }

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

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar1.Value = Math.Min(e.ProgressPercentage, 100);
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
            TableLayoutPanel table = sender as TableLayoutPanel;
            Control droppedOn = table.GetChildAtPoint(new System.Drawing.Point(e.X, e.Y));

            Int32 index = -1;

            if (droppedOn != null)
            {
                //index = Program.GetIndexOf(droppedOn.Action);
            }
            index.ToString();
            var s = e.Data.GetFormats();
            var data = e.Data.GetData("WindowsForms10PersistentObject");
            Program.AddAction((data as ActionFactory).Generate());

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

    }

    class GuiConsole
    {
        public void write(string message)
        {
            MessageBox.Show(Program.mainForm, message, "Python Output");
        }
    }
}
