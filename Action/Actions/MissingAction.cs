using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.XPath;
using CentipedeInterfaces;

namespace Centipede.Actions
{

    [ActionCategory("", DisplayControl = "MissingActionDisplayControl", IconName = "missing")]
    class MissingAction : Action
    {
        public MissingAction(string name, IDictionary<string, object> variables, ICentipedeCore core)
            : base(name, variables, core)
        { }

        public Dictionary<string, object> fields = new Dictionary<string, object>();

        protected override void DoAction()
        {
            throw new FatalActionException("Cannot run action from missing plugin", this);
        }

        public string value;

        protected override void PopulateMembersFromXml(XPathNavigator element)
        {
            foreach (XPathNavigator attribute in element.Select(@"./@*"))
            {
                if(attribute.LocalName == "Assembly") continue;
                if (attribute.LocalName == "Comment")
                {
                    Comment = attribute.Value;
                    continue;
                }
                fields.Add(attribute.LocalName, attribute.Value);
            }
            value = element.Value;
            Name = "Missing Action: " + new String(element.LocalName.SkipWhile(c => c != '.').Skip(1).ToArray());
        }

       
    }


    class MissingActionDisplayControl : ActionDisplayControl
    {
        public MissingActionDisplayControl(IAction action)
            : base(action, false)
        {
            MissingAction missingAction = action as MissingAction;

            var fieldControls = from field in missingAction.fields
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
        }

        

    }
}
