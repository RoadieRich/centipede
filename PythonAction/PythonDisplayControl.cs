using System;
using System.Drawing.Printing;
using System.Windows.Forms;
using ScintillaNET;


namespace PyAction
{
// ReSharper disable UnusedMember.Global
    public partial class PythonDisplayControl : Centipede.Actions.ActionDisplayControl
// ReSharper restore UnusedMember.Global
    {
        public PythonDisplayControl(Centipede.Action action)
        {
            InitializeComponent();
            ThisAction = action as PythonAction;
            SetProperties();

            NameLabel.Text = action.Name;

            var scintilla = new Scintilla
                            {
                                    ConfigurationManager = { Language = "python" },
                                    Dock = DockStyle.Fill,
                                    Margins = { Margin0 = { Width = 20 } }
                            };
            //scintilla.Margins[0].Width = 20;
            scintilla.TextChanged += sourceControl_TextChanged;
            
            this.AttributeTable.Controls.Add(scintilla);
            AttributeTable.SetColumnSpan(scintilla, 2);
            
        }

        public new PythonAction ThisAction
        {
            get
            {
                return base.ThisAction as PythonAction;
            }
            protected set
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
