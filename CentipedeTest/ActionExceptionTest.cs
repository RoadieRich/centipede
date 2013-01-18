using Centipede;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace CentipedeTest
{
    
    
    /// <summary>
    ///This is a test class for ActionExceptionTest and is intended
    ///to contain all ActionExceptionTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ActionExceptionTest
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
///A test for ActionException Constructor
///</summary>
[TestMethod()]
public void ActionExceptionConstructorTest()
{
string message = string.Empty; // TODO: Initialize to an appropriate value
    ActionException target = new ActionException(message);
    Assert.Inconclusive("TODO: Implement code to verify target");
}

/// <summary>
///A test for ActionException Constructor
///</summary>
[TestMethod()]
public void ActionExceptionConstructorTest1()
{
Exception e = null; // TODO: Initialize to an appropriate value
Action action = null; // TODO: Initialize to an appropriate value
    ActionException target = new ActionException(e, action);
    Assert.Inconclusive("TODO: Implement code to verify target");
}

/// <summary>
///A test for ActionException Constructor
///</summary>
[TestMethod()]
public void ActionExceptionConstructorTest2()
{
string message = string.Empty; // TODO: Initialize to an appropriate value
Exception exception = null; // TODO: Initialize to an appropriate value
Action action = null; // TODO: Initialize to an appropriate value
    ActionException target = new ActionException(message, exception, action);
    Assert.Inconclusive("TODO: Implement code to verify target");
}

/// <summary>
///A test for ActionException Constructor
///</summary>
[TestMethod()]
public void ActionExceptionConstructorTest3()
{
string message = string.Empty; // TODO: Initialize to an appropriate value
Action action = null; // TODO: Initialize to an appropriate value
    ActionException target = new ActionException(message, action);
    Assert.Inconclusive("TODO: Implement code to verify target");
}

/// <summary>
///A test for ActionException Constructor
///</summary>
[TestMethod()]
public void ActionExceptionConstructorTest4()
{
Action action = null; // TODO: Initialize to an appropriate value
    ActionException target = new ActionException(action);
    Assert.Inconclusive("TODO: Implement code to verify target");
}
    }
}
