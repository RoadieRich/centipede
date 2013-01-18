using Centipede;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Xml;
using System.Collections.Generic;
using Action = Centipede.Action;


namespace CentipedeTest
{
    
    
    /// <summary>
    ///This is a test class for ActionTest and is intended
    ///to contain all ActionTest Unit Tests
    ///</summary>
    [TestClass]
    public class ActionTest
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


        class ConcreteAction : Action
        {
            /// <summary>
            /// 
            /// </summary>
            /// <param name="name"></param>
            /// <param name="v"></param>
            public ConcreteAction(Dictionary<string, object> v)
                    : base("Concrete Action", v)
            { }

            /// <summary>
            /// Perform the action
            /// </summary>
            protected override void DoAction()
            { }
        }

internal virtual Action CreateAction()
{
    // TODO: Instantiate an appropriate concrete class.
    
    Action target = new ConcreteAction(new Dictionary<string, object>());
    return target;
}

/// <summary>
///A test for AddToXmlElement
///</summary>
[TestMethod()]
public void AddToXmlElementTest()
{
Action target = CreateAction();
    XmlDocument xmlDoc = new XmlDocument();

    XmlElement rootElement = xmlDoc.CreateElement("Actions");
    target.AddToXmlElement(rootElement);
    Assert.Inconclusive("A method that does not return a value cannot be verified.  \nXml output was {0}", rootElement.InnerText);
}

internal virtual Action_Accessor CreateAction_Accessor()
{
    // TODO: Instantiate an appropriate concrete class.
    Action_Accessor target = new Action_Accessor(new PrivateObject(new ConcreteAction(new Dictionary<string, object>())));
    return target;
}

/// <summary>
///A test for CleanupAction
///</summary>
[TestMethod()]
[DeploymentItem("Action.dll")]
public void CleanupActionTest()
{
PrivateObject param0 = null; // TODO: Initialize to an appropriate value
Action_Accessor target = new Action_Accessor(param0); // TODO: Initialize to an appropriate value
    target.CleanupAction();
    Assert.Inconclusive("A method that does not return a value cannot be verified.");
}

/// <summary>
///A test for Dispose
///</summary>
[TestMethod()]
public void DisposeTest()
{
Action target = CreateAction(); // TODO: Initialize to an appropriate value
    target.Dispose();
    Assert.Inconclusive("A method that does not return a value cannot be verified.");
}

/// <summary>
///A test for DoAction
///</summary>
[TestMethod()]
[DeploymentItem("Action.dll")]
public void DoActionTest()
{
    // Private Accessor for DoAction is not found. Please rebuild the containing project or run the Publicize.exe manually.
    Assert.Inconclusive("Private Accessor for DoAction is not found. Please rebuild the containing project" +
            " or run the Publicize.exe manually.");
}

/// <summary>
///A test for FromXml
///</summary>
[TestMethod()]
public void FromXmlTest()
{
XmlElement element = null; // TODO: Initialize to an appropriate value
Dictionary<string, object> variables = null; // TODO: Initialize to an appropriate value
Action expected = null; // TODO: Initialize to an appropriate value
    Action actual;
    actual = Action.FromXml(element, variables);
    Assert.AreEqual(expected, actual);
    Assert.Inconclusive("Verify the correctness of this test method.");
}

/// <summary>
///A test for GetNext
///</summary>
[TestMethod()]
public void GetNextTest()
{
Action target = CreateAction(); // TODO: Initialize to an appropriate value
Action expected = null; // TODO: Initialize to an appropriate value
    Action actual;
    actual = target.GetNext();
    Assert.AreEqual(expected, actual);
    Assert.Inconclusive("Verify the correctness of this test method.");
}

/// <summary>
///A test for InitAction
///</summary>
[TestMethod()]
[DeploymentItem("Action.dll")]
public void InitActionTest()
{
PrivateObject param0 = null; // TODO: Initialize to an appropriate value
Action_Accessor target = new Action_Accessor(param0); // TODO: Initialize to an appropriate value
    target.InitAction();
    Assert.Inconclusive("A method that does not return a value cannot be verified.");
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
AskEventEnums.MessageIcon messageIcon = new AskEventEnums.MessageIcon(); // TODO: Initialize to an appropriate value
    target.Message(message, title, messageIcon);
    Assert.Inconclusive("A method that does not return a value cannot be verified.");
}

/// <summary>
///A test for ParseStringForVariable
///</summary>
[TestMethod()]
[DeploymentItem("Action.dll")]
public void ParseStringForVariableTest()
{
PrivateObject param0 = null; // TODO: Initialize to an appropriate value
Action_Accessor target = new Action_Accessor(param0); // TODO: Initialize to an appropriate value
string str = string.Empty; // TODO: Initialize to an appropriate value
string expected = string.Empty; // TODO: Initialize to an appropriate value
    string actual;
    actual = target.ParseStringForVariable(str);
    Assert.AreEqual(expected, actual);
    Assert.Inconclusive("Verify the correctness of this test method.");
}

/// <summary>
///A test for Run
///</summary>
[TestMethod()]
public void RunTest()
{
Action target = CreateAction(); // TODO: Initialize to an appropriate value
    target.Run();
    Assert.Inconclusive("A method that does not return a value cannot be verified.");
}

/// <summary>
///A test for Complexity
///</summary>
[TestMethod()]
public void ComplexityTest()
{
Action target = CreateAction(); // TODO: Initialize to an appropriate value
    int actual;
    actual = target.Complexity;
    Assert.Inconclusive("Verify the correctness of this test method.");
}
    }
}
