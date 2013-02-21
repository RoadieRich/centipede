using System;
using System.Collections.Generic;
using System.IO;


namespace Centipede.Actions
{
    abstract class BaseTextFileAction : Action
    {
        protected BaseTextFileAction(String name, IDictionary<string, object> v)
            : base(name, v)
        { }

    }

    class OpenTextFileAction : BaseTextFileAction
    {
        public OpenTextFileAction(IDictionary<string, object> v)
            : base("Open Text File", v)
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

            Variables[FileVar] = File.Open(Filename, FileMode.OpenOrCreate, access);
        }


    }
}
