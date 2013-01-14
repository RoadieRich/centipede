using System;
using System.Collections.Generic;
using Centipede;
using System.IO;
using Action = Centipede.Action;


namespace TextFile
{
    [ActionCategory("Text")]
// ReSharper disable UnusedMember.Global
    public class OpenTextFile : Action
// ReSharper restore UnusedMember.Global
    {
        public OpenTextFile(Dictionary<String, Object> variables) 
            : base("Open Text File", variables)
        { }

        // ReSharper disable MemberCanBePrivate.Global
        // ReSharper disable ConvertToConstant.Global
        // ReSharper disable FieldCanBeMadeReadOnly.Global
        [ActionArgument(usage="File to open")]
        public String Filename = "";

        [ActionArgument(usage = "Allow writing to file")]
        public Boolean Write = true;

        [ActionArgument(usage = "Allow reading from file")]
        public Boolean Read = true;

        [ActionArgument(displayName = "File Variable", usage = "Variable to store the opened file, without % signs")]
        public String FileVarName = "TextFile";

        // ReSharper restore FieldCanBeMadeReadOnly.Global
        // ReSharper restore ConvertToConstant.Global
        // ReSharper restore MemberCanBePrivate.Global
        protected override void DoAction()
        {
            FileAccess access = Read ? FileAccess.Read : 0;
            access |= Write ? FileAccess.Write : 0;
            Variables[ParseStringForVariable(FileVarName)] = File.Open(ParseStringForVariable(Filename),FileMode.OpenOrCreate, access);
        }
    }

    [ActionCategory("Text")]
// ReSharper disable UnusedMember.Global
    public class SaveTextFile : Action
// ReSharper restore UnusedMember.Global
    {
        SaveTextFile(Dictionary<String, Object> variables)
            : base("Save Text File", variables)
        { }

        // ReSharper disable MemberCanBePrivate.Global
        // ReSharper disable ConvertToConstant.Global
        // ReSharper disable FieldCanBeMadeReadOnly.Global
        [ActionArgument(displayName = "File Variable", usage = "Variable the file is stored in")]
        public String FileVarName = "TextFile";

        [ActionArgument(displayName = "Close", usage = "Close file after saving")]
        public Boolean CloseAfter = false;

        // ReSharper restore FieldCanBeMadeReadOnly.Global
        // ReSharper restore ConvertToConstant.Global
        // ReSharper restore MemberCanBePrivate.Global
        protected override void DoAction()
        {
            var fs = Variables[ParseStringForVariable(FileVarName)] as FileStream;
            
            fs.Flush();

            if (CloseAfter)
            {
                fs.Close();
            }
        }
    }

    [ActionCategory("Text", helpText="Read an entire line from a file")]
// ReSharper disable UnusedMember.Global
    public class ReadLineFromTextFile : Action
// ReSharper restore UnusedMember.Global
    {
        public ReadLineFromTextFile(Dictionary<String, Object> variables)
            : base("Read Line", variables)
        { }

        // ReSharper disable MemberCanBePrivate.Global
        // ReSharper disable ConvertToConstant.Global
        // ReSharper disable FieldCanBeMadeReadOnly.Global
        [ActionArgument(displayName = "File Variable", usage = "Variable the file is stored in")]
        public String FileVarName = "TextFile";

        [ActionArgument(usage = "Read next line in order, or skip to the line below")]
        public Boolean Sequential = true;

        [ActionArgument(displayName = "Line number")]
        public Int32 LineNumber = 0;

        [ActionArgument(displayName="Destination Variable", usage="Variable to store the read text in")]
        public String DestinationVariable = "Line";

        protected override void DoAction()
        {
            var fs = Variables[ParseStringForVariable(FileVarName)] as FileStream;
            var sr = new EasyFileStream(fs);

            if (!Sequential)
            {
                fs.Seek(LineNumber, SeekOrigin.Begin);
            }



            Variables[ParseStringForVariable(DestinationVariable)] = sr.ReadLine();

        }
    } 

    class EasyFileStream
    {
        public EasyFileStream(FileStream fs)
        {
            _fs = fs;

            if (fs.CanRead)
            {
                _reader = new StreamReader(fs);
            }
            if (fs.CanWrite)
            {
                _writer = new StreamWriter(fs);
            }
        }

        private readonly StreamReader _reader;
        private readonly StreamWriter _writer;
        private readonly FileStream _fs;

// ReSharper disable UnusedMember.Global
        public void Write<T>(T data)
// ReSharper restore UnusedMember.Global
        {
            _writer.Write(data);
        }

        public String ReadLine()
        {
            return _reader.ReadLine();
        }

// ReSharper disable UnusedMember.Global
        public void Seek(long offset, SeekOrigin origin)
// ReSharper restore UnusedMember.Global
        {
            _fs.Seek(offset, origin);

            
        }
    }
}
