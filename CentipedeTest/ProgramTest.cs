using Centipede;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace CentipedeTest
{
    
    
    /// <summary>
    ///This is a test class for ProgramTest and is intended
    ///to contain all ProgramTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ProgramTest
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
///A test for AddAction
///</summary>
[TestMethod()]
public void AddActionTest()
{
Action action = null; // TODO: Initialize to an appropriate value
int index = 0; // TODO: Initialize to an appropriate value
    Program.AddAction(action, index);
    Assert.Inconclusive("A method that does not return a value cannot be verified.");
}

/// <summary>
///A test for Clear
///</summary>
[TestMethod()]
public void ClearTest()
{
    Program.Clear();
    Assert.Inconclusive("A method that does not return a value cannot be verified.");
}

/// <summary>
///A test for Dispose
///</summary>
[TestMethod()]
public void DisposeTest()
{
bool disposing = false; // TODO: Initialize to an appropriate value
    Program.Dispose(disposing);
    Assert.Inconclusive("A method that does not return a value cannot be verified.");
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
///A test for Main
///</summary>
[TestMethod()]
[DeploymentItem("Centipede.exe")]
public void MainTest()
{
    Program_Accessor.Main();
    Assert.Inconclusive("A method that does not return a value cannot be verified.");
}

/// <summary>
///A test for RemoveAction
///</summary>
[TestMethod()]
public void RemoveActionTest()
{
Action action = null; // TODO: Initialize to an appropriate value
    Program.RemoveAction(action);
    Assert.Inconclusive("A method that does not return a value cannot be verified.");
}

/// <summary>
///A test for RunJob
///</summary>
[TestMethod()]
public void RunJobTest()
{
    Program.RunJob();
    Assert.Inconclusive("A method that does not return a value cannot be verified.");
}

/// <summary>
///A test for SaveJob
///</summary>
[TestMethod()]
public void SaveJobTest()
{
string filename = string.Empty; // TODO: Initialize to an appropriate value
    Program.SaveJob(filename);
    Assert.Inconclusive("A method that does not return a value cannot be verified.");
}

/// <summary>
///A test for JobComplexity
///</summary>
[TestMethod()]
public void JobComplexityTest()
{
    int actual;
    actual = Program.JobComplexity;
    Assert.Inconclusive("Verify the correctness of this test method.");
}
    }
}
