using System;
using System.IO;
using System.Runtime.Serialization;
using ResharperAnnotations;


namespace CentipedeInterfaces
{
    /// <summary>
    /// Thrown when an action raises an exception
    /// </summary>
    public class ActionException : Exception
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="action"></param>
        [PublicAPI]
        public ActionException(String message, IAction action)
                : base(message)
        {
            this.ErrorAction = action;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="action"></param>
        [PublicAPI]
        public ActionException(IAction action)
        {
            this.ErrorAction = action;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        /// <param name="action"></param>
        [PublicAPI]
        public ActionException(string message, Exception exception, IAction action)
                : base(message, exception)
        {
            this.ErrorAction = action;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        [PublicAPI]
        public ActionException(string message)
                : base(message) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        /// <param name="action"></param>
        public ActionException(Exception e, IAction action)
                : base(e.Message, e)
        {
            this.ErrorAction = action;
        }

        /// <summary>
        /// 
        /// </summary>
        public readonly IAction ErrorAction;
    }

    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class FatalActionException : ActionException
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="action"></param>
        public FatalActionException(string message, IAction action)
                : base(message, action)
        { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="action"></param>
        public FatalActionException(IAction action)
                : base(action)
        { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        /// <param name="action"></param>
        public FatalActionException(string message, Exception exception, IAction action)
                : base(message, exception, action)
        { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public FatalActionException(string message)
                : base(message)
        { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        /// <param name="action"></param>
        public FatalActionException(Exception e, IAction action)
                : base(e, action)
        { }
    }

    [Serializable]
    public class PluginNotFoundException : FileNotFoundException
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        [UsedImplicitly]
        public PluginNotFoundException(string message, string actionName, string filename)
            : base(message,filename)
        {
            ActionName = actionName;
        }

        public PluginNotFoundException(string message, string actionName)
            : base(message)
        {
            ActionName = actionName;
        }

        public PluginNotFoundException(string message, Exception inner, string actionName)
            : base(message, inner)
        {
            ActionName = actionName;
        }

        protected PluginNotFoundException(
            SerializationInfo info,
            StreamingContext context, string actionName)
            : base(info, context)
        {
            ActionName = actionName;
        }

        public String ActionName { get; private set; }
        

    }
}