using Centipede.Properties;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Drawing;
using System.Globalization;
using System.Resources;
using Centipede.PyAction.Properties;

namespace CentipedeTest
{
    
    
    /// <summary>
    ///This is a test class for ResourcesTest and is intended
    ///to contain all ResourcesTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ResourcesTest
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
///A test for Resources Constructor
///</summary>
[TestMethod()]
public void ResourcesConstructorTest1()
{
    Resources target = new Resources();
    Assert.Inconclusive("TODO: Implement code to verify target");
}

/// <summary>
///A test for Culture
///</summary>
[TestMethod()]
public void CultureTest1()
{
CultureInfo expected = null; // TODO: Initialize to an appropriate value
    CultureInfo actual;
    Resources.Culture = expected;
    actual = Resources.Culture;
    Assert.AreEqual(expected, actual);
    Assert.Inconclusive("Verify the correctness of this test method.");
}

/// <summary>
///A test for ResourceManager
///</summary>
[TestMethod()]
public void ResourceManagerTest1()
{
    ResourceManager actual;
    actual = Resources.ResourceManager;
    Assert.Inconclusive("Verify the correctness of this test method.");
}

/// <summary>
///A test for pycon
///</summary>
[TestMethod()]
public void pyconTest1()
{
    Icon actual;
    actual = Resources.pycon;
    Assert.Inconclusive("Verify the correctness of this test method.");
}

/// <summary>
///A test for Resources Constructor
///</summary>
[TestMethod()]
public void ResourcesConstructorTest()
{
    Resources target = new Resources();
    Assert.Inconclusive("TODO: Implement code to verify target");
}

/// <summary>
///A test for BranchDisplayControl_BranchDisplayControl_Action_if_false
///</summary>
[TestMethod()]
public void BranchDisplayControl_BranchDisplayControl_Action_if_falseTest()
{
    string actual;
    actual = Resources.BranchDisplayControl_BranchDisplayControl_Action_if_false;
    Assert.Inconclusive("Verify the correctness of this test method.");
}

/// <summary>
///A test for BranchDisplayControl_BranchDisplayControl_Condition
///</summary>
[TestMethod()]
public void BranchDisplayControl_BranchDisplayControl_ConditionTest()
{
    string actual;
    actual = Resources.BranchDisplayControl_BranchDisplayControl_Condition;
    Assert.Inconclusive("Verify the correctness of this test method.");
}

/// <summary>
///A test for Centipede
///</summary>
[TestMethod()]
public void CentipedeTest()
{
    Icon actual;
    actual = Resources.Centipede;
    Assert.Inconclusive("Verify the correctness of this test method.");
}

/// <summary>
///A test for Complete_OK
///</summary>
[TestMethod()]
public void Complete_OKTest()
{
    Bitmap actual;
    actual = Resources.Complete_OK;
    Assert.Inconclusive("Verify the correctness of this test method.");
}

/// <summary>
///A test for CriticalError
///</summary>
[TestMethod()]
public void CriticalErrorTest()
{
    Bitmap actual;
    actual = Resources.CriticalError;
    Assert.Inconclusive("Verify the correctness of this test method.");
}

/// <summary>
///A test for Culture
///</summary>
[TestMethod()]
public void CultureTest()
{
CultureInfo expected = null; // TODO: Initialize to an appropriate value
    CultureInfo actual;
    Resources.Culture = expected;
    actual = Resources.Culture;
    Assert.AreEqual(expected, actual);
    Assert.Inconclusive("Verify the correctness of this test method.");
}

/// <summary>
///A test for DemoAction_DoAction_Demo_Action_executed
///</summary>
[TestMethod()]
public void DemoAction_DoAction_Demo_Action_executedTest()
{
    string actual;
    actual = Resources.DemoAction_DoAction_Demo_Action_executed;
    Assert.Inconclusive("Verify the correctness of this test method.");
}

/// <summary>
///A test for If
///</summary>
[TestMethod()]
public void IfTest()
{
    Icon actual;
    actual = Resources.If;
    Assert.Inconclusive("Verify the correctness of this test method.");
}

/// <summary>
///A test for MainWindow_CompletedHandler_Finished
///</summary>
[TestMethod()]
public void MainWindow_CompletedHandler_FinishedTest()
{
    string actual;
    actual = Resources.MainWindow_CompletedHandler_Finished;
    Assert.Inconclusive("Verify the correctness of this test method.");
}

/// <summary>
///A test for MainWindow_ErrorHandler_Error
///</summary>
[TestMethod()]
public void MainWindow_ErrorHandler_ErrorTest()
{
    string actual;
    actual = Resources.MainWindow_ErrorHandler_Error;
    Assert.Inconclusive("Verify the correctness of this test method.");
}

/// <summary>
///A test for MainWindow_LoadBtn_Click_Save_changes_
///</summary>
[TestMethod()]
public void MainWindow_LoadBtn_Click_Save_changes_Test()
{
    string actual;
    actual = Resources.MainWindow_LoadBtn_Click_Save_changes_;
    Assert.Inconclusive("Verify the correctness of this test method.");
}

/// <summary>
///A test for MainWindow_LoadBtn_Click_Unsaved_Changes
///</summary>
[TestMethod()]
public void MainWindow_LoadBtn_Click_Unsaved_ChangesTest()
{
    string actual;
    actual = Resources.MainWindow_LoadBtn_Click_Unsaved_Changes;
    Assert.Inconclusive("Verify the correctness of this test method.");
}

/// <summary>
///A test for ResourceManager
///</summary>
[TestMethod()]
public void ResourceManagerTest()
{
    ResourceManager actual;
    actual = Resources.ResourceManager;
    Assert.Inconclusive("Verify the correctness of this test method.");
}

/// <summary>
///A test for StatusAnnotation_Run
///</summary>
[TestMethod()]
public void StatusAnnotation_RunTest()
{
    Bitmap actual;
    actual = Resources.StatusAnnotation_Run;
    Assert.Inconclusive("Verify the correctness of this test method.");
}

/// <summary>
///A test for ThumbnailView
///</summary>
[TestMethod()]
public void ThumbnailViewTest()
{
    Bitmap actual;
    actual = Resources.ThumbnailView;
    Assert.Inconclusive("Verify the correctness of this test method.");
}

/// <summary>
///A test for generic
///</summary>
[TestMethod()]
public void genericTest()
{
    Icon actual;
    actual = Resources.generic;
    Assert.Inconclusive("Verify the correctness of this test method.");
}

/// <summary>
///A test for pycon
///</summary>
[TestMethod()]
public void pyconTest()
{
    Icon actual;
    actual = Resources.pycon;
    Assert.Inconclusive("Verify the correctness of this test method.");
}
    }
}
