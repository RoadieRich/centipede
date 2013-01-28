using System.Collections.Generic;
using Centipede.Actions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Reflection;
using Centipede;

namespace TestProject1
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
            FieldInfo field = typeof (TestAction).GetField("FieldArgument");
            FieldAndPropertyWrapper target = new FieldAndPropertyWrapper(field);
            PrivateObject privateObject = new PrivateObject(target);
            Assert.AreEqual(field, privateObject.GetFieldOrProperty("_member"));
        }

        /// <summary>
        ///A test for FieldAndPropertyWrapper Constructor
        ///</summary>
        [TestMethod()]
        public void FieldAndPropertyWrapperConstructorTest1()
        {
            PropertyInfo prop = typeof(TestAction).GetProperty("PropertyArgument");
            FieldAndPropertyWrapper target = new FieldAndPropertyWrapper(prop);
            PrivateObject privateObject = new PrivateObject(target);
            Assert.AreEqual(prop, privateObject.GetFieldOrProperty("_member"));
        }

        /// <summary>
        ///A test for Get
        ///</summary>
        public void GetTestHelper<T>()
        {
            PropertyInfo prop = typeof(TestAction).GetProperty("PropertyArgument");
            FieldAndPropertyWrapper target = new FieldAndPropertyWrapper(prop);
            object o = new TestAction(new Dictionary<string, object>());
            T expected = default(T);
            T actual;
            actual = target.Get<T>(o);
            Assert.AreEqual(expected, actual);
            //Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        /// 
        /// </summary>
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
            PropertyInfo prop = typeof(TestAction).GetProperty("PropertyArgument");
            FieldAndPropertyWrapper target = new FieldAndPropertyWrapper(prop);
            ActionArgumentAttribute actual;
            actual = target.GetArguementAttribute();
            Assert.IsNotNull(actual);
            Assert.IsInstanceOfType(actual, typeof (ActionArgumentAttribute));
        }

        /// <summary>
        ///A test for GetFieldTypeCategory
        ///</summary>
        [TestMethod()]
        public void GetFieldTypeCategoryTest()
        {
            PropertyInfo prop = typeof(TestAction).GetProperty("PropertyArgument");
            FieldAndPropertyWrapper target = new FieldAndPropertyWrapper(prop);
            FieldAndPropertyWrapper.FieldType expected = FieldAndPropertyWrapper.FieldType.String;
            FieldAndPropertyWrapper.FieldType actual;
            actual = target.GetFieldTypeCategory();
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for GetMemberType
        ///</summary>
        [TestMethod()]
        public void GetMemberTypeTest()
        {
            PropertyInfo prop = typeof(TestAction).GetProperty("PropertyArgument");
            FieldAndPropertyWrapper target = new FieldAndPropertyWrapper(prop); // TODO: Initialize to an appropriate value
            Type expected = typeof (string); // TODO: Initialize to an appropriate value
            Type actual;
            actual = target.MemberType;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Set
        ///</summary>
        public void SetTestHelper<T>()
        {
            PropertyInfo prop = typeof (TestAction).GetProperty("PropertyArgument");
            FieldAndPropertyWrapper target = new FieldAndPropertyWrapper(prop);
            TestAction action = new TestAction(new Dictionary<String, object>());
            T v = default(T);
            target.Set(action, v);
            
        }

        /// <summary>
        /// 
        /// </summary>
        [TestMethod()]
        public void SetTest()
        {
            SetTestHelper<GenericParameterHelper>();
        }

        /// <summary>
        ///A test for DeclaringType
        ///</summary>
        [TestMethod()]
        public void DeclaringTypeTest()
        {
            PropertyInfo prop = typeof(TestAction).GetProperty("PropertyArgument");
            FieldAndPropertyWrapper target = new FieldAndPropertyWrapper(prop);
            Type actual = target.DeclaringType;
            Assert.AreEqual(typeof (TestAction), actual);
        }

        /// <summary>
        ///A test for Name
        ///</summary>
        [TestMethod()]
        public void NameTest()
        {
            PropertyInfo prop = typeof(TestAction).GetProperty("PropertyArgument");
            FieldAndPropertyWrapper target = new FieldAndPropertyWrapper(prop); // TODO: Initialize to an appropriate value
            string actual = target.Name;
            Assert.AreEqual("PropertyArgument", actual);
        }
    }
}
