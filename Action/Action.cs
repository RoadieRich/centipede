using System;
using System.Collections.Generic;
using System.Xml;
using System.Reflection;
using System.Linq;
using System.IO;
using Centipede.Actions;
using Centipede.StringInject;

// ReSharper disable UnusedMember.Global

[assembly: CLSCompliant(true)]

namespace Centipede
{
    /// <summary>
    /// Base Action class: all actions will subclass this
    /// </summary>
    [Serializable]
    public abstract class Action : IDisposable
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="v"></param>
        protected Action(String name, Dictionary<String, Object> v)
        {
            Name = name;
            Variables = v;
        }

        /// <summary>
        /// 
        /// </summary>
        protected readonly Dictionary<String, Object> Variables;

        /// <summary>
        /// 
        /// </summary>
        public readonly String Name;

        /// <summary>
        /// 
        /// </summary>
        public String Comment = "";

        /// <summary>
        /// 
        /// </summary>
        public Object Tag;

        /// <summary>
        /// 
        /// </summary>
        public Action Next;

        /// <summary>
        /// Override in abstract subclasses to allow code to be executed to e.g. set up variables, 
        /// converting an object from Variables to a specific type.
        /// </summary>
        /// <example>
        /// <code>
        /// protected override void InitAction()
        /// {
        ///     this.TextFile = Variables["TextFile"] as FileStream;
        /// }
        /// </code>
        /// </example>
        protected virtual void InitAction()
        { }

        /// <summary>
        /// Perform the action
        /// </summary>
        protected abstract void DoAction();

        /// <summary>
        /// Cleanup, e.g. saving local variables back into Variables
        /// </summary>
        protected virtual void CleanupAction()
        { }

        /// <summary>
        /// Execute the action, performing init and cleanup as required
        /// </summary>
        public void Run()
        {
            InitAction();
            DoAction();
            CleanupAction();
        }

        /// <summary>
        /// In integer representing the complexity of the action, used to indicate progress in the job"
        /// </summary>
        public virtual Int32 Complexity
        {
            get
            {
                return 1;
            }
        }

        /// <summary>
        /// Get the next action, can be overridden to allow custom flow control
        /// </summary>
        /// <returns>Action</returns>
        public virtual Action GetNext()
        {
            return Next;
        }

        /// <summary>
        /// Parse a string, injecting values from Variables as required
        /// </summary>
        /// <example>
        /// <code>
        /// Variables["variable_a"] = "foo";
        /// Variables["variable_b"] = "bar"
        /// ParseStringForVariable("{variable_a}{variable_b}") //returns "foobar"
        /// </code>
        /// </example>
        /// <param name="str">The string to parse</param>
        /// <returns>String</returns>
        protected String ParseStringForVariable(String str)
        {
            return str.Inject(Variables);
        }


        /// <summary>
        /// Ask a question
        /// </summary>
        /// <param name="message"></param>
        /// <param name="title"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        protected AskEventEnums.DialogResult Ask(String message, String title = "Question", AskEventEnums.AskType options = AskEventEnums.AskType.YesNoCancel)
        {
            var handler = OnAsk;
            if (handler != null)
            {
                var eventArgs = new AskActionEventArgs
                                {
                                        Icon = AskEventEnums.MessageIcon.Question,
                                        Message = message,
                                        Title = title,
                                        Type = options
                                };
                OnAsk(this, eventArgs);
                return eventArgs.Result;

            }
            return AskEventEnums.DialogResult.None;
        }

