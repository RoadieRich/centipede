﻿using Centipede.Actions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace CentipedeTest
{
    
    
    /// <summary>
    ///This is a test class for VariableActionTest and is intended
    ///to contain all VariableActionTest Unit Tests
    ///</summary>
    [TestClass()]
    public class VariableActionTest
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
///A test for VariableAction Constructor
///</summary>
[TestMethod()]
public void VariableActionConstructorTest()
{
Dictionary<string, object> v = null; // TODO: Initialize to an appropriate value
    VariableAction target = new VariableAction(v);
    Assert.Inconclusive("TODO: Implement code to verify target");
}

/// <summary>
///A test for DoAction
///</summary>
[TestMethod()]
[DeploymentItem("Centipede.exe")]
public void DoActionTest()
{
PrivateObject param0 = null; // TODO: Initialize to an appropriate value
VariableAction_Accessor target = new VariableAction_Accessor(param0); // TODO: Initialize to an appropriate value
    target.DoAction();
    Assert.Inconclusive("A method that does not return a value cannot be verified.");
}
    }
}
