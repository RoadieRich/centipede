using System;
using System.Windows.Forms;
using System.Resources;
using System.Reflection;
using System.Linq;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;




namespace Centipede
{
    
    using Variable = Program.Variable;

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
            Program.Variables.Add("console", new Program.Variable("console", new GuiConsole()));

            Control pyAct = new Control();
            pyAct.Text = "Python Action";
            pyAct.Tag = new PythonActionFactory();

            OtherActListBox.Items.Add(pyAct);

            Control pyBranch = new Control();
            pyBranch.Text = "Python Branch";
            pyBranch.Tag = new BranchActionFactory();
            FlowContListBox.Items.Add(pyBranch);


        }


        #region Here be ugle, avert thine eyes
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
        #endregion

        private void button1_Click(object sender, EventArgs eventArgs)
        {
            //Program.RunJob(SetSelectedAction, CompletedHandler, ErrorHandler);
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
            MessageBox.Show(message, "Finished", MessageBoxButtons.OK, icon);
        }

        private void ActListBox_Dbl_Click(object sender, MouseEventArgs e)
        {
            ListBox sendingListBox = (ListBox)sender;

            Control sendingControl = (Control)sendingListBox.Items[0];

            jobActionListBox.Add(((ActionFactory)(sendingControl.Tag)).generate("new action"));
        }

        //private VariableBindingList VariablesList;
        private BindingSource _varBindingSource = new BindingSource();

        private void MainWindow_Load(object sender, EventArgs e)
        {

            _varBindingSource.DataSource = Program.Variables;
            _varBindingSource.RaiseListChangedEvents = true;

            DataGridViewColumn column = new DataGridViewTextBoxColumn();
            column.HeaderText = "Name";
            column.DataPropertyName = "Name";
            VarDataGridView.Columns.Add(column);


            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = "Value";
            column.HeaderText = "Value";
            VarDataGridView.Columns.Add(column);

            VarDataGridView.DataSource = new DataSet1().Variables;
        }

        private void updateTimer_Tick(object sender, EventArgs e)
        {
            ((DataSet1.VariablesDataTable)VarDataGridView.DataSource).Update();
        }
    }



    class GuiConsole
    {
        public void write(string message)
        {
            MessageBox.Show(message, "Python Output");
        }
    }



    //class VariableBindingList : IBindingList
    //{
    //    private SortedDictionary<String, Object> _varDict;
    //    private List<String> Keys;
    //    public VariableBindingList(Dictionary<String, Object> varDictionary)
    //    {
    //        _varDict = new SortedDictionary<string, object>(varDictionary);

    //        Keys = _varDict.Keys.ToList();

    //    }

    //    public void AddIndex(PropertyDescriptor property)
    //    { }

    //    public object AddNew()
    //    {
    //        Variable newVar = new Program.Variable();
    //        _varDict.Add(newVar.Name, newVar.Value);

    //        return newVar;
    //    }

    //    public bool AllowEdit
    //    {
    //        get { return true; }
    //    }

    //    public bool AllowNew
    //    {
    //        get { return true; }
    //    }

    //    public bool AllowRemove
    //    {
    //        get { return true; }
    //    }

    //    public void ApplySort(PropertyDescriptor property, ListSortDirection direction)
    //    { }

    //    public int Find(PropertyDescriptor property, object key)
    //    {
    //        return Keys.IndexOf((String)key);
    //    }

    //    public bool IsSorted
    //    {
    //        get { return false; }
    //    }

    //    public event ListChangedEventHandler ListChanged;

    //    public void RemoveIndex(PropertyDescriptor property)
    //    { }

    //    public void RemoveSort()
    //    {
    //        throw new NotSupportedException();
    //    }

    //    public ListSortDirection SortDirection
    //    {
    //        get { throw new NotSupportedException(); }
    //    }

    //    public PropertyDescriptor SortProperty
    //    {
    //        get { throw new NotSupportedException(); }
    //    }

    //    public bool SupportsChangeNotification
    //    {
    //        get { return true; }
    //    }

    //    public bool SupportsSearching
    //    {
    //        get { return false; }
    //    }

    //    public bool SupportsSorting
    //    {
    //        get { return false; }
    //    }

    //    public int Add(object value)
    //    {
    //        Variable var = (Variable)(KeyValuePair<String, Object>)value;

    //        _varDict.Add(var.Name, var.Value);
    //        return -1;
    //    }

    //    public void Clear()
    //    {
    //        _varDict.Clear();
    //    }

    //    public bool Contains(object value)
    //    {
    //        return _varDict.ContainsValue(value);
    //    }

    //    public int IndexOf(object value)
    //    {
    //        return -1;
    //    }

    //    public void Insert(int index, object value)
    //    {
    //        Variable var = (Variable)value;
    //        _varDict.Add(var.Name, var.Value);
    //    }

    //    public bool IsFixedSize
    //    {
    //        get { return false; }
    //    }

    //    public bool IsReadOnly
    //    {
    //        get { return false; }
    //    }

    //    public void Remove(object key)
    //    {
    //        _varDict.Remove((String)key);
    //    }

    //    public void RemoveAt(int index)
    //    {

    //        _varDict.Remove(Keys[index]);
    //    }

    //    public object this[int index]
    //    {
    //        get
    //        {
    //            return _varDict[Keys[index]];
    //        }
    //        set
    //        {
    //            _varDict[Keys[index]] = value;
    //        }
    //    }
    //    public object this[String index]
    //    {
    //        get
    //        {
    //            return _varDict[index];
    //        }
    //        set
    //        {
    //            _varDict[index] = value;
    //        }
    //    }
    //    public void CopyTo(Array array, int index)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public int Count
    //    {
    //        get { return _varDict.Count; }
    //    }

    //    public bool IsSynchronized
    //    {
    //        get { return false; }
    //    }

    //    public object SyncRoot
    //    {
    //        get { return null; }
    //    }

    //    public IEnumerator GetEnumerator()
    //    {
    //        return _varDict.GetEnumerator();
    //    }
    //}

    //public static class Extensions
    //{
    //    public static void Add(this Dictionary<String, Object> dict, Variable var)
    //    {
    //        dict.Add(var.Name, var.Value);
    //    }

    //}
}
