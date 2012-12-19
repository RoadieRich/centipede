using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Centipede.Actions;

namespace Centipede
{
    class BranchDisplayControl : ActionDisplayControl
    {
        BranchDisplayControl(Action action) 
            : base(action)
        {
            ComboBox actionCombo = new ComboBox();
            actionCombo.DropDown += new EventHandler(actionCombo_DropDown);
            actionCombo.SelectionChangeCommitted += new EventHandler(actionCombo_SelectionChangeCommitted);
            actionCombo.DisplayMember = "DisplayText";
            actionCombo.ValueMember = "Action";
            Label actionComboLabel = new Label();
            actionComboLabel.Text = "Action if false";
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
            ComboBox combo = sender as ComboBox;
            ThisAction.Condition = combo.SelectedValue as BranchCondition;
        }

        void actionCombo_DropDown(object sender, EventArgs e)
        {
            ComboBox combo = sender as ComboBox;
            combo.Items.Clear();
            combo.Items.AddRange(from Action a in Program.Actions
                                     where a != ThisAction
                                     select new {DisplayText = String.Format("{0}: {1}", 
                                                 a.Name, a.Comment), Action = a}
                                );
        }

        public new BranchAction ThisAction
        {
            get
            {
                return base.ThisAction as BranchAction;
            }
        }

        #region Designer required code
        private new void InitializeComponent()
        {
            base.InitializeComponent();
            this.SuspendLayout();
            // 
            // BranchDisplayControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.Name = "BranchDisplayControl";
            this.Size = new System.Drawing.Size(191, 117);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion
    }

    internal static class ListExtention
    {
        public static void AddRange(this System.Windows.Forms.ComboBox.ObjectCollection list, System.Collections.IEnumerable items)
        {
            foreach (Object item in items)
            {
                list.Add(item);
            }
        }
    }
}
