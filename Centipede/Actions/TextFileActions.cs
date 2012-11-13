using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Centipede
{
    abstract class BaseTextFileAction : Action
    {
        protected BaseTextFileAction(String name)
            : base(name)
        { }
    }

    class OpenTextFileAction : BaseTextFileAction
    {
        public OpenTextFileAction()
            : base("Open Text File")
        {
            Attributes["Filename"] = "";
            Attributes["Read"] = "1";
            Attributes["Write"] = "1";
            Attributes["Destination Var"] = "Text File";
        }

        public override void DoAction()
        {
            String filename = ParseAttribute<String>("Filename");
            FileAccess access = ParseBoolAttribute("Read") ? FileAccess.Read : 0;
            access |= ParseBoolAttribute("Write") ? FileAccess.Write : 0;

            Program.Variables[Attributes["Destination Var"] as String] = File.Open(filename, FileMode.OpenOrCreate, access);
        }


    }
}
