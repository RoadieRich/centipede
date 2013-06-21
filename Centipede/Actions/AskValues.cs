using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using CentipedeInterfaces;
using PythonEngine;


namespace Centipede.Actions
{
    [ActionCategory("UI", DisplayName = "Ask for values")]
    class AskValues : Action
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="variables"></param>
        /// <param name="c"></param>
        public AskValues(IDictionary<string, object> variables, ICentipedeCore c)
                : base("Ask For Values", variables, c)
        { }


        [ActionArgument(DisplayName = "Variables", Usage="Variable names, separated by commas", Literal=true)]
        public string VariablesToSet = "";


        [ActionArgument(DisplayName = "Evaluate user input")]
        public Boolean Evaluate = false;

        [ActionArgument(Literal = true, Usage="Text to display in the titlebar of the popup form")]
        public string Title = "Input";

        [ActionArgument]
        public String Prompt = "";


        /// <summary>
        /// Perform the action
        /// </summary>
        /// <exception cref="ActionException">
        /// the action cannot be completed
        /// </exception>
        protected override void DoAction()
        {
            if (String.IsNullOrEmpty(this.VariablesToSet))
            {
                throw new ActionException("No variables listed", this);
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
                            Text = this.Title
                        };

            TableLayoutPanel table = new TableLayoutPanel
                                     {

                                         ColumnCount = 2,
                                         GrowStyle = TableLayoutPanelGrowStyle.AddRows,
                                         Dock = DockStyle.Fill,
                                         AutoSize = true,
                                         AutoSizeMode = AutoSizeMode.GrowAndShrink
                                     };


            var RowPadding = new System.Windows.Forms.Padding();
            RowPadding.All = 10;

            table.Padding = RowPadding;
            
            form.Controls.Add(table);

            if (!String.IsNullOrEmpty(Prompt))
            {
                Label label = new Label
                {
                    Text = this.Prompt,
                    AutoSize = true
                };
                table.Controls.Add(label);
                table.SetColumnSpan(label, 2);
            }

            foreach (string varName in VariablesToSet.Split(',').Select(var => var.Trim()))
            {
                Label lbl = new Label
                            {
                                Text = varName,
                                AutoSize = true
                            };

                TextBox tb = new TextBox
                             {
                                 Tag = varName
                             };
                
                dynamic value;

                Variables.TryGetValue(varName, out value);
                
                if (value != null)
                {
                    tb.Text = value.ToString();
                }
                
                table.Controls.Add(lbl);
                table.Controls.Add(tb);
            }

            table.Controls.AddRange(new Control[]
                                    {
                                        // How do I set form.CancelButton = the new Button?
                                        new Button
                                        {
                                            Text = "Cancel",
                                            DialogResult = DialogResult.Cancel,
                                            Dock = DockStyle.Fill
                                        },
                                        new Button
                                        {
                                            Text = "OK",
                                            DialogResult = DialogResult.OK,
                                            Dock = DockStyle.Fill
                                        }
                                    });

            form.FormClosed += delegate
                               {
                                   if (form.DialogResult == DialogResult.OK)
                                   {
                                       foreach (TextBox tb in table.Controls.OfType<TextBox>())
                                       {
                                           dynamic tryEvaluate = Evaluate ? TryEvaluate(tb.Text) : tb.Text;
                                           Variables[(string)tb.Tag] = tryEvaluate;
                                       }
                                   }
                               };

            DialogResult result = (DialogResult)GetCurrentCore().Window.Invoke(new Func<Form, DialogResult>(form.ShowDialog), GetCurrentCore().Window);

            if (result==DialogResult.Cancel)
            {
                throw new FatalActionException("Cancel Clicked", this);
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
    }

    [ActionCategory("UI", DisplayName = "Checkboxes")]
    class AskBooleans : Action
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="variables"></param>
        /// <param name="c"></param>
        public AskBooleans(IDictionary<string, object> variables, ICentipedeCore c)
                : base("Checkboxes", variables, c)
        { }


        [ActionArgument(DisplayName = "Variables", Usage="Variable names, separated by commas", Literal=true)]
        public string VariablesToSet = "";

        [ActionArgument(Literal = true)]
        public string Title = "Input";

        [ActionArgument]
        public String Prompt = "";

        /// <summary>
        /// Perform the action
        /// </summary>
        /// <exception cref="ActionException">
        /// the action cannot be completed
        /// </exception>
        protected override void DoAction()
        {
            if (String.IsNullOrEmpty(VariablesToSet))
            {
                throw new ActionException("No variables listed", this);
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
                            Text = this.Title
                        };

            TableLayoutPanel table = new TableLayoutPanel
            {

                ColumnCount = 2,
                GrowStyle = TableLayoutPanelGrowStyle.AddRows,
                Dock = DockStyle.Fill,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink
            };


            var RowPadding = new System.Windows.Forms.Padding();
            RowPadding.All = 10;

            table.Padding = RowPadding;

            form.Controls.Add(table);

            if (!String.IsNullOrEmpty(Prompt))
            {
                Label label = new Label
                {
                    Text = this.Prompt,
                    AutoSize = true
                };
                table.Controls.Add(label);
                table.SetColumnSpan(label, 2);
            }

            foreach (string varName in VariablesToSet.Split(',').Select(var => var.Trim()))
            {
                Label lbl = new Label
                {
                    Text = varName,
                    AutoSize = true
                };

                CheckBox cb = new CheckBox
                {
                    Tag = varName
                };

                dynamic value;

                Variables.TryGetValue(varName, out value);

                if (value != null)
                {
                    cb.Checked = Convert.ToBoolean(value);
                }

                table.Controls.Add(lbl);
                table.Controls.Add(cb);
            }
            
            /*
            form.Controls.AddRange(this.VariablesToSet.Split(',')
                                       .Select(var => var.Trim())
                                       .Select(varName => new CheckBox
                                                          {
                                                              Text = varName
                                                          }));

            */

            table.Controls.AddRange(new Control[]
                                   {
                                       new Button
                                       {
                                           Text = "Cancel",
                                           DialogResult = DialogResult.Cancel,
                                           Dock = DockStyle.Fill
                                       },
                                       new Button
                                       {
                                           Text = "OK",
                                           DialogResult = DialogResult.OK,
                                           Dock = DockStyle.Fill
                                       }
                                   });
            
            form.FormClosed += delegate
                               {
                                   switch (form.DialogResult)
                                   {
                                   case DialogResult.OK:
                                       foreach (CheckBox checkBox in table.Controls.OfType<CheckBox>())
                                       {
                                           Variables[(string)checkBox.Tag] = checkBox.Checked;
                                       }
                                       break;
                                   case DialogResult.Cancel:
                                           break;
                                   }

                               };

            DialogResult result =
                (DialogResult)
                GetCurrentCore().Window.Invoke(new Func<Form, DialogResult>(form.ShowDialog), GetCurrentCore().Window);
            
            if (result == DialogResult.Cancel)
            {
                throw new FatalActionException("Cancel clicked", this);
            }

        }
    }
}


