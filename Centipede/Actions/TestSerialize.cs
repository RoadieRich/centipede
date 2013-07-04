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
            BinaryFormatter binaryFormatter = new BinaryFormatter();

            Stream stream = new MemoryStream();

            Dictionary<String, Object> vars = Variables.Where(kvp => !(kvp.Key == "sys" || kvp.Key == "math")).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

            binaryFormatter.Serialize(stream, vars);

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
    }

    [ActionCategory("Other Actions")]
    public class TestDeserialize : Action
    {
        public TestDeserialize(IDictionary<string, object> variables, ICentipedeCore core)
            : base("Test Deserialize", variables, core)
        { }


        [ActionArgument(displayName = "Variable to Deserialize", Literal = true, DisplayOrder = 1)]
        public String InVar = "serialized";

        [ActionArgument(displayName = "Target Variable", DisplayOrder = 0)]
        public String TypeName = "";


        [ActionArgument(displayName = "Destination Variable", Literal = true, DisplayOrder=2)]
        public String OutVar = "deserialized";


        protected override void DoAction()
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();

            MemoryStream stream = new MemoryStream();

            StreamWriter sw = new StreamWriter(stream);
            sw.Write(Variables[InVar]);
            sw.Flush();

            stream.Seek(0, SeekOrigin.Begin);

            IDictionary<String, object> vars = (IDictionary<string, object>)binaryFormatter.Deserialize(stream);

            Variables[OutVar] = vars[InVar];
        }
    }
}
