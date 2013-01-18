using Centipede;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Runtime.Serialization;
using System.Data;
using System.Xml.Schema;

namespace CentipedeTest
{
    
    
    /// <summary>
    ///This is a test class for JobDataSet_VariablesDataTableTest and is intended
    ///to contain all JobDataSet_VariablesDataTableTest Unit Tests
    ///</summary>
    [TestClass()]
    public class JobDataSet_VariablesDataTableTest
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
///A test for VariablesDataTable Constructor
///</summary>
[TestMethod()]
[DeploymentItem("Centipede.exe")]
public void JobDataSet_VariablesDataTableConstructorTest()
{
SerializationInfo info = null; // TODO: Initialize to an appropriate value
StreamingContext context = new StreamingContext(); // TODO: Initialize to an appropriate value
    JobDataSet_Accessor.VariablesDataTable target = new JobDataSet_Accessor.VariablesDataTable(info, context);
    Assert.Inconclusive("TODO: Implement code to verify target");
}

/// <summary>
///A test for VariablesDataTable Constructor
///</summary>
[TestMethod()]
public void JobDataSet_VariablesDataTableConstructorTest1()
{
DataTable table = null; // TODO: Initialize to an appropriate value
    JobDataSet.VariablesDataTable target = new JobDataSet.VariablesDataTable(table);
    Assert.Inconclusive("TODO: Implement code to verify target");
}

/// <summary>
///A test for VariablesDataTable Constructor
///</summary>
[TestMethod()]
public void JobDataSet_VariablesDataTableConstructorTest2()
{
    JobDataSet.VariablesDataTable target = new JobDataSet.VariablesDataTable();
    Assert.Inconclusive("TODO: Implement code to verify target");
}

/// <summary>
///A test for AddVariablesRow
///</summary>
[TestMethod()]
public void AddVariablesRowTest()
{
JobDataSet.VariablesDataTable target = new JobDataSet.VariablesDataTable(); // TODO: Initialize to an appropriate value
string Name = string.Empty; // TODO: Initialize to an appropriate value
object Value = null; // TODO: Initialize to an appropriate value
byte Type = 0; // TODO: Initialize to an appropriate value
JobDataSet.VariablesRow expected = null; // TODO: Initialize to an appropriate value
    JobDataSet.VariablesRow actual;
    actual = target.AddVariablesRow(Name, Value, Type);
    Assert.AreEqual(expected, actual);
    Assert.Inconclusive("Verify the correctness of this test method.");
}

/// <summary>
///A test for AddVariablesRow
///</summary>
[TestMethod()]
public void AddVariablesRowTest1()
{
JobDataSet.VariablesDataTable target = new JobDataSet.VariablesDataTable(); // TODO: Initialize to an appropriate value
JobDataSet.VariablesRow row = null; // TODO: Initialize to an appropriate value
    target.AddVariablesRow(row);
    Assert.Inconclusive("A method that does not return a value cannot be verified.");
}

/// <summary>
///A test for Clone
///</summary>
[TestMethod()]
public void CloneTest()
{
JobDataSet.VariablesDataTable target = new JobDataSet.VariablesDataTable(); // TODO: Initialize to an appropriate value
DataTable expected = null; // TODO: Initialize to an appropriate value
    DataTable actual;
    actual = target.Clone();
    Assert.AreEqual(expected, actual);
    Assert.Inconclusive("Verify the correctness of this test method.");
}

/// <summary>
///A test for CreateInstance
///</summary>
[TestMethod()]
[DeploymentItem("Centipede.exe")]
public void CreateInstanceTest()
{
JobDataSet_Accessor.VariablesDataTable target = new JobDataSet_Accessor.VariablesDataTable(); // TODO: Initialize to an appropriate value
DataTable expected = null; // TODO: Initialize to an appropriate value
    DataTable actual;
    actual = target.CreateInstance();
    Assert.AreEqual(expected, actual);
    Assert.Inconclusive("Verify the correctness of this test method.");
}

/// <summary>
///A test for FindByName
///</summary>
[TestMethod()]
public void FindByNameTest()
{
JobDataSet.VariablesDataTable target = new JobDataSet.VariablesDataTable(); // TODO: Initialize to an appropriate value
string Name = string.Empty; // TODO: Initialize to an appropriate value
JobDataSet.VariablesRow expected = null; // TODO: Initialize to an appropriate value
    JobDataSet.VariablesRow actual;
    actual = target.FindByName(Name);
    Assert.AreEqual(expected, actual);
    Assert.Inconclusive("Verify the correctness of this test method.");
}

/// <summary>
///A test for GetRowType
///</summary>
[TestMethod()]
[DeploymentItem("Centipede.exe")]
public void GetRowTypeTest()
{
JobDataSet_Accessor.VariablesDataTable target = new JobDataSet_Accessor.VariablesDataTable(); // TODO: Initialize to an appropriate value
Type expected = null; // TODO: Initialize to an appropriate value
    Type actual;
    actual = target.GetRowType();
    Assert.AreEqual(expected, actual);
    Assert.Inconclusive("Verify the correctness of this test method.");
}

/// <summary>
///A test for GetTypedTableSchema
///</summary>
[TestMethod()]
public void GetTypedTableSchemaTest()
{
XmlSchemaSet xs = null; // TODO: Initialize to an appropriate value
XmlSchemaComplexType expected = null; // TODO: Initialize to an appropriate value
    XmlSchemaComplexType actual;
    actual = JobDataSet.VariablesDataTable.GetTypedTableSchema(xs);
    Assert.AreEqual(expected, actual);
    Assert.Inconclusive("Verify the correctness of this test method.");
}

/// <summary>
///A test for InitClass
///</summary>
[TestMethod()]
[DeploymentItem("Centipede.exe")]
public void InitClassTest()
{
JobDataSet_Accessor.VariablesDataTable target = new JobDataSet_Accessor.VariablesDataTable(); // TODO: Initialize to an appropriate value
    target.InitClass();
    Assert.Inconclusive("A method that does not return a value cannot be verified.");
}

/// <summary>
///A test for InitVars
///</summary>
[TestMethod()]
public void InitVarsTest()
{
JobDataSet.VariablesDataTable target = new JobDataSet.VariablesDataTable(); // TODO: Initialize to an appropriate value
    target.InitVars();
    Assert.Inconclusive("A method that does not return a value cannot be verified.");
}

/// <summary>
///A test for NewRowFromBuilder
///</summary>
[TestMethod()]
[DeploymentItem("Centipede.exe")]
public void NewRowFromBuilderTest()
{
JobDataSet_Accessor.VariablesDataTable target = new JobDataSet_Accessor.VariablesDataTable(); // TODO: Initialize to an appropriate value
DataRowBuilder builder = null; // TODO: Initialize to an appropriate value
DataRow expected = null; // TODO: Initialize to an appropriate value
    DataRow actual;
    actual = target.NewRowFromBuilder(builder);
    Assert.AreEqual(expected, actual);
    Assert.Inconclusive("Verify the correctness of this test method.");
}

/// <summary>
///A test for NewVariablesRow
///</summary>
[TestMethod()]
public void NewVariablesRowTest()
{
JobDataSet.VariablesDataTable target = new JobDataSet.VariablesDataTable(); // TODO: Initialize to an appropriate value
JobDataSet.VariablesRow expected = null; // TODO: Initialize to an appropriate value
    JobDataSet.VariablesRow actual;
    actual = target.NewVariablesRow();
    Assert.AreEqual(expected, actual);
    Assert.Inconclusive("Verify the correctness of this test method.");
}

/// <summary>
///A test for OnRowChanged
///</summary>
[TestMethod()]
[DeploymentItem("Centipede.exe")]
public void OnRowChangedTest()
{
JobDataSet_Accessor.VariablesDataTable target = new JobDataSet_Accessor.VariablesDataTable(); // TODO: Initialize to an appropriate value
DataRowChangeEventArgs e = null; // TODO: Initialize to an appropriate value
    target.OnRowChanged(e);
    Assert.Inconclusive("A method that does not return a value cannot be verified.");
}

/// <summary>
///A test for OnRowChanging
///</summary>
[TestMethod()]
[DeploymentItem("Centipede.exe")]
public void OnRowChangingTest()
{
JobDataSet_Accessor.VariablesDataTable target = new JobDataSet_Accessor.VariablesDataTable(); // TODO: Initialize to an appropriate value
DataRowChangeEventArgs e = null; // TODO: Initialize to an appropriate value
    target.OnRowChanging(e);
    Assert.Inconclusive("A method that does not return a value cannot be verified.");
}

/// <summary>
///A test for OnRowDeleted
///</summary>
[TestMethod()]
[DeploymentItem("Centipede.exe")]
public void OnRowDeletedTest()
{
JobDataSet_Accessor.VariablesDataTable target = new JobDataSet_Accessor.VariablesDataTable(); // TODO: Initialize to an appropriate value
DataRowChangeEventArgs e = null; // TODO: Initialize to an appropriate value
    target.OnRowDeleted(e);
    Assert.Inconclusive("A method that does not return a value cannot be verified.");
}

/// <summary>
///A test for OnRowDeleting
///</summary>
[TestMethod()]
[DeploymentItem("Centipede.exe")]
public void OnRowDeletingTest()
{
JobDataSet_Accessor.VariablesDataTable target = new JobDataSet_Accessor.VariablesDataTable(); // TODO: Initialize to an appropriate value
DataRowChangeEventArgs e = null; // TODO: Initialize to an appropriate value
    target.OnRowDeleting(e);
    Assert.Inconclusive("A method that does not return a value cannot be verified.");
}

/// <summary>
///A test for RemoveVariablesRow
///</summary>
[TestMethod()]
public void RemoveVariablesRowTest()
{
JobDataSet.VariablesDataTable target = new JobDataSet.VariablesDataTable(); // TODO: Initialize to an appropriate value
JobDataSet.VariablesRow row = null; // TODO: Initialize to an appropriate value
    target.RemoveVariablesRow(row);
    Assert.Inconclusive("A method that does not return a value cannot be verified.");
}

/// <summary>
///A test for Count
///</summary>
[TestMethod()]
public void CountTest()
{
JobDataSet.VariablesDataTable target = new JobDataSet.VariablesDataTable(); // TODO: Initialize to an appropriate value
    int actual;
    actual = target.Count;
    Assert.Inconclusive("Verify the correctness of this test method.");
}

/// <summary>
///A test for Item
///</summary>
[TestMethod()]
public void ItemTest()
{
JobDataSet.VariablesDataTable target = new JobDataSet.VariablesDataTable(); // TODO: Initialize to an appropriate value
int index = 0; // TODO: Initialize to an appropriate value
    JobDataSet.VariablesRow actual;
    actual = target[index];
    Assert.Inconclusive("Verify the correctness of this test method.");
}

/// <summary>
///A test for NameColumn
///</summary>
[TestMethod()]
public void NameColumnTest()
{
JobDataSet.VariablesDataTable target = new JobDataSet.VariablesDataTable(); // TODO: Initialize to an appropriate value
    DataColumn actual;
    actual = target.NameColumn;
    Assert.Inconclusive("Verify the correctness of this test method.");
}

/// <summary>
///A test for TypeColumn
///</summary>
[TestMethod()]
public void TypeColumnTest()
{
JobDataSet.VariablesDataTable target = new JobDataSet.VariablesDataTable(); // TODO: Initialize to an appropriate value
    DataColumn actual;
    actual = target.TypeColumn;
    Assert.Inconclusive("Verify the correctness of this test method.");
}

/// <summary>
///A test for ValueColumn
///</summary>
[TestMethod()]
public void ValueColumnTest()
{
JobDataSet.VariablesDataTable target = new JobDataSet.VariablesDataTable(); // TODO: Initialize to an appropriate value
    DataColumn actual;
    actual = target.ValueColumn;
    Assert.Inconclusive("Verify the correctness of this test method.");
}
    }
}