        /// <summary>
        /// 
        /// </summary>
// ReSharper disable EventNeverSubscribedTo.Global
        public event AskEvent OnAsk = delegate { };
// ReSharper restore EventNeverSubscribedTo.Global

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void AskEvent(object sender, AskActionEventArgs e);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="title"></param>
        /// <param name="messageIcon"></param>
        protected void Message(String message, String title = "Message", AskEventEnums.MessageIcon messageIcon = AskEventEnums.MessageIcon.Information)
        {
            var handler = OnAsk;
            if (handler != null)
            {
                var eventArgs = new AskActionEventArgs
                                               {
                                                       Icon = messageIcon,
                                                       Message = message,
                                                       Title = title,
                                                       Type = AskEventEnums.AskType.OK
                                               };
                OnAsk(this, eventArgs);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [Obsolete]
        public static HashSet<String> TrueValues = new HashSet<string>(new[]{
                                         "yes",
                                         "true",
                                         "1"
                                     });

        /// <summary>
        /// Append xml code for the action to the given parent element
        /// </summary>
        /// <param name="rootElement">The parent element to add the action to</param>
        public void AddToXmlElement(XmlElement rootElement)
        {

            Type thisType = GetType();
            if (rootElement.OwnerDocument != null)
            {
                XmlElement element = rootElement.OwnerDocument.CreateElement(thisType.FullName);

                //foreach (
                //        FieldInfo field in from f in thisType.GetFields()
                //                           where f.GetCustomAttributes(typeof(ActionArgumentAttribute), true).Any()
                //                           select f
                //        )
                //{
                //    element.SetAttribute(field.Name, field.GetValue(this).ToString());
                //}

                //foreach (PropertyInfo prop in from p in thisType.GetProperties()
                //                              where p.GetCustomAttributes(typeof (ActionArgumentAttribute), true).Any()
                //                              select p)
                //{
                //    element.SetAttribute(prop.Name, prop.GetValue(this, null).ToString());
                //}

                foreach (FieldAndPropertyWrapper wrappedMember in from member in thisType.GetMembers()
                                                      where member is FieldInfo || member is PropertyInfo
                                                      select (FieldAndPropertyWrapper)member
                                                          into wrapped
                                                          where wrapped.GetArguementAttribute() != null
                                                          select wrapped
                        )
                {
                    element.SetAttribute(wrappedMember.Name, wrappedMember.Get<dynamic>(this).ToString());
                }

                String pluginFilePath = Path.GetFileName(thisType.Assembly.CodeBase);

                element.SetAttribute("Comment", Comment);
                element.SetAttribute("Assembly", pluginFilePath);

                rootElement.AppendChild(element);
            }
        }

        /// <summary>
        /// Loads an action from an XmlElement
        /// </summary>
        /// <param name="element">the xml to convert</param>
        /// <param name="variables">Program.Variables, passed to the Action.ctor</param>
        /// <returns></returns>
        public static Action FromXml(XmlElement element, Dictionary<String, Object> variables)
        {
            Type[] constructorArgumentTypes = new[] { typeof (Dictionary<String, Object>) };

            Assembly asm;

            //if action is not a plugin:
            if (element.LocalName.Count(c => c == '.') <= 1)
            {
                asm = Assembly.GetEntryAssembly();
            }
            else
            {
                string location = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
                if (location != null)
                {
                    String asmPath = Path.Combine(location,
                                                  Path.Combine(Properties.Settings.Default.PluginFolder,
                                                               element.GetAttribute("Assembly")));
                    asm = Assembly.LoadFile(asmPath);
                }
                else
                {
                    throw new FileNotFoundException();
                }
            }

            Type t = asm.GetType(element.LocalName);

            Object instance = null;

            MethodInfo customFromXmlMethod = t.GetMethod("CustomFromXml");
            if (customFromXmlMethod != null)
            {
                instance = customFromXmlMethod.Invoke(t, new object[] { element, variables });
            }
            else
            {
                ConstructorInfo constructorInfo = t.GetConstructor(constructorArgumentTypes);
                if (constructorInfo != null)
                {
                    instance = constructorInfo.Invoke(new object[] { variables });
                }

                element.Attributes.RemoveNamedItem("Assembly");

                foreach (XmlAttribute attribute in element.Attributes)
                {
                    FieldAndPropertyWrapper field = t.GetField(attribute.Name);
                    MethodInfo parseMethod = field.GetType().GetMethod("Parse", new[] { typeof(String) });
                    field.Set(instance,
                              parseMethod != null
                                      ? parseMethod.Invoke(field, new object[] { attribute.Value })
                                      : attribute.Value);
                }
            }
            return instance as Action;
        }

        
        /// <summary>
        /// 
        /// </summary>
        public virtual void Dispose()
        { }
    }

    /// <summary>
    /// Mark a field of a class as an argument for the function, used to format the ActionDisplayControl
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public sealed class ActionArgumentAttribute : Attribute
    {
    // ReSharper disable InconsistentNaming
    // ReSharper disable UnassignedField.Global
     
        /// <summary>
        /// 
        /// </summary>
        public String displayName;

        /// <summary>
        /// 
        /// </summary>
        public String usage;

        /// <summary>
        /// 
        /// </summary>
        public String onLeaveHandlerName;

        /// <summary>
        /// 
        /// </summary>
        public string onChangedHandlerName;

        /// <summary>
        /// 
        /// </summary>
        public string setterMethodName;

        /// <summary>
        /// 
        /// </summary>
        public string displayControl;

        // ReSharper restore InconsistentNaming
        // ReSharper restore UnassignedField.Global
    }

    /// <summary>
    /// Marks a class as an Action, to be displayed in the GUI listbox
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class ActionCategoryAttribute : Attribute
    {

        // ReSharper disable InconsistentNaming
        // ReSharper disable UnassignedField.Global
        /// <summary>
        /// Marks a class as an Action, to be displayed in the GUI listbox
        /// </summary>
        /// <param name="category"><see cref="ActionCategoryAttribute.category"/></param>
        public ActionCategoryAttribute(String category)
        {
            this.category = category;
        }

        /// <summary>
        /// the category tab to add the action to
        /// </summary>
        public readonly String category;

        /// <summary>
        /// helptext for the action, displayed as a tooltip
        /// </summary>
        public String helpText;

        /// <summary>
        /// The display name for the action, defaults to the classname
        /// </summary>
        public String displayName;

        /// <summary>
        /// name of a custom display control used to display the action on thr Actions listview
        /// </summary>
        public String displayControl;

        /// <summary>
        /// name of the icon in the resource file
        /// </summary>
        public String iconName;

        // ReSharper restore InconsistentNaming
        // ReSharper restore UnassignedField.Global
    }

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
        public ActionException(String message, Action action)
            : base(message)
        {
            ErrorAction = action;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="action"></param>
        public ActionException(Action action)
        {
            ErrorAction = action;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        /// <param name="action"></param>
        public ActionException(string message, Exception exception, Action action)
            : base(message, exception)
        {
            ErrorAction = action;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public ActionException(string message)
            : base(message) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        /// <param name="action"></param>
        public ActionException(Exception e, Action action)
            : base(e.Message, e)
        {
            ErrorAction = action;
        }

        /// <summary>
        /// 
        /// </summary>
        public readonly Action ErrorAction;
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

    /// <summary>
    /// 
    /// </summary>
    public class AskActionEventArgs : EventArgs
    {
        /// <summary>
        /// 
        /// </summary>
// ReSharper disable NotAccessedField.Global
        public String Message;
        /// <summary>
        /// 
        /// </summary>
        public String Title;
        /// <summary>
        /// 
        /// </summary>
        public AskEventEnums.AskType Type;
// ReSharper disable ConvertToConstant.Global
        /// <summary>
        /// 
        /// </summary>
// ReSharper disable FieldCanBeMadeReadOnly.Global
        public AskEventEnums.DialogResult Result = AskEventEnums.DialogResult.None;
// ReSharper restore FieldCanBeMadeReadOnly.Global
// ReSharper restore ConvertToConstant.Global
        /// <summary>
        /// 
        /// </summary>
        public AskEventEnums.MessageIcon Icon = AskEventEnums.MessageIcon.Information;
// ReSharper restore NotAccessedField.Global
    }

    /// <summary>
    /// 
    /// </summary>
    public static class AskEventEnums
    {
        /// <summary>
        /// 
        /// </summary>
        public enum AskType
        {
            /// <summary>
            /// 
            /// </summary>
            AbortRetryIgnore = System.Windows.Forms.MessageBoxButtons.AbortRetryIgnore,
            /// <summary>
            /// 
            /// </summary>
            OK = System.Windows.Forms.MessageBoxButtons.OK,
            /// <summary>
            /// 
            /// </summary>
            OKCancel = System.Windows.Forms.MessageBoxButtons.OKCancel,
            /// <summary>
            /// 
            /// </summary>
            RetryCancel = System.Windows.Forms.MessageBoxButtons.RetryCancel,
            /// <summary>
            /// 
            /// </summary>
            YesNo = System.Windows.Forms.MessageBoxButtons.YesNo,
            /// <summary>
            /// 
            /// </summary>
            YesNoCancel = System.Windows.Forms.MessageBoxButtons.YesNoCancel
        }
        /// <summary>
        /// 
        /// </summary>
        public enum DialogResult
        { 
            /// <summary>
            /// 
            /// </summary>
            Abort = System.Windows.Forms.DialogResult.Abort,
            /// <summary>
            /// 
            /// </summary>
            Cancel = System.Windows.Forms.DialogResult.Cancel,
            /// <summary>
            /// 
            /// </summary>
            Ignore = System.Windows.Forms.DialogResult.Ignore,
            /// <summary>
            /// 
            /// </summary>
            No = System.Windows.Forms.DialogResult.No,
            /// <summary>
            /// 
            /// </summary>
            None = System.Windows.Forms.DialogResult.None,
            /// <summary>
            /// 
            /// </summary>
            OK = System.Windows.Forms.DialogResult.OK,
            /// <summary>
            /// 
            /// </summary>
            Retry = System.Windows.Forms.DialogResult.Retry,
            /// <summary>
            /// 
            /// </summary>
            Yes = System.Windows.Forms.DialogResult.Yes
        }

        /// <summary>
        /// 
        /// </summary>
        public enum MessageIcon
        {
            /// <summary>
            /// 
            /// </summary>
            Asterisk = System.Windows.Forms.MessageBoxIcon.Asterisk,
            /// <summary>
            /// 
            /// </summary>
            Error = System.Windows.Forms.MessageBoxIcon.Error,
            /// <summary>
            /// 
            /// </summary>
            Exclamation = System.Windows.Forms.MessageBoxIcon.Exclamation,
            /// <summary>
            /// 
            /// </summary>
            Hand = System.Windows.Forms.MessageBoxIcon.Hand,
            /// <summary>
            /// 
            /// </summary>
            Information = System.Windows.Forms.MessageBoxIcon.Information,
            /// <summary>
            /// 
            /// </summary>
            None = System.Windows.Forms.MessageBoxIcon.None,
            /// <summary>
            /// 
            /// </summary>
            Question = System.Windows.Forms.MessageBoxIcon.Question,
            /// <summary>
            /// 
            /// </summary>
            Stop = System.Windows.Forms.MessageBoxIcon.Stop,
            /// <summary>
            /// 
            /// </summary>
            Warning = System.Windows.Forms.MessageBoxIcon.Warning
        }
    }
}
// ReSharper restore UnusedMember.Global