using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using CentipedeInterfaces;


namespace Centipede.Actions
{
    public abstract class BaseTextFileAction : Action
    {
        protected BaseTextFileAction(String name, IDictionary<string, object> v, ICentipedeCore c)
            : base(name, v, c)
        { }
        [ActionArgument(
            usage = "Variable to store the opened file in",
            displayName = "File Variable"
        )]
        public String FileVar = "TextFile";

        protected override void InitAction()
        {
            Object obj;
            Variables.TryGetValue(FileVar, out obj);
            FileStream = (FileStream)(obj is DBNull ? null : obj);
        }

        protected FileStream FileStream;

        protected override void CleanupAction()
        {
            Variables[FileVar] = FileStream;
        }

        public override void Dispose()
        {
            
        }

    }

    [ActionCategory("Text File Actions")]
    public class OpenTextFileAction : BaseTextFileAction
    {
        public OpenTextFileAction(IDictionary<string, object> v, ICentipedeCore c)
            : base("Open Text File", v, c)
        { }

        [ActionArgument(usage="Filename to open")]
        public String Filename = "";

        [ActionArgument(usage="Allow read access")]
        public Boolean Read = true;

        [ActionArgument(usage="Allow write access to file")]
        public Boolean Write = true;
        
        protected override void DoAction()
        {
            FileAccess access = Read ? FileAccess.Read : 0;
            access |= Write ? FileAccess.Write : 0;

            FileStream = File.Open(ParseStringForVariable(Filename), FileMode.OpenOrCreate, access);
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
    
    [ActionCategory("Text File Actions", displayName = "Find and Replace")]
    public class RegexAction : BaseTextFileAction
    {
        public RegexAction(IDictionary<string, object> v, ICentipedeCore c)
                : base("Find and Replace", v, c)
        { }


        [ActionArgument]
        public String Regex = "";

        [ActionArgument]
        public string Replacement;

        /// <summary>
        /// Perform the action
        /// </summary>
        /// <exception cref="ActionException">
        /// the action cannot be completed
        /// </exception>
        /// <exception cref="FatalActionException">The job needs to halt</exception>
        protected override void DoAction()
        {
            Regex re = new Regex(ParseStringForVariable(Regex));


            var temp = new MemoryStream();
            FileStream.CopyTo(temp);
            temp.Seek(0, SeekOrigin.Begin);
            var reader = new StreamReader(temp);
            FileStream.Seek(0, SeekOrigin.Begin);
            FileStream.SetLength(0);
            var writer = new StreamWriter(FileStream);

            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                //FileStream.Seek(-line.Length, SeekOrigin.Current);
                var pos1 = FileStream.Position;
                while (re.Match(line).Success)
                {
                    line = re.Replace(line, Replacement);   
                }
                writer.WriteLine(line);
                var pos2 = FileStream.Position;
            }
        }
    }

    
}
