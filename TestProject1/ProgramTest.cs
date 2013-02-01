using System.Collections.Generic;
using Centipede;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Rhino.Mocks;
using Action = Centipede.Action;

#pragma warning disable 1591

namespace TestProject1
{


    /// <summary>
    ///This is a test class for ProgramTest and is intended
    ///to contain all ProgramTest Unit Tests
    ///</summary>
    [TestClass]
    public class ProgramTest
    {
        ///<summary>Use TestInitialize to run code before running each test</summary>
        [TestInitialize]
        public void MyTestInitialize()
        {
            _mocks = new MockRepository();

            Program.Instance.Actions.Clear();
            Program.Instance.Variables.Clear();

        }

        [TestCleanup]
        public void MyTestCleanup()
        {
            _mocks = null;
        }

        public interface IActionAddedListener
        {
            void Handler(Action action, int index);
        }



        /// <summary>
        /// Check <see cref="Program.AddAction" /> adds an action to the actions list.
        /// </summary>
        [TestMethod]
        public void TestAddAction()
        {
            Action mockAction = _mocks.DynamicMock<Action>("", null);

            Program prog = _mocks.PartialMock<Program>();

            prog.AddAction(mockAction);

            Assert.AreEqual(1, prog.Actions.Count);

            prog.AddAction(mockAction);

            Assert.AreEqual(2, prog.Actions.Count);

            try
            {
                prog.AddAction(mockAction, int.MaxValue);
                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException)
            {
                //pass
            }

            Assert.AreNotEqual(3, Program.Instance.Actions.Count);
        }

        /// <summary>
        /// Check ActionAdded event is raised with appropriate indexes
        /// </summary>
        [TestMethod]
        public void TestActionAddedIsRaised()
        {
            IActionAddedListener actionAddedListener = _mocks.DynamicMock<IActionAddedListener>();

            Program.Instance.ActionAdded += actionAddedListener.Handler;

            Action actionMock = _mocks.Stub<Action>("mock", Program.Instance.Variables);
            Action actionMock2 = _mocks.Stub<Action>("mock", null);
            Action actionMock3 = _mocks.Stub<Action>("mock", null);

            actionAddedListener.Handler(actionMock, 0);
            actionAddedListener.Handler(actionMock2, 0);
            actionAddedListener.Handler(actionMock3, 2);
            //subscriber.Handler(mocks.Stub<Action>("mock", null), 10);

            _mocks.ReplayAll();

            Program.Instance.AddAction(actionMock);
            Program.Instance.AddAction(actionMock2, 0);
            Program.Instance.AddAction(actionMock3);

            _mocks.VerifyAll();

        }

        [TestMethod]
        public void TestAddActionSetsActionNext()
        {
            Action actionMock = _mocks.Stub<Action>("mock", Program.Instance.Variables);
            Action actionMock2 = _mocks.Stub<Action>("mock", Program.Instance.Variables);

            IActionAddedListener actionAddedListener = _mocks.StrictMock<IActionAddedListener>();

            Program.Instance.ActionAdded += actionAddedListener.Handler;
            actionAddedListener.Stub(sub => sub.Handler(null, 0)).IgnoreArguments();

            _mocks.ReplayAll();

            Program.Instance.AddAction(actionMock);
            Program.Instance.AddAction(actionMock2);


            Assert.AreEqual(actionMock2, actionMock.Next);

            _mocks.VerifyAll();


        }

        public interface IRemoveListener
        {
            void Handler(Action action);
        }

        /// <summary>
        ///Check Remove is called for all actions Clear
        ///</summary>
        [TestMethod()]
        public void ClearTest()
        {   
            Action mockAction = _mocks.DynamicMock<Action>("", null);
            Action mockAction2 = _mocks.DynamicMock<Action>("", null);
            
            _mocks.ReplayAll();

            Program.Instance.AddAction(mockAction);
            Program.Instance.AddAction(mockAction2);

            Assert.AreEqual(Program.Instance.Actions.Count, 2); //sanity check
            
            Program.Instance.Clear();

            Assert.AreEqual(Program.Instance.Actions.Count, 0);

            _mocks.VerifyAll();
        }

        private MockRepository _mocks;

        /// <summary>
        ///A test for Dispose
        ///</summary>
        [TestMethod()]
        public void DisposeTest()
        {
            Action mockAction = _mocks.DynamicMock<Action>("", null);
            mockAction.Expect(a => a.Dispose());
            Program.Instance.Actions.Add(mockAction);

            _mocks.ReplayAll();

            Program.Instance.Dispose();

            _mocks.VerifyAll();
        }

