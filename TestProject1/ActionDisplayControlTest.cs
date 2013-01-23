using System.Collections.Generic;
using Centipede;
using Centipede.Actions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Windows.Forms;
using Action = Centipede.Action;


namespace TestProject1
{
    
    
    /// <summary>
    ///This is a test class for ActionDisplayControlTest and is intended
    ///to contain all ActionDisplayControlTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ActionDisplayControlTest
    {


        private TestContext testContextInstance;

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
        ///A test for ActionDisplayControl Constructor
        ///</summary>
        [TestMethod()]
        public void ActionDisplayControlConstructorTest()
        {
            Action action = new TestAction(new Dictionary<string, object>());
            ActionDisplayControl target = new ActionDisplayControl(action);
            PrivateObject targetPrivate = new PrivateObject(target);
            Assert.AreEqual(action, target.ThisAction);
        }

        /// <summary>
        ///A test for ActMenuDelete_Click
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Action.dll")]
        public void ActMenuDelete_ClickTest()
        {
            ActionDisplayControl adc = new ActionDisplayControl(new TestAction(new Dictionary<string, object>()));
            ActionDisplayControl_Accessor target = new ActionDisplayControl_Accessor(new PrivateObject(adc));
            object sender = null, received = null;
            EventArgs e = new CentipedeEventArgs(null, new List<Action>(),new Dictionary<string, object>() );
            int handlerCalled = 0;


            adc.Deleted += (delegate(object sndr, CentipedeEventArgs cea)
                                   {
                                       handlerCalled++;
                                       received = sndr;
                                   });
            target.ActMenuDelete_Click(sender, e);
            Assert.AreEqual(adc, received);
            Assert.AreEqual(1, handlerCalled);
        }

        /// <summary>
        ///A test for CommentTextBox_TextChanged
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Action.dll")]
        public void CommentTextBox_TextChangedTest()
        {
            ActionDisplayControl_Accessor target = new ActionDisplayControl_Accessor(new TestAction(new Dictionary<string, object>())); // TODO: Initialize to an appropriate value
            EventArgs e = null; // TODO: Initialize to an appropriate value
            String newComment = @"test";
            TextBox sender = new TextBox
                             {
                                     Text = newComment
                             };
            
            target.CommentTextBox_TextChanged(sender, e);
            Assert.AreEqual(newComment, target.ThisAction.Comment);
            
        }

        /// <summary>
        ///A test for Dispose
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Action.dll")]
        public void DisposeTest()
        {
            TestAction action = new TestAction(new Dictionary<string, object>());
            ActionDisplayControl_Accessor target = new ActionDisplayControl_Accessor(action);
            target.Dispose(false);
            Assert.AreEqual(0, action.DisposeCalled);
            target.Dispose(true);
            Assert.AreEqual(1, action.DisposeCalled);
        }

        /// <summary>
        ///A test for ExpandButton_Click
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Action.dll")]
        public void ExpandButton_ClickTest()
        {
            ActionDisplayControl_Accessor target = new ActionDisplayControl_Accessor(); // TODO: Initialize to an appropriate value
            object sender = null; // TODO: Initialize to an appropriate value
            EventArgs e = null; // TODO: Initialize to an appropriate value
            int visibilityChanged = 0;
            target.AttributeTable.VisibleChanged += delegate { visibilityChanged++; };
            bool currentVisibility = target.AttributeTable.Visible;
            target.ExpandButton_Click(sender, e);
            Assert.AreEqual(1, visibilityChanged);
            Assert.AreEqual(!currentVisibility, target.AttributeTable.Visible);
            target.ExpandButton_Click(sender, e);
            Assert.AreEqual(2, visibilityChanged);
            Assert.AreEqual(currentVisibility, target.AttributeTable.Visible);
        }

        /// <summary>
        ///A test for GenerateArguments
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Action.dll")]
        public void GenerateArgumentsTest()
        {
            ActionDisplayControl_Accessor target = new ActionDisplayControl_Accessor(new TestAction(new Dictionary<string, object>())); // TODO: Initialize to an appropriate value
            
            Assert.AreEqual(4, target.AttributeTable.Controls.Count);
        }

        /// <summary>
        ///A test for GenerateFieldControls
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Action.dll")]
        public void GenerateFieldControlsTest()
        {
            TestAction action = new TestAction(new Dictionary<string, object>());
            ActionDisplayControl_Accessor target = new ActionDisplayControl_Accessor(action); // TODO: Initialize to an appropriate value
            FieldAndPropertyWrapper_Accessor arg = new FieldAndPropertyWrapper_Accessor(action.GetType().GetField("FieldArgument"));
            
            Control[] actual;
            actual = target.GenerateFieldControls(arg);
            Assert.AreEqual(2, actual.Length);
            Assert.IsInstanceOfType(actual[0], typeof(Label));
            Assert.IsInstanceOfType(actual[1], typeof(TextBox));
            Label label = actual[0] as Label;
            TextBox textBox = actual[1] as TextBox;
            Assert.AreEqual("field argument value", textBox.Text);

        }

        /// <summary>
        ///A test for GetArgumentName
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Action.dll")]
        public void GetArgumentNameTest()
        {
            TestAction action = new TestAction(new Dictionary<string, object>());
            ActionDisplayControl_Accessor target = new ActionDisplayControl_Accessor(action); // TODO: Initialize to an appropriate value
            FieldAndPropertyWrapper_Accessor argument = new FieldAndPropertyWrapper_Accessor(typeof(TestAction).GetField("FieldArgument")); // TODO: Initialize to an appropriate value
            
            string actual = target.GetArgumentName(argument);
            
            Assert.AreEqual("Field Argument", actual);
        }

        class ActionWithChangedHandler : Action
        {
            public ActionWithChangedHandler()
                    : base("", new Dictionary<string, object>())
            { }

            /// <summary>
            /// Perform the action
            /// </summary>
            protected override void DoAction()
            { }

            [ActionArgument(onChangedHandlerName = "changed")]
            public string Argument = "";

            public object senderReceived;
            public EventArgs EventArgs;

            public void changed(object sender, EventArgs e)
            {
                this.senderReceived = sender;
                this.EventArgs = e;
            }
        }

        /// <summary>
        ///A test for GetChangedHandler
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Action.dll")]
        public void GetChangedHandlerTest()
        {
            ActionWithChangedHandler action = new ActionWithChangedHandler();
            ActionDisplayControl_Accessor target = new ActionDisplayControl_Accessor(action); // TODO: Initialize to an appropriate value
            FieldAndPropertyWrapper_Accessor arg = typeof (ActionWithChangedHandler).GetField("Argument");

            EventHandler handler = target.GetChangedHandler(arg);
            
            object sender = new object();
            EventArgs e = new EventArgs();

            handler(sender, e);

            Assert.AreSame(sender, action.senderReceived);
            Assert.AreSame(e, action.EventArgs);
        }

        class ActionWithLeaveHandler : Action
        {
            public ActionWithLeaveHandler()
                : base("", new Dictionary<string, object>())
            { }

            /// <summary>
            /// Perform the action
            /// </summary>
            protected override void DoAction()
            { }

            [ActionArgument(onLeaveHandlerName = "changed")]
            public string Argument = "";

            public object senderReceived;
            public EventArgs EventArgs;

            public void changed(object sender, EventArgs e)
            {
                this.senderReceived = sender;
                this.EventArgs = e;
            }
        }

        /// <summary>
        ///A test for GetLeaveHandler
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Action.dll")]
        public void GetLeaveHandlerTest()
        {
            ActionWithLeaveHandler action = new ActionWithLeaveHandler();
            ActionDisplayControl_Accessor target = new ActionDisplayControl_Accessor(action); // TODO: Initialize to an appropriate value
            FieldAndPropertyWrapper_Accessor arg = action.GetType().GetField("Argument");

            EventHandler handler = target.GetLeaveHandler(arg);

            object sender = new object();
            EventArgs e = new EventArgs();

            handler(sender, e);

            Assert.AreSame(sender, action.senderReceived);
            Assert.AreSame(e, action.EventArgs);
        }

    }
}
