using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Forms;
using CentipedeInterfaces;


namespace Centipede
{
    public partial class JobPropertyForm : Form
    {
        private CentipedeJob Job { get; set; }

        public JobPropertyForm(ref CentipedeJob job)
        {
            Job = job;
            
            InitializeComponent();

            if (!String.IsNullOrEmpty(job.Name))
            {
                JobNameTextbox.Text = job.Name;
            }
            if (!String.IsNullOrEmpty(job.InfoUrl))
            {
                InfoUrlTextbox.Text = job.InfoUrl;
            }
            if (!String.IsNullOrEmpty(job.Author))
            {
                AuthorTextbox.Text = job.Author;
            }
            if (!String.IsNullOrEmpty(job.AuthorContact))
            {
                ContactTextbox.Text = job.AuthorContact;
            }
        }

        private void BrowseButton_Click(object sender, EventArgs e)
        {
            openFileDialog1.FileName = InfoUrlTextbox.Text;
            openFileDialog1.ShowDialog(this);
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            InfoUrlTextbox.Text = ((OpenFileDialog)sender).FileName;
        }

        private void OKBtn_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(JobNameTextbox.Text))
            {
                MessageBox.Show(this, "Jobname Cannot be empty", null, MessageBoxButtons.OK, MessageBoxIcon.Error);
            
                DialogResult = DialogResult.None;
                return;
            
            }
            Job.InfoUrl = InfoUrlTextbox.Text;
            Job.Author = AuthorTextbox.Text;
            Job.AuthorContact = ContactTextbox.Text;
            Job.Name = JobNameTextbox.Text;

            Close();
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(this, "Set default author and contact details to current values?", "Set Defaults", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result != DialogResult.Yes)
            {
                return;
            }
            Properties.Settings.Default.DefaultAuthor = this.AuthorTextbox.Text;
            Properties.Settings.Default.DefaultContact = this.ContactTextbox.Text;
        }

        private void EmailLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(string.Format(@"mailto:{0}", ContactTextbox.Text));
        }

        private void TextBoxTextChanged(object sender, EventArgs e)
        {
            Dirty = true;
        }

        public bool Dirty { get; private set; }

        private void CancelBtn_Click(object sender, EventArgs e)
        {
            Dirty = false;
        }
    }
}
