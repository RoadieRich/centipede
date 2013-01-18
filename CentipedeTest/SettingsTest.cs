using Centipede.Properties;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace CentipedeTest
{
    
    
    /// <summary>
    ///This is a test class for SettingsTest and is intended
    ///to contain all SettingsTest Unit Tests
    ///</summary>
    [TestClass()]
    public class SettingsTest
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
///A test for Settings Constructor
///</summary>
[TestMethod()]
public void SettingsConstructorTest1()
{
    Settings target = new Settings();
    Assert.Inconclusive("TODO: Implement code to verify target");
}

/// <summary>
///A test for SettingChangingEventHandler
///</summary>
[TestMethod()]
[DeploymentItem("Centipede.exe")]
public void SettingChangingEventHandlerTest()
{
    // Private Accessor for Centipede.Properties.Settings is not found. Please rebuild the containing project or run the Publicize.exe manually.
    Assert.Inconclusive("Private Accessor for Centipede.Properties.Settings is not found. Please rebuild t" +
            "he containing project or run the Publicize.exe manually.");
}

/// <summary>
///A test for SettingsSavingEventHandler
///</summary>
[TestMethod()]
[DeploymentItem("Centipede.exe")]
public void SettingsSavingEventHandlerTest()
{
    // Private Accessor for Centipede.Properties.Settings is not found. Please rebuild the containing project or run the Publicize.exe manually.
    Assert.Inconclusive("Private Accessor for Centipede.Properties.Settings is not found. Please rebuild t" +
            "he containing project or run the Publicize.exe manually.");
}

/// <summary>
///A test for Default
///</summary>
[TestMethod()]
public void DefaultTest1()
{
    Settings actual;
    actual = Settings.Default;
    Assert.Inconclusive("Verify the correctness of this test method.");
}

/// <summary>
///A test for PluginFolder
///</summary>
[TestMethod()]
public void PluginFolderTest1()
{
Settings target = new Settings(); // TODO: Initialize to an appropriate value
    string actual;
    actual = target.PluginFolder;
    Assert.Inconclusive("Verify the correctness of this test method.");
}

/// <summary>
///A test for Settings Constructor
///</summary>
[TestMethod()]
public void SettingsConstructorTest()
{
    Settings target = new Settings();
    Assert.Inconclusive("TODO: Implement code to verify target");
}

/// <summary>
///A test for Default
///</summary>
[TestMethod()]
public void DefaultTest()
{
    Settings actual;
    actual = Settings.Default;
    Assert.Inconclusive("Verify the correctness of this test method.");
}

/// <summary>
///A test for PluginFolder
///</summary>
[TestMethod()]
public void PluginFolderTest()
{
Settings target = new Settings(); // TODO: Initialize to an appropriate value
    string actual;
    actual = target.PluginFolder;
    Assert.Inconclusive("Verify the correctness of this test method.");
}
    }
}
