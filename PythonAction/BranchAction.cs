using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using System.Xml;
using System.Xml.XPath;
using Centipede;
using Centipede.Actions;
using Centipede.Properties;
using CentipedeInterfaces;
using Action = Centipede.Action;


namespace PyAction
{
    /// <summary>
    /// 
    /// </summary>

    [ActionCategory(@"Flow Control", displayName = "Branch", displayControl = @"BranchDisplayControl", iconName = @"branch")]
    public class BranchAction : Action
    {
        /// <summary>
        /// Basic branch action - has two possible "next" actions, 
        /// which are chosen according to condition.
        /// </summary>
        /// <param name="v"></param>
        /// <param name="c"></param>
        /// <remarks>
        /// Condition is a simple python expression, which is evalutated in a "safe" scope: 
        /// changes made to any (python) variables will be lost.
        /// </remarks>
        public BranchAction(IDictionary<string, object> v, ICentipedeCore c)
            : base(Resources.BranchAction_BranchAction_Branch, v, c)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        [ActionArgument(displayName = @"Condition")]
        public String ConditionSource = @"False";

        /// <summary>
        /// 
        /// </summary>
        [ActionArgument(
            usage = @"Next action if condition returns true (otherwise, proceed normally)",
            displayName = @"Next Action if True"
        )]
        public Action NextIfTrue;

        private Boolean _result;

        /// <summary>
        /// 
        /// </summary>
        protected override void DoAction()
        {
            var pye = PythonEngine.PythonEngine.Instance;
            
            var scope = pye.GetNewScope(Variables);
            _result = pye.Evaluate<bool>(ConditionSource, scope);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override IAction GetNext()
        {
            return _result ? NextIfTrue : Next;
        }

        [Localizable(false)]
        protected override void PopulateXmlTag(XmlElement element)
        {
            element.SetAttribute("Target", GetCurrentCore().Job.Actions.IndexOf(NextIfTrue).ToString(CultureInfo.InvariantCulture));
            element.InnerText = ConditionSource;
        }

        [Localizable(false)]
        protected override void PopulateMembersFromXml(XPathNavigator element)
        {
            Comment = element.SelectSingleNode("@Comment").Value;
            ConditionSource = element.Value;
            int index = int.Parse(element.SelectSingleNode("@Target").Value);
            AfterLoadEvent instanceOnAfterLoad = null;
            instanceOnAfterLoad = delegate{
                NextIfTrue = (Action)GetCurrentCore().Job.Actions[index];
                GetCurrentCore().AfterLoad -= instanceOnAfterLoad;
                ((ActionDisplayControl)Tag).UpdateControls();
            };
            GetCurrentCore().AfterLoad += instanceOnAfterLoad;
        }
    }

/*
    public class BranchActionI18N
    {
        public String Base = @"Resources";
        public static Dictionary<String, String> NextIfTrue = new Dictionary<String, String>
                                                              {
                                                                      {
                                                                              @"Usage",
                                                                              @"BranchActionUsage"
                                                                      },
                                                                      {
                                                                              @"Display Name",
                                                                              @"BranchActionDisplayName"
                                                                      }
                                                              };

        public static Dictionary<String, String> Condition = new Dictionary<string, string>
                                                             {
                                                                     {
                                                                             @"Display Name",
                                                                             @"BranchActionI18N_Condition_DisplayName"
                                                                     }
                                                             };
    }
*/  
}
