﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using CentipedeInterfaces;


namespace Centipede.Actions
{
    [ActionCategory("UI", displayName = "Ask for values")]
    class AskValues : Action
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="v"></param>
        /// <param name="c"></param>
        public AskValues(IDictionary<string, object> v, ICentipedeCore c)
                : base("Ask For Values", v, c)
        { }


        [ActionArgument(displayName = "Variables", usage="Variable names, separated by commas")]
        public string VariablesToSet = "";

        /// <summary>
        /// Perform the action
        /// </summary>
        /// <exception cref="ActionException">
        /// the action cannot be completed
        /// </exception>
        protected override void DoAction()
        {
            Form form = new Form
                        {
                                AutoSize = true,
                                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                                StartPosition = FormStartPosition.CenterParent,
                                MinimizeBox = false,
                                MaximizeBox = false,
                                SizeGripStyle = SizeGripStyle.Hide
                        };
            TableLayoutPanel table = new TableLayoutPanel
                                     {

                                             ColumnCount = 2,
                                             GrowStyle = TableLayoutPanelGrowStyle.AddRows,
                                             Dock = DockStyle.Fill,
                                             AutoSize = true,
                                             AutoSizeMode = AutoSizeMode.GrowAndShrink

                                     };

            form.Controls.Add(table);
            if (String.IsNullOrEmpty(VariablesToSet))
            {
                throw new ActionException("No variables listed", this);
            }
            foreach (string varName in VariablesToSet.Split(',').Select(var => var.Trim()))
            {
                Label lbl = new Label
                            {
                                    Text = varName
                            };
                Object value = Variables[varName];

                TextBox tb = new TextBox
                             {
                                     Text = (value ?? "").ToString(),
                                     Tag = varName
                             };
                table.Controls.Add(lbl);
                table.Controls.Add(tb);
            }

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

            form.FormClosed += delegate(object sender, FormClosedEventArgs e)
                               {
                                   switch (form.DialogResult)
                                   {
                                   case DialogResult.OK:
                                       foreach (TextBox tb in table.Controls.OfType<TextBox>())
                                       {
                                           Variables[(string)tb.Tag] = tb.Text;
                                       }
                                       break;
                                   case DialogResult.Cancel:
                                       throw new FatalActionException("Cancel Clicked", this);
                                   }
                               };

            form.ShowDialog();
            
        }
    }

    [ActionCategory("UI", displayName = "Ask for values")]
    class AskBooleans : Action
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="v"></param>
        /// <param name="c"></param>
        public AskBooleans(IDictionary<string, object> v, ICentipedeCore c)
                : base("Ask For Values", v, c)
        { }


        [ActionArgument(displayName = "Variables", usage="Variable names, separated by commas")]
        public string VariablesToSet = "";

        /// <summary>
        /// Perform the action
        /// </summary>
        /// <exception cref="ActionException">
        /// the action cannot be completed
        /// </exception>
        protected override void DoAction()
        {
            Form form = new Form
                        {
                                AutoSize = true,
                                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                                StartPosition = FormStartPosition.CenterParent,
                                MinimizeBox = false,
                                MaximizeBox = false,
                                SizeGripStyle = SizeGripStyle.Hide
                        };
            
            if (String.IsNullOrEmpty(VariablesToSet))
            {
                throw new ActionException("No variables listed", this);
            }

            form.Controls.AddRange(this.VariablesToSet.Split(',')
                                       .Select(var => var.Trim())
                                       .Select(varName => new CheckBox
                                                          {
                                                                  Text = varName
                                                          }));
                
            

            form.Controls.AddRange(new Control[]
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

            form.FormClosed += delegate(object sender, FormClosedEventArgs e)
                               {
                                   switch (form.DialogResult)
                                   {
                                   case DialogResult.OK:
                                       foreach (CheckBox checkBox in form.Controls.OfType<CheckBox>())
                                       {
                                           Variables[checkBox.Text] = checkBox.Checked;
                                       }
                                       break;
                                   case DialogResult.Cancel:
                                       throw new FatalActionException("Cancel Clicked", this);
                                   }
                               };

            form.ShowDialog();
            
        }
    }
}


