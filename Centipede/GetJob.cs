using System;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using Microsoft.Win32;

namespace Centipede
{
    internal partial class GetJob : Form
    {
        public GetJob()
        {
            InitializeComponent();
        }
        public GetJobResult Result;

        
        internal string GetJobFileName()
        {
            switch (Result)
            {
            case GetJobResult.New:
                return "";
            case GetJobResult.Open:
                var selectedJob = FavouritesListbox.SelectedItem as JobControl;
                if (selectedJob == null)
                {
                    throw new NullReferenceException("selectedJob");
                }
                return selectedJob.Filename;
            case GetJobResult.Other:
                return OtherOpenDialogue.FileName;
            default:
                return null;
            }
        }

        private void BrowseButton_Click(object sender, EventArgs e)
        {
            if (BrowseLoadDialogue != null)
                BrowseLoadDialogue.ShowDialog(this);
        }

        private void AddFavourite(string filename)
        {
            FavouritesListbox.Items.Add(new JobControl(filename));
            String faveFilename = GetFaveFilename();
            var xmlDoc = new XmlDocument();
            
            using (FileStream xmlFile = File.Open(faveFilename, FileMode.OpenOrCreate, FileAccess.Read))
            {
                xmlDoc.Load(xmlFile);
            }
            
            XmlElement jobEle = xmlDoc.CreateElement("Job");
            jobEle.SetAttribute("Filename", filename);
            xmlDoc.GetElementsByTagName("favourites")[0].AppendChild(jobEle);

            using (FileStream xmlFile = File.Open(faveFilename, FileMode.OpenOrCreate, FileAccess.Write))
            {
                xmlDoc.Save(XmlWriter.Create(xmlFile));
            }
            
        }

        private void NewButton_Click(object sender, EventArgs e)
        {
            Result = GetJobResult.New;
            Close();
        }

        private void LoadButton_Click(object sender, EventArgs e)
        {
            Result = GetJobResult.Open;
            Close();
        }

        private void GetJob_Load(object sender, EventArgs e)
        {
            var xmlDoc = new XmlDocument();

            String filename = GetFaveFilename();

            using (FileStream xmlFile = File.Open(filename, FileMode.OpenOrCreate, FileAccess.Read))
            {
                xmlDoc.Load(xmlFile);

                if (!xmlDoc.HasChildNodes)
                {
                    return;
                }
                XmlElement faveXmlStoreElement = xmlDoc.GetElementsByTagName("favourites")[0] as XmlElement;
                if (faveXmlStoreElement == null)
                {
                    throw new ArgumentNullException("sender");
                }
                foreach (XmlElement favouriteItem in faveXmlStoreElement.ChildNodes)
                {
                    FavouritesListbox.Items.Add(new JobControl(favouriteItem.GetAttribute("Filename")));
                }
            }
        }

        private static String GetFaveFilename()
        {
            var filename = Registry.CurrentUser.GetValue(@"Chemineer\Centipede\FavouriteFile") as String;

            if (filename == null)
            {
                Registry.CurrentUser.SetValue(@"Chemineer\Centipede\FavouriteFile", @"%APPDATA%\Centipede\favourites.xml");
                filename = Environment.ExpandEnvironmentVariables(@"%APPDATA%\Centipede\favourites.xml");

                if (!File.Exists(filename))
                {
                    var file = new StreamWriter(File.Create(filename));
                    file.Write(@"<?xml version=""1.0"" encoding=""utf-8""?><favourites />");
                    file.Close();
                }
            }
            return Environment.ExpandEnvironmentVariables(filename);
        }

        private void OtherButton_Click(object sender, EventArgs e)
        {
            OtherOpenDialogue.ShowDialog(this);
        }

        private void BrowseLoadDialogue_FileOk(object sender, CancelEventArgs e)
        {
            var dialog = sender as FileDialog;
            if (dialog == null)
            {
                throw new ArgumentNullException("sender");
            }
            AddFavourite(dialog.FileName);
        }

        private void OtherOpenDialogue_FileOk(object sender, CancelEventArgs e)
        {
            var dialogue = sender as FileDialog;
            if (dialogue == null)
            {
                throw new ArgumentNullException("sender");
            }
            Program.Instance.LoadJob(dialogue.FileName);
            Result = GetJobResult.Other;
            Close();
        }

        private void FavouritesListbox_MouseDoubleClick(object sender, MouseEventArgs e)
        {


            
            //Close();
        }
    }

    enum GetJobResult
    {
/*
        Cancel,
*/
        New,
        Open,
        Other
    }

    sealed class JobControl : Control
    {
        public readonly string Filename;

        public JobControl(String filename)
        {
            Filename = filename;
            Text = GetJobName();
        }

        private string GetJobName()
        {
            return Path.GetFileNameWithoutExtension(Filename);
        }
    }
}
