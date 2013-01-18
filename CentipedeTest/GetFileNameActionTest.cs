using Centipede.Actions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace CentipedeTest
{
    
    
    /// <summary>
    ///This is a test class for GetFileNameActionTest and is intended
    ///to contain all GetFileNameActionTest Unit Tests
    ///</summary>
    [TestClass()]
    public class GetFileNameActionTest
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
///A test for GetFileNameAction Constructor
///</summary>
[TestMethod()]
public void GetFileNameActionConstructorTest()
{
Dictionary<string, object> v = null; // TODO: Initialize to an appropriate value
    GetFileNameAction target = new GetFileNameAction(v);
    Assert.Inconclusive("TODO: Implement code to verify target");
}

/// <summary>
///A test for CleanupAction
///</summary>
[TestMethod()]
[DeploymentItem("Centipede.exe")]
public void CleanupActionTest()
{
PrivateObject param0 = null; // TODO: Initialize to an appropriate value
GetFileNameAction_Accessor target = new GetFileNameAction_Accessor(param0); // TODO: Initialize to an appropriate value
    target.CleanupAction();
    Assert.Inconclusive("A method that does not return a value cannot be verified.");
}

/// <summary>
///A test for DoAction
///</summary>
[TestMethod()]
[DeploymentItem("Centipede.exe")]
public void DoActionTest()
{
PrivateObject param0 = null; // TODO: Initialize to an appropriate value
GetFileNameAction_Accessor target = new GetFileNameAction_Accessor(param0); // TODO: Initialize to an appropriate value
    target.DoAction();
    Assert.Inconclusive("A method that does not return a value cannot be verified.");
}

/// <summary>
///A test for GetFileNameDialogue_FileOk
///</summary>
[TestMethod()]
[DeploymentItem("Centipede.exe")]
public void GetFileNameDialogue_FileOkTest()
{
PrivateObject param0 = null; // TODO: Initialize to an appropriate value
GetFileNameAction_Accessor target = new GetFileNameAction_Accessor(param0); // TODO: Initialize to an appropriate value
object sender = null; // TODO: Initialize to an appropriate value
CancelEventArgs e = null; // TODO: Initialize to an appropriate value
    target.GetFileNameDialogue_FileOk(sender, e);
    Assert.Inconclusive("A method that does not return a value cannot be verified.");
}

/// <summary>
///A test for InitAction
///</summary>
[TestMethod()]
[DeploymentItem("Centipede.exe")]
public void InitActionTest()
{
PrivateObject param0 = null; // TODO: Initialize to an appropriate value
GetFileNameAction_Accessor target = new GetFileNameAction_Accessor(param0); // TODO: Initialize to an appropriate value
    target.InitAction();
    Assert.Inconclusive("A method that does not return a value cannot be verified.");
}
    }
}
