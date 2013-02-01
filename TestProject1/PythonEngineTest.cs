//using PythonEngine;

using System.Globalization;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Microsoft.Scripting;
using Microsoft.Scripting.Hosting;
using System.Collections.Generic;
using PythonEngine;
using Engine = PythonEngine.PythonEngine;

namespace TestProject1
{


    /// <summary>
    ///This is a test class for PythonEngineTest and is intended
    ///to contain all PythonEngineTest Unit Tests
    ///</summary>
    [TestClass]
    public class PythonEngineTest
    {
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

        #endregion

        ///Use TestCleanup to run code after each test has run
        [TestCleanup]
        public void MyTestCleanup()
        {
            PrivateType pt = new PrivateType(typeof (Engine));
            pt.SetStaticField("_instance", null);
        }


        /// <summary>
        ///A test for PythonEngine Constructor
        ///</summary>
        [TestMethod]
        [DeploymentItem("PythonEngine.dll")]
        public void PythonEngineConstructorTest()
        {
            PrivateType pt = new PrivateType(typeof (Engine));
            var valueBefore = pt.GetStaticField("_instance");
            var actual = Engine.Instance;
            Assert.AreNotEqual(valueBefore, actual);
            Assert.AreEqual(actual, pt.GetStaticField("_instance"));
            Assert.IsInstanceOfType(actual, typeof (Engine));
        }

        /// <summary>
        ///A test for Compile
        ///</summary>
        [TestMethod]
        public void CompileTest_EmptyString()
        {
            Engine target = Engine.Instance;
            string code = String.Empty;
            const SourceCodeKind kind = SourceCodeKind.Expression;

            bool thrown = false;
            CompiledCode compiledCode = null;
            try
            {
                compiledCode = target.Compile(code, kind);
            }
            catch (Exception e)
            {
                thrown = true;
                Assert.IsInstanceOfType(e, typeof (SyntaxErrorException));

            }
            Assert.IsNull(compiledCode);
            Assert.IsTrue(thrown, "Attempting to compile empty string didn't throw");
        }

        /// <summary>
        ///A test for Compile
        ///</summary>
        [TestMethod]
        public void CompileTest_PassStatementAsExpression()
        {
            Engine target = Engine.Instance;
            const string code = @"print 'hello world'";
            const SourceCodeKind kind = SourceCodeKind.Expression;
            bool thrown = false;
            CompiledCode compiledCode = null;
            try
            {
                compiledCode = target.Compile(code, kind);
            }
            catch (Exception e)
            {
                thrown = true;
                Assert.IsInstanceOfType(e, typeof (SyntaxErrorException));
            }

            Assert.IsNull(compiledCode);
            Assert.IsTrue(thrown, "Attempting to compile empty string didn't throw");
        }

        /// <summary>
        ///A test for Compile
        ///</summary>
        [TestMethod]
        public void CompileTest()
        {
            PythonEngine.PythonEngine target = PythonEngine.PythonEngine.Instance;
            const string code = @"print 'hello world'";
            const SourceCodeKind kind = SourceCodeKind.SingleStatement;
            CompiledCode actual = target.Compile(code, kind);
            Assert.IsNotNull(actual);

        }

