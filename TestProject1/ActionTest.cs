using Centipede;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using Action = Centipede.Action;


namespace TestProject1
{
    
    
    /// <summary>
    ///This is a test class for ActionTest and is intended
    ///to contain all ActionTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ActionTest
    {
        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get;
            set;
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
        ///A test for AddToXmlElement
        ///</summary>
        [TestMethod()]
        public void AddToXmlElementTest()
        {
            Assert.Inconclusive("You want me to test what?");
        }

        //internal virtual Action_Accessor CreateAction_Accessor()
        //{
        //    // TODO: Instantiate an appropriate concrete class.
        //    //Action_Accessor target = 
        //    //return target;
        //}

        /// <summary>
        ///A test for Ask
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Action.dll")]
        public void AskTest()
        {
            TestAction action = new TestAction(new Dictionary<string, object>());

            String message = TestHelpers.RandomString(100);
            String title = TestHelpers.RandomString(10);
            AskEventEnums.AskType askType = (AskEventEnums.AskType)TestHelpers.RandomInt;
            AskEventEnums.DialogResult actual = (AskEventEnums.DialogResult)(-1),
                                       expected = (AskEventEnums.DialogResult)Math.Abs(TestHelpers.RandomInt);

            Action.AskEvent actionOnOnAsk = (sender, e) =>
                                                {
                                                    Action thisAction = action;
                                                    Assert.AreEqual(askType, e.Type);
                                                    Assert.AreEqual(message, e.Message);
                                                    Assert.AreEqual(title, e.Title);
                                                    e.Result = expected;
                                                };
            action.OnAsk += actionOnOnAsk;

            
            //protected AskEventEnums.DialogResult Ask(String message,
            //String title = "Question", 
            //AskEventEnums.AskType options = AskEventEnums.AskType.YesNoCancel)
            action.TestFunctions = () =>
                                       {
                                           Action thisAction = action;
                                           PrivateObject po = new PrivateObject(thisAction);

                                           actual =
                                                   (AskEventEnums.DialogResult)po.Invoke("Ask", message, title, askType);
                                       };

            action.Run();
            action.OnAsk -= actionOnOnAsk;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Dispose
        ///</summary>
        [TestMethod()]
        public void DisposeTest()
        {
            Action target = new TestAction(null); // TODO: Initialize to an appropriate value
            target.Dispose();
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for FromXml
        ///</summary>
        [TestMethod()]
        public void FromXmlTest()
        {
            Assert.Inconclusive("Do what now?");
        }

        /// <summary>
        ///A test for GetNext
        ///</summary>
        [TestMethod()]
        public void GetNextTest()
        {
            Action target = new TestAction(null); // TODO: Initialize to an appropriate value
            Action expected = new TestAction(null); // TODO: Initialize to an appropriate value

            target.Next = expected;

            Action actual = target.GetNext();
            Assert.AreSame(expected, actual);
            
        }

        /// <summary>
        ///A test for Message
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Action.dll")]
        public void MessageTest()
        {
            PrivateObject param0 = null; // TODO: Initialize to an appropriate value
            Action_Accessor target = new Action_Accessor(param0); // TODO: Initialize to an appropriate value
            string message = string.Empty; // TODO: Initialize to an appropriate value
            string title = string.Empty; // TODO: Initialize to an appropriate value
            const AskEventEnums.MessageIcon messageIcon = new AskEventEnums.MessageIcon(); // TODO: Initialize to an appropriate value
            //target.Message(message, title, messageIcon);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for ParseStringForVariable
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Action.dll")]
        public void ParseStringForVariableTest()
        {
            //Action_Accessor target = new Action_Accessor(param0); // TODO: Initialize to an appropriate value
            string str = string.Empty; // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
          //  actual = target.ParseStringForVariable(str);
            //Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for Run
        ///</summary>
        [TestMethod()]
        public void RunTest()
        {
            TestAction action = new TestAction(new Dictionary<string, object>());
            Action target = action;
            target.Run();
            Assert.AreEqual(1, action.InitActionCalled);
            Assert.AreEqual(1, action.DoActionCalled);
            Assert.AreEqual(1, action.CleanupActionCalled);
        }
    }
}
