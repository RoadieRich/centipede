using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization;
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

        public static readonly string PipeName = @"CentipedePipe";
        public static readonly int PortNumber = 9876;

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

        private Socket _serverStream;
        private NamedPipeServerStream _messagePipe;
        private Process _process;
        private BackgroundWorker _bgw;

        protected override void DoAction()
        {
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

            this._serverStream = new Socket(AddressFamily.InterNetwork,
                                            SocketType.Stream,
                                            ProtocolType.IP);
            this._serverStream.Bind(new IPEndPoint(IPAddress.Loopback, PortNumber));

            this._bgw = new BackgroundWorker();
            this._bgw.WorkerSupportsCancellation = true;
            this._bgw.DoWork += BgwOnDoWork;

            this._bgw.RunWorkerAsync();


            Socket connection = null;
            try
            {
                this._serverStream.Listen(0);

                connection = this._serverStream.Accept();

                var ss = new SocketStream(connection);


                if (!String.IsNullOrWhiteSpace(this.InputVars))
                {
                    List<string> variables =
                        this.InputVars.Split(',').Select(s => s.Trim()).ToList();

                    //send number of variables
                    CentipedeSerializer.Serialize(ss, variables.Count);

                    try
                    {
                        foreach (var variable in variables)
                        {
                            object o = this.Variables[variable];
                            try
                            {
                                CentipedeSerializer.Serialize(ss, o);
                            }
                            catch (SerializationException e)
                            {
                                throw new FatalActionException(
                                    string.Format(
                                                  "Cannot send variable {0} to subjob, type {1} is not supported.",
                                                  variable,
                                                  o.GetType()),
                                    e,
                                    this);
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        throw new ActionException(e, this);
                    }
                }
                else
                {
                    //no variables to send
                    CentipedeSerializer.Serialize(ss, 0);
                }

                var subJobSuccess = (bool) CentipedeSerializer.Deserialize(ss);

                if (!subJobSuccess)
                {
                    throw new ActionException("Subjob was not completed", this);
                }


                //recieve vars, if any
                var varsToRecieve = CentipedeSerializer.Deserialize<int>(ss);

                var received = new List<object>(varsToRecieve);

                for (int i = 0; i < varsToRecieve; i++)
                {
                    received.Add(CentipedeSerializer.Deserialize(ss));
                }

                if (!String.IsNullOrWhiteSpace(this.OutputVars))
                {
                    List<string> outputVars =
                        this.OutputVars.Split(',').Select(s => s.Trim()).ToList();

                    if (varsToRecieve != outputVars.Count)
                    {
                        throw new FatalActionException(
                            string.Format("Wrong number of variables, expected {0}, got {1}",
                                          outputVars.Count,
                                          varsToRecieve));
                    }

                    foreach (var tuple in outputVars.Zip(received, Tuple.Create))
                    {
                        this.Variables[tuple.Item1] = tuple.Item2;
                    }
                }
                else
                {
                    if (varsToRecieve != 0)
                    {
                        throw new ActionException(
                            string.Format("Recieved {0} {1} from subjob, was not expecting any.",
                                          varsToRecieve,
                                          "variable".Pluralize(varsToRecieve)));
                    }
                }
            }
            finally
            {
                try
                {
                    this._serverStream.Close();
                    if (connection != null)
                    {
                        connection.Close();
                    }
                    this._messagePipe.Close();
                    if (!this._process.HasExited)
                    {
                        this._process.CloseMainWindow();
                        this._process.Close();
                        if (!this._process.WaitForExit(10000))
                        {
                            this._process.Kill();
                            this._process = null;
                        }
                    }
                }
                catch (Exception e)
                {
                    this.OnMessage(new MessageEventArgs
                                   {
                                       Message = e.Message,
                                       Level = MessageLevel.Debug
                                   });
                }
            }
        }

        private void BgwOnDoWork(object sender, DoWorkEventArgs e)
        {
            _messagePipe = new NamedPipeServerStream(@"CentipedeMessagePipe", PipeDirection.InOut);
            _messagePipe.WaitForConnection();

            var bgw = (BackgroundWorker) sender;

            try
            {
                while (!bgw.CancellationPending)
                {
                    string actionName;
                    MessageEventArgs messageEventArgs =
                        CentipedeSerializer.DeserializeMessage(_messagePipe,
                                                               out actionName);

                    messageEventArgs.Message = String.Format("Sub job message from {0}: {1}",
                                                             actionName,
                                                             messageEventArgs.Message);

                    OnMessage(messageEventArgs);
                }
            }
            catch (IOException)
            {
                e.Cancel = true;
                bgw.CancelAsync();
            }
        }

        protected override void InitAction()
        {
            base.InitAction();
            GetCurrentCore().JobCompleted -= CloseServerStream;
        }

        protected override void CleanupAction()
        {
            base.CleanupAction();
            this.CloseStreams();
        }

        private void CloseServerStream(object sender, JobCompletedEventArgs jobCompletedEventArgs)
        {
            this.CloseStreams();
        }

        private void CloseStreams()
        {
            if (this._bgw != null)
            {
                this._bgw.CancelAsync();
                this._bgw = null;
            }

            if (this._serverStream != null)
            {
                this._serverStream.Close();
                this._serverStream = null;
            }

            if (this._messagePipe != null)
            {
                this._messagePipe.Close();
                this._messagePipe = null;
            }
        }
    }

    public abstract class SubJobEntryExitPoint : Action
    {
        protected SubJobEntryExitPoint(string name,
                                       IDictionary<string, object> variables,
                                       ICentipedeCore core)
            : base(name, variables, core)
        { }

        protected static SocketStream ClientStream;
    }

    [ActionCategory("Flow Control", DisplayName = "Subjob Entry")]
    public class SubJobEntry : SubJobEntryExitPoint
    {
        protected override void InitAction()
        {
            base.InitAction();
            GetCurrentCore().JobCompleted -= OnJobCompleted;

            _messagePipe = new NamedPipeClientStream(@".",
                                                     @"CentipedeMessagePipe",
                                                     PipeDirection.InOut);

            foreach (var action in GetCurrentCore().Job.Actions)
            {
                action.SetMessageHandler(SubJobMessageHandler);
            }
        }

        private void SubJobMessageHandler(object sender, MessageEventArgs e)
        {
            if (!_messagePipe.IsConnected)
            {
                //MessageBox.Show("a");
                _messagePipe.Connect();
            }
            //MessageBox.Show("b");
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
                CentipedeSerializer.Serialize(ClientStream, false);
            }
            ClientStream.Close();
            _messagePipe.Close();
            System.Windows.Forms.Application.Exit();
        }

        public SubJobEntry(IDictionary<string, object> variables, ICentipedeCore core)
            : base("SubJob Entry", variables, core)
        { }

        [ActionArgument(DisplayName = "Input Variables",
            Usage = "Comma-separated list of variables to set within the subjob",
            Literal = true)]
        public String InputVars = "";

        private NamedPipeClientStream _messagePipe;

        protected override void DoAction()
        {

            var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Connect(IPAddress.Loopback, SubJobAction.PortNumber);
            ClientStream = new SocketStream(socket);
            try
            {

                //recieve vars, if any
                var varsToReceive = CentipedeSerializer.Deserialize<int>(ClientStream);

                var received = new List<object>(varsToReceive);
                for (int i = 0; i < varsToReceive; i++)
                {
                    received.Add(CentipedeSerializer.Deserialize(ClientStream));
                }

                if (!String.IsNullOrWhiteSpace(this.InputVars))
                {

                    var enumerable = InputVars.Split(',').Select(s => s.Trim()).ToList();

                    if (varsToReceive != enumerable.Count)
                    {
                        throw new FatalActionException(
                            string.Format("Wrong number of variables, expected {0}, got {1}",
                                          enumerable.Count,
                                          varsToReceive),
                            this);
                    }

                    foreach (var tuple in enumerable.Zip(received, Tuple.Create))
                    {
                        Variables[tuple.Item1] = tuple.Item2;
                    }
                }
                else
                {
                    if (varsToReceive != 0)
                    {
                        throw new ActionException(
                            string.Format(
                                          "Recieved {0} {1} from parent job, was not expecting any.",
                                          varsToReceive,
                                          "variable".Pluralize(varsToReceive)),
                            this);
                    }
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
            //ClientStream.WaitForPipeDrain();
            try
            {
                if (!String.IsNullOrWhiteSpace(OutVars))
                {
                    var variables = this.OutVars.Split(',').Select(s => s.Trim()).ToList();
                    CentipedeSerializer.Serialize(ClientStream, variables.Count);

                    foreach (var variable in variables)
                    {
                        object o = Variables[variable];
                        try
                        {
                            CentipedeSerializer.Serialize(ClientStream, o);
                        }
                        catch (SerializationException e)
                        {
                            var message =
                                string.Format(
                                              "Cannot send variable {0} to parent job, type {1} is not supported.",
                                              variable,
                                              o.GetType());

                            throw new FatalActionException(message, e, this);
                        }
                    }
                }
                else
                {
                    CentipedeSerializer.Serialize(ClientStream, 0);
                }
            }
            catch (IOException e)
            {
                OnMessage(new MessageEventArgs
                          {
                              Message = string.Format(
                                                      "Sending variables to parent raised IOException: {0}",
                                                      e.Message),
                              Level = MessageLevel.Debug
                          });

            }
            finally
            {
                ClientStream.Close();
            }
        }
    }
}