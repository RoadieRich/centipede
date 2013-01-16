using System;
using System.Linq;
using System.Windows.Forms;
using Centipede.Properties;
using ScintillaNET;


namespace Centipede.Actions
{
// ReSharper disable UnusedMember.Global
    class BranchDisplayControl : ActionDisplayControl
// ReSharper restore UnusedMember.Global
    {
        public BranchDisplayControl(Action action)
        {
            base.ThisAction = action;

            action.Tag = this;

            InitializeComponent();
            var actionCombo = new ComboBox();
            actionCombo.DropDown += actionCombo_DropDown;
            actionCombo.SelectionChangeCommitted +=
                    (sender, e) => ThisAction.NextIfFalse = ((dynamic)(sender as ComboBox).SelectedItem).A as Action;
            var actionComboLabel = new Label { Text = Resources.BranchDisplayControl_BranchDisplayControl_Action_if_false };
            AttributeTable.Controls.Add(actionComboLabel);
            AttributeTable.Controls.Add(actionCombo);

            Scintilla conditionControl = new Scintilla
                                         {
                                                 ConfigurationManager = { Language = "python" },
                                                 Height = 20,
                                                 AcceptsReturn = false,
                                                 Text = ThisAction.ConditionSource
                                         };
            conditionControl.TextChanged += (sender, e) => { ThisAction.ConditionSource = (sender as Scintilla).Text; };

            AttributeTable.Controls.Add(new Label { Text = Resources.BranchDisplayControl_BranchDisplayControl_Condition });
            AttributeTable.Controls.Add(conditionControl);
        }
        
        void actionCombo_DropDown(object sender, EventArgs e)
        {
            var combo = sender as ComboBox;
            combo.Items.Clear();
            var items = from Action a in Program.Actions
                        where a != ThisAction
                        select new
                               {
                                       S = String.Format("{0}: {1}",
                                                         a.Name, a.Comment),
                                       A = a
                               }
                        into c
                        select c;
            combo.Items.AddRange(items);
            combo.DisplayMember = "S";
            combo.ValueMember = "A";
        }

        private new BranchAction ThisAction
        {
            get
            {
                return base.ThisAction as BranchAction;
            }
        }

        private new void InitializeComponent()
        {
            SuspendLayout();
            base.InitializeComponent();
            
            Name = "BranchDisplayControl";
            Size = new System.Drawing.Size(206, 107);
            NameLabel.Text = ThisAction.Name;
            
            ResumeLayout(false);
            PerformLayout();

        }
    }

    internal static class ListExtention
    {
        public static void AddRange(this ComboBox.ObjectCollection list, System.Collections.IEnumerable items)
        {
            foreach (Object item in items)
            {
                list.Add(item);
            }
        }
    }
}
