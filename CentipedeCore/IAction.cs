using System;
using System.Collections.Generic;
using System.Xml;
using Centipede;
using PythonEngine;
using ResharperAnnotations;


namespace CentipedeInterfaces
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public interface IAction : IDisposable
    {
        /// <summary>
        /// Gets or sets the object that contains data associated with tje action.
        /// </summary>
        /// <example>
        /// In the GUI version of Centipede, <c>Tag</c> is used for a reference to the ActionDisplayControl displaying 
        /// the action
        /// </example>
        /// <returns>
        /// An Object that contains data about the control. The default is null.
        /// </returns>
        Object Tag { get; set; }

        /// <summary>
        /// An <see cref="Int32" /> representing the complexity of the action,
        /// used to gauge progress in the Job
        /// </summary>
        /// <value>
        /// Defaults to 1
        /// </value>
        Int32 Complexity { get; }

        /// <summary>
        /// Execute the action, performing init and cleanup as required
        /// </summary>
        /// <exception cref="ActionException">if the action fails</exception>
        void Run();

        /// <summary>
        /// Get the next action, can be overridden to allow custom flow control
        /// </summary>
        /// <returns>
        /// The next Action to be processed, or <see langword="null" /> if this
        /// is the last action in the job.
        /// </returns>
        IAction GetNext();

        IAction Next { get; set; }
        string Name { get; }
        string Comment { get; set; }

        string ToString();

        /// <summary>
        /// Append xml code for the action to the given parent element
        /// </summary>
        /// <param name="rootElement">
        /// The parent element to add the action to
        /// </param>
        void AddToXmlElement(XmlElement rootElement);

        /// <summary>
        /// 
        /// </summary>
        void Dispose();

        /// <summary>
        /// 
        /// </summary>
        event MessageEvent MessageHandler;

        /// <summary>
        /// 
        /// </summary>
        event AskEvent AskHandler;

        IEnumerable<Expression> FindPythonExpressions(string text);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void AskEvent(object sender, AskEventArgs e);


    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void MessageEvent(object sender, MessageEventArgs e);

    /// <summary>
    /// 
    /// </summary>
    public class MessageEventArgs : EventArgs
    {
        /// <summary>
        /// 
        /// </summary>
        public String Message
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public MessageLevel Level
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public MessageEventArgs()
        { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="level"></param>
        public MessageEventArgs(string message, MessageLevel level)
        {
            Message = message;
            Level = level;
        }
    }
    public class Expression
    {
        public int Start;
        public int End;
        public string Template;
        public string Code;
        public IPythonByteCode Compiled;

        public void CompileWith(IPythonEngine engine)
        {
            this.Compiled = engine.Compile(Code, SourceCodeType.Expression);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    [Flags]
    public enum MessageLevel : byte
    {
        /// <summary>
        /// Error
        /// </summary>
        Error = 0x1,

        /// <summary>
        /// Warning
        /// </summary>
        Warning = 0x2,

        /// <summary>
        /// Message originating from action
        /// </summary>
        Action = 0x4,

        /// <summary>
        /// Message from Core - action started, finished, string parsed
        /// </summary>
        Core = 0x8,

        /// <summary>
        /// Variable Changed, created, etc
        /// </summary>
        [DisplayText("Variable Changes")]
        VariableChange = 0x10,
        
        /// <summary>
        /// Debugging information - can be extemely verbose
        /// </summary>
        Debug = 0x20,

        /// <summary>
        /// All levels
        /// </summary>
        All = Error | Warning | Action | Core | VariableChange | Debug,

        /// <summary>
        /// 
        /// </summary>
        Default = Error | Warning | Action

    }
}