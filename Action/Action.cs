using System;
using System.Collections.Generic;
using System.Xml;
using System.Reflection;
using System.Linq;
using System.IO;
using System.Xml.Serialization;
namespace Centipede
{
    /// <summary>
    /// Base Action class: all actions will subclass this
    /// </summary>
    [Serializable]
    public abstract class Action
    {
        public Action(String name, Dictionary<String,Object> variables)
        {
            Name = name;
            Variables = variables;
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


        static public Action ReadFrom(TextReader reader)
        {
            
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(Action), "Centipede");

            return xmlSerializer.Deserialize(reader) as Action;

        }

        public void WriteTo(TextWriter writer)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(this.GetType(), "Centipede");
            xmlSerializer.Serialize(writer, this);
        }
        
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
                                            Properties.Settings.Default.PluginFolder,
                                            element.GetAttribute("Assembly"));
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
        public String helpText = "";
        public String displayName = "";
        public String DisplayControl = "";
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
