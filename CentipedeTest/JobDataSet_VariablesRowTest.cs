using Centipede;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Data;

namespace CentipedeTest
{
    
    
    /// <summary>
    ///This is a test class for JobDataSet_VariablesRowTest and is intended
    ///to contain all JobDataSet_VariablesRowTest Unit Tests
    ///</summary>
    [TestClass()]
    public class JobDataSet_VariablesRowTest
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
///A test for VariablesRow Constructor
///</summary>
[TestMethod()]
public void JobDataSet_VariablesRowConstructorTest()
{
DataRowBuilder rb = null; // TODO: Initialize to an appropriate value
    JobDataSet.VariablesRow target = new JobDataSet.VariablesRow(rb);
    Assert.Inconclusive("TODO: Implement code to verify target");
}

/// <summary>
///A test for IsTypeNull
///</summary>
[TestMethod()]
public void IsTypeNullTest()
{
DataRowBuilder rb = null; // TODO: Initialize to an appropriate value
JobDataSet.VariablesRow target = new JobDataSet.VariablesRow(rb); // TODO: Initialize to an appropriate value
bool expected = false; // TODO: Initialize to an appropriate value
    bool actual;
    actual = target.IsTypeNull();
    Assert.AreEqual(expected, actual);
    Assert.Inconclusive("Verify the correctness of this test method.");
}

/// <summary>
///A test for IsValueNull
///</summary>
[TestMethod()]
public void IsValueNullTest()
{
DataRowBuilder rb = null; // TODO: Initialize to an appropriate value
JobDataSet.VariablesRow target = new JobDataSet.VariablesRow(rb); // TODO: Initialize to an appropriate value
bool expected = false; // TODO: Initialize to an appropriate value
    bool actual;
    actual = target.IsValueNull();
    Assert.AreEqual(expected, actual);
    Assert.Inconclusive("Verify the correctness of this test method.");
}

/// <summary>
///A test for SetTypeNull
///</summary>
[TestMethod()]
public void SetTypeNullTest()
{
DataRowBuilder rb = null; // TODO: Initialize to an appropriate value
JobDataSet.VariablesRow target = new JobDataSet.VariablesRow(rb); // TODO: Initialize to an appropriate value
    target.SetTypeNull();
    Assert.Inconclusive("A method that does not return a value cannot be verified.");
}

/// <summary>
///A test for SetValueNull
///</summary>
[TestMethod()]
public void SetValueNullTest()
{
DataRowBuilder rb = null; // TODO: Initialize to an appropriate value
JobDataSet.VariablesRow target = new JobDataSet.VariablesRow(rb); // TODO: Initialize to an appropriate value
    target.SetValueNull();
    Assert.Inconclusive("A method that does not return a value cannot be verified.");
}

/// <summary>
///A test for Name
///</summary>
[TestMethod()]
public void NameTest()
{
DataRowBuilder rb = null; // TODO: Initialize to an appropriate value
JobDataSet.VariablesRow target = new JobDataSet.VariablesRow(rb); // TODO: Initialize to an appropriate value
string expected = string.Empty; // TODO: Initialize to an appropriate value
    string actual;
    target.Name = expected;
    actual = target.Name;
    Assert.AreEqual(expected, actual);
    Assert.Inconclusive("Verify the correctness of this test method.");
}

/// <summary>
///A test for Type
///</summary>
[TestMethod()]
public void TypeTest()
{
DataRowBuilder rb = null; // TODO: Initialize to an appropriate value
JobDataSet.VariablesRow target = new JobDataSet.VariablesRow(rb); // TODO: Initialize to an appropriate value
byte expected = 0; // TODO: Initialize to an appropriate value
    byte actual;
    target.Type = expected;
    actual = target.Type;
    Assert.AreEqual(expected, actual);
    Assert.Inconclusive("Verify the correctness of this test method.");
}

/// <summary>
///A test for Value
///</summary>
[TestMethod()]
public void ValueTest()
{
DataRowBuilder rb = null; // TODO: Initialize to an appropriate value
JobDataSet.VariablesRow target = new JobDataSet.VariablesRow(rb); // TODO: Initialize to an appropriate value
object expected = null; // TODO: Initialize to an appropriate value
    object actual;
    target.Value = expected;
    actual = target.Value;
    Assert.AreEqual(expected, actual);
    Assert.Inconclusive("Verify the correctness of this test method.");
}
    }
}
