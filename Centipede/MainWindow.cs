using System;
using System.Windows.Forms;
using System.Resources;
using System.Reflection;
using System.Linq;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
//using Variable = Centipede.Program.Variable;
using System.Threading;
using System.Data;
using System.Drawing;

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

            
            

        }

        private void button1_Click(object sender, EventArgs eventArgs)
        {
            Program.RunJob(UpdateHandler, CompletedHandler, ErrorHandler);

        }

        private Boolean ErrorHandler(ActionException e, out Action nextAction)
        {
            String message;
            if (e.ErrorAction != null)
            {
                UpdateHandler(e.ErrorAction);

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

        private void UpdateHandler(Action currentAction)
        {
            //ActionsVarsTabControl.SelectTab(ActionsTab);
            //jobActionListBox.SelectedItem = currentAction.Tag;

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
            }
            VarDataGridView.Refresh();
            
            //var it = from KeyValuePair<String, Object> v in Program.Variables
            //         join VarDataSet.VariablesRow r in _dataSet.Variables.Rows
            //         on v.Key equals r.Name
            //         where r.Value != v.Value
            //         select v;

            //foreach (var v in it)
            //{
            //    var r = _dataSet.Variables.FindByName(v.Key);
            //    r.SetField("Value", v.Value);
            //    //r.AcceptChanges();
            //    VarDataGridView.Refresh();
            //}
            
            
            

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

        private void ActListBox_Dbl_Click(object sender, MouseEventArgs e)
        {
            //ListView sendingListView = (ListView)sender;

            //ActionFactory sendingActionFactory = (ActionFactory)sendingListView.SelectedItems[0];

            //jobActionListBox.Add(sendingActionFactory.generate("new action"));
        }

        private JobDataSet _dataSet = new JobDataSet();

        private void MainWindow_Load(object sender, EventArgs e)
        {
            VarDataGridView.DataSource = _dataSet.Variables;
            _dataSet.Variables.VariablesRowChanged += new JobDataSet.VariablesRowChangeEventHandler(Variables_VariablesRowChanged);
            _dataSet.Variables.RowDeleted += new DataRowChangeEventHandler(Variables_RowDeleted);

            ActionContainer.Controls.Add(new ActionDisplayControl(new DemoAction()));
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
            JobDataSet.VariablesDataTable table = (JobDataSet.VariablesDataTable)sender;
            JobDataSet.VariablesRow row = (JobDataSet.VariablesRow)e.Row;

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
                _dataSet.Variables.RemoveVariablesRow((JobDataSet.VariablesRow)((DataRowView)r.DataBoundItem).Row);
            }
        }

        private void VarDataGridView_CellContextMenuStripNeeded(object sender, DataGridViewCellContextMenuStripNeededEventArgs e)
        {
            ((DataGridView)sender).ClearSelection();
            ((DataGridView)sender).Rows[e.RowIndex].Selected = true;
            e.ContextMenuStrip = VarsContextMenu;
            e.ContextMenuStrip.Show();
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar1.PerformStep();
        }

        

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

        }

        private void LoadBtn_Click(object sender, EventArgs e)
        {
            Program.Variables.ToString();
        }
    }

    class GuiConsole
    {
        public void write(string message)
        {
            MessageBox.Show(message, "Python Output");
        }
    }
}