        /// <summary>
        ///A test for Evaluate
        ///</summary>
        [TestMethod]
        public void EvaluateTestHelper()
        {
            Engine pye = Engine.Instance;
            int expected = TestHelpers.RandomInt;
            String expression = expected.ToString(CultureInfo.InvariantCulture);
            int actual = pye.Evaluate<int>(expression);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Evaluate
        ///</summary>
        [TestMethod]
        public void EvaluateTestHelperWithScope()
        {
            Engine pye = Engine.Instance;
            string name = TestHelpers.RandomName();
            string expression = name;

            int expected = TestHelpers.RandomInt;
            int actual = pye.Evaluate<int>(expression, Engine.Instance.GetNewScope(new Dictionary<string, object>
                                                                                   {
                                                                                           { name, expected }
                                                                                   }));
            Assert.AreEqual(expected, actual);
        }


        /// <summary>
        ///A test for Evaluate
        ///</summary>
        [TestMethod]
        public void EvaluateTestHelperWithOutOfScopeVariable()
        {
            Engine pye = Engine.Instance;
            string expression = TestHelpers.RandomName();

            bool thrown = false;
            try
            {
                pye.Evaluate<int>(expression, Engine.Instance.GetNewScope(new Dictionary<string, object>()));
            }
            catch (Exception e)
            {
                thrown = true;
                Assert.IsInstanceOfType(e, typeof (PythonException));
            }

            Assert.IsTrue(thrown);
        }

        /// <summary>
        ///A test for Execute
        ///</summary>
        [TestMethod]
        public void ExecuteTest()
        {
            Engine engine = Engine.Instance;
            string varName = TestHelpers.RandomName();
            Double expected = TestHelpers.RandomFloat;
            string code = string.Format("{0} = {1}", varName, expected);
            engine.SetVariable(varName, 1 - expected);
            engine.Execute(code);
            Double actual = engine.GetVariable<Double>(varName);
            Assert.AreEqual(expected, actual);
        }


        /// <summary>
        ///A test for Execute
        ///</summary>
        [TestMethod]
        public void ExecuteEvaluateTest()
        {
            Engine engine = Engine.Instance;
            string fibsCode = String.Join("\n",
                                      new[]
                                      {
                                              @"def fibs(n):",
                                              @"    a = 0",
                                              @"    b = 1",
                                              @"    for i in range(n):",
                                              @"        c = b",
                                              @"        b = a + b",
                                              @"        a = c",
                                              @"    return a"
                                      });
            engine.Execute(fibsCode);
            const string fibsCall = "fibs({0})";
            const int runs = 10;
            Func<int, int> csFibs = n =>
                                        {
                                            int a = 0, b = 1;
                                            for (int i = 0; i < n; i++)
                                            {
                                                int c = b;
                                                b = a + b;
                                                a = c;
                                            }
                                            return a;
                                        };

            for (int i = 1; i < runs; i++)
            {
                string code = String.Format(fibsCall, i);
                int actual = engine.Evaluate<int>(code);
                int expected = csFibs(i);
                Assert.AreEqual(expected, actual, "Result for {0} incorrect, expected {1}, got {2}", i, expected,actual);
            }
        }

        /// <summary>
        ///A test for GetNewScope
        ///</summary>
        [TestMethod]
        public void GetNewScopeTest()
        {

            const string expected = @"EXPECTED";
            string name = TestHelpers.RandomName();
            Dictionary<string, object> variables = new Dictionary<string, object>
                                                   {
                                                           {name, expected}
                                                   };
            PythonScope scope = Engine.Instance.GetNewScope(variables);

            Engine.Instance.SetVariable(name, "unexpected");

            String actual = Engine.Instance.Evaluate<String>(name, scope);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for GetVariable, getting var set from c#
        ///</summary>
        [TestMethod]
        public void GetVariableTest1()
        {
            string name = TestHelpers.RandomName();
            object expected = TestHelpers.RandomString(10); // TODO: Initialize to an appropriate value
            Engine.Instance.SetVariable(name, expected);
            object actual = Engine.Instance.GetVariable(name);
            Assert.AreEqual(expected, actual);
        }


        /// <summary>
        ///A test for GetVariable, getting var set from python
        ///</summary>
        [TestMethod]
        public void GetVariableTest2()
        {
            string name = TestHelpers.RandomName(); 
            object expected = TestHelpers.RandomString(10);
            Engine.Instance.Execute(String.Format(@"{0} = ""{1}""", name, expected));
            object actual = Engine.Instance.GetVariable(name);
            Assert.AreEqual(expected, actual);
        }
        /// <summary>
        ///A test for SetVariable
        ///</summary>
        [TestMethod]
        public void SetVariableTest()
        {
            string name = TestHelpers.RandomName();
            const int expected = 42;
            Engine.Instance.SetVariable(name, expected);
            int actual = Engine.Instance.Evaluate<int>(name);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for VariableExists using var set from c#
        ///</summary>
        [TestMethod]
        public void VariableExistsTest1()
        {
            string name = TestHelpers.RandomName();
            Engine.Instance.SetVariable(name, 42);
            Assert.IsTrue(Engine.Instance.VariableExists(name));
        }

        /// <summary>
        ///A test for VariableExists using var set from python
        ///</summary>
        [TestMethod]
        public void VariableExistsTest2()
        {
            string name = TestHelpers.RandomName();
            Engine.Instance.Execute(String.Format(@"{0} = 42", name));
            Assert.IsTrue(Engine.Instance.VariableExists(name));
        }

        /// <summary>
        ///A test for VariableExists using var set from custom scope - shouldn't exist in engine afterwards
        ///</summary>
        [TestMethod]
        public void VariableExistsTest3()
        {
            string name = TestHelpers.RandomName();
            var scope = Engine.Instance.GetNewScope(new Dictionary<string, object> { { name, 42 } });
            Engine.Instance.Execute(string.Format(@"{0} = 3", name), scope);
            Assert.IsTrue(scope.ContainsVariable(name));
            Assert.IsFalse(Engine.Instance.VariableExists(name));
        }

        /// <summary>
        ///A test for Instance
        ///</summary>
        [TestMethod]
        public void InstanceTest()
        {
            PythonEngine.PythonEngine expected = PythonEngine.PythonEngine.Instance;

            for (int i = 0; i < 10; i++)
            {

                Thread t = new Thread(() =>
                                          {
                                              PythonEngine.PythonEngine actual = PythonEngine.PythonEngine.Instance;
                                              Assert.AreSame(expected,actual);
                                          });
                t.Start();
            }
        }
    }

}
