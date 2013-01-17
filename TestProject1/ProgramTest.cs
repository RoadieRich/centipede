using System.Collections.Generic;
using Centipede;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Action = Centipede.Action;


namespace TestProject1
{
    
    
    /// <summary>
    ///This is a test class for ProgramTest and is intended
    ///to contain all ProgramTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ProgramTest
    {
        private TestContext testContextInstance;
        private readonly Dictionary<string, object> _fakeVarDict = new Dictionary<string, object>();

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        /// <summary>
        ///A test for AddAction
        ///</summary>
        [TestMethod()]
        public void AddActionTest()
        {
            Program.Actions.Clear();
            int addActionCallbackCalls = 0;
            Program.AddActionCallback cb = delegate { addActionCallbackCalls++; };
            Program.ActionAdded += cb;
        
        Action action = new TestAction(_fakeVarDict);
            
            Program.AddAction(action);
            Assert.AreEqual(Program.Actions.Count, 1);
            Assert.AreEqual(addActionCallbackCalls, 1);
            Action action2 = new TestAction(_fakeVarDict);
            Program.AddAction(action2);
            Assert.AreEqual(action.GetNext(), action2, "Didn't set next when adding to end");
            Action action0 = new TestAction(_fakeVarDict);
            Program.AddAction(action0, 0);
            Assert.AreEqual(action0.GetNext(),action, "didn't set next when adding with index");
            Assert.AreEqual(addActionCallbackCalls, 3);

            Program.Actions.Clear();
            Program.ActionAdded -= cb;
        }

        /// <summary>
        ///A test for Clear
        ///</summary>
        [TestMethod()]
        public void ClearTest()
        {
            Program.Actions.Clear();
            TestAction testAction = new TestAction(_fakeVarDict);
            Program.Actions.Add(testAction);

            Program.Clear();

            Assert.AreEqual(Program.Actions.Count, 0);
            
        }

        /// <summary>
        ///A test for Dispose
        ///</summary>
        [TestMethod()]
        public void DisposeTest()
        {
            bool disposing = true;
            TestAction testAction = new TestAction(_fakeVarDict);
            Program.Actions.Add(testAction);
            Program.Dispose(disposing);
            Assert.IsTrue(testAction.DisposeCalled);
        }

        /// <summary>
        ///A test for LoadJob
        ///</summary>
        [TestMethod()]
        public void LoadJobTest()
        {
            string jobFileName = string.Empty; // TODO: Initialize to an appropriate value
            Program.LoadJob(jobFileName);
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
            Program.ActionRemoved += cb;
            
            Program.Actions.Add(action);
            Program.RemoveAction(action);
            Program.ActionRemoved -= cb;
            Assert.AreEqual(Program.Actions.Count, 0);
            Assert.AreEqual(callbackCalls, 1, "Callback wasn't called");

        }

        /// <summary>
        ///Test RunJob Events
        ///</summary>
        [TestMethod()]
        public void RunJobTest()
        {
            TestAction testAction=new TestAction(new Dictionary<string, object>());
            Program.Actions.Add(testAction);
            int beforeCBCalls = 0;
            Program.ActionUpdateCallback beforeCB = delegate(Action action)
                                                        {
                                                            beforeCBCalls++;
                                                            Assert.AreEqual(testAction, action,
                                                                            "Wrong action passed into beforeCB");
                                                        };
            Program.BeforeAction += beforeCB;
            int afterCBCalls = 0;
            Program.ActionUpdateCallback afterCB = delegate(Action action)
                                                       {
                                                           afterCBCalls++;
                                                           Assert.AreEqual(testAction, action,
                                                                           "Wrong action passed into afterCB");
                                                       };
            Program.ActionCompleted += afterCB;

            bool? completedCBArgument = null;
            int completedCBCalls = 0;
            Program.CompletedHandler completedCb = succeeded =>
                                                       {
                                                           completedCBCalls++;
                                                           completedCBArgument = succeeded;
                                                       };

            Program.JobCompleted += completedCb;

            Program.RunJob();

            Assert.IsTrue(testAction.InitActionCalled, "Action not initialised");
            Assert.IsTrue(testAction.DoActionCalled, "Action not done");
            Assert.IsTrue(testAction.CleanupActionCalled, "Action not cleaned up");
            Assert.AreEqual(beforeCBCalls, 1, "beforeCB called {0} times", beforeCBCalls);
            Assert.AreEqual(afterCBCalls, 1, "beforeCB called {0} times", beforeCBCalls);
            Assert.AreEqual(completedCBCalls, 1, "completedCB called {0} times, should be 1", completedCBCalls);
            Assert.IsTrue((bool)completedCBArgument, "completedCB passed incorrect value");

            
            foreach (bool jobShouldContinue in new[] { true, false })
            {
                int errorCBCalls = 0;
                ActionException testException = new ActionException(testAction);
                Program.ErrorHandler errorCb = (ActionException exception, out Action action) =>
                                                   {
                                                       errorCBCalls++;
                                                       Assert.AreEqual(exception, testException,
                                                                       "Unknown exception passed in");
                                                       action = null;
                                                       return jobShouldContinue;
                                                   };

                testAction.TestFunctions = () =>
                                               {
                                                   throw testException;
                                               };
                Program.ActionErrorOccurred += errorCb;
                Program.RunJob();
                Program.ActionErrorOccurred -= errorCb;
                Assert.AreEqual(errorCBCalls, 1, "ErrorCB called {0} times", errorCBCalls);
                Assert.AreEqual(completedCBArgument, jobShouldContinue);
            }
        }

        /// <summary>
        ///A test for SaveJob
        ///</summary>
        [TestMethod()]
        public void SaveJobTest()
        {
            string filename; // TODO: Initialize to an appropriate value
            //Program.SaveJob(filename);

            Assert.Inconclusive("Too difficult to test right now");
        }
    }
}
