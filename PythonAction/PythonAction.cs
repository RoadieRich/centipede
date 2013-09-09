using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using System.Xml.XPath;
using Centipede;
using CentipedeInterfaces;
using PythonEngine;
using System.Linq;
using Action = Centipede.Action;


namespace PyAction
{

    [ActionCategory("Other Actions",
            IconName = @"pycon",
            DisplayName = "Python Action",
            DisplayControl = @"PythonDisplayControl"
            )]
    // ReSharper disable ClassNeverInstantiated.Global
    public class PythonAction : Action
            // ReSharper restore ClassNeverInstantiated.Global
    {

        public PythonAction(IDictionary<string, object> variables, ICentipedeCore c)
                : base("Python Action", variables, c)
        {
            _source = "";
        }

        [ActionArgument(Usage = "Source code to be executed", Literal = true)]
        public String Source
        {
            // ReSharper disable MemberCanBePrivate.Global
            get
            {
                return _source;
            }
            set
            {
                _source = value;
                //Complexity is the count of non-black, non-comment lines
                int count =
                    value.Split(Environment.NewLine.ToCharArray()).
                          Count(
                              s => !String.IsNullOrWhiteSpace(s)
                                   || !s.TrimStart().StartsWith(@"#"));
                _complexity = count;

            }
        }

        private int _complexity;
        private string _source;

        /*
                public void UpdateSource(object sender, EventArgs e)
                {
                    ScintillaNET.Scintilla control = sender as ScintillaNET.Scintilla;
                    Source = control.Text;
                    this._complexity = Source.Split(System.Environment.NewLine.ToCharArray()).Length;
                }
        */

        protected override void DoAction()
        {
            IPythonEngine engine = GetCurrentCore().PythonEngine;
            //if (!engine.VariableExists(@"variables"))
            //{
            //    engine.SetVariable(@"variables", Variables);
            //}
            try
            {
                engine.Execute(Source);
            }
            catch (PythonException e)
            {
                throw new ActionException(e, this);
            }
        }

        public override int Complexity
        {
            get
            {
                return _complexity;
            }
        }

        [Localizable(false)]
        public override void AddToXmlElement(XmlElement rootElement)
        {
            XmlDocument ownerDocument = rootElement.OwnerDocument;
            Type thisType = GetType();

            XmlElement element = ownerDocument.CreateElement(thisType.FullName);
            XmlText sourceText = ownerDocument.CreateTextNode(Source);

            element.AppendChild(sourceText);
            String pluginFilePath = Path.GetFileName(thisType.Assembly.CodeBase);

            element.SetAttribute("Comment", Comment);
            element.SetAttribute("Assembly", pluginFilePath);
            element.SetAttribute("Complexity", Complexity.ToString(CultureInfo.InvariantCulture));

            rootElement.AppendChild(element);
        }

        [Localizable(false)]
        protected override void PopulateMembersFromXml(XPathNavigator element)
        {
            Source = element.Value;
            Comment = element.SelectSingleNode("@Comment").Value;
        }
    }

    [ActionCategory("Other Actions",
            IconName = @"pycon",
            DisplayName = "Import Python Module")]
    public class Import : Action
    {
        public Import(IDictionary<string, object> variables, ICentipedeCore core)
            : base("Import module", variables, core)
        { }

        [ActionArgument(Literal=true)]
        public string Modules = "";

        protected override void DoAction()
        {
            IPythonEngine engine = GetCurrentCore().PythonEngine;

            try
            {
                foreach (var module in Modules.Split(',').Select(s => s.Trim()))
                {
                    if (module == "os")
                    {
                        //workaround for os module not included in embedded ironpython
                        string pythonLibDir =
                            Path.Combine(
                                         Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86),
                                         "ironpython 2.7",
                                         "lib"); //default install location

                        if (!Directory.Exists(pythonLibDir))
                        {
                            var activeForm = Form.ActiveForm ?? (Form) this.GetCurrentCore().Tag;
                            pythonLibDir =
                                (string) activeForm.Invoke(new Func<string>(this.GetPythonLibDir));
                            if (String.IsNullOrEmpty(pythonLibDir))
                            {
                                throw new FatalActionException("Could not find Python Lib folder",
                                                               this);
                            }
                            engine.ImportModule("sys");
                            engine.Execute(string.Format("sys.path.append('{0}')\n",
                                                         pythonLibDir));
                            engine.ImportModule("os");
                        }
                    }
                    else
                    {
                        engine.ImportModule(module);
                    }
                }
            }
            catch (PythonException e)
            {
                throw new ActionException(e, this);
            }
        }

        private string GetPythonLibDir()
        {
            var dialog = new FolderBrowserDialog()
                         {
                             SelectedPath=@"c:/program files (x86)/ironpython 2.7/lib",
                             
                             Description =
                                 "Cannot find your IronPython Lib directory.\n" +
                                 "Please select it before continuing.",
                             ShowNewFolderButton = false
                         };


            var result = dialog.ShowDialog();
            if (result == DialogResult.Cancel)
                return null;
            return dialog.SelectedPath;
        }
    }
}
