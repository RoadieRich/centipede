using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Xml;
using CentipedeInterfaces;


namespace Centipede.Actions
{
    [ActionCategory("Flow Control", displayName = "Run Centipede Job")]
    class SubJob : Action
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="v"></param>
        /// <param name="c"></param>
        public SubJob(IDictionary<string, object> v, ICentipedeCore c)
                : base("Subjob", v,c)
        { }

        
        [ActionArgument(displayName="Job Filename")]
        public String JobFileName = "";

        
        [ActionArgument(displayName = "Input Variables", usage="Comma-separated list of variables to set within the subjob")]
        public String InputVars = "";

        [ActionArgument(displayName = "Output Variables", usage = "Comma-separated list of variables to retrieve from the subjob")]
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
                core.Variables.AddVariablesTableRow(inputVarName, Variables[inputVarName]);
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
            
            newMain.ShowDialog();
            Thread runjobThread = new Thread(delegate(object arg)
                                             {
                                                 ICentipedeCore subjobCore = (ICentipedeCore)arg;
                                                 subjobCore.RunJob();
                                             });
            runjobThread.Start(core);
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
