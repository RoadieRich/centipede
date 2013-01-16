using System;
using System.Collections.Generic;
using System.IO;


namespace Centipede.Actions
{
    abstract class BaseTextFileAction : Action
    {
        protected BaseTextFileAction(String name, Dictionary<String, Object> v)
            : base(name, v)
        { }
    }

// ReSharper disable UnusedMember.Global
    class OpenTextFileAction : BaseTextFileAction
// ReSharper restore UnusedMember.Global
    {
        public OpenTextFileAction(Dictionary<String, Object> v)
            : base("Open Text File", v)
        { }

        [ActionArgument(usage="Filename to open")]
// ReSharper disable ConvertToConstant.Global
// ReSharper disable FieldCanBeMadeReadOnly.Global
// ReSharper disable MemberCanBePrivate.Global
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

        // ReSharper restore MemberCanBePrivate.Global
        // ReSharper restore FieldCanBeMadeReadOnly.Global
        // ReSharper restore ConvertToConstant.Global
        protected override void DoAction()
        {
            FileAccess access = Read ? FileAccess.Read : 0;
            access |= Write ? FileAccess.Write : 0;

            Program.Variables[FileVar] = File.Open(Filename, FileMode.OpenOrCreate, access);
        }


    }
}
