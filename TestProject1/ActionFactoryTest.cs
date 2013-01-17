using Centipede;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Action = Centipede.Action;


namespace TestProject1
{
    
    
    /// <summary>
    ///This is a test class for ActionFactoryTest and is intended
    ///to contain all ActionFactoryTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ActionFactoryTest
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
        ///A test for ActionFactory Constructor
        ///</summary>
        [TestMethod()]
        public void ActionFactoryConstructorTest()
        {
            ActionCategoryAttribute catAttribute = null; // TODO: Initialize to an appropriate value
            Type pluginType = null; // TODO: Initialize to an appropriate value
            ActionFactory target = new ActionFactory(catAttribute, pluginType);
            Assert.Inconclusive("TODO: Implement code to verify target");
        }

        /// <summary>
        ///A test for ActionFactory Constructor
        ///</summary>
        [TestMethod()]
        public void ActionFactoryConstructorTest1()
        {
            string displayName = string.Empty; // TODO: Initialize to an appropriate value
            Type actionType = null; // TODO: Initialize to an appropriate value
            ActionFactory target = new ActionFactory(displayName, actionType);
            Assert.Inconclusive("TODO: Implement code to verify target");
        }

        /// <summary>
        ///A test for Generate
        ///</summary>
        [TestMethod()]
        public void GenerateTest()
        {
            ActionCategoryAttribute catAttribute = null; // TODO: Initialize to an appropriate value
            Type pluginType = null; // TODO: Initialize to an appropriate value
            ActionFactory target = new ActionFactory(catAttribute, pluginType); // TODO: Initialize to an appropriate value
            Action expected = null; // TODO: Initialize to an appropriate value
            Action actual;
            actual = target.Generate();
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }
    }
}
