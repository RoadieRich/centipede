using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Centipede;
using Centipede.Actions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Action = Centipede.Action;


namespace TestProject1
{
    /// <summary>
    ///     This is a test class for ActionDisplayControlTest and is intended
    ///     to contain all ActionDisplayControlTest Unit Tests
    /// </summary>
    [TestClass]
    public class ActionDisplayControlTest
    {
        /*
        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get;
            set;
        }
*/

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
        ///     Check that ADC constructor correctly assigns ThisAction
        /// </summary>
        [TestMethod]
        public void ActionDisplayControlConstructorThisActionTest()
        {
            Action action = new TestAction(new Dictionary<string, object>());
            var target = new ActionDisplayControl(action);
            Assert.AreEqual(action, target.ThisAction);
        }

        /// <summary>
        ///     Check arguments passed to Deleted event listener(s)
        /// </summary>
        [TestMethod]
        [DeploymentItem("Action.dll")]
        public void ActMenuDelete_ClickTest()
        {
            var adc = new ActionDisplayControl(new TestAction(new Dictionary<string, object>()));
            var target = new ActionDisplayControl_Accessor(new PrivateObject(adc));
            object received = null;
            EventArgs e = new CentipedeEventArgs(new List<Action>(), new Dictionary<string, object>());
            int handlerCalled = 0;
            EventArgs receivedArgs = null;

            adc.Deleted += (delegate(object sndr, CentipedeEventArgs cea)
                                {
                                    handlerCalled++;
                                    received = sndr;
                                    receivedArgs = cea;
                                });

            target.ActMenuDelete_Click(null, e);
            Assert.AreEqual(adc, received);
            Assert.AreEqual(1, handlerCalled);
            Assert.AreEqual(e, receivedArgs);
        }

        /// <summary>
        ///     A test for CommentTextBox_TextChanged
        /// </summary>
        [TestMethod]
        [DeploymentItem("Action.dll")]
        public void CommentTextBox_TextChangedTest()
        {
            var target =
                    new ActionDisplayControl_Accessor(new TestAction(new Dictionary<string, object>()));
            String newComment = TestHelpers.RandomString(12);
            var sender = new TextBox
                         {
                                 Text = newComment
                         };

            target.CommentTextBox_TextChanged(sender, null);
            Assert.AreEqual(newComment, target.ThisAction.Comment);
        }

        /// <summary>
        ///     A test for Dispose
        /// </summary>
        [TestMethod]
        [DeploymentItem("Action.dll")]
        public void DisposeTest()
        {
            var action = new TestAction(new Dictionary<string, object>());
            var target = new ActionDisplayControl_Accessor(action);
            target.Dispose(false);
            Assert.AreEqual(0, action.DisposeCalled);
            target.Dispose(true);
            Assert.AreEqual(1, action.DisposeCalled);
        }

        /// <summary>
        ///     A test for ExpandButton_Click
        /// </summary>
        [TestMethod]
        [DeploymentItem("Action.dll")]
        public void ExpandButton_ClickTest()
        {
            var target = new ActionDisplayControl_Accessor();
            int visibilityChanged = 0;
            target.AttributeTable.VisibleChanged += delegate { visibilityChanged++; };
            bool currentVisibility = target.AttributeTable.Visible;
            target.ExpandButton_Click(null, null);
            Assert.AreEqual(1, visibilityChanged);
            Assert.AreEqual(!currentVisibility, target.AttributeTable.Visible);
            target.ExpandButton_Click(null, null);
            Assert.AreEqual(2, visibilityChanged);
            Assert.AreEqual(currentVisibility, target.AttributeTable.Visible);
        }

        /// <summary>
        ///     A test for GenerateArguments
        /// </summary>
        [TestMethod]
        [DeploymentItem("Action.dll")]
        public void GenerateArgumentsTest()
        {
            var target =
                    new ActionDisplayControl_Accessor(new TestAction(new Dictionary<string, object>()));

            Assert.AreEqual(4, target.AttributeTable.Controls.Count);
        }

