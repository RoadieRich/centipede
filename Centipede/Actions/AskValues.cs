using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using CentipedeInterfaces;
using PythonEngine;


namespace Centipede.Actions
{
    [ActionCategory("User Interface", DisplayName = "Ask for Input (Text or Numeric)")]
    class AskValues : Action
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="variables"></param>
        /// <param name="c"></param>
        public AskValues(IDictionary<string, object> variables, ICentipedeCore c)
                : base("Ask for Input (Text or Numeric)", variables, c)
        { }


        [ActionArgument(Usage = "(Optional) Text to display in the titlebar of the popup form")]
        public string Title = "Input";

        [ActionArgument(Usage = "(Optional) Message to display in the popup form")]
        public String Prompt = "Please enter the following values";

        [ActionArgument(DisplayName = "Variables", Usage = "(Required) Names of variables to be updated, separated by commas")]
        public string VariablesToSet = "Var1, Var2, Var3";

        [ActionArgument(DisplayName = "Labels", Usage = "(Optional) Labels for each variable, separated by commas")]
        public string LabelsToDisplay = "";

        [ActionArgument(DisplayName = "Evaluate user input", Usage="Controls whether user input is evaluated or taken literally")]
        public Boolean Evaluate = false;


        /// <summary>
        /// Perform the action
        /// </summary>
        /// <exception cref="ActionException">
        /// the action cannot be completed
        /// </exception>
        protected override void DoAction()
        {
            string myTitle = ParseStringForVariable(Title); 
            string myPrompt = ParseStringForVariable(Prompt);
            string myVariablesToSet = ParseStringForVariable(VariablesToSet);
            string myLabels = ParseStringForVariable(LabelsToDisplay); 
            
            if (String.IsNullOrEmpty(myVariablesToSet))
            {
                throw new ActionException("No variable names provided", this);
            }


            Form form = new Form
                        {
                            AutoSize = true,
                            AutoSizeMode = AutoSizeMode.GrowAndShrink,
                            StartPosition = FormStartPosition.CenterParent,
                            MinimizeBox = false,
                            MaximizeBox = false,
                            SizeGripStyle = SizeGripStyle.Hide,
                            ShowIcon = false,
                            ShowInTaskbar = false,
                            Text = myTitle
                        };

            TableLayoutPanel table = new TableLayoutPanel
                                     {

                                         ColumnCount = 2,
                                         GrowStyle = TableLayoutPanelGrowStyle.AddRows,
                                         Dock = DockStyle.Fill,
                                         AutoSize = true,
                                         AutoSizeMode = AutoSizeMode.GrowAndShrink
                                     };


            Padding TablePadding = new Padding();
            TablePadding.All = 10;
            table.Padding = TablePadding;
            
            form.Controls.Add(table);

            if (!String.IsNullOrEmpty(myPrompt))
            {
                Label label = new Label
                {
                    Text = myPrompt,
                    AutoSize = true
                };
                table.Controls.Add(label);
                table.SetColumnSpan(label, 2);
            }


            // Get list of labels
            string[] lblStrings = myLabels.Split(',');
            
            // Get list of variables
            string[] varNames = myVariablesToSet.Split(',');

            // Create label and text box for each variable
            for (int i = 0; i < varNames.Length; i++)
            {
                Label lbl = new Label
                            {
                                Text = lblStrings.Length == varNames.Length ? lblStrings[i].Trim() : varNames[i].Trim(),  // If labels list is wrong size, use varNames instead
                                TextAlign = System.Drawing.ContentAlignment.MiddleLeft,
                                Dock = DockStyle.Fill,
                                AutoSize = true
                            };

                TextBox tb = new TextBox
                             {
                                 Tag = varNames[i].Trim(),
                                 Dock = DockStyle.Fill
                             };
                
                dynamic value;

                Variables.TryGetValue(varNames[i].Trim(), out value);
                
                if (value != null)
                {
                    MessageEventArgs msg = new MessageEventArgs("Setting textbox (" + varNames[i].Trim() + ") using existing value : " + value, MessageLevel.Debug);
                    OnMessage(msg);
                    tb.Text = value.ToString();
                }
                
                table.Controls.Add(lbl);
                table.Controls.Add(tb);
            }

            Button btnCancel = new Button
            {
                Text = "Cancel",
                DialogResult = DialogResult.Cancel,
                Dock = DockStyle.Fill
            };

            form.CancelButton = btnCancel;

            Button btnOK = new Button
            {
                Text = "OK",
                DialogResult = DialogResult.OK,
                Dock = DockStyle.Fill
            };

            form.AcceptButton = btnOK;

            //table.Controls.Add(btnCancel);  // Removed cancel button for consistent appearance when table columns resize
            table.Controls.Add(btnOK);
            table.SetColumnSpan(btnOK, 2);

            form.FormClosing += FormClosing;

            DialogResult result =
                (DialogResult)
                GetCurrentCore().Window.Invoke(new Func<Form, DialogResult>(form.ShowDialog), GetCurrentCore().Window);

            switch (result)
            {
                case DialogResult.OK:
                    foreach (TextBox tb in table.Controls.OfType<TextBox>())
                    {
                        dynamic tryEvaluate = Evaluate ? TryEvaluate(tb.Text) : tb.Text;
                        Variables[(string)tb.Tag] = tryEvaluate;
                    }
                    break;

                case DialogResult.Cancel:
                    throw new FatalActionException("User input cancelled", this);
            }
        }

