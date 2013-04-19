using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using CentipedeInterfaces;
using Action = Centipede.Action;


namespace TextFile
{
    public abstract class BaseTextFileAction : Action
    {
        protected BaseTextFileAction(String name, IDictionary<string, object> variables, ICentipedeCore c)
            : base(name, variables, c)
        { }
        [ActionArgument(
            Usage = "Variable to store the opened file in",
            DisplayName = "File Variable"
        )]
        public String FileVar = "TextFile";

        protected override void InitAction()
        {
            Object obj;
            Variables.TryGetValue(this.FileVar, out obj);
            this.FileStream = (FileStream)(obj is DBNull ? null : obj);
        }

        protected FileStream FileStream;

        protected override void CleanupAction()
        {
            Variables[this.FileVar] = this.FileStream;
        }

        public override void Dispose()
        {
            
        }

    }

    [ActionCategory("Text File Actions")]
    public class OpenTextFileAction : BaseTextFileAction
    {
        public OpenTextFileAction(IDictionary<string, object> variables, ICentipedeCore c)
            : base("Open Text File", variables, c)
        { }

        [ActionArgument(Usage="Filename to open")]
        public String Filename = "{Filename}";

        [ActionArgument(Usage="Allow read access")]
        public Boolean Read = true;

        [ActionArgument(Usage="Allow write access to file")]
        public Boolean Write = true;
        
        protected override void DoAction()
        {
            FileAccess access = this.Read ? FileAccess.Read : 0;
            access |= this.Write ? FileAccess.Write : 0;

            FileStream = File.Open(ParseStringForVariable(this.Filename), FileMode.OpenOrCreate, access);
            GetCurrentCore().JobCompleted += delegate
            {
                if (Variables[FileVar] != null)
                    lock (Variables[FileVar])
                    {
                        if (Variables[FileVar] != null)
                        {
                            ((FileStream)Variables[FileVar]).Close();
                            Variables[FileVar] = null;


                        }
                    }
            };
        }


    }
    
    [ActionCategory("Text File Actions", DisplayName = "Find and Replace")]
    public class RegexAction : Action //BaseTextFileAction
    {
        public RegexAction(IDictionary<string, object> variables, ICentipedeCore c)
                : base("Find and Replace", variables, c)
        { }

        [ActionArgument(Literal=true)]
        public String FromFileVar = "Filename";

        [ActionArgument(Literal = true)]
        public String ToFileVar = "";
        
        [ActionArgument]
        public String Regex = "";

        [ActionArgument]
        public string Replacement = "";

        /// <summary>
        /// Perform the action
        /// </summary>
        /// <exception cref="ActionException">
        /// the action cannot be completed
        /// </exception>
        /// <exception cref="FatalActionException">The job needs to halt</exception>
        protected override void DoAction()
        {

            FileStream fromFile = (FileStream)Variables[this.FromFileVar];
            FileStream toFile = (FileStream)Variables[this.ToFileVar];

            Regex re = new Regex(ParseStringForVariable(this.Regex));

            string replace = ParseStringForVariable(this.Replacement);

            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (StreamWriter writer = new StreamWriter(memoryStream))
                using (StreamReader reader = new StreamReader(fromFile))
                {
                    while (!reader.EndOfStream)
                    {
                        writer.WriteLine(re.Replace(reader.ReadLine(), replace));
                    }
                }
                toFile.SetLength(0);
                memoryStream.CopyTo(toFile);
            }
        }
    }

    
}