        /// <summary>
        ///A test for LoadJob
        ///</summary>
        [TestMethod()]
        public void LoadJobTest()
        {
            string jobFileName = string.Empty; // TODO: Initialize to an appropriate value
            Program.Instance.LoadJob(jobFileName);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for RemoveAction
        ///</summary>
        [TestMethod()]
        public void RemoveActionTest()
        {
            Action action = new TestAction(new Dictionary<string, object>()); // TODO: Initialize to an appropriate value
            int callbackCalls =0;
            Program.ActionRemovedHandler cb = action1 =>
                                                  {
                                                      Assert.AreEqual(action1, action,
                                                                      "Unknown action passed into handler");
                                                      callbackCalls++;
                                                  };
            Program.Instance.ActionRemoved += cb;
            
            Program.Instance.Actions.Add(action);
            Program.Instance.RemoveAction(action);
            Program.Instance.ActionRemoved -= cb;
            Assert.AreEqual(Program.Instance.Actions.Count, 0);
            Assert.AreEqual(callbackCalls, 1, "Callback wasn't called");

        }

        /// <summary>
        ///Test RunJob Events
        ///</summary>
        [TestMethod()]
        public void RunJobTest()
        {
            TestAction testAction = new TestAction(new Dictionary<string, object>());
            Program.Instance.Actions.Add(testAction);
            int beforeCBCalls = 0;
            Program.ActionUpdateCallback beforeCB = delegate(Action action)
                                                        {
                                                            beforeCBCalls++;
                                                            Assert.AreEqual(testAction, action,
                                                                            "Wrong action passed into beforeCB");
                                                        };
            Program.Instance.BeforeAction += beforeCB;
            int afterCBCalls = 0;
            Program.ActionUpdateCallback afterCB = delegate(Action action)
                                                       {
                                                           afterCBCalls++;
                                                           Assert.AreEqual(testAction, action,
                                                                           "Wrong action passed into afterCB");
                                                       };
            Program.Instance.ActionCompleted += afterCB;

            bool? completedCBArgument = null;
            int completedCBCalls = 0;
            Program.CompletedHandler completedCb = succeeded =>
                                                       {
                                                           completedCBCalls++;
                                                           completedCBArgument = succeeded;
                                                       };

            Program.Instance.JobCompleted += completedCb;

            Program.Instance.RunJob();

            Assert.AreEqual(testAction.DoActionCalled, 1, "DoAction called {0} times, should be 1.",
                            testAction.DoActionCalled);
            Assert.AreEqual(testAction.GetNextCalled, 1, "GetNext called {0} times, should be 1.",
                            testAction.GetNextCalled);
            Assert.AreEqual(beforeCBCalls, 1, "beforeCB called {0} times, should be 1", beforeCBCalls);
            Assert.AreEqual(afterCBCalls, 1, "beforeCB called {0} times, should be 1", beforeCBCalls);
            Assert.AreEqual(completedCBCalls, 1, "completedCB called {0} times, should be 1", completedCBCalls);
            Assert.IsTrue((bool)completedCBArgument, "completedCB passed incorrect value");

            Program.ErrorHandler errorCb;
            foreach (bool jobShouldContinue in new[] { true, false })
            {
                int errorCBCalls = 0;
                ActionException testException = new ActionException(testAction);
                bool ret = jobShouldContinue;
                errorCb = (ActionException exception, out Action action) =>
                                                   {
                                                       errorCBCalls++;
                                                       Assert.AreEqual(exception, testException,
                                                                       "Unknown exception passed in");
                                                       action = null;
                                                       return ret;
                                                   };

                testAction.TestFunctions = () =>
                                               {
                                                   throw testException;
                                               };
                Program.Instance.ActionErrorOccurred += errorCb;
                Program.Instance.RunJob();
                Program.Instance.ActionErrorOccurred -= errorCb;
                Assert.AreEqual(errorCBCalls, 1, "ErrorCB called {0} times", errorCBCalls);
                Assert.AreEqual(completedCBArgument, jobShouldContinue);
            }
            errorCb = (ActionException exception, out Action action) =>
                                               {
                                                   Assert.IsInstanceOfType(exception, typeof (ActionException),
                                                                           "Unknown exception type passed into error handler");
                                                   Assert.AreEqual(exception.ErrorAction, testAction,
                                                                   "Exception ErrorAction not set correctly");
                                                   action = null;
                                                   return true;
                                               };

            testAction.TestFunctions = () =>
                                           {
                                               throw new Exception();
                                           };
            Program.Instance.ActionErrorOccurred += errorCb;
            Program.Instance.RunJob();
            Program.Instance.ActionErrorOccurred -= errorCb;
            

        }

        /// <summary>
        ///A test for SaveJob
        ///</summary>
        [TestMethod()]
        public void SaveJobTest()
        {
            //string filename; // TODO: Initialize to an appropriate value
            //Program.Instance.SaveJob(filename);

            Assert.Inconclusive("Too difficult to test right now");
        }
    }
}
