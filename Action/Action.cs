﻿using System;
using System.Collections.Generic;
using System.Xml;
using System.Reflection;
using System.Linq;
using System.IO;
using System.Xml.XPath;
using Centipede.Actions;
using Centipede.StringInject;
using CentipedeInterfaces;
using ResharperAnnotations;

[assembly: CLSCompliant(true)]

namespace Centipede
{
    /// <summary>
    /// Base Action class: all actions will subclass this
    /// </summary>
    [Serializable]
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public abstract class Action : IDisposable, IAction
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="v"></param>
        /// <param name="core"></param>
        protected Action(String name, IDictionary<String, Object> v, ICentipedeCore core)
        {
            Name = name;
            Variables = v;
            _core = core;
        }

        /// <summary>
        /// 
        /// </summary>
        protected readonly IDictionary<string, object> Variables;

        private ICentipedeCore _core;

        /// <summary>
        /// 
        /// </summary>
        public String Comment { get; set; }

        /// <summary>
        /// Gets or sets the object that contains data associated with tje action.
        /// </summary>
        /// <example>
        /// In the GUI version of Centipede, <c>Tag</c> is used for a reference
        /// to the <see cref="ActionDisplayControl" /> displaying the action
        /// </example>
        /// <returns>
        /// An Object that contains data about the control. The default is null.
        /// </returns>
        public Object Tag
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public IAction Next { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Override in <see langword="abstract" /> subclasses to allow code to
        /// be executed to e.g. set up variables, converting an object from
        /// <see cref="Centipede.Action.Variables" /> to a specific type.
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
        /// <exception cref="ActionException">
        /// the action cannot be completed
        /// </exception>
        /// <exception cref="FatalActionException">The job needs to halt</exception>
        protected abstract void DoAction();

        /// <summary>
        /// Cleanup, e.g. saving local variables back into
        /// <see cref="Centipede.Action.Variables" />
        /// </summary>
        protected virtual void CleanupAction()
        { }

        /// <summary>
        /// Execute the action, performing init and cleanup as required
        /// </summary>
        /// <exception cref="ActionException">if the action fails</exception>
        public void Run()
        {
            OnMessage(new MessageEventArgs{
                Message = string.Format("Processing action {0}", Name), 
                Level = MessageLevel.Notice});
            InitAction();
            DoAction();
            CleanupAction();
            OnMessage(new MessageEventArgs(string.Format("Finished action {0}", Name), MessageLevel.Notice));
        }

        /// <summary>
        /// An <see cref="Int32" /> representing the complexity of the action,
        /// used to gauge progress in the Job
        /// </summary>
        /// <value>
        /// Defaults to 1
        /// </value>
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
        /// <returns>
        /// The next Action to be processed, or <see langword="null" /> if this
        /// is the last action in the job.
        /// </returns>
        public virtual IAction GetNext()
        {
            return Next;
        }

        /// <summary>
        /// Parse a string, injecting values from
        /// <see cref="Centipede.Action.Variables" /> as required
        /// </summary>
        /// <example>
        /// <code>
        /// Variables["variable_a"] = "foo";
        /// Variables["variable_b"] = "bar"
        /// ParseStringForVariable("{variable_a}{variable_b}") //returns "foobar"
        /// </code>
        /// </example>
        /// <param name="str">The string to parse</param>
        /// <returns>
        /// String
        /// </returns>
        protected String ParseStringForVariable(String str)
        {
            return str.Inject(Variables);
        }


        /// <summary>
        /// <see cref="Action.Ask" /> a question
        /// </summary>
        /// <param name="message"></param>
        /// <param name="title"></param>
        /// <param name="options"></param>
        /// <returns>
        /// 
        /// </returns>
        protected AskEventEnums.DialogResult Ask(String message, String title = "Question",
                                                 AskEventEnums.AskType options = AskEventEnums.AskType.YesNoCancel)
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
        public event AskEvent OnAsk;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="level"></param>
        protected void Message(String message,
                               MessageLevel level)
        { }


        /// <summary>
        /// Append xml code for the action to the given parent element
        /// </summary>
        /// <param name="rootElement">
        /// The parent element to add the action to
        /// </param>
        public virtual void AddToXmlElement(XmlElement rootElement)
        {

            Type thisType = GetType();
            if (rootElement.OwnerDocument == null)
            {
                return;
            }
            XmlElement element = rootElement.OwnerDocument.CreateElement(thisType.FullName);

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

            element.SetAttribute(@"Comment", Comment);
            element.SetAttribute(@"Assembly", pluginFilePath);

            rootElement.AppendChild(element);
        }

        /// <summary>
        /// Loads an action from an XmlElement
        /// </summary>
        /// <param name="element">the xml to convert</param>
        /// <param name="variables">
        /// Program.Variables, passed to the Action.ctor
        /// </param>
        /// <param name="core"></param>
        /// <returns>
        /// 
        /// </returns>
        public static Action FromXml(XmlElement element, IDictionary<String, Object> variables, ICentipedeCore core)
        {

            return FromXml(element.CreateNavigator(), variables, core);
        }

        public static Action FromXml(XPathNavigator element, IDictionary<String,Object> variables, ICentipedeCore core)
        {

            //This is probably broken somewhere.

            Type[] constructorArgumentTypes = new[] { typeof(IDictionary<String, Object>), typeof(ICentipedeCore) };

            Assembly asm;

            //if action is not a plugin:
            if (element.LocalName.StartsWith(@"Centipede"))
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
                                                               element.SelectSingleNode("@Assembly").Value));
                    asm = Assembly.LoadFile(asmPath);
                }
                else
                {
                    throw new FileNotFoundException();
                }
            }

            Type t = asm.GetType(element.LocalName);

            Action instance = null;
            ConstructorInfo constructorInfo = t.GetConstructor(constructorArgumentTypes);
            if (constructorInfo != null)
            {
                instance = (Action)constructorInfo.Invoke(new object[] { variables, core });
            }

            if (instance == null)
            {
                throw new ApplicationException();
            }

            instance.PopulateMembersFromXml(element);

            return instance;
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void Dispose()
        { }

        /// <summary>
        /// Populate members from the given Xml Element.
        /// </summary>
        /// <remarks>
        /// Should be implemented if <see cref="Action.AddToXmlElement" /> is
        /// non-trivially overridden.
        /// </remarks>
        /// <param name="element">The XmlElement describing the action</param>
        protected virtual void PopulateMembersFromXml(XPathNavigator element)
        {

            Type actionType = GetType();

            //element.Attributes.RemoveNamedItem(@"Assembly");

            foreach (XPathNavigator attribute in element.Select(@"./@*"))
            {
                if (attribute.Name == "Assembly")
                {
                    continue;
                }
                FieldAndPropertyWrapper field = (FieldAndPropertyWrapper)actionType.GetMember(attribute.Name).First();
                MethodInfo parseMethod = field.MemberType.GetMethod(@"Parse", new[] { typeof (String) });
                field.Set(this,
                          parseMethod != null
                                  ? parseMethod.Invoke(field, new object[] { attribute.Value })
                                  : attribute.Value);
            }
        }

        /// <summary>
        /// Allows programmers to specify jobs using arrow notaton
        /// </summary>
        /// <example>
        /// <code> action1 &gt; action2 &gt; action2 </code>
        /// </example>
        /// <param name="prev"></param>
        /// <param name="next"></param>
        public static Action operator >(Action prev, Action next)
        {
            prev.Next = next;
            return next;
        }

        /// <summary>
        /// Allows programmers to specify jobs using arrow notaton
        /// </summary>
        /// <example>
        /// <code>
        /// action3 &lt; action2 &lt; action1
        /// </code>
        /// </example>
        /// <param name="prev"></param>
        /// <param name="next"></param>
        public static Action operator <(Action next, Action prev)
        {
            prev.Next = next;
            return prev;
        }

        /// <summary>
        /// Get the next <paramref name="action" />
        /// </summary>
        /// <param name="action"></param>
        /// <returns>
        /// 
        /// </returns>
        public static Action operator ++(Action action)
        {
            return action.GetNext() as Action;
        }

        /// <summary>
        /// 
        /// </summary>
        public event MessageHandlerDelegate MessageHandler;

        [ResharperAnnotations.PublicAPI]
        protected void OnMessage(MessageEventArgs e)
        {
            MessageHandlerDelegate handlerDelegate = MessageHandler;
            if (handlerDelegate != null)
            {
                handlerDelegate(this, e);
            }
        }

        /// <summary>
        /// Returns the core that contains the <see cref="Action"/>.
        /// </summary>
        /// <returns></returns>
        public ICentipedeCore GetCurrentCore()
        {
            return _core;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override String ToString()
        {
            return String.IsNullOrWhiteSpace(Comment) ? Name : String.Format("{0} ({1})", Name, Comment);
        }
    }


    
    /// <summary>
    /// <see cref="I18N" /> resources for a class
    /// </summary>
    public abstract class I18N
    {

    }

}