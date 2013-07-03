using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
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

        private ICentipedeCore _centipedeCore;
        private Form _targetwindow;
        private AutoResetEvent _are;

        /// <summary>
        /// Perform the action
        /// </summary>
        /// <exception cref="ActionException">
        /// the action cannot be completed
        /// </exception>
        protected override void DoAction()
        {

            this._targetwindow = (Form)GetCurrentCore().Tag;

            //BackgroundWorker bgw = new BackgroundWorker();
            //bgw.DoWork += this.Target;
            this._are = new AutoResetEvent(false);
            //GetCurrentCore().Window.Invoke(new System.Action(bgw.RunWorkerAsync));

            Thread t = new Thread(this.Target);
            t.SetApartmentState(ApartmentState.STA);
            t.Start();
            this._are.WaitOne();
        }

        private void Target()
        {
            this._centipedeCore = new CentipedeCore(null);
            
            MainWindow newMain = new MainWindow(this._centipedeCore);
            
            foreach (String inputVarName in this.InputVars.Split(',').Select(s => s.Trim()))
            {
                this._centipedeCore.Variables.SetVariable(inputVarName, Variables[inputVarName]);
            }
            newMain.LoadJobAfterLoad(JobFileName);
            newMain.RunJobAfterLoad();
            newMain.FormClosed += delegate
                                      {
                                          foreach (var outputVar in this.OutputVars.Split(',').Select(s => s.Trim()))
                                          {
                                              Variables[outputVar] = this._centipedeCore.Variables[outputVar];
                                              this._centipedeCore.Variables[outputVar] = null;
                                              this._are.Set();
                                          }
                                      };


            newMain.Show();
        }

        public override int Complexity
        {
            get
            {
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load(ParseStringForVariable(JobFileName));
                var nav = xmlDocument.CreateNavigator();
                return (int)(double)nav.Evaluate(@"count(//Actions/*)");
            }
        }
    }
}
