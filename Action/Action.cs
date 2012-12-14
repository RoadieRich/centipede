using System;
using System.Collections.Generic;
using System.Xml;
using System.Reflection;
using System.Linq;
using System.IO;
using Centipede.StringInject;

[assembly:CLSCompliant(true)]

namespace Centipede
{
    /// <summary>
    /// Base Action class: all actions will subclass this
    /// </summary>
    [Serializable]
    public abstract class Action
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="variables"></param>
        protected Action(String name, Dictionary<String,Object> variables)
        {
            Name = name;
            Variables = variables;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="s"></param>
        public delegate void Setter(String s);

        /// <summary>
        /// 
        /// </summary>
        protected Dictionary<String, Object> Variables;

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
        public Action Next = null;

        /// <summary>
        /// 
        /// </summary>
        protected virtual void InitAction()
        { }

        /// <summary>
        /// 
        /// </summary>
        protected abstract void DoAction();
        
        /// <summary>
        /// 
        /// </summary>
        protected virtual void CleanupAction()
        { }

        /// <summary>
        /// 
        /// </summary>
        public void Run()
        {
            InitAction();
            DoAction();
            CleanupAction();
        }

        private Int32 _complexity = 1;

        /// <summary>
        /// 
        /// </summary>
        public virtual Int32 Complexity
        {
            get
            {
                return _complexity;
            }
            protected set
            {
                _complexity = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual Action GetNext()
        {
            return Next;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        protected String ParseStringForVariable(String str)
        {
            return str.Inject(Variables);
        }

        /// <summary>
        /// 
        /// </summary>
        public static HashSet<String> TrueValues = new HashSet<string>(new String[]{
                                         "yes",
                                         "true",
                                         "1"
                                     });
                
        /// <summary>
        /// 
        /// </summary>
        /// <param name="rootElement"></param>
        public void AddToXmlElement(XmlElement rootElement)
        {

            Type thisType = this.GetType();
            XmlElement element = rootElement.OwnerDocument.CreateElement(thisType.FullName);

            foreach (FieldInfo field in thisType.GetFields().Where(f=>f.GetCustomAttributes(typeof(ActionArgumentAttribute),true).Count()>0))
            {
                element.SetAttribute(field.Name, field.GetValue(this).ToString());
            }
            String pluginFilePath = Path.GetFileName(thisType.Assembly.CodeBase);

            element.SetAttribute("Comment", this.Comment);
            element.SetAttribute("Assembly", pluginFilePath);

            rootElement.AppendChild(element);
        }
               
        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        /// <param name="variables"></param>
        /// <returns></returns>
        static public Action FromXml(XmlElement element, Dictionary<String, Object> variables)
        {
            Type[] constructorArgumentTypes = { typeof(Dictionary<String, Object>) };
            
            Assembly asm;

            //if action is not a plugin:
            if (element.LocalName.Count(c => c == '.') <= 1)
            {
                asm = Assembly.GetEntryAssembly();
            }
            else
            {
                String asmPath = Path.Combine(
                                            Path.GetDirectoryName(Assembly.GetEntryAssembly().Location),
                                            Path.Combine(Properties.Settings.Default.PluginFolder,
                                            element.GetAttribute("Assembly")));
                asm = Assembly.LoadFile(asmPath);
            }
            
            Type t = asm.GetType(element.LocalName);

            Object instance;

            MethodInfo customFromXmlMethod = t.GetMethod("CustomFromXml");
            if (customFromXmlMethod != null)
            {
                instance = customFromXmlMethod.Invoke(t, new object[] {element, variables});
            }
            else
            {
                instance = t.GetConstructor(constructorArgumentTypes).Invoke(new object[] { variables });

                element.Attributes.RemoveNamedItem("Assembly");

                foreach (XmlAttribute attribute in element.Attributes)
                {
                    FieldInfo field = t.GetField(attribute.Name);
                    MethodInfo parseMethod = field.FieldType.GetMethod("Parse", new Type[] { typeof(String) });
                    Object parsedValue = attribute.Value;
                    if (parseMethod != null)
                    {
                        field.SetValue(instance, parseMethod.Invoke(field, new object[] { attribute.Value }));
                    }
                    else
                    {
                        field.SetValue(instance, attribute.Value);
                    }
                }
            }
            return instance as Action;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    [System.AttributeUsage(AttributeTargets.Field)]
    public sealed class ActionArgumentAttribute : System.Attribute
    {
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
        public String setterMethodName;

        /// <summary>
        /// 
        /// </summary>
        public Object setterMethod;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public delegate Boolean ArgumentSetter(String value);
    }

    /// <summary>
    /// 
    /// </summary>
    [System.AttributeUsage(AttributeTargets.Class)]
    public sealed class ActionCategoryAttribute : System.Attribute
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="category"></param>
        public ActionCategoryAttribute(String category)
        {
            this.category = category;
        }

        /// <summary>
        /// 
        /// </summary>
        public readonly String category;

        /// <summary>
        /// 
        /// </summary>
        public String helpText;

        /// <summary>
        /// 
        /// </summary>
        public String displayName;

        /// <summary>
        /// 
        /// </summary>
        public String displayControl;

        /// <summary>
        /// 
        /// </summary>
        public String iconName;

    }

    /// <summary>
    /// 
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
        public readonly Action ErrorAction = null;
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
