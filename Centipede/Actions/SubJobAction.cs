using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Pipes;
using System.Linq;
using System.Runtime.Serialization;
using System.Windows.Forms;
using CentipedeInterfaces;


namespace Centipede.Actions
{
    [ActionCategory("Flow Control", DisplayName = "Run Subjob")]
    internal class SubJobAction : Action
    {
        public SubJobAction(IDictionary<string, object> variables, ICentipedeCore c)
            : base("Run Sub-job", variables, c)
        {
            if (GetCurrentCore().Job.Actions.OfType<SubJobEntry>().Any())
            {
                throw new ApplicationException("Cannot nest subjobs more than one layer deep");
            }
        }

        public static readonly string PipeName = "CentipedePipe";

        [ActionArgument(DisplayName = "Job Filename")]
        public String JobFileName = "";

        [ActionArgument(DisplayName = "Input Variables",
            Usage = "Comma-separated list of variables to set within the subjob",
            Literal = true)]
        public String InputVars = "";

        [ActionArgument(DisplayName = "Output Variables",
            Usage = "Comma-separated list of variables to retrieve from the subjob",
            Literal = true)]
        public String OutputVars = "";

        private NamedPipeServerStream _ServerStream;

        protected override void DoAction()
        {
            Process process = new Process
                              {
                                  StartInfo =
                                  {
                                      FileName = this.JobFileName,

                                      Verb = "Run",
                                      UseShellExecute = true
                                  }
                              };

            process.Start();

            this._ServerStream = new NamedPipeServerStream(PipeName, PipeDirection.InOut);
            this._ServerStream.WaitForConnection();

            var variables = this.InputVars.Split(',').Select(s => s.Trim()).ToList();

            foreach (var variable in variables)
            {
                object o = Variables[variable];
                try
                {
                    CentipedeSerializer.Serialize(this._ServerStream, o);
                }
                catch (SerializationException e)
                {
                    this._ServerStream.Close();
                    throw new FatalActionException(
                        string.Format("Cannot send variable {0} to subjob, type {1} is not supported.",
                                      variable,
                                      o.GetType()),
                        e,
                        this);
                }
            }

            bool subJobSuccess = (bool)CentipedeSerializer.Deserialize(this._ServerStream);

            if (!subJobSuccess)
            {
                throw new ActionException("Subjob was not completed", this);
            }

            foreach (var outputVar in OutputVars.Split(',').Select(s => s.Trim()))
            {
                Variables[outputVar] = CentipedeSerializer.Deserialize(this._ServerStream);
            }
        }

        protected override void InitAction()
        {
            base.InitAction();

            try
            {
                GetCurrentCore().JobCompleted -= CloseServerStream;
            }
            catch
            { }
        }

        protected override void CleanupAction()
        {
            base.CleanupAction();
            GetCurrentCore().JobCompleted += CloseServerStream;
        }

        private void CloseServerStream(object sender, JobCompletedEventArgs jobCompletedEventArgs)
        {
            _ServerStream.Close();
        }
    }

    public abstract class SubJobEntryExitPoint : Action
    {
        protected SubJobEntryExitPoint(string name, IDictionary<string, object> variables, ICentipedeCore core)
            : base(name, variables, core)
        { }

        protected static NamedPipeClientStream ClientStream;
    }

    [ActionCategory("Flow Control", DisplayName = "Subjob Entry")]
    public class SubJobEntry : SubJobEntryExitPoint
    {
        protected override void InitAction()
        {
            base.InitAction();
            try
            {
                GetCurrentCore().JobCompleted -= OnJobCompleted;
            }
            catch
            { }
        }

        protected override void CleanupAction()
        {
            base.CleanupAction();
            GetCurrentCore().JobCompleted += OnJobCompleted;
        }

        private void OnJobCompleted(object sender, JobCompletedEventArgs jobCompletedEventArgs)
        {
            if (!jobCompletedEventArgs.Completed)
            {
                try
                {
                    CentipedeSerializer.Serialize(ClientStream, false);
                }
                catch (SerializationException e)
                { }
                ClientStream.Close();
                Application.Exit();
            }
        }

        public SubJobEntry(IDictionary<string, object> variables, ICentipedeCore core)
            : base("SubJob Entry", variables, core)
        { }

        [ActionArgument(DisplayName = "Input Variables",
            Usage = "Comma-separated list of variables to set within the subjob",
            Literal = true)]
        public String InputVars = "";

        protected override void DoAction()
        {
            ClientStream = new NamedPipeClientStream(@".", SubJobAction.PipeName);
            ClientStream.Connect();

            try
            {
                foreach (var variable in InputVars.Split(',').Select(s => s.Trim()))
                {
                    Variables[variable] = CentipedeSerializer.Deserialize(ClientStream);
                }
            }
            catch (ObjectDisposedException e)
            {
                throw new FatalActionException("Getting variables from parent job failed.", e, this);
            }
        }
    }

    [ActionCategory("Flow Control", DisplayName = "Subjob Exit")]
    public class SubJobExitPoint : SubJobEntryExitPoint
    {
        public SubJobExitPoint(IDictionary<string, object> variables, ICentipedeCore core)
            : base("SubJob Exit", variables, core)
        { }

        [ActionArgument(DisplayName = "Output Variables",
            Usage = "Comma-separated list of variables to return to the higher level job",
            Literal = true)]
        public String OutVars = "";


        protected override void DoAction()
        {
            CentipedeSerializer.Serialize(ClientStream, true);
            ClientStream.WaitForPipeDrain();
            foreach (var variable in OutVars.Split(',').Select(s => s.Trim()))
            {
                object o = Variables[variable];
                try
                {
                    CentipedeSerializer.Serialize(ClientStream, o);
                }
                catch (SerializationException e)
                {
                    throw new FatalActionException(
                        string.Format("Cannot send variable {0} to parent job, type {1} is not supported.",
                                      variable,
                                      o.GetType()),
                        e,
                        this);
                }
            }
            ClientStream.Close();

            Application.Exit();
        }
    }
}