        /// <summary>
        ///     A test for GenerateFieldControls
        /// </summary>
        [TestMethod]
        [DeploymentItem("Action.dll")]
        public void GenerateFieldControlsTest()
        {
            var action = new TestAction(new Dictionary<string, object>());
            var target = new ActionDisplayControl_Accessor(action);
            var arg =
                    new FieldAndPropertyWrapper(action.GetType().GetField("FieldArgument"));

            Control[] actual = target.GenerateFieldControls(arg);
            Assert.AreEqual(2, actual.Length);
            Assert.IsInstanceOfType(actual[0], typeof (Label));
            Assert.IsInstanceOfType(actual[1], typeof (TextBox));
            var label = actual[0] as Label;
            var textBox = actual[1] as TextBox;
            Assert.AreEqual("field argument value", textBox.Text);
            Assert.AreEqual("Field Argument", label.Text);
        }

        /// <summary>
        ///     A test for GetArgumentName
        /// </summary>
        [TestMethod]
        [DeploymentItem("Action.dll")]
        public void GetArgumentNameTest()
        {
            var action = new TestAction(new Dictionary<string, object>());
            var target = new ActionDisplayControl_Accessor(action);
            var argument =
                    new FieldAndPropertyWrapper(typeof (TestAction).GetField("FieldArgument"));

            //string actual = target.GetArgumentName(argument);

            //Assert.AreEqual("Field Argument", actual);
        }

        private class ActionWithChangedHandler : Action
        {
            // ReSharper disable UnusedMember.Local

            public ActionWithChangedHandler()
                    : base("", new Dictionary<string, object>())
            { }

            /// <summary>
            ///     Perform the action
            /// </summary>
            protected override void DoAction()
            { }

            [ActionArgument(onChangedHandlerName = "Changed")]
            public string Argument = "";

            public object SenderReceived;
            public EventArgs EventArgs;

            public void Changed(object sender, EventArgs e)
            {
                SenderReceived = sender;
                EventArgs = e;
            }

            // ReSharper restore UnusedMember.Local
        }

        /// <summary>
        ///     A test for GetChangedHandler
        /// </summary>
        [TestMethod]
        [DeploymentItem("Action.dll")]
        public void GetChangedHandlerTest()
        {
            var action = new ActionWithChangedHandler();
            var target = new ActionDisplayControl_Accessor(action);
            FieldAndPropertyWrapper arg = typeof (ActionWithChangedHandler).GetField("Argument");

            EventHandler handler = target.GetChangedHandler(arg);

            var sender = new object();
            var e = new EventArgs();

            handler(sender, e);

            Assert.AreSame(sender, action.SenderReceived);
            Assert.AreSame(e, action.EventArgs);
        }

        private class ActionWithLeaveHandler : Action
        {
            // ReSharper disable UnusedMember.Local
            public ActionWithLeaveHandler()
                    : base("", new Dictionary<string, object>())
            { }

            /// <summary>
            ///     Perform the action
            /// </summary>
            protected override void DoAction()
            { }

            [ActionArgument(onLeaveHandlerName = "Leaving")]
            public string Argument = "";

            public object SenderReceived;
            public EventArgs EventArgs;

            public void Leaving(object sender, EventArgs e)
            {
                SenderReceived = sender;
                EventArgs = e;
            }

            // ReSharper restore UnusedMember.Local
        }

        /// <summary>
        ///     A test for GetLeaveHandler
        /// </summary>
        [TestMethod]
        [DeploymentItem("Action.dll")]
        public void GetLeaveHandlerTest()
        {
            var action = new ActionWithLeaveHandler();
            var target = new ActionDisplayControl_Accessor(action);
            FieldAndPropertyWrapper arg = action.GetType().GetField("Argument");

            EventHandler handler = target.GetLeaveHandler(arg);

            var sender = new object();
            var e = new EventArgs();

            handler(sender, e);

            Assert.AreSame(sender, action.SenderReceived);
            Assert.AreSame(e, action.EventArgs);
        }
    }
}
