using Centipede;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Runtime.Serialization;
using System.Data;
using System.Xml.Schema;
using System.Xml;
using System.ComponentModel;

namespace CentipedeTest
{
    
    
    /// <summary>
    ///This is a test class for JobDataSetTest and is intended
    ///to contain all JobDataSetTest Unit Tests
    ///</summary>
    [TestClass()]
    public class JobDataSetTest
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
///A test for JobDataSet Constructor
///</summary>
[TestMethod()]
[DeploymentItem("Centipede.exe")]
public void JobDataSetConstructorTest()
{
SerializationInfo info = null; // TODO: Initialize to an appropriate value
StreamingContext context = new StreamingContext(); // TODO: Initialize to an appropriate value
    JobDataSet_Accessor target = new JobDataSet_Accessor(info, context);
    Assert.Inconclusive("TODO: Implement code to verify target");
}

/// <summary>
///A test for JobDataSet Constructor
///</summary>
[TestMethod()]
public void JobDataSetConstructorTest1()
{
    JobDataSet target = new JobDataSet();
    Assert.Inconclusive("TODO: Implement code to verify target");
}

/// <summary>
///A test for Clone
///</summary>
[TestMethod()]
public void CloneTest()
{
JobDataSet target = new JobDataSet(); // TODO: Initialize to an appropriate value
DataSet expected = null; // TODO: Initialize to an appropriate value
    DataSet actual;
    actual = target.Clone();
    Assert.AreEqual(expected, actual);
    Assert.Inconclusive("Verify the correctness of this test method.");
}

/// <summary>
///A test for GetSchemaSerializable
///</summary>
[TestMethod()]
[DeploymentItem("Centipede.exe")]
public void GetSchemaSerializableTest()
{
JobDataSet_Accessor target = new JobDataSet_Accessor(); // TODO: Initialize to an appropriate value
XmlSchema expected = null; // TODO: Initialize to an appropriate value
    XmlSchema actual;
    actual = target.GetSchemaSerializable();
    Assert.AreEqual(expected, actual);
    Assert.Inconclusive("Verify the correctness of this test method.");
}

/// <summary>
///A test for GetTypedDataSetSchema
///</summary>
[TestMethod()]
public void GetTypedDataSetSchemaTest()
{
XmlSchemaSet xs = null; // TODO: Initialize to an appropriate value
XmlSchemaComplexType expected = null; // TODO: Initialize to an appropriate value
    XmlSchemaComplexType actual;
    actual = JobDataSet.GetTypedDataSetSchema(xs);
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
JobDataSet_Accessor target = new JobDataSet_Accessor(); // TODO: Initialize to an appropriate value
    target.InitClass();
    Assert.Inconclusive("A method that does not return a value cannot be verified.");
}

/// <summary>
///A test for InitVars
///</summary>
[TestMethod()]
public void InitVarsTest()
{
JobDataSet target = new JobDataSet(); // TODO: Initialize to an appropriate value
    target.InitVars();
    Assert.Inconclusive("A method that does not return a value cannot be verified.");
}

/// <summary>
///A test for InitVars
///</summary>
[TestMethod()]
public void InitVarsTest1()
{
JobDataSet target = new JobDataSet(); // TODO: Initialize to an appropriate value
bool initTable = false; // TODO: Initialize to an appropriate value
    target.InitVars(initTable);
    Assert.Inconclusive("A method that does not return a value cannot be verified.");
}

/// <summary>
///A test for InitializeDerivedDataSet
///</summary>
[TestMethod()]
[DeploymentItem("Centipede.exe")]
public void InitializeDerivedDataSetTest()
{
JobDataSet_Accessor target = new JobDataSet_Accessor(); // TODO: Initialize to an appropriate value
    target.InitializeDerivedDataSet();
    Assert.Inconclusive("A method that does not return a value cannot be verified.");
}

/// <summary>
///A test for ReadXmlSerializable
///</summary>
[TestMethod()]
[DeploymentItem("Centipede.exe")]
public void ReadXmlSerializableTest()
{
JobDataSet_Accessor target = new JobDataSet_Accessor(); // TODO: Initialize to an appropriate value
XmlReader reader = null; // TODO: Initialize to an appropriate value
    target.ReadXmlSerializable(reader);
    Assert.Inconclusive("A method that does not return a value cannot be verified.");
}

/// <summary>
///A test for SchemaChanged
///</summary>
[TestMethod()]
[DeploymentItem("Centipede.exe")]
public void SchemaChangedTest()
{
JobDataSet_Accessor target = new JobDataSet_Accessor(); // TODO: Initialize to an appropriate value
object sender = null; // TODO: Initialize to an appropriate value
CollectionChangeEventArgs e = null; // TODO: Initialize to an appropriate value
    target.SchemaChanged(sender, e);
    Assert.Inconclusive("A method that does not return a value cannot be verified.");
}

/// <summary>
///A test for ShouldSerializeRelations
///</summary>
[TestMethod()]
[DeploymentItem("Centipede.exe")]
public void ShouldSerializeRelationsTest()
{
JobDataSet_Accessor target = new JobDataSet_Accessor(); // TODO: Initialize to an appropriate value
bool expected = false; // TODO: Initialize to an appropriate value
    bool actual;
    actual = target.ShouldSerializeRelations();
    Assert.AreEqual(expected, actual);
    Assert.Inconclusive("Verify the correctness of this test method.");
}

/// <summary>
///A test for ShouldSerializeTables
///</summary>
[TestMethod()]
[DeploymentItem("Centipede.exe")]
public void ShouldSerializeTablesTest()
{
JobDataSet_Accessor target = new JobDataSet_Accessor(); // TODO: Initialize to an appropriate value
bool expected = false; // TODO: Initialize to an appropriate value
    bool actual;
    actual = target.ShouldSerializeTables();
    Assert.AreEqual(expected, actual);
    Assert.Inconclusive("Verify the correctness of this test method.");
}

/// <summary>
///A test for ShouldSerializeVariables
///</summary>
[TestMethod()]
[DeploymentItem("Centipede.exe")]
public void ShouldSerializeVariablesTest()
{
JobDataSet_Accessor target = new JobDataSet_Accessor(); // TODO: Initialize to an appropriate value
bool expected = false; // TODO: Initialize to an appropriate value
    bool actual;
    actual = target.ShouldSerializeVariables();
    Assert.AreEqual(expected, actual);
    Assert.Inconclusive("Verify the correctness of this test method.");
}

/// <summary>
///A test for Relations
///</summary>
[TestMethod()]
public void RelationsTest()
{
JobDataSet target = new JobDataSet(); // TODO: Initialize to an appropriate value
    DataRelationCollection actual;
    actual = target.Relations;
    Assert.Inconclusive("Verify the correctness of this test method.");
}

/// <summary>
///A test for SchemaSerializationMode
///</summary>
[TestMethod()]
public void SchemaSerializationModeTest()
{
JobDataSet target = new JobDataSet(); // TODO: Initialize to an appropriate value
SchemaSerializationMode expected = new SchemaSerializationMode(); // TODO: Initialize to an appropriate value
    SchemaSerializationMode actual;
    target.SchemaSerializationMode = expected;
    actual = target.SchemaSerializationMode;
    Assert.AreEqual(expected, actual);
    Assert.Inconclusive("Verify the correctness of this test method.");
}

/// <summary>
///A test for Tables
///</summary>
[TestMethod()]
public void TablesTest()
{
JobDataSet target = new JobDataSet(); // TODO: Initialize to an appropriate value
    DataTableCollection actual;
    actual = target.Tables;
    Assert.Inconclusive("Verify the correctness of this test method.");
}

/// <summary>
///A test for Variables
///</summary>
[TestMethod()]
public void VariablesTest()
{
JobDataSet target = new JobDataSet(); // TODO: Initialize to an appropriate value
    JobDataSet.VariablesDataTable actual;
    actual = target.Variables;
    Assert.Inconclusive("Verify the correctness of this test method.");
}
    }
}
