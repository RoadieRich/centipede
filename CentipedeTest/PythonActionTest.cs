using PyAction;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace CentipedeTest
{
    
    
    /// <summary>
    ///This is a test class for PythonActionTest and is intended
    ///to contain all PythonActionTest Unit Tests
    ///</summary>
    [TestClass()]
    public class PythonActionTest
    {


        private TestContext _testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return _testContextInstance;
            }
            set
            {
                _testContextInstance = value;
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
///A test for PythonAction Constructor
///</summary>
[TestMethod()]
public void PythonActionConstructorTest()
{
Dictionary<string, object> v = new Dictionary<string, object>();
    PythonAction target = new PythonAction(v);

    Assert.Inconclusive("TODO: Implement code to verify target");
}

/// <summary>
///A test for DoAction
///</summary>
[TestMethod()]
[DeploymentItem("PythonAction.dll")]
public void DoActionTest()
{
    // Creation of the private accessor for 'Microsoft.VisualStudio.TestTools.TypesAndSymbols.Assembly' failed
    Assert.Inconclusive("Creation of the private accessor for \'Microsoft.VisualStudio.TestTools.TypesAndSy" +
            "mbols.Assembly\' failed");
}

/// <summary>
///A test for Complexity
///</summary>
[TestMethod()]
public void ComplexityTest()
{
Dictionary<string, object> v = null; // TODO: Initialize to an appropriate value
PythonAction target = new PythonAction(v); // TODO: Initialize to an appropriate value
    int actual;
    actual = target.Complexity;
    Assert.Inconclusive("Verify the correctness of this test method.");
}

/// <summary>
///A test for Source
///</summary>
[TestMethod()]
public void SourceTest()
{
Dictionary<string, object> v = null; // TODO: Initialize to an appropriate value
PythonAction target = new PythonAction(v); // TODO: Initialize to an appropriate value
string expected = string.Empty; // TODO: Initialize to an appropriate value
    string actual;
    target.Source = expected;
    actual = target.Source;
    Assert.AreEqual(expected, actual);
    Assert.Inconclusive("Verify the correctness of this test method.");
}
    }
}
