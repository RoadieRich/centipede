using Centipede;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Data;

namespace CentipedeTest
{
    
    
    /// <summary>
    ///This is a test class for JobDataSet_VariablesRowChangeEventTest and is intended
    ///to contain all JobDataSet_VariablesRowChangeEventTest Unit Tests
    ///</summary>
    [TestClass()]
    public class JobDataSet_VariablesRowChangeEventTest
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
///A test for VariablesRowChangeEvent Constructor
///</summary>
[TestMethod()]
public void JobDataSet_VariablesRowChangeEventConstructorTest()
{
JobDataSet.VariablesRow row = null; // TODO: Initialize to an appropriate value
DataRowAction action = new DataRowAction(); // TODO: Initialize to an appropriate value
    JobDataSet.VariablesRowChangeEvent target = new JobDataSet.VariablesRowChangeEvent(row, action);
    Assert.Inconclusive("TODO: Implement code to verify target");
}

/// <summary>
///A test for Action
///</summary>
[TestMethod()]
public void ActionTest()
{
JobDataSet.VariablesRow row = null; // TODO: Initialize to an appropriate value
DataRowAction action = new DataRowAction(); // TODO: Initialize to an appropriate value
JobDataSet.VariablesRowChangeEvent target = new JobDataSet.VariablesRowChangeEvent(row, action); // TODO: Initialize to an appropriate value
    DataRowAction actual;
    actual = target.Action;
    Assert.Inconclusive("Verify the correctness of this test method.");
}

/// <summary>
///A test for Row
///</summary>
[TestMethod()]
public void RowTest()
{
JobDataSet.VariablesRow row = null; // TODO: Initialize to an appropriate value
DataRowAction action = new DataRowAction(); // TODO: Initialize to an appropriate value
JobDataSet.VariablesRowChangeEvent target = new JobDataSet.VariablesRowChangeEvent(row, action); // TODO: Initialize to an appropriate value
    JobDataSet.VariablesRow actual;
    actual = target.Row;
    Assert.Inconclusive("Verify the correctness of this test method.");
}
    }
}
