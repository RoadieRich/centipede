﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Xml;
using System.Xml.XPath;
using CentipedeInterfaces;

namespace Centipede.Actions
{

    /// <summary>
    /// 
    /// </summary>
    [ActionCategory("", DisplayControl = "MissingActionDisplayControl", IconName = "missing")]
    public class MissingAction : Action
    {
        /// <summary>
        /// Create an action to represent an action from a plugin the user doesn't have installed
        /// </summary>
        /// <param name="name"></param>
        /// <param name="variables"></param>
        /// <param name="core"></param>
        public MissingAction(string name, IDictionary<string, object> variables, ICentipedeCore core)
            : base(name, variables, core)
        { }

        internal Dictionary<string, object> Fields = new Dictionary<string, object>();


        /// <summary>
        /// Perform the action
        /// </summary>
        /// <exception cref="ActionException">
        /// the action cannot be completed
        /// </exception>
        /// <exception cref="FatalActionException">The job needs to halt</exception>
        protected override void DoAction()
        {
            throw new FatalActionException("Cannot run action from missing plugin", this);
        }

        internal string Value;
        private string _element;
        private string _name;

        /// <summary>
        /// Populate members from the given Xml Element.
        /// </summary>
        /// <remarks>
        /// Should be implemented if <see cref="Action.AddToXmlElement" /> is
        /// non-trivially overridden.
        /// </remarks>
        /// <param name="element">The XmlElement describing the action</param>
        protected override void PopulateMembersFromXml(XPathNavigator element)
        {

            this._element = _generateElementFromNav(element).OuterXml;

            var attributes = from attr in element.Select(@"./@*").OfType<XPathNavigator>()
                             where attr.LocalName != "Assembly"
                             select attr;
            
            foreach (XPathNavigator attribute in attributes)
            {
                if (attribute.LocalName == "Comment")
                {
                    Comment = attribute.Value;
                    continue;
                }
                this.Fields.Add(attribute.LocalName, attribute.Value);
            }
            this.Value = element.Value;
            this._name = element.LocalName;
            Name = "Missing Action: " + new String(_name.SkipWhile(c => c != '.').Skip(1).ToArray());
        }

        private XmlNode _generateElementFromNav(XPathNavigator element)
        {
            return (XmlNode)element.UnderlyingObject;
        }

        /// <summary>
        /// Append xml code for the action to the given parent element
        /// </summary>
        /// <param name="rootElement">
        /// The parent element to add the action to
        /// </param>
        public override void AddToXmlElement(XmlElement rootElement)
        {
            XmlDocument doc = rootElement.OwnerDocument;
            var e = doc.CreateElement(_name);
            e.InnerXml = _element;
            rootElement.AppendChild(e.FirstChild);
        }
       
    }


    class MissingActionDisplayControl : ActionDisplayControl
    {
        public MissingActionDisplayControl(IAction action)
            : base(action, false)
        {
            MissingAction missingAction = (MissingAction)action;

            var fieldControls = from field in missingAction.Fields
                                select new Control[]
                                       {
                                           new Label
                                           {
                                               Text = field.Key
                                           },
                                           new TextBox
                                           {
                                               Text = field.Value.ToString(),
                                               ReadOnly = true
                                           }
                                       };

            foreach (var fieldControl in fieldControls)
            {
                AttributeTable.Controls.AddRange(fieldControl);
            }

            CommentTextBox.ReadOnly = true;
        }

        

    }
}
