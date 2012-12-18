using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Centipede.PyAction;
using ScintillaNET;

namespace Centipede.PyAction
{
    public partial class PythonDisplayControl : Centipede.Actions.ActionDisplayControl
    {
        public PythonDisplayControl(Centipede.Action action)
        {
            InitializeComponent();
            base.ThisAction = action as PythonAction;
            SetProperties();
            PythonAction pyAct = action as PythonAction;

            Scintilla scintilla = new Scintilla();
           
            scintilla.TextChanged += new EventHandler(sourceControl_TextChanged);
            scintilla.ConfigurationManager.Language = "python";
            scintilla.Margins[0].Width = 20;
            scintilla.Dock = DockStyle.Fill;

            this.AttributeTable.Controls.Add(scintilla);
            AttributeTable.SetColumnSpan(scintilla, 2);
            
        }

        public new PythonAction ThisAction
        {
            get
            {
                return base.ThisAction as PythonAction;
            }
        }

        void sourceControl_TextChanged(object sender, EventArgs e)
        {
            Scintilla source = sender as Scintilla;
            ThisAction.Source = source.Text;
        }

        private void PythonDisplayControl_Load(object sender, EventArgs e)
        {
            ThisAction.ToString();
        }
    }
}
