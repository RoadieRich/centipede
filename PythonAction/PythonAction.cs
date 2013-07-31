using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
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
                foreach (var module in Modules.Split(','))
                {
                    engine.ImportModule(module.Trim());
                }
            }
            catch (PythonException e)
            {
                throw new ActionException(e, this);
            }
        }
    }
}
