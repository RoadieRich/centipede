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
        private static bool _showDemo;

        private static void RunHeadless(List<string> arguments)
        {
            CentipedeCore core = new CentipedeCore(arguments);
            core.LoadJob(arguments.First());
            core.RunJob();
        }

        /// <summary>
        ///     The main entry point for the application.
        /// </summary>
        /// <param name="argv">the command line arguments.</param>
        [STAThread]
        private static void Main(String[] argv)
        {

            bool headless = false;
            var options = new OptionSet
                                              {
                                                  {
                                                      "r|run", "run", value => _run = value != null
                                                  },
                                                  {
                                                      "n|nogui", "run without gui",
                                                      value => headless = value != null
                                                  },
                                                  {
                                                      "D", value => _showDemo = value != null
                                                  }
                                              };
            options.Add("?|help", s => options.WriteOptionDescriptions(Console.Out));

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

            var core = new CentipedeCore(arguments);
            _mainForm = new MainWindow(core, arguments) {ShowDemoAction = _showDemo};
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

