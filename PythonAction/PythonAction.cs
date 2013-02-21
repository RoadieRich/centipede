using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Xml;
using Centipede;
using PythonEngine;
using Action = Centipede.Action;


namespace PyAction
{

    [ActionCategory("Other Actions",
            iconName = @"pycon",
            displayName = "Python Action",
            displayControl = @"PythonDisplayControl"
            )]
    // ReSharper disable ClassNeverInstantiated.Global
    public class PythonAction : Centipede.Action
            // ReSharper restore ClassNeverInstantiated.Global
    {

        public PythonAction(IDictionary<String, Object> v)
                : base("Python Action", v)
        { }

        [ActionArgument(usage = "Source code to be executed")]
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
                _complexity = value.Split(Environment.NewLine.ToCharArray()).Length;
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
            PythonEngine.PythonEngine engine = PythonEngine.PythonEngine.Instance;
            if (!engine.VariableExists(@"variables"))
            {
                engine.SetVariable(@"variables", Variables);
            }
            try
            {
                engine.Execute(ParseStringForVariable(Source));
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

        protected override void PopulateMembersFromXml(XmlElement element)
        {
            Source = element.InnerText;
            Comment = element.GetAttribute("Comment");
        }
    }
}
