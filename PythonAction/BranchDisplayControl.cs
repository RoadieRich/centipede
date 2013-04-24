using System;
using System.Linq;
using System.Windows.Forms;
using Centipede;
using Centipede.Actions;
using Centipede.Properties;
using CentipedeInterfaces;
using ScintillaNET;
using Action = Centipede.Action;
using ResharperAnnotations;

namespace PyAction
{
    [UsedImplicitly]
    class BranchDisplayControl : ActionDisplayControl
    {
        private readonly ComboBox _actionCombo;
        private readonly Scintilla _conditionControl;

        public BranchDisplayControl(IAction action)
                : base(action, false)
        {
            InitializeComponent();

            base.ThisAction = action;
            _actionCombo = new ComboBox
                           {
                                   DisplayMember = @"Text",
                                   ValueMember = @"Action",
                                   Dock = DockStyle.Fill
                           };

            _actionCombo.DropDown += actionCombo_DropDown;
            _actionCombo.SelectionChangeCommitted +=
                    (sender, e) =>
                        {
                            ComboBox combo = sender as ComboBox;
                            if (combo == null)
                            {
                                throw new ArgumentException();
                            }
                            ThisAction.NextIfTrue = (combo.SelectedValue) as Action;
                        };
            var actionComboLabel = new Label { Text = "Action if True" };
            AttributeTable.Controls.Add(actionComboLabel);
            AttributeTable.Controls.Add(_actionCombo);

            _conditionControl = new Scintilla
                                {
                                    ConfigurationManager = { Language = @"python" },
                                    Height = 20,
                                    AcceptsReturn = false,
                                    Text = ThisAction.ConditionSource,
                                    Dock = DockStyle.Fill
                                };
            _conditionControl.TextChanged += (sender, e) =>
                                             {
                                                 Scintilla scintilla = sender as Scintilla;
                                                 if (scintilla == null)
                                                 {
                                                     throw new ArgumentException();
                                                 }
                                                 ThisAction.ConditionSource = (scintilla).Text;
                                             };

            AttributeTable.Controls.Add(new Label { Text = Resources.BranchDisplayControl_BranchDisplayControl_Condition });
            AttributeTable.Controls.Add(_conditionControl);
        }
        
        void actionCombo_DropDown(object sender, EventArgs e)
        {
            ComboBox combo = sender as ComboBox;
            if (combo == null)
            {
                throw new ArgumentException();
            }
            PopulateComboBox();
        }

        private void PopulateComboBox()
        {
            ICentipedeCore core = ThisAction.GetCurrentCore();
            lock (core.Job)
            {
                var actionIter = from Action a in core.Job.Actions
                                 where a != ThisAction
                                 select new
                                        {
                                            Text = String.Format("{0}: {1}",
                                                                 a.Name, a.Comment),
                                            Action = a
                                        };
                _actionCombo.DataSource = actionIter.ToList();
            }
        }

        public override void UpdateControls()
        {
            PopulateComboBox();
            _actionCombo.SelectedIndex = ((MainWindow)ParentForm).Core.Job.Actions.WhereNot(ThisAction.Equals).ToList().IndexOf(ThisAction.NextIfTrue);
            _conditionControl.Text = ThisAction.ConditionSource;
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
            base.InitializeComponent();

        }
    }
}
