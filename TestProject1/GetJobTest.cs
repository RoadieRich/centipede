using Centipede;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Windows.Forms;
using System.ComponentModel;

namespace TestProject1
{
    
    
    /// <summary>
    ///This is a test class for GetJobTest and is intended
    ///to contain all GetJobTest Unit Tests
    ///</summary>
    [TestClass()]
    public class GetJobTest
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
        ///A test for AddFavourite
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Centipede.exe")]
        public void AddFavouriteTest()
        {
            GetJob_Accessor target = new GetJob_Accessor(); // TODO: Initialize to an appropriate value
            string filename = string.Empty; // TODO: Initialize to an appropriate value
            target.AddFavourite(filename);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for FavouritesListbox_MouseDoubleClick
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Centipede.exe")]
        public void FavouritesListbox_MouseDoubleClickTest()
        {
            GetJob_Accessor target = new GetJob_Accessor(); // TODO: Initialize to an appropriate value
            object sender = null; // TODO: Initialize to an appropriate value
            MouseEventArgs e = null; // TODO: Initialize to an appropriate value
            target.FavouritesListbox_MouseDoubleClick(sender, e);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for FavouritesListbox_SelectedIndexChanged
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Centipede.exe")]
        public void FavouritesListbox_SelectedIndexChangedTest()
        {
            GetJob_Accessor target = new GetJob_Accessor(); // TODO: Initialize to an appropriate value
            object sender = null; // TODO: Initialize to an appropriate value
            EventArgs e = null; // TODO: Initialize to an appropriate value
            //target.FavouritesListbox_SelectedIndexChanged(sender, e);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for BrowseButton_Click
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Centipede.exe")]
        public void BrowseButton_ClickTest()
        {
            GetJob_Accessor target = new GetJob_Accessor(); // TODO: Initialize to an appropriate value
            object sender = null; // TODO: Initialize to an appropriate value
            EventArgs e = null; // TODO: Initialize to an appropriate value
            target.BrowseButton_Click(sender, e);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for BrowseLoadDialogue_FileOk
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Centipede.exe")]
        public void BrowseLoadDialogue_FileOkTest()
        {
            GetJob_Accessor target = new GetJob_Accessor(); // TODO: Initialize to an appropriate value
            object sender = null; // TODO: Initialize to an appropriate value
            CancelEventArgs e = null; // TODO: Initialize to an appropriate value
            target.BrowseLoadDialogue_FileOk(sender, e);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for GetFaveFilename
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Centipede.exe")]
        public void GetFaveFilenameTest()
        {
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual = GetJob_Accessor.GetFaveFilename();
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for GetJobFileName
        ///</summary>
        [TestMethod()]
        public void GetJobFileNameTest()
        {
            GetJob target = new GetJob(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual = target.GetJobFileName();
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for GetJob_FormClosing
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Centipede.exe")]
        public void GetJob_FormClosingTest()
        {
            GetJob_Accessor target = new GetJob_Accessor(); // TODO: Initialize to an appropriate value
            object sender = null; // TODO: Initialize to an appropriate value
            FormClosingEventArgs e = null; // TODO: Initialize to an appropriate value
            //target.GetJob_FormClosing(sender, e);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for GetJob_Load
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Centipede.exe")]
        public void GetJob_LoadTest()
        {
            GetJob_Accessor target = new GetJob_Accessor(); // TODO: Initialize to an appropriate value
            object sender = null; // TODO: Initialize to an appropriate value
            EventArgs e = null; // TODO: Initialize to an appropriate value
            target.GetJob_Load(sender, e);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for LoadButton_Click
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Centipede.exe")]
        public void LoadButton_ClickTest()
        {
            GetJob_Accessor target = new GetJob_Accessor(); // TODO: Initialize to an appropriate value
            object sender = null; // TODO: Initialize to an appropriate value
            EventArgs e = null; // TODO: Initialize to an appropriate value
            target.LoadButton_Click(sender, e);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for NewButton_Click
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Centipede.exe")]
        public void NewButton_ClickTest()
        {
            GetJob_Accessor target = new GetJob_Accessor(); // TODO: Initialize to an appropriate value
            object sender = null; // TODO: Initialize to an appropriate value
            EventArgs e = null; // TODO: Initialize to an appropriate value
            target.NewButton_Click(sender, e);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for OtherButton_Click
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Centipede.exe")]
        public void OtherButton_ClickTest()
        {
            GetJob_Accessor target = new GetJob_Accessor(); // TODO: Initialize to an appropriate value
            object sender = null; // TODO: Initialize to an appropriate value
            EventArgs e = null; // TODO: Initialize to an appropriate value
            target.OtherButton_Click(sender, e);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for OtherOpenDialogue_FileOk
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Centipede.exe")]
        public void OtherOpenDialogue_FileOkTest()
        {
            GetJob_Accessor target = new GetJob_Accessor(); // TODO: Initialize to an appropriate value
            object sender = null; // TODO: Initialize to an appropriate value
            CancelEventArgs e = null; // TODO: Initialize to an appropriate value
            target.OtherOpenDialogue_FileOk(sender, e);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }
    }
}
