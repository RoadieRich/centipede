using Centipede.Actions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Centipede;
using System.Windows.Forms;
using System.Drawing;

namespace CentipedeTest
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
Action action = null; // TODO: Initialize to an appropriate value
    ActionDisplayControl target = new ActionDisplayControl(action);
    Assert.Inconclusive("TODO: Implement code to verify target");
}

/// <summary>
///A test for ActionDisplayControl Constructor
///</summary>
[TestMethod()]
[DeploymentItem("Action.dll")]
public void ActionDisplayControlConstructorTest1()
{
    ActionDisplayControl_Accessor target = new ActionDisplayControl_Accessor();
    Assert.Inconclusive("TODO: Implement code to verify target");
}

/// <summary>
///A test for ActMenuDelete_Click
///</summary>
[TestMethod()]
[DeploymentItem("Action.dll")]
public void ActMenuDelete_ClickTest()
{
ActionDisplayControl_Accessor target = new ActionDisplayControl_Accessor(); // TODO: Initialize to an appropriate value
object sender = null; // TODO: Initialize to an appropriate value
EventArgs e = null; // TODO: Initialize to an appropriate value
    target.ActMenuDelete_Click(sender, e);
    Assert.Inconclusive("A method that does not return a value cannot be verified.");
}

/// <summary>
///A test for CommentTextBox_TextChanged
///</summary>
[TestMethod()]
[DeploymentItem("Action.dll")]
public void CommentTextBox_TextChangedTest()
{
ActionDisplayControl_Accessor target = new ActionDisplayControl_Accessor(); // TODO: Initialize to an appropriate value
object sender = null; // TODO: Initialize to an appropriate value
EventArgs e = null; // TODO: Initialize to an appropriate value
    target.CommentTextBox_TextChanged(sender, e);
    Assert.Inconclusive("A method that does not return a value cannot be verified.");
}

/// <summary>
///A test for Dispose
///</summary>
[TestMethod()]
[DeploymentItem("Action.dll")]
public void DisposeTest()
{
ActionDisplayControl_Accessor target = new ActionDisplayControl_Accessor(); // TODO: Initialize to an appropriate value
bool disposing = false; // TODO: Initialize to an appropriate value
    target.Dispose(disposing);
    Assert.Inconclusive("A method that does not return a value cannot be verified.");
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
    target.ExpandButton_Click(sender, e);
    Assert.Inconclusive("A method that does not return a value cannot be verified.");
}

/// <summary>
///A test for GenerateArguments
///</summary>
[TestMethod()]
[DeploymentItem("Action.dll")]
public void GenerateArgumentsTest()
{
ActionDisplayControl_Accessor target = new ActionDisplayControl_Accessor(); // TODO: Initialize to an appropriate value
    target.GenerateArguments();
    Assert.Inconclusive("A method that does not return a value cannot be verified.");
}

/// <summary>
///A test for GenerateFieldControls
///</summary>
[TestMethod()]
[DeploymentItem("Action.dll")]
public void GenerateFieldControlsTest()
{
ActionDisplayControl_Accessor target = new ActionDisplayControl_Accessor(); // TODO: Initialize to an appropriate value
FieldAndPropertyWrapper_Accessor arg = null; // TODO: Initialize to an appropriate value
Control[] expected = null; // TODO: Initialize to an appropriate value
    Control[] actual;
    actual = target.GenerateFieldControls(arg);
    Assert.AreEqual(expected, actual);
    Assert.Inconclusive("Verify the correctness of this test method.");
}

/// <summary>
///A test for GetArgumentName
///</summary>
[TestMethod()]
[DeploymentItem("Action.dll")]
public void GetArgumentNameTest()
{
ActionDisplayControl_Accessor target = new ActionDisplayControl_Accessor(); // TODO: Initialize to an appropriate value
FieldAndPropertyWrapper_Accessor argument = null; // TODO: Initialize to an appropriate value
string expected = string.Empty; // TODO: Initialize to an appropriate value
    string actual;
    actual = target.GetArgumentName(argument);
    Assert.AreEqual(expected, actual);
    Assert.Inconclusive("Verify the correctness of this test method.");
}

/// <summary>
///A test for GetChangedHandler
///</summary>
[TestMethod()]
[DeploymentItem("Action.dll")]
public void GetChangedHandlerTest()
{
ActionDisplayControl_Accessor target = new ActionDisplayControl_Accessor(); // TODO: Initialize to an appropriate value
FieldAndPropertyWrapper_Accessor arg = null; // TODO: Initialize to an appropriate value
EventHandler expected = null; // TODO: Initialize to an appropriate value
    EventHandler actual;
    actual = target.GetChangedHandler(arg);
    Assert.AreEqual(expected, actual);
    Assert.Inconclusive("Verify the correctness of this test method.");
}

