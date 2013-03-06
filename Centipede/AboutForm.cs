using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using Centipede.Properties;


namespace Centipede
{
    public partial class AboutForm : Form
    {
        public AboutForm()
        {
            InitializeComponent();
        }

        private void AboutForm_Load(object sender, EventArgs e)
        {
            pictureBox1.Image = Resources.Centipede.ToBitmap();

            this.textBox1.Text = (Assembly.GetEntryAssembly()
                                          .GetCustomAttributes(typeof (AssemblyCopyrightAttribute), false)
                                          .OfType<AssemblyCopyrightAttribute>()
                                          .FirstOrDefault() ?? new AssemblyCopyrightAttribute(String.Empty))
                    .Copyright;
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(@"mailto:Rich@RoadieRich.co.uk");
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(@"mailto:Andrew.Carter@Robn.com");
        }
    }
}
