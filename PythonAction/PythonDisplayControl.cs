using System;
using System.Collections.Generic;
using System.Windows.Forms;
using ScintillaNET;


namespace PyAction
{

    public partial class PythonDisplayControl : Centipede.Actions.ActionDisplayControl
    {

        public PythonDisplayControl()
        {
            InitializeComponent();
        }

        public PythonDisplayControl(PythonAction action)
            : base(action, false)
        {
            InitializeComponent();

            Scintilla scintilla = new Scintilla
                                  {
                                      ConfigurationManager = {Language = @"python"},
                                      Dock = DockStyle.Fill,

                                      Margins = {Margin0 = {Width = 20}},
                                      Text = action.Source,
                                      Scrolling = {ScrollBars = ScrollBars.Vertical}
                                  };
            scintilla.TextChanged += sourceControl_TextChanged;
            scintilla.TextChanged += SetDirty;

            AttributeTable.Controls.Add(scintilla);
            AttributeTable.SetColumnSpan(scintilla, 2);

        }

        private new PythonAction ThisAction
        {
            get { return base.ThisAction as PythonAction; }
            set { base.ThisAction = value; }
        }

        private void sourceControl_TextChanged(object sender, EventArgs e)
        {
            var source = sender as Scintilla;
            if (source != null)
            {
                ThisAction.Source = source.Text;
            }
        }
    }
}