using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows.Forms;

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

            Program.Variables.Add("console", new GuiConsole());
            
            ActionFactory fact = new PythonActionFactory();            
            fact.ImageIndex = 0;
            OtherActListBox.Items.Add(fact);
            
            fact = new PythonBranchActionFactory();
            fact.ImageIndex = 0;
            FlowContListBox.Items.Add(fact);
            
            fact = new BranchActionFactory();
            fact.ImageIndex = 1;
            FlowContListBox.Items.Add(fact);

            fact = new DemoActionFactory();
            OtherActListBox.Items.Add(fact);

            
            

        }

        private Boolean ErrorHandler(ActionException e, ref Action nextAction)
        {
            String message;
            if (e.ErrorAction != null)
            {
                UpdateHandlerDone(e.ErrorAction);
                ActionDisplayControl adc = e.ErrorAction.Tag as ActionDisplayControl;
                if (adc.InvokeRequired)
                {
                    adc.Invoke(SetActionDisplayState, adc, ActionState.Error);
                }
                else
                {
                    adc.State = ActionState.Error;
                }
                    

                message = "Error occurred in " + e.ErrorAction.Name + "\r\n\r\nMessage was:\r\n" + e.Message;
            }
            else
            {
                message = "Error: " + "\r\n\r\n" + e.Message;
            }
            //CbRFtCiDVis506ZJ0xsnslwj5Et8ECVZCR6y48yd76REzIGL3N4d9F94ET9KsxyE
            DialogResult result = MessageBox.Show(
                message,
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
            foreach (KeyValuePair<String,Object> v in Program.Variables.ToArray())
            {
                if (v.Key == "console")
                {
                    continue;
                }
                JobDataSet.VariablesRow row = _dataSet.Variables.FindByName(v.Key);
                if (row != null)
                {
                    if (row.Value != v.Value )
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
                    adc.Invoke(SetActionDisplayState, adc, ActionState.Completed);
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

        private void ListBox_Dbl_Click(object sender, MouseEventArgs e)
        {
            ListView sendingListView = sender as ListView;

            ActionFactory sendingActionFactory = sendingListView.SelectedItems[0] as ActionFactory;

            Program.AddAction(sendingActionFactory.Generate());
        }

        private JobDataSet _dataSet = new JobDataSet();
        private delegate void SetStateDeligate(ActionDisplayControl adc, ActionState state);
        
        private static void SetActionDisplayedState(ActionDisplayControl adc, ActionState state)
        {
            lock (adc)
            {
                adc.State = state;
            }
        }
        
        private SetStateDeligate SetActionDisplayState = new SetStateDeligate(SetActionDisplayedState);
        

        private void MainWindow_Load(object sender, EventArgs e)
        {
            VarDataGridView.DataSource = _dataSet.Variables;
            _dataSet.Variables.VariablesRowChanged += new JobDataSet.VariablesRowChangeEventHandler(Variables_VariablesRowChanged);
            _dataSet.Variables.RowDeleted += new DataRowChangeEventHandler(Variables_RowDeleted);

            SetActionDisplayState = new SetStateDeligate((adc, state) => adc.State = state);
                
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

            Program.SetupTestActions();


            //backgroundWorker1.RunWorkerAsync();
            
        }

        void Program_BeforeAction(Action action)
        {
            ActionDisplayControl adc = action.Tag as ActionDisplayControl;
            if (adc.InvokeRequired)
            {
                adc.Invoke(SetActionDisplayState, adc, ActionState.Running);
            }
            else
            {
                adc.State = ActionState.Running;
            }
        }

        void Program_ActionAdded(Action action, int index)
        {
            ActionDisplayControl adc = new ActionDisplayControl(action);
            ActionContainer.Controls.Add(adc);
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
            progressBar1.Value = Math.Min(e.ProgressPercentage,100);
        }

        private void MainWindow_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void MainWindow_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyData == Keys.F5)
            {
                RunButton.PerformClick();
            }
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
