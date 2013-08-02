using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Xml;
using System.Xml.XPath;
using Centipede.Actions;
using Centipede.Properties;
using CentipedeInterfaces;
using Action = Centipede.Action;


namespace PyAction
{
    /// <summary>
    /// Basic branch action - has two possible "next" actions, 
    /// which are chosen according to condition.
    /// </summary>
    [ActionCategory(@"Flow Control", DisplayName = "Branch", DisplayControl = @"BranchDisplayControl",
                    IconName = @"branch")]
    public class BranchAction : Action
    {
        /// <summary>
        /// create a new Branch action
        /// </summary>
        /// <param name="variables"></param>
        /// <param name="c"></param>
        public BranchAction(IDictionary<string, object> variables, ICentipedeCore c)
            : base(Resources.BranchAction_BranchAction_Branch, variables, c)
        { }

        /// <summary>
        /// A simple python expression, evaluated and interpreted as a bool to determine which action to exceute nexrt.
        /// </summary>
        /// <remarks>
        ///     ConditionSource is evalutated in a "safe" scope - changes made to any variables will be discarded.
        /// </remarks>
        [ActionArgument(DisplayName = @"Condition")]
        public String ConditionSource = @"False";

        /// <summary>
        /// the action to jmp top if the condition executes to true.
        /// </summary>
        [ActionArgument(
            Usage = @"Next action if condition returns true (otherwise, proceed normally)",
            DisplayName = @"Next Action if True"
            )]
        public IAction NextIfTrue;

        private Boolean _result;

        /// <summary>
        /// 
        /// </summary>
        protected override void DoAction()
        {
            var pye = GetCurrentCore().PythonEngine;

            var scope = pye.GetNewScope(Variables);
            _result = pye.Evaluate<bool>(ConditionSource, scope);
        }

        public override IAction GetNext()
        {
            return _result ? NextIfTrue : Next;
        }

        [Localizable(false)]
        protected override void PopulateXmlTag(XmlElement element)
        {
            element.SetAttribute("Target",
                                 GetCurrentCore().Job.Actions.IndexOf(NextIfTrue).ToString(CultureInfo.InvariantCulture));
            element.InnerText = ConditionSource;
        }

        [Localizable(false)]
        protected override void PopulateMembersFromXml(XPathNavigator element)
        {
            
            Comment = element.SelectSingleNode("@Comment").Value;
            ConditionSource = element.Value;
            int index = int.Parse(element.SelectSingleNode("@Target").Value);
            
            AfterLoadEvent instanceOnAfterLoad = null;

            /// Target can be saved as "-1" if the target does not exist
            if (index != -1) instanceOnAfterLoad = delegate
                                      {
                                          NextIfTrue = GetCurrentCore().Job.Actions[index];
                                          GetCurrentCore().AfterLoad -= instanceOnAfterLoad;
                                          ((ActionDisplayControl)Tag).UpdateControls();
                                      };
            GetCurrentCore().AfterLoad += instanceOnAfterLoad;
        }
    }
}
