using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Centipede.Properties;


namespace Centipede
{
    public partial class AboutForm : Form
    {
        private readonly Dictionary<FileInfo, List<Type>> _pluginFiles;

        public AboutForm(Dictionary<FileInfo, List<Type>> pluginFiles)
        {
            _pluginFiles = pluginFiles;
            InitializeComponent();
        }

        private void AboutForm_Load(object sender, EventArgs e)
        {
            pictureBox1.Image = Resources.Centipede.ToBitmap();

            string indent =
                    String.Concat(new string[_pluginFiles.Keys.Select(fi => fi.Name.Length).Max() + 2].Select(s => " "));

            StringBuilder sb = new StringBuilder();

            foreach (KeyValuePair<FileInfo, List<Type>> pluginFile in _pluginFiles)
            {
                FileInfo file = pluginFile.Key;
                List<Type> types = pluginFile.Value;
                sb.Append(file).Append(@": ").Append(indent.Substring(file.Name.Length + 2));
                foreach (Type action in types)
                {
                    sb.AppendLine(action.Name).Append(indent);
                }
                sb.AppendLine();
            }

            this.textBox1.Text = sb.ToString();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(@"mailto:Rich@RoadieRich.co.uk");
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(@"mailto:Andrew.Carter@Robn.com");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
