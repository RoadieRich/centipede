using System;
using System.Collections.Generic;
using System.Linq;
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
            IEnumerator<string> enumerator = argv.GetEnumerator();

            Dictionary<string,string> result = new Dictionary<string, string>();
            
            string lastKey = String.Empty;

            while (enumerator.MoveNext())
            {
                
                string trimmed = enumerator.Current.TrimStart('/', '-', '+');
                if (trimmed != enumerator.Current)
                {
                    string flag = enumerator.Current.Substring(0, 1);
                    lastKey = trimmed;
                    result.Add(trimmed, flag == "/" ? null : flag);
                }
                else
                {
                    result[lastKey] = trimmed;
                }
            }

            return result;

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

