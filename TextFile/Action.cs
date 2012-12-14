using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Centipede;
using System.IO;

namespace Centipede.TextFile
{
    [ActionCategory("Text")]
    public class OpenTextFile : Action
    {
        public OpenTextFile(Dictionary<String, Object> variables) 
            : base("Open Text File", variables)
        { }

        [ActionArgument(usage="File to open")]
        public String Filename = "";

        [ActionArgument(usage = "Allow writing to file")]
        public Boolean Write = true;

        [ActionArgument(usage = "Allow reading from file")]
        public Boolean Read = true;

        [ActionArgument(displayName = "File Variable", usage = "Variable to store the opened file, without % signs")]
        public String FileVarName = "TextFile";

        protected override void DoAction()
        {
            FileAccess access = Read ? FileAccess.Read : 0;
            access |= Write ? FileAccess.Write : 0;
            Variables[ParseStringForVariable(FileVarName)] = File.Open(ParseStringForVariable(Filename),FileMode.OpenOrCreate, access);
        }
    }

    [ActionCategory("Text")]
    public class SaveTextFile : Action
    {
        SaveTextFile(Dictionary<String, Object> variables)
            : base("Save Text File", variables)
        { }

        [ActionArgument(displayName = "File Variable", usage = "Variable the file is stored in")]
        public String FileVarName = "TextFile";

        [ActionArgument(displayName = "Close", usage = "Close file after saving")]
        Boolean CloseAfter = false;

        protected override void DoAction()
        {
            FileStream fs = Variables[ParseStringForVariable(FileVarName)] as FileStream;
            
            fs.Flush();

            if (CloseAfter)
            {
                fs.Close();
            }
        }
    }

    [ActionCategory("Text", helpText="Read an entire line from a file")]
    public class ReadLineFromTextFile : Action
    {
        public ReadLineFromTextFile(Dictionary<String, Object> variables)
            : base("Read Line", variables)
        { }

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
            FileStream fs = Variables[ParseStringForVariable(FileVarName)] as FileStream;
            EasyFileStream sr = new EasyFileStream(fs);

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

        private StreamReader _reader;
        private StreamWriter _writer;
        private FileStream _fs;

        public void Write<T>(T data)
        {
            _writer.Write(data);
        }

        public String ReadLine()
        {
            return _reader.ReadLine();
        }

        public void Seek(long offset, SeekOrigin origin)
        {
            _fs.Seek(offset, origin);

            
        }
    }
}
