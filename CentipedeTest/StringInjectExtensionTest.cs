using Centipede.StringInject;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections;

namespace CentipedeTest
{
    
    
    /// <summary>
    ///This is a test class for StringInjectExtensionTest and is intended
    ///to contain all StringInjectExtensionTest Unit Tests
    ///</summary>
    [TestClass()]
    public class StringInjectExtensionTest
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
///A test for GetPropertyHash
///</summary>
[TestMethod()]
[DeploymentItem("Action.dll")]
public void GetPropertyHashTest()
{
object properties = null; // TODO: Initialize to an appropriate value
Hashtable expected = null; // TODO: Initialize to an appropriate value
    Hashtable actual;
    actual = StringInjectExtension_Accessor.GetPropertyHash(properties);
    Assert.AreEqual(expected, actual);
    Assert.Inconclusive("Verify the correctness of this test method.");
}

/// <summary>
///A test for Inject
///</summary>
[TestMethod()]
public void InjectTest()
{
string formatString = string.Empty; // TODO: Initialize to an appropriate value
object injectionObject = null; // TODO: Initialize to an appropriate value
string expected = string.Empty; // TODO: Initialize to an appropriate value
    string actual;
    actual = StringInjectExtension.Inject(formatString, injectionObject);
    Assert.AreEqual(expected, actual);
    Assert.Inconclusive("Verify the correctness of this test method.");
}

/// <summary>
///A test for Inject
///</summary>
[TestMethod()]
public void InjectTest1()
{
string formatString = string.Empty; // TODO: Initialize to an appropriate value
Hashtable attributes = null; // TODO: Initialize to an appropriate value
string expected = string.Empty; // TODO: Initialize to an appropriate value
    string actual;
    actual = StringInjectExtension.Inject(formatString, attributes);
    Assert.AreEqual(expected, actual);
    Assert.Inconclusive("Verify the correctness of this test method.");
}

/// <summary>
///A test for Inject
///</summary>
[TestMethod()]
public void InjectTest2()
{
string formatString = string.Empty; // TODO: Initialize to an appropriate value
IDictionary dictionary = null; // TODO: Initialize to an appropriate value
string expected = string.Empty; // TODO: Initialize to an appropriate value
    string actual;
    actual = StringInjectExtension.Inject(formatString, dictionary);
    Assert.AreEqual(expected, actual);
    Assert.Inconclusive("Verify the correctness of this test method.");
}

/// <summary>
///A test for InjectSingleValue
///</summary>
[TestMethod()]
public void InjectSingleValueTest()
{
string formatString = string.Empty; // TODO: Initialize to an appropriate value
string key = string.Empty; // TODO: Initialize to an appropriate value
object replacementValue = null; // TODO: Initialize to an appropriate value
string expected = string.Empty; // TODO: Initialize to an appropriate value
    string actual;
    actual = StringInjectExtension.InjectSingleValue(formatString, key, replacementValue);
    Assert.AreEqual(expected, actual);
    Assert.Inconclusive("Verify the correctness of this test method.");
}
    }
}
