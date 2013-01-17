using Centipede.Actions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Centipede;
using Action = Centipede.Action;


namespace TestProject1
{
    
    
    /// <summary>
    ///This is a test class for BranchDisplayControlTest and is intended
    ///to contain all BranchDisplayControlTest Unit Tests
    ///</summary>
    [TestClass()]
    public class BranchDisplayControlTest
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
        ///A test for BranchDisplayControl Constructor
        ///</summary>
        [TestMethod()]
        public void BranchDisplayControlConstructorTest()
        {
            BranchAction action = null; // TODO: Initialize to an appropriate value
            BranchDisplayControl target = new BranchDisplayControl(action);
            Assert.Inconclusive("TODO: Implement code to verify target");
        }

        /// <summary>
        ///A test for actionCombo_DropDown
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Centipede.exe")]
        public void actionCombo_DropDownTest()
        {
            PrivateObject param0 = null; // TODO: Initialize to an appropriate value
            BranchDisplayControl_Accessor target = new BranchDisplayControl_Accessor(param0); // TODO: Initialize to an appropriate value
            object sender = null; // TODO: Initialize to an appropriate value
            EventArgs e = null; // TODO: Initialize to an appropriate value
            target.actionCombo_DropDown(sender, e);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for ThisAction
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Centipede.exe")]
        public void ThisActionTest()
        {
            PrivateObject param0 = null; // TODO: Initialize to an appropriate value
            BranchDisplayControl_Accessor target = new BranchDisplayControl_Accessor(param0); // TODO: Initialize to an appropriate value
            BranchAction actual;
            actual = target.ThisAction;
            Assert.Inconclusive("Verify the correctness of this test method.");
        }
    }
}
