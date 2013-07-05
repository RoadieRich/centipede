using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
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


        [ActionArgument(displayName = "Destination Variable", Literal = true)]
        public String OutVar = "serialized";


        protected override void DoAction()
        {
            Stream stream = new MemoryStream();

            var variable = Variables[InVar];

            CentipedeSerializer.Serialize(stream, variable);

            stream.Seek(0, SeekOrigin.Begin);

            string text = new StreamReader(stream).ReadToEnd();

            if (!String.IsNullOrEmpty(OutVar))
            {
                Variables[OutVar] = text;
            }
            else
            {
                MessageBox.Show(text);
            }

        }
        [ActionArgument]
        public string InVar;
    }

    [ActionCategory("Other Actions")]
    public class TestDeserialize : Action
    {
        public TestDeserialize(IDictionary<string, object> variables, ICentipedeCore core)
            : base("Test Deserialize", variables, core)
        { }


        [ActionArgument(displayName = "Variable to Deserialize", Literal = true, DisplayOrder = 1)]
        public String InVar = "serialized";
        
        [ActionArgument(displayName = "Destination Variable", Literal = true, DisplayOrder=2)]
        public String OutVar = "deserialized";


        protected override void DoAction()
        {
            MemoryStream stream = new MemoryStream();

            StreamWriter sw = new StreamWriter(stream);
            sw.Write(Variables[InVar]);
            sw.Flush();

            stream.Seek(0, SeekOrigin.Begin);
            
            Variables[OutVar] = CentipedeSerializer.Deserialize(stream);
        }
    }
}
