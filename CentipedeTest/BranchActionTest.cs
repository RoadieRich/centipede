using Centipede.Actions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using Centipede;

namespace CentipedeTest
{
    
    
    /// <summary>
    ///This is a test class for BranchActionTest and is intended
    ///to contain all BranchActionTest Unit Tests
    ///</summary>
    [TestClass()]
    public class BranchActionTest
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
///A test for BranchAction Constructor
///</summary>
[TestMethod()]
public void BranchActionConstructorTest()
{
Dictionary<string, object> v = null; // TODO: Initialize to an appropriate value
    BranchAction target = new BranchAction(v);
    Assert.Inconclusive("TODO: Implement code to verify target");
}

/// <summary>
///A test for DoAction
///</summary>
[TestMethod()]
[DeploymentItem("Centipede.exe")]
public void DoActionTest()
{
PrivateObject param0 = null; // TODO: Initialize to an appropriate value
BranchAction_Accessor target = new BranchAction_Accessor(param0); // TODO: Initialize to an appropriate value
    target.DoAction();
    Assert.Inconclusive("A method that does not return a value cannot be verified.");
}

/// <summary>
///A test for GetNext
///</summary>
[TestMethod()]
public void GetNextTest()
{
Dictionary<string, object> v = null; // TODO: Initialize to an appropriate value
BranchAction target = new BranchAction(v); // TODO: Initialize to an appropriate value
Action expected = null; // TODO: Initialize to an appropriate value
    Action actual;
    actual = target.GetNext();
    Assert.AreEqual(expected, actual);
    Assert.Inconclusive("Verify the correctness of this test method.");
}
    }
}
