using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using CentipedeInterfaces;
using PythonEngine;


namespace Centipede.Actions
{
    public abstract class UIAction : Action
    {
        protected UIAction(string name, string verb, IDictionary<string, object> variables, ICentipedeCore core) : base(name, variables, core)
        {
            if (!String.IsNullOrEmpty(verb))
            {
                this.Prompt = String.Format("Please {0} the following values", verb);
            }
        }

        protected UIAction(string name, IDictionary<string, object> variables, ICentipedeCore core)
            : base(name, variables, core)
        { }



        protected virtual Form MakeForm()
        {
            string myTitle = this.ParseStringForVariable(this.Title);
            string myPrompt = this.ParseStringForVariable(this.Prompt);
            
            var form = new Form
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

            var table = new TableLayoutPanel
                        {
                            ColumnCount = 2,
                            GrowStyle = TableLayoutPanelGrowStyle.AddRows,
                            Dock = DockStyle.Fill,
                            AutoSize = true,
                            AutoSizeMode = AutoSizeMode.GrowAndShrink,
                            Padding = new Padding {All = 10},
                        };

            form.Controls.Add(table);

            if (!String.IsNullOrEmpty(myPrompt))
            {
                var promptLabel = new Label
                                  {
                                      Text = myPrompt,
                                      AutoSize = true
                                  };
                table.Controls.Add(promptLabel);
                table.SetColumnSpan(promptLabel, 2);
            }
            
            foreach (var controlArr in this.GetControls())
            {
                table.Controls.AddRange(controlArr);
                if (controlArr.Length == 1)
                {
                    table.SetColumnSpan(controlArr.Single(), 2);
                }
            }

            var btnCancel = new Button
                            {
                                Text = "Cancel",
                                DialogResult = DialogResult.Cancel,
                                Dock = DockStyle.Fill
                            };

            form.CancelButton = btnCancel;

            var btnOK = new Button
                        {
                            Text = "OK",
                            DialogResult = DialogResult.OK,
                            Dock = DockStyle.Fill
                        };

            form.AcceptButton = btnOK;

            table.Controls.Add(btnOK);
            table.SetColumnSpan(btnOK, 2);
            return form;
        }

        protected UIAction(string name, ICentipedeCore core) : base(name, core)
        { }

        protected UIAction(string name, string verb, ICentipedeCore core)
            : base(name, core)
        {
            if (!String.IsNullOrEmpty(verb))
            {
                this.Prompt = String.Format("Please {0} the following values", verb);
            }
        }

        [ActionArgument(Usage = "(Optional) Text to display in the titlebar of the popup form")]
        public string Title = "Input";
        
        [ActionArgument(Usage = "(Optional) Message to display in the popup form")]
        public string Prompt = "Please set the following values";

        protected sealed override void DoAction()
        {
            var form = this.MakeForm();

            form.FormClosed += this.FormClosed;

            var mainform = (Form)GetCurrentCore().Tag;

            var showDialog = new Func<Form, DialogResult>(form.ShowDialog);
            var result = (DialogResult)mainform.Invoke(showDialog, mainform);
        }

        protected abstract IEnumerable<Control[]> GetControls();

        protected virtual void FormClosed(object sender, FormClosedEventArgs e)
        { }
    }


    [ActionCategory("User Interface", DisplayName = "Ask for Input (Text or Numeric)", IconName="ui")]
    class AskValues : UIAction
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="variables"></param>
        /// <param name="c"></param>
        public AskValues(ICentipedeCore c)
            : base("Ask for Input (Text or Numeric)", "set", c)
        { }

        [ActionArgument(DisplayName = "Variables",
                        Usage = "(Required) Names of variables to be updated, separated by commas.  You can " +
                                "optionally add a label for the variable by adding a colon and the text to display"
            )]
        public string VariablesToSet = "Var1:Label for Var1, Var2, Var3";

        [ActionArgument(DisplayName = "Evaluate user input",
                        Usage = "Controls whether user input is evaluated or taken literally")]
        public bool Evaluate;

        private IEnumerable<Control[]> _controls;

