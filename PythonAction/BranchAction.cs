using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Xml;
using Centipede;
using Centipede.Actions;
using Centipede.Properties;
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
        /// <remarks>
        /// Condition is a simple python expression, which is evalutated in a "safe" scope: 
        /// changes made to any (python) variables will be lost.
        /// </remarks>
        public BranchAction(IDictionary<String, Object> v)
            : base(Resources.BranchAction_BranchAction_Branch, v)
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
        public override Action GetNext()
        {
            return _result ? NextIfTrue : Next;
        }

        [Localizable(false)]
        public override void AddToXmlElement(XmlElement rootElement)
        {
            Type thisType = GetType();
            XmlElement element = rootElement.OwnerDocument.CreateElement(thisType.FullName);
            element.SetAttribute("Comment", Comment);
            element.SetAttribute("Target", MainWindow.Instance.Core.Actions.IndexOf(NextIfTrue).ToString(CultureInfo.InvariantCulture));
            element.InnerText = ConditionSource;
            rootElement.AppendChild(element);
        }

        [Localizable(false)]
        protected override void PopulateMembersFromXml(XmlElement element)
        {
            Comment = element.GetAttribute("Comment");
            ConditionSource = element.InnerText;
            int index = int.Parse(element.GetAttribute("Target"));
            CentipedeCore.AfterLoadEventHandler instanceOnAfterLoad = null;
            instanceOnAfterLoad = delegate{ 
                NextIfTrue = MainWindow.Instance.Core.Actions[index];
                MainWindow.Instance.Core.AfterLoad -= instanceOnAfterLoad;
                ((ActionDisplayControl)Tag).UpdateControls();
            };
            MainWindow.Instance.Core.AfterLoad += instanceOnAfterLoad;
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
