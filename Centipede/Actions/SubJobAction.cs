using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Xml;
using CentipedeInterfaces;


namespace Centipede.Actions
{
    [ActionCategory("Flow Control", DisplayName = "Run Centipede Job")]
    class SubJobAction : Action
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="variables"></param>
        /// <param name="c"></param>
        public SubJobAction(IDictionary<string, object> variables, ICentipedeCore c)
                : base("Run Sub-job", variables,c)
        { }

        
        [ActionArgument(DisplayName="Job Filename")]
        public String JobFileName = "";


        [ActionArgument(DisplayName = "Input Variables",
                Usage = "Comma-separated list of variables to set within the subjob", 
                Literal = true)]
        public String InputVars = "";

        [ActionArgument(DisplayName = "Output Variables",
                Usage = "Comma-separated list of variables to retrieve from the subjob", 
                Literal = true)]
        public String OutputVars = "";

        /// <summary>
        /// Perform the action
        /// </summary>
        /// <exception cref="ActionException">
        /// the action cannot be completed
        /// </exception>
        protected override void DoAction()
        {
            ICentipedeCore core = new CentipedeCore(null);

            foreach (String inputVarName in InputVars.Split(',').Select(s => s.Trim()))
            {
                core.Variables.SetVariable(inputVarName, Variables[inputVarName]);
            }

            core.LoadJob(ParseStringForVariable(JobFileName));

            MainWindow newMain = new MainWindow(core);
            newMain.FormClosed += delegate
                                  {
                                      foreach (var outputVar in OutputVars.Split(',').Select(s => s.Trim()))
                                      {
                                          Variables[outputVar] = core.Variables[outputVar];
                                          core.Variables[outputVar] = null;
                                      }
                                  };
            
            newMain.ShowDialog(GetCurrentCore().Window);
            Thread runjobThread = new Thread(delegate(object o)
                                             {
                                                 using (this)
                                                 {
                                                     core.RunJob((Boolean)o);
                                                 }
                                             });
                                             
            runjobThread.Start(GetCurrentCore().IsStepping);
            runjobThread.Join();
            
        }

        public override int Complexity
        {
            get
            {
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load(ParseStringForVariable(JobFileName));
                var nav = xmlDocument.CreateNavigator();
                return (int)nav.Evaluate(@"number(Actions/*)");
            }
        }
    }
}
