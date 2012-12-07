using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace Centipede
{
    internal partial class GetJob : Form
    {
        public GetJob()
        {
            InitializeComponent();
        }

        internal string getJobFileName()
        {
            if (DialogResult == System.Windows.Forms.DialogResult.Yes)

                return "";
            else if (DialogResult == System.Windows.Forms.DialogResult.No)
                return "New Clicked";
            else
                return "Canceled";
        }

        private void BrowseButton_Click(object sender, EventArgs e)
        {
            DialogResult result = openFileDialog1.ShowDialog(this);
            if (result == System.Windows.Forms.DialogResult.Cancel)
            {
                return;
            }
            else
            {
                int index = FavouritesListbox.Items.Add(new JobControl(openFileDialog1.FileName));
                FavouritesListbox.SelectedIndex = index;
            }
        }

        private void FavouritesListbox_DoubleClick(object sender, EventArgs e)
        {
            FavouritesListbox.Items.ToString();
        }
    }

    class JobControl : Control
    {
        public readonly string Filename;

        public JobControl(String filename)
        {
            Filename = filename;
            Text = GetJobName();
        }

        private string GetJobName()
        {
            return Path.GetFileNameWithoutExtension(Filename);
        }
    }
}
