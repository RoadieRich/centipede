using System.Collections.Generic;
using Centipede;
using CentipedeInterfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using PythonEngine;
using Rhino;
using Rhino.Mocks;
using Action = Centipede.Action;


namespace TestProject
{
    
    
    /// <summary>
    ///This is a test class for ActionTest and is intended
    ///to contain all ActionTest Unit Tests
    ///</summary>
    [TestClass()]
    [DeploymentItem("Action.dll")]
    public class ActionTest
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

        class ActionWrapper : Action
        {
            /// <summary>
            /// 
            /// </summary>
            /// <param name="mockCore"></param>
            public ActionWrapper(ICentipedeCore mockCore)
                : base("", null, mockCore)
            { }

            /// <summary>
            /// Perform the action
            /// </summary>
            /// <exception cref="ActionException">
            /// the action cannot be completed
            /// </exception>
            /// <exception cref="FatalActionException">The job needs to halt</exception>
            protected override void DoAction()
            { }

            public new string ParseStringForVariable(string str)
            {
                return base.ParseStringForVariable(str);
            }
        }

        

        
        [TestMethod]
        public void TestEmptyString()
        {
            MockRepository mocks = new MockRepository();
            ICentipedeCore mockCore = mocks.DynamicMock<ICentipedeCore>();
            IPythonEngine pythonEngine = mocks.DynamicMock<IPythonEngine>();
            
            mockCore.Stub(c => c.PythonEngine)
                    .Return(pythonEngine);

            pythonEngine.Stub(e => e.Compile("", PythonByteCode.SourceCodeType.AutoDetect))
                        .IgnoreArguments()
                        .Return(null);


            mocks.ReplayAll();

            ActionWrapper testAction = new ActionWrapper(mockCore);

            testAction.ParseStringForVariable("");

            //mocks.VerifyAll();

            pythonEngine.AssertWasNotCalled(e => e.Compile(Arg<String>.Is.Anything, Arg<PythonByteCode.SourceCodeType>.Is.Anything));
        }

        [TestMethod]
        public void TestStringWithNoCode()
        {
            MockRepository mocks = new MockRepository();
            ICentipedeCore mockCore = mocks.DynamicMock<ICentipedeCore>();
            IPythonEngine pythonEngine = mocks.DynamicMock<IPythonEngine>();

            mockCore.Stub(c => c.PythonEngine)
                    .Return(pythonEngine);

            pythonEngine.Stub(e => e.Compile("", PythonByteCode.SourceCodeType.AutoDetect))
                        .IgnoreArguments();
                  //.Return(null);

            mocks.ReplayAll();

            ActionWrapper testAction = new ActionWrapper(mockCore);

            var original = "A string without any code";

            var result = testAction.ParseStringForVariable(original);

            pythonEngine.AssertWasNotCalled(e => e.Compile("", PythonByteCode.SourceCodeType.Expression));
            Assert.AreEqual(original, result);
        }

        [TestMethod]
        public void TestStringWithEmptyBraces()
        {
            MockRepository mocks = new MockRepository();
            ICentipedeCore mockCore = mocks.DynamicMock<ICentipedeCore>();
            IPythonEngine pythonEngine = mocks.DynamicMock<IPythonEngine>();

            mockCore.Stub(c => c.PythonEngine)
                    .Return(pythonEngine);

            pythonEngine.Expect(e =>e.Compile(Arg<String>.Is.Equal(""),
                                                   Arg<PythonByteCode.SourceCodeType>.Is.Anything))
                    .Throw(new PythonParseException(mocks.Stub<Exception>()));
            
            mocks.ReplayAll();

            ActionWrapper testAction = new ActionWrapper(mockCore);

            var original = "String with {}";

            var result = testAction.ParseStringForVariable(original);

            mocks.VerifyAll();
            
            Assert.AreEqual(original, result);
        }

        [TestMethod]
        public void TestStringWithValidCode()
        {
            MockRepository mocks = new MockRepository();
            ICentipedeCore mockCore = mocks.DynamicMock<ICentipedeCore>();
            IPythonEngine pythonEngine = mocks.StrictMock<IPythonEngine>();
            IPythonByteCode mockPythonBytecode = mocks.Stub<IPythonByteCode>();

            mockCore.Stub(c => c.PythonEngine)
                    .Return(pythonEngine);

            pythonEngine.Expect(e => e.Compile(Arg<String>.Is.Equal("1+2"), 
                                               Arg<PythonByteCode.SourceCodeType>.Is.Anything))
                        .Return(mockPythonBytecode);

            pythonEngine.Expect(e => e.Evaluate(mockPythonBytecode)).Return(3);

            mocks.ReplayAll();

            ActionWrapper testAction = new ActionWrapper(mockCore);

            var original = "String with {1+2}";
            var expected = "String with 3";
            var result = testAction.ParseStringForVariable(original);

            mocks.VerifyAll();

            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void TestStringWithInvalidCode()
        {
            MockRepository mocks = new MockRepository();
            ICentipedeCore mockCore = mocks.DynamicMock<ICentipedeCore>();
            IPythonEngine pythonEngine = mocks.StrictMock<IPythonEngine>();

            mockCore.Stub(c => c.PythonEngine)
                    .Return(pythonEngine);

            pythonEngine.Expect(e => e.Compile(Arg<String>.Is.Equal("this is invalid python"),
                                               Arg<PythonByteCode.SourceCodeType>.Is.Anything))
                        .Throw(new PythonParseException(mocks.Stub<Exception>()));

            mocks.ReplayAll();

            ActionWrapper testAction = new ActionWrapper(mockCore);

            var original = "String with {this is invalid python}";
            
            var result = testAction.ParseStringForVariable(original);

            mocks.VerifyAll();

            Assert.AreEqual(original, result);
        }

        [TestMethod]
        public void TestStringWithValidAndInvalid()
        {
            MockRepository mocks = new MockRepository();
            ICentipedeCore mockCore = mocks.DynamicMock<ICentipedeCore>();
            IPythonEngine pythonEngine = mocks.StrictMock<IPythonEngine>();
            IPythonByteCode mockPythonByteCode = mocks.Stub<IPythonByteCode>();

            mockCore.Stub(c => c.PythonEngine)
                    .Return(pythonEngine);

            pythonEngine.Expect(e => e.Compile(Arg<String>.Is.Equal("\"String\""),
                                               Arg<PythonByteCode.SourceCodeType>.Is.Anything))
                        .Return(mockPythonByteCode);

            pythonEngine.Expect(e => e.Evaluate(Arg<IPythonByteCode>.Is.Equal(mockPythonByteCode),
                                                Arg<PythonScope>.Is.Anything))
                        .Return(3);

            pythonEngine.Expect(e => e.Compile(Arg<String>.Is.Equal("this is invalid python"),
                                               Arg<PythonByteCode.SourceCodeType>.Is.Anything))
                        .Throw(new PythonParseException(mocks.Stub<Exception>()));


            ActionWrapper testAction = new ActionWrapper(mockCore);

            var original = @"{1+2} with {invalid python}";
            var expected = "3 with {invalid python}";
            var result = testAction.ParseStringForVariable(original);

            mocks.VerifyAll();

            //mockCore.PythonEngine.AssertWasNotCalled(e => e.Evaluate(Arg<PythonByteCode>.Is.Anything, Arg<PythonScope>.Is.Anything));
            Assert.AreEqual(expected, result);
        }

    }
}
