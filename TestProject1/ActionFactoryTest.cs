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
            string category = "category";
            string helpTextValue = "help text";
            string iconNameValue = "icon name";
            string displayNameValue = "display name";
            ActionCategoryAttribute catAttribute = new ActionCategoryAttribute(category)
                                                   {
                                                           helpText = helpTextValue,
                                                           displayName = displayNameValue,
                                                           iconName=iconNameValue
                                                   };
            Type pluginType = typeof (TestAction);
            ActionFactory target = new ActionFactory(catAttribute, pluginType);
            Assert.AreEqual(displayNameValue, target.Text, "Display name not set correctly");
            PrivateObject targetPrivate = new PrivateObject(target);
            Assert.AreEqual(pluginType,targetPrivate.GetField("_actionType"), "_actionType field not set correctly");

        }

        /// <summary>
        ///A test for ActionFactory Constructor
        ///</summary>
        [TestMethod()]
        public void ActionFactoryConstructorTest1()
        {
            string displayNameValue = "display name";
            Type actionType = typeof(TestAction);
            ActionFactory target = new ActionFactory(displayNameValue, actionType);
            
            Assert.AreEqual(displayNameValue, target.Text, "Display name not set correctly");
            PrivateObject targetPrivate = new PrivateObject(target);
            Assert.AreEqual(actionType, targetPrivate.GetField("_actionType"), "_actionType field not set correctly");
        }

        /// <summary>
        ///A test for Generate
        ///</summary>
        [TestMethod()]
        public void GenerateTest()
        {
            ActionFactory target = new ActionFactory("DisplayName", typeof (TestAction));
            Action actual = target.Generate();
            Assert.IsInstanceOfType(actual, typeof(TestAction), "Wrong type returned from Generate()");
            TestAction ta = actual as TestAction;

            Assert.AreEqual(1, ta.CtorCalled, "Ctor called {0} times, expected 1", ta.CtorCalled);
            Assert.AreEqual(0,
                            ta.CleanupActionCalled + ta.DisposeCalled + ta.DoActionCalled + ta.GetNextCalled +
                            ta.InitActionCalled, "Extra methods called in Generate()");
        }
    }
}