        protected override void FormClosed(object sender, FormClosedEventArgs e)
        {
            var form = (Form) sender;

            DialogResult result = form.DialogResult;
            switch (result)
            {
            case DialogResult.OK:
                foreach (TextBox tb in _controls.Select(a => a[1]).OfType<TextBox>())
                {
                    dynamic tryEvaluate = Evaluate ? TryEvaluate(tb.Text) : tb.Text;
                    Variables[(string) tb.Tag] = tryEvaluate;
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
                var evaluated = engine.Evaluate(str);
                return evaluated;
            }
            catch (PythonParseException)
            {
                return str;
            }
        }

        protected override IEnumerable<Control[]> GetControls()
        {
            string myVariablesToSet = this.ParseStringForVariable(this.VariablesToSet);

            if (string.IsNullOrEmpty(myVariablesToSet))
            {
                throw new ActionException("No variable names provided", this);
            }


            // Get list of variables
            var varNames = myVariablesToSet.Split(',').Select(s => s.Trim());
            var controls = new List<Control[]>();

            // Create label and text box for each variable
            foreach (string varName in varNames)
            {
                string actualVarName, labelText;
                if (varName.Contains(':'))
                {
                    var parts = varName.Split(':');
                    actualVarName = parts[0].Trim();
                    labelText = parts[1].Trim();
                }
                else
                {
                     labelText = actualVarName = varName;
                }
                
                var lbl = new Label
                          {
                              Text = labelText,
                              TextAlign = System.Drawing.ContentAlignment.MiddleLeft,
                              Dock = DockStyle.Fill,
                              AutoSize = true
                          };

                var tb = new TextBox
                         {
                             Tag = actualVarName,
                             Dock = DockStyle.Fill
                         };

                object obj;
                if (this.Variables.TryGetValue(actualVarName.Trim(), out obj))
                {
                    var valString = obj.ToString();
                    var msg = new MessageEventArgs(
                        "Setting textbox (" + actualVarName.Trim() + ") using existing value : " +
                        valString,
                        MessageLevel.Debug);
                    this.OnMessage(msg);
                    tb.Text = valString;
                }

                controls.Add(new Control[] { lbl, tb });
            }
            this._controls = controls;
            return controls;
        }
    }

    //-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

    [ActionCategory("User Interface", DisplayName = "Ask for Input (True / False)", IconName = "ui")]
    class AskBooleans : UIAction
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="variables"></param>
        /// <param name="c"></param>
        public AskBooleans(ICentipedeCore c)
                : base("Ask for Input (True / False)", "set", c)
        { }

        [ActionArgument(DisplayName = "Variables",
            Usage = "(Required) Names of variables to be updated, separated by commas, the checkbox action reads and " +
                    "stores variables as True or False.  You can optionally add a label for the variable by adding a " +
                    "colon and the text to display"
            )]
        public string VariablesToSet = "Boolean1:Label for Boolean1, Boolean2, Boolean3";

        private List<Control[]> _controls;


        /// <summary>
        /// Perform the action
        /// </summary>
        /// <exception cref="ActionException">
        /// the action cannot be completed
        /// </exception>
        protected override IEnumerable<Control[]> GetControls()
        {
            string myVariablesToSet = ParseStringForVariable(VariablesToSet);

            if (String.IsNullOrEmpty(myVariablesToSet))
            {
                throw new ActionException("No variable names provided", this);
            }

            var controls = new List<Control[]>();

            // Get list of variables
            var varNames = myVariablesToSet.Split(',').Select(s => s.Trim());

            // Create label and text box for each variable
            foreach (string varName in varNames)
            {
                string labelText, actualVarName;
                if (varName.Contains(':'))
                {
                    var parts = varName.Split(':');
                    actualVarName = parts[0].Trim();
                    labelText = parts[1].Trim();
                }
                else
                {
                    labelText = actualVarName = varName;
                }

                Label lbl = new Label
                            {
                                Text = labelText,
                                TextAlign = System.Drawing.ContentAlignment.MiddleLeft,
                                Dock = DockStyle.Fill,
                                AutoSize = true
                            };

                CheckBox cb = new CheckBox
                              {
                                  Tag = actualVarName
                              };

                object value;

                if (Variables.TryGetValue(actualVarName, out value))
                {
                    var message = String.Format("Setting checkbox ({0}) using existing value : {1}", actualVarName, value);
                    var msg = new MessageEventArgs
                              {
                                  Message =
                                      message,
                                  Level = MessageLevel.Debug
                              };
                    OnMessage(msg);
                    cb.Checked = Convert.ToBoolean(value);
                }

                controls.Add(new Control[] {lbl, cb});
            }

            this._controls = controls;
            return controls;
        }


        protected override void FormClosed(object sender, FormClosedEventArgs e)
        {
            var form = (Form) sender;
            DialogResult result = form.DialogResult;
            switch (result)
            {
            case DialogResult.OK:
                foreach (var checkBox in _controls.Select(a => a[1]).OfType<CheckBox>())
                {
                    Variables[(string) checkBox.Tag] = checkBox.Checked;
                }
                break;

            case DialogResult.Cancel:
                throw new FatalActionException("User input cancelled", this);
            }
        }
    }
}


