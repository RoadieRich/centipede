using Centipede;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Windows.Forms;
using System.Data;
using Centipede.Actions;
using System.ComponentModel;
using Action = Centipede.Action;


namespace TestProject1
{
    
    
    /// <summary>
    ///This is a test class for MainWindowTest and is intended
    ///to contain all MainWindowTest Unit Tests
    ///</summary>
    [TestClass()]
    public class MainWindowTest
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
        ///A test for ActionContainer_DragDrop
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Centipede.exe")]
        public void ActionContainer_DragDropTest()
        {
            MainWindow_Accessor target = new MainWindow_Accessor(); // TODO: Initialize to an appropriate value
            object sender = null; // TODO: Initialize to an appropriate value
            DragEventArgs e = null; // TODO: Initialize to an appropriate value
            target.ActionContainer_DragDrop(sender, e);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for ActionContainer_DragEnter
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Centipede.exe")]
        public void ActionContainer_DragEnterTest()
        {
            MainWindow_Accessor target = new MainWindow_Accessor(); // TODO: Initialize to an appropriate value
            object sender = null; // TODO: Initialize to an appropriate value
            DragEventArgs e = null; // TODO: Initialize to an appropriate value
            target.ActionContainer_DragEnter(sender, e);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for AddToActionTab
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Centipede.exe")]
        public void AddToActionTabTest()
        {
            MainWindow_Accessor target = new MainWindow_Accessor(); // TODO: Initialize to an appropriate value
            Type pluginType = null; // TODO: Initialize to an appropriate value
            target.AddToActionTab(pluginType);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for BeginDrag
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Centipede.exe")]
        public void BeginDragTest()
        {
            MainWindow_Accessor target = new MainWindow_Accessor(); // TODO: Initialize to an appropriate value
            object sender = null; // TODO: Initialize to an appropriate value
            ItemDragEventArgs e = null; // TODO: Initialize to an appropriate value
            target.BeginDrag(sender, e);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for CompletedHandler
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Centipede.exe")]
        public void CompletedHandlerTest()
        {
            MainWindow_Accessor target = new MainWindow_Accessor(); // TODO: Initialize to an appropriate value
            const bool success = false; // TODO: Initialize to an appropriate value
            //target.CompletedHandler(success);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for DataStore_Variables_RowDeleted
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Centipede.exe")]
        public void DataStore_Variables_RowDeletedTest()
        {
            MainWindow_Accessor target = new MainWindow_Accessor(); // TODO: Initialize to an appropriate value
            object sender = null; // TODO: Initialize to an appropriate value
            DataRowChangeEventArgs e = null; // TODO: Initialize to an appropriate value
            target.DataStore_Variables_RowDeleted(sender, e);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for ErrorHandler
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Centipede.exe")]
        public void ErrorHandlerTest()
        {
            MainWindow_Accessor target = new MainWindow_Accessor(); // TODO: Initialize to an appropriate value
            ActionException e = null; // TODO: Initialize to an appropriate value
            Action nextAction = null; // TODO: Initialize to an appropriate value
            Action nextActionExpected = null; // TODO: Initialize to an appropriate value
            const bool expected = false; // TODO: Initialize to an appropriate value

            bool actual = target.ErrorHandler(e, out nextAction);
            Assert.AreEqual(nextActionExpected, nextAction);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for GenerateNewTabPage
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Centipede.exe")]
        public void GenerateNewTabPageTest()
        {
            MainWindow_Accessor target = new MainWindow_Accessor(); // TODO: Initialize to an appropriate value
            string category = string.Empty; // TODO: Initialize to an appropriate value
            ListView expected = null; // TODO: Initialize to an appropriate value
            ListView actual = target.GenerateNewTabPage(category);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for GetActionPlugins
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Centipede.exe")]
        public void GetActionPluginsTest()
        {
            MainWindow_Accessor target = new MainWindow_Accessor(); // TODO: Initialize to an appropriate value
            target.GetActionPlugins();
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for ItemActivate
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Centipede.exe")]
        public void ItemActivateTest()
        {
            MainWindow_Accessor target = new MainWindow_Accessor(); // TODO: Initialize to an appropriate value
            object sender = null; // TODO: Initialize to an appropriate value
            EventArgs e = null; // TODO: Initialize to an appropriate value
            target.ItemActivate(sender, e);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for LoadBtn_Click
        ///</summary>
        //[TestMethod()]
        //[DeploymentItem("Centipede.exe")]
        //public void LoadBtn_ClickTest()
        //{
        //    MainWindow_Accessor target = new MainWindow_Accessor(); // TODO: Initialize to an appropriate value
        //    object sender = null; // TODO: Initialize to an appropriate value
        //    EventArgs e = null; // TODO: Initialize to an appropriate value
        //    target.LoadBtn_Click(sender, e);
        //    Assert.Inconclusive("A method that does not return a value cannot be verified.");
        //}

        /// <summary>
        ///A test for LoadJob
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Centipede.exe")]
        public void LoadJobTest()
        {
            MainWindow_Accessor target = new MainWindow_Accessor(); // TODO: Initialize to an appropriate value
            //target.LoadJob();
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for MainWindow_Load
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Centipede.exe")]
        public void MainWindow_LoadTest()
        {
            MainWindow_Accessor target = new MainWindow_Accessor(); // TODO: Initialize to an appropriate value
            object sender = null; // TODO: Initialize to an appropriate value
            EventArgs e = null; // TODO: Initialize to an appropriate value
            target.MainWindow_Load(sender, e);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for MainWindow_PreviewKeyDown
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Centipede.exe")]
        public void MainWindow_PreviewKeyDownTest()
        {
            MainWindow_Accessor target = new MainWindow_Accessor(); // TODO: Initialize to an appropriate value
            object sender = null; // TODO: Initialize to an appropriate value
            PreviewKeyDownEventArgs e = null; // TODO: Initialize to an appropriate value
            target.MainWindow_PreviewKeyDown(sender, e);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for Program_ActionAdded
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Centipede.exe")]
        public void Program_ActionAddedTest()
        {
            MainWindow_Accessor target = new MainWindow_Accessor(); // TODO: Initialize to an appropriate value
            Action action = null; // TODO: Initialize to an appropriate value
            const int index = 0; // TODO: Initialize to an appropriate value
            target.Program_ActionAdded(action, index);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for Program_ActionRemoved
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Centipede.exe")]
        public void Program_ActionRemovedTest()
        {
            MainWindow_Accessor target = new MainWindow_Accessor(); // TODO: Initialize to an appropriate value
            Action action = null; // TODO: Initialize to an appropriate value
            target.Program_ActionRemoved(action);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for Program_AfterLoad
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Centipede.exe")]
        public void Program_AfterLoadTest()
        {
            MainWindow_Accessor target = new MainWindow_Accessor(); // TODO: Initialize to an appropriate value
            object sender = null; // TODO: Initialize to an appropriate value
            EventArgs e = null; // TODO: Initialize to an appropriate value
            target.Program_AfterLoad(sender, e);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for Program_BeforeAction
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Centipede.exe")]
        public void Program_BeforeActionTest()
        {
            MainWindow_Accessor target = new MainWindow_Accessor(); // TODO: Initialize to an appropriate value
            Action action = null; // TODO: Initialize to an appropriate value
            //target.Program_BeforeAction(action);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for RunButton_Click
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Centipede.exe")]
        public void RunButton_ClickTest()
        {
            MainWindow_Accessor target = new MainWindow_Accessor(); // TODO: Initialize to an appropriate value
            object sender = null; // TODO: Initialize to an appropriate value
            EventArgs e = null; // TODO: Initialize to an appropriate value
            target.RunButton_Click(sender, e);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for SaveJob
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Centipede.exe")]
        public void SaveJobTest()
        {
            MainWindow_Accessor target = new MainWindow_Accessor(); // TODO: Initialize to an appropriate value
            target.SaveJob();
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }
        
        /// <summary>
        ///A test for UpdateHandlerDone
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Centipede.exe")]
        public void UpdateHandlerDoneTest()
        {
            MainWindow_Accessor target = new MainWindow_Accessor(); // TODO: Initialize to an appropriate value
            Action currentAction = null; // TODO: Initialize to an appropriate value
            target.UpdateHandlerDone(currentAction);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for VarDataGridView_CellContextMenuStripNeeded
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Centipede.exe")]
        public void VarDataGridView_CellContextMenuStripNeededTest()
        {
            MainWindow_Accessor target = new MainWindow_Accessor(); // TODO: Initialize to an appropriate value
            object sender = null; // TODO: Initialize to an appropriate value
            DataGridViewCellContextMenuStripNeededEventArgs e = null; // TODO: Initialize to an appropriate value
            target.VarDataGridView_CellContextMenuStripNeeded(sender, e);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for VarMenuDelete_Click
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Centipede.exe")]
        public void VarMenuDelete_ClickTest()
        {
            MainWindow_Accessor target = new MainWindow_Accessor(); // TODO: Initialize to an appropriate value
            object sender = null; // TODO: Initialize to an appropriate value
            EventArgs e = null; // TODO: Initialize to an appropriate value
            target.VarMenuDelete_Click(sender, e);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for Variables_VariablesRowChanged
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Centipede.exe")]
        public void Variables_VariablesRowChangedTest()
        {
            MainWindow_Accessor target = new MainWindow_Accessor(); // TODO: Initialize to an appropriate value
            object sender = null; // TODO: Initialize to an appropriate value
            JobDataSet.VariablesRowChangeEvent e = null; // TODO: Initialize to an appropriate value
            target.Variables_VariablesRowChanged(sender, e);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for adc_Deleted
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Centipede.exe")]
        public void adc_DeletedTest()
        {
            MainWindow_Accessor target = new MainWindow_Accessor(); // TODO: Initialize to an appropriate value
            object sender = null; // TODO: Initialize to an appropriate value
            CentipedeEventArgs e = null; // TODO: Initialize to an appropriate value
            //target.adc_Deleted(sender, e);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for backgroundWorker1_DoWork
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Centipede.exe")]
        public void backgroundWorker1_DoWorkTest()
        {
            MainWindow_Accessor target = new MainWindow_Accessor(); // TODO: Initialize to an appropriate value
            object sender = null; // TODO: Initialize to an appropriate value
            DoWorkEventArgs e = null; // TODO: Initialize to an appropriate value
            target.backgroundWorker1_DoWork(sender, e);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for saveFileDialog1_FileOk
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Centipede.exe")]
        public void saveFileDialog1_FileOkTest()
        {
            MainWindow_Accessor target = new MainWindow_Accessor(); // TODO: Initialize to an appropriate value
            object sender = null; // TODO: Initialize to an appropriate value
            CancelEventArgs e = null; // TODO: Initialize to an appropriate value
            target.saveFileDialog1_FileOk(sender, e);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for backgroundWorker1_ProgressChanged
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Centipede.exe")]
        public void backgroundWorker1_ProgressChangedTest()
        {
            MainWindow_Accessor target = new MainWindow_Accessor(); // TODO: Initialize to an appropriate value
            object sender = null; // TODO: Initialize to an appropriate value
            ProgressChangedEventArgs e = null; // TODO: Initialize to an appropriate value
            target.backgroundWorker1_ProgressChanged(sender, e);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }
    }
}
