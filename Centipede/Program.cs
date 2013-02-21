using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using Centipede.Properties;


    //  LINQ
    //   \o/
    // All the
    // things


namespace Centipede
{
    internal class Program
    {
        private static MainWindow _mainForm;

        /// <summary>
        ///     The main entry point for the application.
        /// </summary>
        /// <param name="argv"></param>
        [STAThread]
        private static void Main(String[] argv)
        {
            if (argv.Contains("/nogui"))
            {
                return;
            }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            _mainForm = new MainWindow(new CentipedeCore());
            Application.Run(_mainForm);
        }

    }

    
}

