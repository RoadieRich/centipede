﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Runtime.Serialization;
using CentipedeInterfaces;
using System.Threading;


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

            this._ServerStream = new NamedPipeServerStream(PipeName, PipeDirection.InOut, 100);

            BackgroundWorker bgw = new BackgroundWorker();
            bgw.DoWork += BgwOnDoWork;

            bgw.RunWorkerAsync();

            try
            {
                Stopwatch timeout = new Stopwatch();
                timeout.Start();
                while (!this._ServerStream.IsConnected)
                {
                    if (timeout.ElapsedMilliseconds > 5000)
                    {
                        throw new FatalActionException(
                            "Subjob did not start in time, or did not have a subjob entry action", this);
                    }
                    Thread.Sleep(10);
                }

                var variables = this.InputVars.Split(',').Select(s => s.Trim()).ToList();

                //send number of variables

                CentipedeSerializer.Serialize(_ServerStream, variables.Count);

                try
                {
                    foreach (var variable in variables)
                    {
                        object o = Variables[variable];
                        try
                        {
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
                }
                catch (IOException e)
                {
                    OnMessage(new MessageEventArgs
                              {
                                  Message = e.Message,
                                  Level = MessageLevel.Debug
                              });
                }




                bool subJobSuccess = (bool)CentipedeSerializer.Deserialize(this._ServerStream);

                if (!subJobSuccess)
                {
                    throw new ActionException("Subjob was not completed", this);
                }

                var outputVars = OutputVars.Split(',').Select(s => s.Trim()).ToList();

                int varsToRecieve = CentipedeSerializer.Deserialize<int>(this._ServerStream);

                if (varsToRecieve != outputVars.Count)
                {
                    throw new FatalActionException(string.Format("Wrong number of varialbes, expected {0}, got {1}",
                                                                 outputVars.Count,
                                                                 varsToRecieve));
                }

                foreach (var outputVar in outputVars)
                {
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
                try
                {
                    this._ServerStream.Close();
                    this._messagePipe.Close();
                    if (!this._process.HasExited)
                    {
                        this._process.CloseMainWindow();
                        this._process.Close();
                        if (!this._process.WaitForExit(10000))
                        {
                            this._process.Kill();
                        }
                    }
                }
                catch (Exception e)
                {
                    OnMessage(new MessageEventArgs
                              {
                                  Message = e.Message,
                                  Level = MessageLevel.Debug
                              });
                }
            }
        }

        private void BgwOnDoWork(object sender, DoWorkEventArgs doWorkEventArgs)
        {
            _messagePipe = new NamedPipeServerStream(@"CentipedeMessagePipe", PipeDirection.InOut);
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

        public override void Dispose()
        {
            this.CleanupAction();
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
            GetCurrentCore().JobCompleted -= OnJobCompleted;
            ////MessageBox.Show("7");
            _messagePipe = new NamedPipeClientStream(@".", @"CentipedeMessagePipe", PipeDirection.InOut);


            foreach (var action in GetCurrentCore().Job.Actions)
            {
                action.MessageHandler += SubJobMessageHandler;

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
                ClientStream.Close();
                _messagePipe.Close();
                System.Windows.Forms.Application.Exit();
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
            ClientStream.Connect();
            try
            {
                int varsToReceive = (int)CentipedeSerializer.Deserialize(ClientStream);
                var enumerable = InputVars.Split(',').Select(s => s.Trim()).ToList();

                if (varsToReceive != enumerable.Count)
                {
                    throw new FatalActionException(string.Format("Wrong number of varialbes, expected {0}, got {1}", enumerable.Count, varsToReceive), this);
                }

                int i = 0;
                foreach (var variable in enumerable)
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

            var variables = this.OutVars.Split(',').Select(s => s.Trim()).ToList();
            CentipedeSerializer.Serialize(ClientStream, variables.Count);
            try
            {
                foreach (var variable in variables)
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
            }
            catch (IOException e)
            {
                OnMessage(new MessageEventArgs
                          {
                              Message = string.Format("Sending variables to parent raised IOException: {0}", e.Message),
                              Level = MessageLevel.Debug
                          });

            }
            finally
            {
                ClientStream.Close();
                System.Windows.Forms.Application.Exit();
            }
        }
    }
}