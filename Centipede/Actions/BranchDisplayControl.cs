using System;
using System.Linq;
using System.Windows.Forms;
using Centipede.Properties;


namespace Centipede.Actions
{
// ReSharper disable UnusedMember.Global
    class BranchDisplayControl : ActionDisplayControl
// ReSharper restore UnusedMember.Global
    {
        BranchDisplayControl(Action action) 
            : base(action)
        {
            var actionCombo = new ComboBox();
            actionCombo.DropDown += actionCombo_DropDown;
            actionCombo.SelectionChangeCommitted += actionCombo_SelectionChangeCommitted;
            actionCombo.DisplayMember = "DisplayText";
            actionCombo.ValueMember = "Action";
            var actionComboLabel = new Label { Text = Resources.BranchDisplayControl_BranchDisplayControl_Action_if_false };
            AttributeTable.Controls.Add(actionComboLabel);
            AttributeTable.Controls.Add(actionCombo);

            Control[] conditionControls = ThisAction.Condition.MakeControls();
            AttributeTable.Controls.AddRange(conditionControls);
            if (conditionControls.Length == 1)
            {
                AttributeTable.SetColumnSpan(conditionControls[0], 2);
            }
        }

        void actionCombo_SelectionChangeCommitted(object sender, EventArgs e)
        {
            var combo = sender as ComboBox;
            ThisAction.Condition = combo.SelectedValue as BranchCondition;
        }

        void actionCombo_DropDown(object sender, EventArgs e)
        {
            var combo = sender as ComboBox;
            combo.Items.Clear();
            combo.Items.AddRange(from Action a in Program.Actions
                                     where a != ThisAction
                                     select new {DisplayText = String.Format("{0}: {1}", 
                                                 a.Name, a.Comment), Action = a}
                                );
        }

        private new BranchAction ThisAction
        {
            get
            {
                return base.ThisAction as BranchAction;
            }
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // BranchDisplayControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.Name = "BranchDisplayControl";
            this.Size = new System.Drawing.Size(186, 87);
            this.ResumeLayout(false);
            this.PerformLayout();

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