        private dynamic TryEvaluate(String str)
        {

            try
            {
                IPythonEngine engine = GetCurrentCore().PythonEngine;
                var compiled = engine.Compile(str, SourceCodeType.Expression);
                var evaluated = engine.Evaluate(compiled);
                return evaluated;
            }
            catch (PythonParseException)
            {
                return str;
            }
        }
        
        //-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

        private void FormClosing(object sender, FormClosingEventArgs a)
        {
            Form dialog = (Form)sender;

            switch (dialog.DialogResult)
            {
                case DialogResult.OK:
                    break;
                case DialogResult.Cancel:
                    DialogResult result = MessageBox.Show("Cancelling user input will abort the current job.  Retry?", "Centipede", MessageBoxButtons.RetryCancel, MessageBoxIcon.Warning);
                    if (result == DialogResult.Retry) a.Cancel = true;
                    break;
            }
        }
    }

    //-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

    [ActionCategory("User Interface", DisplayName = "Ask for Input (True / False)")]
    class AskBooleans : Action
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="variables"></param>
        /// <param name="c"></param>
        public AskBooleans(IDictionary<string, object> variables, ICentipedeCore c)
                : base("Ask for Input (True / False)", variables, c)
        { }

        [ActionArgument(Usage = "(Optional) Text to display in the titlebar of the popup form")]
        public string Title = "Input";

        [ActionArgument(Usage = "(Optional) Message to display in the popup form")]
        public String Prompt = "Please set the following values";

        [ActionArgument(DisplayName = "Variables", Usage = "(Required) Names of variables to be updated, separated by commas, the checkbox action reads and stores variables as True or False")]
        public string VariablesToSet = "Boolean1, Boolean2, Boolean3";

        [ActionArgument(DisplayName = "Labels", Usage = "(Optional) Labels for each variable, separated by commas")]
        public string LabelsToDisplay = "";


        /// <summary>
        /// Perform the action
        /// </summary>
        /// <exception cref="ActionException">
        /// the action cannot be completed
        /// </exception>
        protected override void DoAction()
        {
            string myTitle = ParseStringForVariable(Title);
            string myPrompt = ParseStringForVariable(Prompt);
            string myVariablesToSet = ParseStringForVariable(VariablesToSet);
            string myLabels = ParseStringForVariable(LabelsToDisplay); 
            
            if (String.IsNullOrEmpty(myVariablesToSet))
            {
                throw new ActionException("No variable names provided", this);
            }


            Form form = new Form
                        {
                            AutoSize = true,
                            AutoSizeMode = AutoSizeMode.GrowAndShrink,
                            StartPosition = FormStartPosition.CenterParent,
                            MinimizeBox = false,
                            MaximizeBox = false,
                            SizeGripStyle = SizeGripStyle.Hide,
                            ShowIcon = false,
                            ShowInTaskbar = false,
                            Text = myTitle
                        };

            TableLayoutPanel table = new TableLayoutPanel
            {

                ColumnCount = 2,
                GrowStyle = TableLayoutPanelGrowStyle.AddRows,
                Dock = DockStyle.Fill,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink
            };


            Padding TablePadding = new Padding();
            TablePadding.All = 10;
            table.Padding = TablePadding;

            form.Controls.Add(table);

            if (!String.IsNullOrEmpty(myPrompt))
            {
                Label label = new Label
                {
                    Text = myPrompt,
                    AutoSize = true
                };
                table.Controls.Add(label);
                table.SetColumnSpan(label, 2);
            }

            // Get list of labels
            string[] lblStrings = myLabels.Split(',');

            // Get list of variables
            string[] varNames = myVariablesToSet.Split(',');

            // Create label and text box for each variable
            for (int i = 0; i < varNames.Length; i++)
            {
                Label lbl = new Label
                {
                    Text = lblStrings.Length == varNames.Length ? lblStrings[i].Trim() : varNames[i].Trim(),  // If labels list is wrong size, use varNames instead
                    TextAlign = System.Drawing.ContentAlignment.MiddleLeft,
                    Dock = DockStyle.Fill,
                    AutoSize = true
                };

                CheckBox cb = new CheckBox
                {
                    Tag = varNames[i].Trim()
                };

                dynamic value;

                Variables.TryGetValue(varNames[i].Trim(), out value);

                if (value != null)
                {
                    MessageEventArgs msg = new MessageEventArgs("Setting checkbox (" + varNames[i].Trim() + ") using existing value : " + value, MessageLevel.Debug);
                    OnMessage(msg);
                    cb.Checked = Convert.ToBoolean(value);
                }

                table.Controls.Add(lbl);
                table.Controls.Add(cb);
            }


            Button btnCancel = new Button
            {
                Text = "Cancel",
                DialogResult = DialogResult.Cancel,
                Dock = DockStyle.Fill
            };

            form.CancelButton = btnCancel;

            Button btnOK = new Button
            {
                Text = "OK",
                DialogResult = DialogResult.OK,
                Dock = DockStyle.Fill
            };

            form.AcceptButton = btnOK;

            //table.Controls.Add(btnCancel);  // Removed cancel button for consistent appearance when table columns resize
            table.Controls.Add(btnOK);
            table.SetColumnSpan(btnOK, 2);

            form.FormClosing += FormClosing;

            DialogResult result =
                (DialogResult)
                GetCurrentCore().Window.Invoke(new Func<Form, DialogResult>(form.ShowDialog), GetCurrentCore().Window);

            switch (result)
            {
                case DialogResult.OK:
                    foreach (CheckBox checkBox in table.Controls.OfType<CheckBox>())
                    {
                        Variables[(string)checkBox.Tag] = checkBox.Checked;
                    }                    
                    break;

                case DialogResult.Cancel:
                    throw new FatalActionException("User input cancelled", this);
            }

        }

        //-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

        private void FormClosing(object sender, FormClosingEventArgs a)
        {
            Form dialog = (Form)sender;

            switch (dialog.DialogResult)
            {
                case DialogResult.OK:
                    break;
                case DialogResult.Cancel:
                    DialogResult result = MessageBox.Show("Cancelling user input will abort the current job.  Retry?", "Centipede", MessageBoxButtons.RetryCancel, MessageBoxIcon.Warning);
                    if (result == DialogResult.Retry) a.Cancel = true;
                    break;
            }
        }
    }
}


