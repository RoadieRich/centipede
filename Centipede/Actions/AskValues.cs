﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using CentipedeInterfaces;
using PythonEngine;


namespace Centipede.Actions
{
    [ActionCategory("User Interface", DisplayName = "Ask for Input (Text or Numeric)", IconName="ui")]
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
        public string Prompt = "Please enter the following values";

        [ActionArgument(DisplayName = "Variables", 
                        Usage = "(Required) Names of variables to be updated, separated by commas.  You can " + 
                                "optionally add a label for the variable by adding a colon and the text to display"
            )]
        public string VariablesToSet = "Var1:Label for Var1, Var2, Var3";

        [ActionArgument(DisplayName = "Evaluate user input",
                        Usage = "Controls whether user input is evaluated or taken literally")]
        public bool Evaluate;

        
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
            
            if (string.IsNullOrEmpty(myVariablesToSet))
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


            Padding tablePadding = new Padding { All = 10 };
            table.Padding = tablePadding;
            
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

            
            // Get list of variables
            string[] varNames = myVariablesToSet.Split(',');

            // Create label and text box for each variable
            foreach (string varName in varNames)
            {
                string labelText, actualVarName;
                if (varName.Contains(':'))
                {
                    var parts = varName.Split(':');
                    labelText = parts[1];
                    actualVarName = parts[0];
                }
                else
                {
                    labelText = varName;
                    actualVarName = varName;
                }

                Label lbl = new Label
                            {
                                Text = labelText.Trim(),
                                TextAlign = System.Drawing.ContentAlignment.MiddleLeft,
                                Dock = DockStyle.Fill,
                                AutoSize = true
                            };

                TextBox tb = new TextBox
                             {
                                 Tag = actualVarName.Trim(),
                                 Dock = DockStyle.Fill
                             };
                
                dynamic value;

                Variables.TryGetValue(actualVarName.Trim(), out value);

                if (value != null)
                {
                    MessageEventArgs msg =
                        new MessageEventArgs(
                            "Setting textbox (" + actualVarName.Trim() + ") using existing value : " + value,
                            MessageLevel.Debug);
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

            var mainform = (Form)GetCurrentCore().Tag;
            DialogResult result =
                (DialogResult)
                mainform.Invoke(new Func<Form, DialogResult>(form.ShowDialog), mainform);

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
                    DialogResult result = MessageBox.Show("Cancelling user input will abort the current job.  Retry?",
                                                          "Centipede",
                                                          MessageBoxButtons.RetryCancel,
                                                          MessageBoxIcon.Warning);
                    if (result == DialogResult.Retry) a.Cancel = true;
                    break;
            }
        }
    }

    //-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

    [ActionCategory("User Interface", DisplayName = "Ask for Input (True / False)", IconName = "ui")]
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

        [ActionArgument(DisplayName = "Variables",
            Usage = "(Required) Names of variables to be updated, separated by commas, the checkbox action reads and " +
                    "stores variables as True or False.  You can optionally add a label for the variable by adding a " +
                    "colon and the text to display"
            )]
        public string VariablesToSet = "Boolean1:Label for Boolean1, Boolean2, Boolean3";


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


            Padding tablePadding = new Padding { All = 10 };
            table.Padding = tablePadding;

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

            // Get list of variables
            string[] varNames = myVariablesToSet.Split(',');

            // Create label and text box for each variable
            foreach (string varName in varNames)
            {
                string labelText, actualVarName;
                if (varName.Contains(':'))
                {
                    var parts = varName.Split(':');
                    labelText = parts[1];
                    actualVarName = parts[0];
                }
                else
                {
                    labelText = varName;
                    actualVarName = varName;
                }
            
                Label lbl = new Label
                {
                    Text = labelText.Trim(),
                    TextAlign = System.Drawing.ContentAlignment.MiddleLeft,
                    Dock = DockStyle.Fill,
                    AutoSize = true
                };

                CheckBox cb = new CheckBox
                {
                    Tag = actualVarName.Trim()
                };

                dynamic value;

                Variables.TryGetValue(actualVarName.Trim(), out value);

                if (value != null)
                {
                    MessageEventArgs msg =
                        new MessageEventArgs(
                            "Setting checkbox (" + actualVarName.Trim() + ") using existing value : " + value,
                            MessageLevel.Debug);
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

            // Removed cancel button for consistent appearance when table columns resize
            //table.Controls.Add(btnCancel);  
            table.Controls.Add(btnOK);
            table.SetColumnSpan(btnOK, 2);

            form.FormClosing += FormClosing;

            var mainform = (Form)GetCurrentCore().Tag;
            DialogResult result =
                (DialogResult)
                mainform.Invoke(new Func<Form, DialogResult>(form.ShowDialog), mainform);

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

        //-------------------------------------------------------------------------------------------------------------

        private void FormClosing(object sender, FormClosingEventArgs a)
        {
            Form dialog = (Form)sender;

            switch (dialog.DialogResult)
            {
                case DialogResult.OK:
                    break;
                case DialogResult.Cancel:
                    DialogResult result = MessageBox.Show("Cancelling user input will abort the current job.  Retry?",
                                                          "Centipede",
                                                          MessageBoxButtons.RetryCancel,
                                                          MessageBoxIcon.Warning);
                    if (result == DialogResult.Retry) a.Cancel = true;
                    break;
            }
        }
    }
}


