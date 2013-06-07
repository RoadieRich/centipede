using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using NDesk.Options;


//  LINQ
//   \o/
// All the
// things


namespace Centipede
{
    internal static class Program
    {
        private static MainWindow _mainForm;
        private static bool _run;

        private static void RunHeadless(List<string> argv)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     The main entry point for the application.
        /// </summary>
        /// <param name="argv">the command line arguments.  Currently unused.</param>
        [STAThread]
        private static void Main(String[] argv)
        {

            bool headless = false;

            NDesk.Options.OptionSet options = new OptionSet
                                              {
                                                  {
                                                      "r|run", "run", value => _run = value != null
                                                  },
                                                  {
                                                      "n|nogui", "run without gui", value => headless = value != null
                                                  }
                                              };

            List<string> extras = options.Parse(argv);

            if (headless)
            {
                RunHeadless(extras);
            }
            else
            {
                RunGui(extras);
            }
        }

        private static void RunGui(List<string> arguments)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            CentipedeCore core = new CentipedeCore(arguments);
            _mainForm = new MainWindow(core, arguments);
            if (arguments.Any())
            {
                _mainForm.LoadJobAfterLoad(arguments.First());
                if (_run)
                {
                    _mainForm.RunJobAfterLoad();
                }
            }

            Application.Run(_mainForm);
        }
    }
}

