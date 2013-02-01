using System;
using System.Linq;
using System.Windows.Forms;
using Centipede;
using Centipede.Actions;
using Centipede.Properties;
using ScintillaNET;
using Action = Centipede.Action;


namespace PyAction
{
// ReSharper disable UnusedMember.Global
    class BranchDisplayControl : ActionDisplayControl
// ReSharper restore UnusedMember.Global
    {
        public BranchDisplayControl(BranchAction action)
                : base(action, false)
        {
            InitializeComponent();
            var actionCombo = new ComboBox { DisplayMember = @"Text", ValueMember = @"Action" };

            actionCombo.DropDown += actionCombo_DropDown;
            actionCombo.SelectionChangeCommitted +=
                    (sender, e) =>
                        {
                            ComboBox combo = sender as ComboBox;
                            if (combo == null)
                            {
                                throw new ArgumentException();
                            }
                            ThisAction.NextIfTrue = (combo.SelectedValue) as Action;
                        };
            var actionComboLabel = new Label { Text = Resources.BranchDisplayControl_BranchDisplayControl_Action_if_false };
            AttributeTable.Controls.Add(actionComboLabel);
            AttributeTable.Controls.Add(actionCombo);

            Scintilla conditionControl = new Scintilla
                                         {
                                                 ConfigurationManager = { Language = @"python" },
                                                 Height = 20,
                                                 AcceptsReturn = false,
                                                 Text = ThisAction.ConditionSource,
                                                 Dock = DockStyle.Fill
                                         };
            conditionControl.TextChanged += (sender, e) =>
                                                {
                                                    Scintilla scintilla = sender as Scintilla;
                                                    if (scintilla == null)
                                                    {
                                                        throw new ArgumentException();
                                                    }
                                                    ThisAction.ConditionSource = (scintilla).Text;
                                                };

            AttributeTable.Controls.Add(new Label { Text = Resources.BranchDisplayControl_BranchDisplayControl_Condition });
            AttributeTable.Controls.Add(conditionControl);
        }
        
        void actionCombo_DropDown(object sender, EventArgs e)
        {
            ComboBox combo = sender as ComboBox;
            if (combo == null)
            {
                throw new ArgumentException();
            }
            var actionIter = from Action a in Program.Instance.Actions
                             where a != ThisAction
                             select new
                                    {
                                            Text = String.Format("{0}: {1}",
                                                              a.Name, a.Comment),
                                            Action = a
                                    };
            combo.DataSource = actionIter.ToList();
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
            Name = "BranchDisplayControl";
            Size = new System.Drawing.Size(206, 107);
            NameLabel.Text = ThisAction.Name;
            
            ResumeLayout(false);
            PerformLayout();

        }
    }
}
