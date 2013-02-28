using System;
using System.Collections.Generic;
using System.Windows.Forms;


    //  LINQ
    //   \o/
    // All the
    // things


namespace Centipede
{
    internal static class Program
    {
        private static MainWindow _mainForm;

        private static Dictionary<string, string> ParseArguments(IEnumerable<string> argv)
        {
            string key = null;
            List<String> valueWords = new List<string>();
            Dictionary<String, String> arguments = new Dictionary<string, string>();
            foreach (string s in argv)
            {
                if (s.StartsWith(@"/") || s.StartsWith(@"-") || s.StartsWith(@"+"))
                {
                    if (key != null)
                    {
                        arguments.Add(key, valueWords.Count > 0 ? String.Join(@" ", valueWords) : null);
                    }

                    key = s.Substring(1);
                    valueWords.Clear();
                }
                else
                {
                    valueWords.Add(s);
                }

            }

            return arguments;
        }

        private static void RunHeadless(Dictionary<string, string> argv)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     The main entry point for the application.
        /// </summary>
        /// <param name="argv"></param>
        [STAThread]
        private static void Main(String[] argv)
        {
            Dictionary<String, String> arguments = ParseArguments(argv);

            if (arguments.ContainsKey(@"/nogui"))
            {
                RunHeadless(arguments);
            }
            else
            {
                RunGui(arguments);
            }
        }

        private static void RunGui(Dictionary<string, string> arguments)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            _mainForm = new MainWindow(new CentipedeCore(arguments), arguments);
            Application.Run(_mainForm);
        }
    }

    
}

