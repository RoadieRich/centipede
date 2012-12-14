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
        { }

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

        protected override void DoAction()
        {
            FileAccess access = Read ? FileAccess.Read : 0;
            access |= Write ? FileAccess.Write : 0;

            Program.Variables[FileVar] = File.Open(Filename, FileMode.OpenOrCreate, access);
        }


    }
}
