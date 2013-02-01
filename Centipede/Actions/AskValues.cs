using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;


namespace Centipede.Actions
{
    [ActionCategory("UI", displayName = "Ask for values")]
    class AskValues : Action
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="v"></param>
        public AskValues(string name, Dictionary<string, object> v)
                : base("Ask For Values", v)
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
            Form form = new Form();
            TableLayoutPanel table = new TableLayoutPanel { 
                ColumnCount = 2,
                GrowStyle=TableLayoutPanelGrowStyle.AddRows,
                Anchor = AnchorStyles.Top|AnchorStyles.Right|AnchorStyles.Left
            };

            form.Controls.Add(table);

            foreach (string varName in VariablesToSet.Split(',').Select(var => var.Trim()))
            {
                Label lbl = new Label
                            {
                                    Text = varName
                            };
                TextBox tb = new TextBox
                             {
                                     Text = Variables[varName].ToString(),
                                     Tag = varName
                             };
                table.Controls.Add(lbl);
                table.Controls.Add(tb);
            }

            form.Controls.Add(new Button
                              {
                                      Text = "OK",
                                      DialogResult=DialogResult.OK
                              });

            form.FormClosed += delegate
                                   {
                                       if (form.DialogResult == DialogResult.OK)
                                       {
                                           foreach (TextBox tb in table.Controls.OfType<TextBox>())
                                           {
                                               Variables[(string)tb.Tag] = tb.Text;
                                           }
                                       }
                                   };

            form.ShowDialog();
            
        }
    }
}
