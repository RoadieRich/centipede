using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Centipede
{
    public partial class GetJob : Form
    {

        public GetJob()
        {
            InitializeComponent();
        }

        internal string getJobFileName()
        {
            if (DialogResult == System.Windows.Forms.DialogResult.Yes)
                return "Load Clicked";
            else if (DialogResult == System.Windows.Forms.DialogResult.No)
                return "New Clicked";
            else
                return "Canceled";
        }
    }
}