/// <summary>
///A test for GetLeaveHandler
///</summary>
[TestMethod()]
[DeploymentItem("Action.dll")]
public void GetLeaveHandlerTest()
{
ActionDisplayControl_Accessor target = new ActionDisplayControl_Accessor(); // TODO: Initialize to an appropriate value
FieldAndPropertyWrapper_Accessor arg = null; // TODO: Initialize to an appropriate value
EventHandler expected = null; // TODO: Initialize to an appropriate value
    EventHandler actual;
    actual = target.GetLeaveHandler(arg);
    Assert.AreEqual(expected, actual);
    Assert.Inconclusive("Verify the correctness of this test method.");
}

/// <summary>
///A test for InitializeComponent
///</summary>
[TestMethod()]
[DeploymentItem("Action.dll")]
public void InitializeComponentTest()
{
ActionDisplayControl_Accessor target = new ActionDisplayControl_Accessor(); // TODO: Initialize to an appropriate value
    target.InitializeComponent();
    Assert.Inconclusive("A method that does not return a value cannot be verified.");
}

/// <summary>
///A test for SetProperties
///</summary>
[TestMethod()]
[DeploymentItem("Action.dll")]
public void SetPropertiesTest()
{
ActionDisplayControl_Accessor target = new ActionDisplayControl_Accessor(); // TODO: Initialize to an appropriate value
    target.SetProperties();
    Assert.Inconclusive("A method that does not return a value cannot be verified.");
}

/// <summary>
///A test for attrValue_TextChanged
///</summary>
[TestMethod()]
[DeploymentItem("Action.dll")]
public void attrValue_TextChangedTest()
{
ActionDisplayControl_Accessor target = new ActionDisplayControl_Accessor(); // TODO: Initialize to an appropriate value
object sender = null; // TODO: Initialize to an appropriate value
EventArgs e = null; // TODO: Initialize to an appropriate value
    target.attrValue_TextChanged(sender, e);
    Assert.Inconclusive("A method that does not return a value cannot be verified.");
}

/// <summary>
///A test for BackColor
///</summary>
[TestMethod()]
[DeploymentItem("Action.dll")]
public void BackColorTest()
{
ActionDisplayControl_Accessor target = new ActionDisplayControl_Accessor(); // TODO: Initialize to an appropriate value
Color expected = new Color(); // TODO: Initialize to an appropriate value
    target.BackColor = expected;
    Assert.Inconclusive("Write-only properties cannot be verified.");
}

/// <summary>
///A test for Selected
///</summary>
[TestMethod()]
[DeploymentItem("Action.dll")]
public void SelectedTest()
{
ActionDisplayControl_Accessor target = new ActionDisplayControl_Accessor(); // TODO: Initialize to an appropriate value
bool expected = false; // TODO: Initialize to an appropriate value
    bool actual;
    target.Selected = expected;
    actual = target.Selected;
    Assert.AreEqual(expected, actual);
    Assert.Inconclusive("Verify the correctness of this test method.");
}

/// <summary>
///A test for State
///</summary>
[TestMethod()]
public void StateTest()
{
ActionDisplayControl target = new ActionDisplayControl(); // TODO: Initialize to an appropriate value
ActionState expected = new ActionState(); // TODO: Initialize to an appropriate value
    ActionState actual;
    target.State = expected;
    actual = target.State;
    Assert.AreEqual(expected, actual);
    Assert.Inconclusive("Verify the correctness of this test method.");
}

/// <summary>
///A test for StatusMessage
///</summary>
[TestMethod()]
public void StatusMessageTest()
{
ActionDisplayControl target = new ActionDisplayControl(); // TODO: Initialize to an appropriate value
string expected = string.Empty; // TODO: Initialize to an appropriate value
    target.StatusMessage = expected;
    Assert.Inconclusive("Write-only properties cannot be verified.");
}

/// <summary>
///A test for ThisAction
///</summary>
[TestMethod()]
[DeploymentItem("Action.dll")]
public void ThisActionTest()
{
ActionDisplayControl_Accessor target = new ActionDisplayControl_Accessor(); // TODO: Initialize to an appropriate value
Action expected = null; // TODO: Initialize to an appropriate value
    Action actual;
    target.ThisAction = expected;
    actual = target.ThisAction;
    Assert.AreEqual(expected, actual);
    Assert.Inconclusive("Verify the correctness of this test method.");
}
    }
}
