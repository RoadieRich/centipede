using Centipede.Actions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Reflection;
using Centipede;

namespace CentipedeTest
{
    
    
    /// <summary>
    ///This is a test class for FieldAndPropertyWrapperTest and is intended
    ///to contain all FieldAndPropertyWrapperTest Unit Tests
    ///</summary>
    [TestClass()]
    public class FieldAndPropertyWrapperTest
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
///A test for FieldAndPropertyWrapper Constructor
///</summary>
[TestMethod()]
public void FieldAndPropertyWrapperConstructorTest()
{
FieldInfo field = null; // TODO: Initialize to an appropriate value
    FieldAndPropertyWrapper target = new FieldAndPropertyWrapper(field);
    Assert.Inconclusive("TODO: Implement code to verify target");
}

/// <summary>
///A test for FieldAndPropertyWrapper Constructor
///</summary>
[TestMethod()]
public void FieldAndPropertyWrapperConstructorTest1()
{
PropertyInfo prop = null; // TODO: Initialize to an appropriate value
    FieldAndPropertyWrapper target = new FieldAndPropertyWrapper(prop);
    Assert.Inconclusive("TODO: Implement code to verify target");
}

/// <summary>
///A test for Get
///</summary>
public void GetTestHelper<T>()

{
PropertyInfo prop = null; // TODO: Initialize to an appropriate value
FieldAndPropertyWrapper target = new FieldAndPropertyWrapper(prop); // TODO: Initialize to an appropriate value
object o = null; // TODO: Initialize to an appropriate value
T expected = default(T); // TODO: Initialize to an appropriate value
    T actual;
    actual = target.Get<T>(o);
    Assert.AreEqual(expected, actual);
    Assert.Inconclusive("Verify the correctness of this test method.");
}

[TestMethod()]
public void GetTest()
{
    GetTestHelper<GenericParameterHelper>();
}

/// <summary>
///A test for GetArguementAttribute
///</summary>
[TestMethod()]
public void GetArguementAttributeTest()
{
PropertyInfo prop = null; // TODO: Initialize to an appropriate value
FieldAndPropertyWrapper target = new FieldAndPropertyWrapper(prop); // TODO: Initialize to an appropriate value
ActionArgumentAttribute expected = null; // TODO: Initialize to an appropriate value
    ActionArgumentAttribute actual;
    actual = target.GetArguementAttribute();
    Assert.AreEqual(expected, actual);
    Assert.Inconclusive("Verify the correctness of this test method.");
}

/// <summary>
///A test for GetFieldTypeCategory
///</summary>
[TestMethod()]
public void GetFieldTypeCategoryTest()
{
PropertyInfo prop = null; // TODO: Initialize to an appropriate value
FieldAndPropertyWrapper target = new FieldAndPropertyWrapper(prop); // TODO: Initialize to an appropriate value
FieldAndPropertyWrapper.FieldType expected = new FieldAndPropertyWrapper.FieldType(); // TODO: Initialize to an appropriate value
    FieldAndPropertyWrapper.FieldType actual;
    actual = target.GetFieldTypeCategory();
    Assert.AreEqual(expected, actual);
    Assert.Inconclusive("Verify the correctness of this test method.");
}

/// <summary>
///A test for GetMemberType
///</summary>
[TestMethod()]
public void GetMemberTypeTest()
{
PropertyInfo prop = null; // TODO: Initialize to an appropriate value
FieldAndPropertyWrapper target = new FieldAndPropertyWrapper(prop); // TODO: Initialize to an appropriate value
Type expected = null; // TODO: Initialize to an appropriate value
    Type actual;
    actual = target.GetMemberType();
    Assert.AreEqual(expected, actual);
    Assert.Inconclusive("Verify the correctness of this test method.");
}

/// <summary>
///A test for Set
///</summary>
public void SetTestHelper<T>()

{
PropertyInfo prop = null; // TODO: Initialize to an appropriate value
FieldAndPropertyWrapper target = new FieldAndPropertyWrapper(prop); // TODO: Initialize to an appropriate value
object o = null; // TODO: Initialize to an appropriate value
T v = default(T); // TODO: Initialize to an appropriate value
    target.Set<T>(o, v);
    Assert.Inconclusive("A method that does not return a value cannot be verified.");
}

[TestMethod()]
public void SetTest()
{
    SetTestHelper<GenericParameterHelper>();
}

/// <summary>
///A test for op_Explicit
///</summary>
[TestMethod()]
public void op_ExplicitTest()
{
MemberInfo m = null; // TODO: Initialize to an appropriate value
FieldAndPropertyWrapper expected = null; // TODO: Initialize to an appropriate value
    FieldAndPropertyWrapper actual;
    actual = ((FieldAndPropertyWrapper)(m));
    Assert.AreEqual(expected, actual);
    Assert.Inconclusive("Verify the correctness of this test method.");
}

/// <summary>
///A test for op_Implicit
///</summary>
[TestMethod()]
public void op_ImplicitTest()
{
FieldInfo f = null; // TODO: Initialize to an appropriate value
FieldAndPropertyWrapper expected = null; // TODO: Initialize to an appropriate value
    FieldAndPropertyWrapper actual;
    actual = f;
    Assert.AreEqual(expected, actual);
    Assert.Inconclusive("Verify the correctness of this test method.");
}

/// <summary>
///A test for op_Implicit
///</summary>
[TestMethod()]
public void op_ImplicitTest1()
{
PropertyInfo p = null; // TODO: Initialize to an appropriate value
FieldAndPropertyWrapper expected = null; // TODO: Initialize to an appropriate value
    FieldAndPropertyWrapper actual;
    actual = p;
    Assert.AreEqual(expected, actual);
    Assert.Inconclusive("Verify the correctness of this test method.");
}

/// <summary>
///A test for DeclaringType
///</summary>
[TestMethod()]
public void DeclaringTypeTest()
{
PropertyInfo prop = null; // TODO: Initialize to an appropriate value
FieldAndPropertyWrapper target = new FieldAndPropertyWrapper(prop); // TODO: Initialize to an appropriate value
    Type actual;
    actual = target.DeclaringType;
    Assert.Inconclusive("Verify the correctness of this test method.");
}

/// <summary>
///A test for Name
///</summary>
[TestMethod()]
public void NameTest()
{
PropertyInfo prop = null; // TODO: Initialize to an appropriate value
FieldAndPropertyWrapper target = new FieldAndPropertyWrapper(prop); // TODO: Initialize to an appropriate value
    string actual;
    actual = target.Name;
    Assert.Inconclusive("Verify the correctness of this test method.");
}
    }
}
