using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;
using CentipedeInterfaces;

namespace Centipede.Actions
{
    [ActionCategory("Other Actions")]
    public class TestSerialize : Action
    {
        public TestSerialize(IDictionary<string, object> variables, ICentipedeCore core)
            : base("Test Serialize", variables, core)
        { }


        [ActionArgument(displayName = "Variable to Serialize", Literal = true)]
        public String InVar = "";
        [ActionArgument(displayName = "Destination Variable", Literal = true)]
        public String OutVar = "";


        protected override void DoAction()
        {
            object obj = Variables[InVar];
            XmlSerializer xmlSerializer = new XmlSerializer(obj.GetType());
            
            MemoryStream memoryStream = new MemoryStream();
            
            xmlSerializer.Serialize(memoryStream, obj);
            memoryStream.Seek(0, SeekOrigin.Begin);
            string text = (new StreamReader(memoryStream)).ReadToEnd();

            if (!String.IsNullOrEmpty(OutVar))
            {
                Variables[OutVar] = text;
            }
            else
            {
                MessageBox.Show(text);
            }

        }
    }

    [ActionCategory("Other Actions")]
    public class TestDeserialize : Action
    {
        public TestDeserialize(IDictionary<string, object> variables, ICentipedeCore core)
            : base("Test Deserialize", variables, core)
        { }


        [ActionArgument(displayName = "Variable to Deserialize", Literal = true, DisplayOrder = 0)]
        public String InVar = "";

        [ActionArgument(displayName = "Variable to Deserialize", DisplayOrder = 1)]
        public String TypeName = "";


        [ActionArgument(displayName = "Destination Variable", Literal = true, DisplayOrder=2)]
        public String OutVar = "";


        protected override void DoAction()
        {
            Type t = Assembly.GetCallingAssembly().GetType(ParseStringForVariable(TypeName));
            XmlSerializer xmlSerializer = new XmlSerializer(t);

            object obj = xmlSerializer.Deserialize(new StringReader((string)Variables[this.InVar]));
            
            Variables[OutVar] = obj;
        }
    }
}
