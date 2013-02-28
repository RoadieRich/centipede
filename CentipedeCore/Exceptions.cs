using System;
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

    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class ValidationException : Exception
    {
        /// <summary>
        /// 
        /// </summary>
        public ValidationException()
        { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public ValidationException(string message)
            : base(message)
        { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="inner"></param>
        public ValidationException(string message, Exception inner)
            : base(message, inner)
        { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected ValidationException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context)
        { }
    }
}