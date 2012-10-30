using System;
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
            Program.Variables["console"] = new GuiConsole();

            Control pyAct = new Control();
            pyAct.Text = "Python Action";
            pyAct.Tag = new PythonActionFactory();

            OtherActListBox.Items.Add(pyAct);

            Control pyBranch = new Control();
            pyBranch.Text = "Python Branch";
            pyBranch.Tag = new BranchActionFactory();
            FlowContListBox.Items.Add(pyBranch);

            
        }


        //#region Here be ugle, avert thine eyes
        //private void AddVarButton_Click(object sender, EventArgs e) //XXX: This is ugly
        //{
        //    //Vars.ListBox.Controls.Add(new Button());
        //    //return;
        //    switch (TypeComboBox.Text)
        //    {
        //        case "Int":
        //            AddInt();
        //            break;
        //        case "Float":
        //            AddFloat();
        //            break;
        //        case "String":
        //            AddString();
        //            break;
        //        default:
        //            AddMisc();
        //            break;
        //    }
        //}

        //private void AddInt()
        //{
        //    JobVariable<Int32> varToAdd = new JobVariable<Int32>(NameTextBox.Text, Int32.Parse(ValueTextBox.Text));
        //    Program.Variables.Add(NameTextBox.Text, varToAdd);
        //    VariableDisplayControl control = new VariableDisplayControl();
        //    control.Init(varToAdd);
        //    VarsListBox.Controls.Add(control);
        //}
        //private void AddFloat()
        //{
        //    JobVariable<Double> varToAdd = new JobVariable<Double>(NameTextBox.Text, Double.Parse(ValueTextBox.Text));
        //    Program.Variables.Add(NameTextBox.Text, varToAdd);
        //    VariableDisplayControl control = new VariableDisplayControl();
        //    control.Init(varToAdd);
        //    VarsListBox.Controls.Add(control);
        //}
        //private void AddString()
        //{
        //    JobVariable<String> varToAdd = new JobVariable<String>(NameTextBox.Text, ValueTextBox.Text);
        //    Program.Variables.Add(NameTextBox.Text, varToAdd);
        //    VariableDisplayControl control = new VariableDisplayControl();
        //    control.Init(varToAdd);
        //    VarsListBox.Controls.Add(control);
        //}
        //private void AddMisc()
        //{
        //    JobVariable<Object> varToAdd = new JobVariable<Object>(NameTextBox.Text, "Misc", ValueTextBox.Text, UnitComboBox.SelectedText);
        //    Program.Variables.Add(NameTextBox.Text, varToAdd);
        //    VariableDisplayControl control = new VariableDisplayControl();
        //    control.Init(varToAdd);
        //    VarsListBox.Controls.Add(control);
        //}
        //#endregion

        private void button1_Click(object sender, EventArgs eventArgs)
        {
            Program.RunJob(SetSelectedAction, CompletedHandler, ErrorHandler);
        }

        private Boolean ErrorHandler(ActionException e, out Action nextAction)
        {
            String message;
            if (e.ErrorAction != null)
            {
                SetSelectedAction(e.ErrorAction);

                message = "Error occurred in " + e.ErrorAction.Name + "\r\n\r\nMessage was:\r\n" + e.Message;
            }
            else
            {
                message = "Error: " + "\r\n\r\n" + e.Message;
            }

            DialogResult result = MessageBox.Show(message,
                "Error",
                MessageBoxButtons.AbortRetryIgnore,
                MessageBoxIcon.Exclamation);

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

        private void SetSelectedAction(Action currentAction)
        {
            ActionsVarsTabControl.SelectTab(ActionsTab);
            jobActionListBox.SelectedItem = currentAction.Tag;
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
            MessageBox.Show(message,"Finished",MessageBoxButtons.OK,icon);
        }

        private void ActListBox_Dbl_Click(object sender, MouseEventArgs e)
        {
            ListBox sendingListBox = (ListBox)sender;

            Control sendingControl = (Control)sendingListBox.Items[0];

           jobActionListBox.Add(((ActionFactory)(sendingControl.Tag)).generate("new action"));
        }

        private BindingSource _varBindingSource = new BindingSource();

        private void MainWindow_Load(object sender, EventArgs e)
        {
            //Program.Variables.
            Program.Variables.Add("Test", "Hello World");
            Program.Variables.Add("Foo", "Bar");

            _varBindingSource.AllowNew = true;
            _varBindingSource.DataSource = Program.Variables;
            MessageBox.Show(_varBindingSource.AllowNew.ToString());
            
            DataGridViewColumn column = new DataGridViewTextBoxColumn();
            column.HeaderText = "Name";
            column.DataPropertyName = "Key";
            column.ReadOnly = false;
            VarDataGridView.Columns.Add(column);

            //random useless change

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = "Value";
            column.HeaderText = "Value";
            column.ReadOnly = false;
            VarDataGridView.Columns.Add(column);

            //DataGridViewComboBoxColumn comboColumn = new DataGridViewComboBoxColumn();
            //comboColumn.DataSource = Enum.GetValues(typeof(VarTypes));
            //comboColumn.DataPropertyName = "VarType";
            //comboColumn.HeaderText = "Type";
            //VarDataGridView.Columns.Add(comboColumn);

            VarDataGridView.DataSource = Program.Variables.Values;
            
            Program.Variables.Add("final", "42");
        }

        private void VarDataGridView_UserAddedRow(object sender, DataGridViewRowEventArgs e)
        {
            Program.Variables.Add((string)e.Row.Cells[0].Value, e.Row.Cells[1].Value);
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
