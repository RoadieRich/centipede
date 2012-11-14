using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Centipede
{
    abstract class BaseTextFileAction : Action
    {
        protected BaseTextFileAction(String name, Dictionary<String, Object> variables)
            : base(name, variables)
        { }
    }

    class OpenTextFileAction : BaseTextFileAction
    {
        public OpenTextFileAction(Dictionary<String, Object> variables)
            : base("Open Text File", variables)
        {
            Attributes["Filename"] = "";
            Attributes["Read"] = "1";
            Attributes["Write"] = "1";
            Attributes["Destination Var"] = "Text File";
        }

        [ActionArgument(usage="Filename to open")]
        public String Filename = "";

        [ActionArgument(usage="Allow read access")]
        public Boolean Read = true;

        [ActionArgument(usage="Allow write access to file")]
        public Boolean Write = true;

        [ActionArgument(
            usage = "Variable to store the opened file in",
            displayName = "File Variable"
        )]
        public String FileVar = "Text File";

        public override void DoAction()
        {
            String filename = ParseAttribute<String>("Filename");
            FileAccess access = ParseBoolAttribute("Read") ? FileAccess.Read : 0;
            access |= ParseBoolAttribute("Write") ? FileAccess.Write : 0;

            Program.Variables[Attributes["Destination Var"] as String] = File.Open(filename, FileMode.OpenOrCreate, access);
        }


    }
}
