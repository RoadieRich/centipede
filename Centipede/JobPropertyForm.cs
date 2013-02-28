using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Forms;

namespace Centipede
{
    public partial class JobPropertyForm : Form
    {
        private readonly CentipedeCore _core;

        public JobPropertyForm(CentipedeCore core)
        {
            _core = core;
            InitializeComponent();

            if (!String.IsNullOrEmpty(core.Job.Name))
            {
                JobNameTextbox.Text = core.Job.Name;
            }
            if (!String.IsNullOrEmpty(core.Job.InfoUrl))
            {
                InfoUrlTextbox.Text = core.Job.InfoUrl;
            }
            if (!String.IsNullOrEmpty(core.Job.Author))
            {
                JobNameTextbox.Text = core.Job.Name;
            }
            if (!String.IsNullOrEmpty(core.Job.AuthorContact))
            {
                ContactTextbox.Text = core.Job.AuthorContact;
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
                MessageBox.Show("Jobname Cannot be empty", null, MessageBoxButtons.OK, MessageBoxIcon.Error);
            
                DialogResult = DialogResult.None;
                return;
            
            }
            _core.Job.InfoUrl = InfoUrlTextbox.Text;
            _core.Job.Author = AuthorTextbox.Text;
            _core.Job.AuthorContact = ContactTextbox.Text;
            this._core.Job.Name = JobNameTextbox.Text;

            Close();
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Set default author and contact details to current values?", "Set Defaults", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result != DialogResult.Yes)
            {
                return;
            }
            Properties.Settings.Default.DefaultAuthor = this.AuthorTextbox.Text;
            Properties.Settings.Default.DefaultContact = this.ContactTextbox.Text;
            Properties.Settings.Default.Save();
        }

        private void EmailLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(string.Format(@"mailto:{0}", ContactTextbox.Text));
        }
    }
}
