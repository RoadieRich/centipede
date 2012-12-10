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
        public GetJobResult Result;
        internal string getJobFileName()
        {
            switch (Result)
            {
                case GetJobResult.New:
                    return "";
                case GetJobResult.Open:
                    JobControl selectedJob = FavouritesListbox.SelectedItem as JobControl;
                    return selectedJob.Filename;
                default:
                    return null;
            
            }
        }

        private void BrowseButton_Click(object sender, EventArgs e)
        {
            DialogResult result = openFileDialog1.ShowDialog(this);
        }


        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            int index = FavouritesListbox.Items.Add(new JobControl(openFileDialog1.FileName));
            FavouritesListbox.SelectedIndex = index;
        }

        private void NewButton_Click(object sender, EventArgs e)
        {
            Result = GetJobResult.New;
            this.Close();
        }

        private void LoadButton_Click(object sender, EventArgs e)
        {
            Result = GetJobResult.Open;
            this.Close();
        }
    }

    enum GetJobResult
    {
        New,
        Open,
        Cancel
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
