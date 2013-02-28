using System;
using System.Xml;
using ResharperAnnotations;


namespace CentipedeInterfaces
{
    public interface IAction
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
        /// 
        /// </summary>
        [UsedImplicitly]
        event AskEvent OnAsk;

        /// <summary>
        /// Append xml code for the action to the given parent element
        /// </summary>
        /// <param name="rootElement">
        /// The parent element to add the action to
        /// </param>
        [UsedImplicitly]
        void AddToXmlElement(XmlElement rootElement);

        /// <summary>
        /// 
        /// </summary>
        void Dispose();

        /// <summary>
        /// 
        /// </summary>
        [UsedImplicitly]
        event MessageHandlerDelegate MessageHandler;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void AskEvent(object sender, AskActionEventArgs e);


    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void MessageHandlerDelegate(object sender, MessageEventArgs e);

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

    /// <summary>
    /// 
    /// </summary>
    [Flags]
    public enum MessageLevel
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
        /// Message
        /// </summary>
        Message = 0x4,

        /// <summary>
        /// Notice
        /// </summary>
        Notice = 0x8,

        /// <summary>
        /// Variable Changed
        /// </summary>
        VariableChange = 0x10,

        /// <summary>
        /// All levels
        /// </summary>
        All = Error | Warning | Message | Notice | VariableChange
    }
}