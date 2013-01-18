using Centipede;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace CentipedeTest
{
    
    
    /// <summary>
    ///This is a test class for AskJobTitleTest and is intended
    ///to contain all AskJobTitleTest Unit Tests
    ///</summary>
    [TestClass()]
    public class AskJobTitleTest
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
///A test for AskJobTitle Constructor
///</summary>
[TestMethod()]
public void AskJobTitleConstructorTest()
{
    AskJobTitle target = new AskJobTitle();
    Assert.Inconclusive("TODO: Implement code to verify target");
}

/// <summary>
///A test for Dispose
///</summary>
[TestMethod()]
[DeploymentItem("Centipede.exe")]
public void DisposeTest()
{
AskJobTitle_Accessor target = new AskJobTitle_Accessor(); // TODO: Initialize to an appropriate value
bool disposing = false; // TODO: Initialize to an appropriate value
    target.Dispose(disposing);
    Assert.Inconclusive("A method that does not return a value cannot be verified.");
}

/// <summary>
///A test for InitializeComponent
///</summary>
[TestMethod()]
[DeploymentItem("Centipede.exe")]
public void InitializeComponentTest()
{
AskJobTitle_Accessor target = new AskJobTitle_Accessor(); // TODO: Initialize to an appropriate value
    target.InitializeComponent();
    Assert.Inconclusive("A method that does not return a value cannot be verified.");
}

/// <summary>
///A test for button1_Click
///</summary>
[TestMethod()]
[DeploymentItem("Centipede.exe")]
public void button1_ClickTest()
{
AskJobTitle_Accessor target = new AskJobTitle_Accessor(); // TODO: Initialize to an appropriate value
object sender = null; // TODO: Initialize to an appropriate value
EventArgs e = null; // TODO: Initialize to an appropriate value
    target.button1_Click(sender, e);
    Assert.Inconclusive("A method that does not return a value cannot be verified.");
}
    }
}
