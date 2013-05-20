using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Xml;
using System.Reflection;
using System.Linq;
using System.IO;
using System.Xml.XPath;
using Centipede.Actions;
using CentipedeInterfaces;
using PythonEngine;
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
        /// Construct a new action
        /// </summary>
        /// <param name="name">name of the action</param>
        /// <param name="variables">Variables</param>
        /// <param name="core">The <see cref="ICentipedeCore">core</see> that will execute the <see cref="CentipedeJob">job</see> this action is a part of.</param>
        protected Action(String name, IDictionary<String, Object> variables, ICentipedeCore core)
        {
            Name = name;
            Variables = variables;
            _core = core;
        }

        /// <summary>
        /// A reference to the job's variable store
        /// </summary>
        protected readonly IDictionary<string, object> Variables;

        private ICentipedeCore _core;
        public static string PluginFolder;

        /// <summary>
        /// A (user specified) comment on the purpose of the action.
        /// </summary>
        public String Comment { get; set; }

        /// <summary>
        /// Gets or sets the object that contains data associated with the action.
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
        /// The action to process after this is finished.  Set by the <see cref="ICentipedeCore">Core</see>.
        /// </summary>
        public IAction Next { get; set; }

        /// <summary>
        /// The name of the action, as displayed in the GUI.
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
                Level = MessageLevel.Core});
            InitAction();
            DoAction();
            CleanupAction();
            OnMessage(new MessageEventArgs(string.Format("Finished action {0}", Name), MessageLevel.Core));
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

        
        protected String OldParseStringForVariable([NotNull] String str)
        {

            IPythonEngine pythonEngine = GetCurrentCore().PythonEngine;
            
            Regex expressionRegex = new Regex(@"(?<template>{(?<expression>.*?)})", RegexOptions.ExplicitCapture);
            MatchCollection matches = expressionRegex.Matches(str);

            var enumerable = from Match match in matches
                             select (string)pythonEngine.Evaluate(match.Groups[@"expression"].Value).ToString();
            string injected = enumerable.Aggregate(str, expressionRegex.Replace);


            OnMessage(MessageLevel.Core, "String {0} parsed to {1}", str, injected);
            return injected;
        }

        /// <summary>
        /// Parse a string, executing python code as required.  Code snippets should be surrounded by braces, and can
        /// return any type of value - although if the python class does not implement a sensible <c>__str__</c>, it 
        /// will not make much sense.
        /// </summary>
        /// <example>
        /// (in python)
        /// <code para="str">
        /// variable_a = "foo";
        /// variable_b = "bar"
        /// </code>
        /// (in c#)
        /// <code>
        /// ParseStringForVariable("{variable_a}{variable_b}") //returns "foobar"
        /// </code>
        /// </example>
        /// <param name="str">The string to parse</param>
        /// <returns>
        /// String
        /// </returns>
        /// <remarks>This has been constructed to be as robust as possible, but as always, doing silly things will
        /// result in silly things happening.  For instance <c>__import__("sys").execute("yes | rm -rf /")</c> will 
        /// do exactly what you expect on a linux machine. However, it has yet to be seen if it is possible to
        /// break the parser to execute such code without it being clearly visible. 
        /// <para/>
        /// The end user should always remember to treat jobs as executables, and not to run anything received from
        /// an untrusted source without carefully checking it over first.</remarks>
        protected String ParseStringForVariable([NotNull] String str)
        {
            OnMessage(MessageLevel.Debug, "Attempting to parse string {0}", str);
            
            IPythonEngine pythonEngine = GetCurrentCore().PythonEngine;

            string orig = str;
            var expressions = FindPythonExpressions(str);
            foreach (Expression expression in expressions)
            {
                String result = expression.Template;
                try
                {
                    this.OnMessage(MessageLevel.Debug, "Python expression found: {0}", expression.Code);
                    dynamic r = pythonEngine.Evaluate(expression.Compiled);
                    result = r.ToString();
                    this.OnMessage(MessageLevel.Debug, "Expression evaluated to {0}", result);
                }
                catch (PythonException e)
                {
                    this.OnMessage(MessageLevel.Debug,
                                   "Expression could not be evaluated: Python raised exception {0}",
                                   e.Message);
                }
                str = str.Replace(expression.Template, result);
                break;
            }


            OnMessage(MessageLevel.Debug, "String {0} parsed to {1}", orig, str);

            return str;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public IEnumerable<Expression> FindPythonExpressions(string str)
        {

            IPythonEngine pythonEngine = this.GetCurrentCore().PythonEngine;
            List<Expression> pythonExpressions = new List<Expression>();
            for (int i = 0; i < str.Length; i++)
            {
                string str1 = str;
                if (str[i] != '{')
                {
                    continue;
                }

                int opening = i;

                //pythonExpressions.AddRange(
                //    from clsing in str1.IndexesWhere('}'.Equals)
                //    where clsing > opening
                //    select new Expression
                //           {
                //               Template = str1.Substring(opening,
                //                                         clsing - opening +
                //                                         1),
                //               Code =
                //                   str1.Substring(opening + 1,
                //                                  clsing - opening - 1),
                //               Start = opening,
                //               End = clsing
                //           }
                //    into expression
                //        where this.Throws<PythonParseException>(() => expression.CompileWith(pythonEngine))
                //        //OnMessage(MessageLevel.Debug, "Attempted to compile {0}, error was {1}.", expression.Code, e.Message);
                //        select expression
                //    );

                foreach (int clsing in str1.IndexesWhere('}'.Equals))
                {
                    if (clsing > opening)
                    {
                        Expression expression = new Expression
                                                {
                                                    Template = str1.Substring(opening, clsing - opening + 1),
                                                    Code = str1.Substring(opening + 1, clsing - opening - 1),
                                                    Start = opening,
                                                    End = clsing
                                                };

                        PythonParseException parseException;
                        if (!this.Throws(expression.CompileWith, pythonEngine, out parseException))
                        {
                            this.OnMessage(MessageLevel.Debug, "Parsed {0} as valid python", expression.Template);
                            pythonExpressions.Add(expression);
                        }
                    }
                }
            }
            return pythonExpressions;
        }

        private bool Throws<TException>(System.Action action) where TException : Exception
        {
            TException exception;
            return Throws(action, out exception);
        }

        private bool Throws<TException, T>(System.Action<T> action, T arg) where TException : Exception
        {
            TException exception;
            return Throws(action, arg, out exception);
        }

        protected bool Throws(System.Action action) 
        {
            return this.Throws<Exception>(action);
        }
        


        protected bool Throws(System.Action action, out Exception exception) 
        {
            return this.Throws<Exception>(action, out exception);
        }

        protected bool Throws<TException>(System.Action action, out TException exception) where TException : Exception
        {
            try
            {
                action();
                exception = null;
                return false;
            }
            catch (TException e)
            {
                exception = e;
                return true;
            }
        }

        protected bool Throws<TException, T>(System.Action<T> action, T arg, out TException exception) where TException : Exception
        {
            try
            {
                action(arg);
                exception = null;
                return false;
            }
            catch (TException e)
            {
                exception = e;
                return true;
            }
        }

        protected bool Throws<TException, T1, T2>(System.Action<T1, T2> action, T1 arg1, T2 arg2, out TException exception) where TException : Exception
        {
            try
            {
                action(arg1, arg2);
                exception = null;
                return false;
            }
            catch (TException e)
            {
                exception = e;
                return true;
            }
        }


        private static Func<int, bool> Predicate(int opening)
        {
            return closing => closing > opening;
        }

        /// <summary>
        ///  Ask the user a question
        /// </summary>
        /// <param name="message" />
        /// <param name="title" />
        /// <param name="options" />
        /// <returns />
        protected AskEventEnums.DialogResult Ask(String message, String title = "Question",
                                                 AskEventEnums.AskType options = AskEventEnums.AskType.YesNoCancel)
        {
            var eventArgs = new AskEventArgs
                            {
                                Icon = AskEventEnums.MessageIcon.Question,
                                Message = message,
                                Title = title,
                                Type = options
                            };
                OnAsk(eventArgs);
                return eventArgs.Result;

            
            
        }

        /// <summary>
        /// The event raised when the action needs to ask the user a question. 
        /// </summary>
        /// <remarks>Set by <see cref="ICentipedeCore"/></remarks>
        public event AskEvent AskHandler;

        /// <summary>
        /// Raise the <see cref="AskEvent">Ask Event</see>.
        /// </summary>
        /// <param name="e"></param>
        protected void OnAsk(AskEventArgs e)
        {
            AskEvent handler = AskHandler;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        /// <summary>
        /// Raise the <see cref=" Message"/> event, with the given <paramref name="level"/> and 
        /// <paramref name="message"/>, formatting the message with the provided <paramref name="args">arguments</paramref>.
        /// </summary>
        /// <param name="level">The <see cref="MessageLevel"/>level of the message to send to the user</param>
        /// <param name="message">Message to send.  Supports <see cref="string.Format(string,object[])">String.Format()</see>
        /// type formatting arguments.</param>
        /// <param name="args">arguments to format <paramref name="message"/> with.</param>
        [StringFormatMethod(formatParameterName: @"message")]
        private void OnMessage(MessageLevel level, string message, params object[] args)
        {
            OnMessage(new MessageEventArgs(String.Format(message, args), level));
        }

        /// <summary>
        /// Pass the user a warning level <see cref="Message">message</see>.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="args"></param>
        [StringFormatMethod(@"message")]
        protected void Warning(String message, params object[] args)
        {
            OnMessage(MessageLevel.Warning, message, args);
        }


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

            element.SetAttribute(@"Comment", Comment);
            
            PopulateXmlTag(element);

            String pluginFilePath = Path.GetFileName(thisType.Assembly.CodeBase);
            element.SetAttribute(@"Assembly", pluginFilePath);

            rootElement.AppendChild(element);
        }

        /// <summary>
        /// Populate the Xml Tag passed in through <paramref name="element"/> with the argument values
        /// </summary>
        /// <remarks>Comment is set by the superclass, but can be overridden, should you really need to.</remarks>
        /// <param name="element"></param>
        [Localizable(false)]
        protected virtual void PopulateXmlTag(XmlElement element)
        {
            Type thisType = GetType();
            var wrappers = from member in thisType.GetMembers()
                           where member is FieldInfo || member is PropertyInfo
                           select (FieldAndPropertyWrapper)member
                           into wrapped
                           where wrapped.GetArguementAttribute() != null
                           select wrapped;

            foreach (FieldAndPropertyWrapper wrappedMember in wrappers)
            {
                element.SetAttribute(wrappedMember.Name, wrappedMember.Get<dynamic>(this).ToString());
            }
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

        /// <summary>
        /// Loads an action from an XmlElement
        /// </summary>
        /// <param name="element">the xml to convert</param>
        /// <param name="variables">
        /// Program.Variables, passed to the Action.ctor
        /// </param>
        /// <param name="core"></param>
        /// <returns></returns>
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
                                                  Path.Combine(PluginFolder,
                                                               element.SelectSingleNode(@"@Assembly").Value));
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
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
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
                if (attribute.Name == @"Assembly")
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
        /// Raised when the action needs to pass a message to the user
        /// </summary>
        public event MessageEvent MessageHandler;

        /// <summary>
        /// Raise <see cref="MessageEvent"/> with the given <see cref="MessageEventArgs"/>
        /// </summary>
        /// <param name="e"></param>
        [PublicAPI]
        protected void OnMessage(MessageEventArgs e)
        {
            MessageEvent @event = MessageHandler;
            if (@event != null)
            {
                @event(this, e);
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
        /// returns a <see cref="string"/> that represents the current <see cref="Action"/>
        /// </summary>
        /// <returns>a <see cref="string"/> that represents the current <see cref="Action"/></returns>
        public override String ToString()
        {
            return String.IsNullOrWhiteSpace(Comment)
                       ? Name
                       : String.Format("{0} ({1})", Name, Comment);
        }

        /// <summary>
        /// Passes the user a <see cref="MessageLevel.Action"/> level message
        /// </summary>
        /// <param name="message">message to pass.  Supports <see cref="string.Format(string,object[])">String.Format()</see>
        /// style formatting arguments</param>
        /// <param name="args">Formatting arguments</param>
        [StringFormatMethod(@"message")]
        protected void Message([NotNull] string message, params object[] args)
        {
            OnMessage(MessageLevel.Action, message, args);
        }
    }
}