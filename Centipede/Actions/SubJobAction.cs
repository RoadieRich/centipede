using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Threading;
using System.Windows.Forms;
using CentipedeInterfaces;
using ResharperAnnotations;
using Timer = System.Threading.Timer;


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
        private NamedPipeServerStream _messagePipe;
        private Process _process;

        protected override void DoAction()
        {
            Timeout timeout = new Timeout(1000);
            EventHandler ontimeout = delegate { throw new ActionException("The pipe timed out", this); };
            this._process = new Process
                            {
                                StartInfo =
                                {
                                    FileName = this.JobFileName,

                                    Verb = "Run",
                                    UseShellExecute = true
                                }
                            };

            this._process.Start();

            this._ServerStream = new NamedPipeServerStream(PipeName, PipeDirection.InOut);
            

            try
            {
                using(new Timeout(1000, ontimeout))
                    this._ServerStream.WaitForConnection();

                var variables = this.InputVars.Split(',').Select(s => s.Trim()).ToList();

                foreach (var variable in variables)
                {
                    object o = Variables[variable];
                    try
                    {
                        using(new Timeout(1000, ontimeout))
                            CentipedeSerializer.Serialize(this._ServerStream, o);
                    }
                    catch (SerializationException e)
                    {
                        throw new FatalActionException(
                            string.Format("Cannot send variable {0} to subjob, type {1} is not supported.",
                                          variable,
                                          o.GetType()),
                            e,
                            this);
                    }
                }

                this._messagePipe = new NamedPipeServerStream(@"CentipedeMessagePipe", PipeDirection.In);
                BackgroundWorker bgw = new BackgroundWorker();
                bgw.DoWork += BgwOnDoWork;

                bgw.RunWorkerAsync();
                using (new Timeout(60 * 5 * 1000, ontimeout))
                {
                    bool subJobSuccess = (bool)CentipedeSerializer.Deserialize(this._ServerStream);

                    if (!subJobSuccess)
                    {
                        throw new ActionException("Subjob was not completed", this);
                    }
                }
                foreach (var outputVar in OutputVars.Split(',').Select(s => s.Trim()))
                {
                    using (new Timeout(1000, ontimeout))
                        Variables[outputVar] = CentipedeSerializer.Deserialize(this._ServerStream);
                    
                }
            }
            catch (Exception e)
            {
                OnMessage(new MessageEventArgs
                          {
                              Level = MessageLevel.Error,
                              Message = e.Message
                          });
            }
            finally
            {
                this._ServerStream.Close();
                this._messagePipe.Close();
                if (!this._process.HasExited)
                {
                    try
                    {
                        this._process.CloseMainWindow();
                        this._process.Close();
                        if (!this._process.WaitForExit(2000))
                        {
                            _process.Kill();
                        }

                    }
                    catch (InvalidOperationException)
                    { }
                    catch (Win32Exception)
                    { }
                }
            }
        }

        private void BgwOnDoWork(object sender, DoWorkEventArgs doWorkEventArgs)
        {
            _messagePipe.WaitForConnection();
            try
            {
                while (true)
                {
                    string actionName;
                    MessageEventArgs messageEventArgs = CentipedeSerializer.DeserializeMessage(_messagePipe,
                                                                                               out actionName);

                    messageEventArgs.Message = String.Format("Sub job message from {0}: {1}",
                                                             actionName,
                                                             messageEventArgs.Message);

                    OnMessage(messageEventArgs);
                }
            }
            catch (IOException)
            { }
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
            _ServerStream.Close();
            _messagePipe.Close();
        }

        private void CloseServerStream(object sender, JobCompletedEventArgs jobCompletedEventArgs)
        {
            _ServerStream.Close();
            _messagePipe.Close();
        }
    }

    public abstract class SubJobEntryExitPoint : Action
    {
        protected SubJobEntryExitPoint(string name, IDictionary<string, object> variables, ICentipedeCore core)
            : base(name, variables, core)
        {
            _ontimeout = delegate { throw new FatalActionException("The pipe timed out", this); };
        }

        protected static NamedPipeClientStream ClientStream;
        protected EventHandler _ontimeout;
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

                _messagePipe = new NamedPipeClientStream(@".", @"CentipedeMessagePipe", PipeDirection.Out);
                

                foreach (var action in GetCurrentCore().Job.Actions)
                {
                    action.MessageHandler += SubJobMessageHandler;

                }
            }
            catch
            { }
        }

        private void SubJobMessageHandler(object sender, MessageEventArgs e)
        {
            if (!_messagePipe.IsConnected)
            {
                _messagePipe.Connect();
            }
            CentipedeSerializer.SerializeMessage(_messagePipe, sender, e);
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
                _messagePipe.Close();
                Application.Exit();
            }
        }

        public SubJobEntry(IDictionary<string, object> variables, ICentipedeCore core)
            : base("SubJob Entry", variables, core)
        {
        }

        [ActionArgument(DisplayName = "Input Variables",
            Usage = "Comma-separated list of variables to set within the subjob",
            Literal = true)]
        public String InputVars = "";

        private NamedPipeClientStream _messagePipe;

        protected override void DoAction()
        {
            ClientStream = new NamedPipeClientStream(@".", SubJobAction.PipeName);
            using(new Timeout(1000, _ontimeout))
                ClientStream.Connect();
            try
            {
                foreach (var variable in InputVars.Split(',').Select(s => s.Trim()))
                {
                    using (new Timeout(1000, this._ontimeout))
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
                    using (new Timeout(1000, this._ontimeout))
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

    internal class Timeout : IDisposable
    {
        private readonly int _milliseconds;
        private Timer _timer;

        public Timeout(int milliseconds)
        {
            _milliseconds = milliseconds;
            this._timer = new Timer(this.Callback);
        }
        public Timeout(int milliseconds, EventHandler e) : this(milliseconds)
        {
            this.OnTimeout += e;
            this.Start();
        }

        private const int _Infinite = System.Threading.Timeout.Infinite;

        public void Start()
        {
            this._timer.Change(this._milliseconds, _Infinite);
        }

        public void Stop()
        {
            this._timer.Change(_Infinite, _Infinite);
        }

        private void Callback(object state)
        {
            var handler = this.OnTimeout;
            if (handler != null)
            {
                handler.Invoke(this, null);
            }
        }

        public event EventHandler OnTimeout;
        public void Dispose()
        {
            this.Stop();
            this._timer.Dispose();
        }
    }
}