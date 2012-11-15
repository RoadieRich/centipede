using System;
using System.Collections.Generic;

namespace Centipede
{
    /// <summary>
    /// Base Action class: all actions will subclass this
    /// </summary>
    public abstract class Action
    {
        public Action(String name, Dictionary<String,Object> variables, Object tag = null)
        {
            Name = name;
            Variables = variables;
            Tag = tag;
        }

        public delegate void Setter(String s);

        protected Dictionary<String, Object> Variables;

        public Dictionary<String, Object> Attributes = new Dictionary<string, object>();
        //public Dictionary<String, String> AttributeInfo = new Dictionary<string, string>();
        public readonly String Name;
        public String Comment = "";
        public Object Tag;

        public Action Next = null;

        public abstract void DoAction();


        public virtual Action GetNext()
        {
            return Next;
        }

        public T ParseAttribute<T>(String attributeName)
        {
            String attrText = Attributes[attributeName] as String;
            if (attrText.StartsWith("%"))
            {
                return (T)Variables[attrText.Substring(1)];
            }
            else
            {
                return (T)Attributes[attributeName];
            }
        }

        protected String ParseStringForVariable(String str)
        {
            if (str.StartsWith("%"))
            {
                return Variables[str.Substring(1)] as String;
            }
            else
            {
                return str;
            }
        }

        public static HashSet<String> TrueValues = new HashSet<string>(new String[]{
                                         "yes",
                                         "true",
                                         "1"
                                     });

        public Boolean ParseBoolAttribute(String attributeName)
        {
            return TrueValues.Contains(ParseAttribute<String>(attributeName));
        }

    }

    [System.AttributeUsage(AttributeTargets.Field)]
    public sealed class ActionArgumentAttribute : System.Attribute
    {
        public String displayName;
        public String usage;
        public String setterMethodName;
        public Object setter;
    }

    [System.AttributeUsage(AttributeTargets.Class)]
    public sealed class ActionCategoryAttribute : System.Attribute
    {
        public ActionCategoryAttribute(String category)
        {
            this.category = category;
        }

        public readonly String category;
        public String helpText;
    }

    public class ActionException : Exception
    {
        public ActionException(String message, Action action)
            : base(message)
        {
            ErrorAction = action;
        }

        public ActionException(Action action)
        {
            ErrorAction = action;
        }

        public ActionException(string message, Exception exception, Action action)
            : base(message, exception)
        {
            ErrorAction = action;
        }

        public ActionException(string message)
            : base(message) { }

        public ActionException(Exception e, Action action)
            : base(e.Message, e)
        {
            ErrorAction = action;
        }

        public readonly Action ErrorAction = null;
    }

    [Serializable]
    public class ValidationException : Exception
    {
        public ValidationException()
        { }
        public ValidationException(string message)
            : base(message)
        { }
        public ValidationException(string message, Exception inner)
            : base(message, inner)
        { }
        protected ValidationException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context)
        { }
    }
}
