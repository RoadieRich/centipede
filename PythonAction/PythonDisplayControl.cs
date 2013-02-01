using System;
using System.Windows.Forms;
using ScintillaNET;


namespace PyAction
{
// ReSharper disable UnusedMember.Global
    public partial class PythonDisplayControl : Centipede.Actions.ActionDisplayControl
// ReSharper restore UnusedMember.Global
    {
        public PythonDisplayControl(PythonAction action)
                : base(action, false)
        {
            InitializeComponent();
            
            var scintilla = new Scintilla
                            {
                                    ConfigurationManager = { Language = @"python" },
                                    Dock = DockStyle.Fill,
                                    Margins = { Margin0 = { Width = 20 } },
                                    Text = action.Source
                            };
            //scintilla.Margins[0].Width = 20;
            scintilla.TextChanged += sourceControl_TextChanged;
            
            AttributeTable.Controls.Add(scintilla);
            AttributeTable.SetColumnSpan(scintilla, 2);
            
        }

        private new PythonAction ThisAction
        {
            get
            {
                return base.ThisAction as PythonAction;
            }
            set
            {
                base.ThisAction = value;
            }
        }

        void sourceControl_TextChanged(object sender, EventArgs e)
        {
            var source = sender as Scintilla;
            if (source != null)
            {
                ThisAction.Source = source.Text;
            }
        }
    }
}
