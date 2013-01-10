﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Microsoft.Win32;
using System.Xml;
using Centipede;

namespace Centipede
{
    internal partial class GetJob : Form
    {
        public GetJob()
        {
            InitializeComponent();
        }
        public GetJobResult Result;

        
        internal string getJobFileName()
        {
            switch (Result)
            {
                case GetJobResult.New:
                    return "";
                case GetJobResult.Open:
                    JobControl selectedJob = FavouritesListbox.SelectedItem as JobControl;
                    return selectedJob.Filename;
                default:
                    return null;
            }
        }

        private void BrowseButton_Click(object sender, EventArgs e)
        {
            DialogResult result = BrowseLoadDialogue.ShowDialog(this);
        }

        private void AddFavourite(string filename)
        {
            FavouritesListbox.Items.Add(new JobControl(filename));
            String faveFilename = GetFaveFilename();
            XmlDocument xmlDoc = new XmlDocument();
            
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
            this.Close();
        }

        private void LoadButton_Click(object sender, EventArgs e)
        {
            Result = GetJobResult.Open;
            this.Close();
        }

        private void GetJob_Load(object sender, EventArgs e)
        {
            XmlDocument xmlDoc = new XmlDocument();

            String filename = GetFaveFilename();

            using (FileStream xmlFile = File.Open(filename, FileMode.OpenOrCreate, FileAccess.Read))
            {
                xmlDoc.Load(xmlFile);

                if (xmlDoc.HasChildNodes)
                {
                    XmlElement faveXmlStoreElement = xmlDoc.GetElementsByTagName("favourites")[0] as XmlElement;
                    foreach (XmlElement favouriteItem in faveXmlStoreElement.ChildNodes)
                    {
                        FavouritesListbox.Items.Add(new JobControl(favouriteItem.GetAttribute("Filename")));
                    }
                }
            }
        }

        private static String GetFaveFilename()
        {
            String filename = Registry.CurrentUser.GetValue(@"Chemineer\Centipede\FavouriteFile") as String;

            if (filename == null)
            {
                Registry.CurrentUser.SetValue(@"Chemineer\Centipede\FavouriteFile", @"%APPDATA%\Centipede\favourites.xml");
                filename = System.Environment.ExpandEnvironmentVariables(@"%APPDATA%\Centipede\favourites.xml");

                if (!File.Exists(filename))
                {
                    StreamWriter file = new StreamWriter(File.Create(filename));
                    file.Write(@"<?xml version=""1.0"" encoding=""utf-8""?><favourites />");
                    file.Close();
                }
            }
            return System.Environment.ExpandEnvironmentVariables(filename);
        }

        private void OtherButton_Click(object sender, EventArgs e)
        {
            OtherOpenDialogue.ShowDialog(this);
        }

        private void BrowseLoadDialogue_FileOk(object sender, CancelEventArgs e)
        {
            FileDialog dialog = sender as FileDialog;
            AddFavourite(dialog.FileName);
        }

        private void GetJob_FormClosing(object sender, FormClosingEventArgs e)
        {
        }

        private void OtherOpenDialogue_FileOk(object sender, CancelEventArgs e)
        {
            FileDialog dialogue = sender as FileDialog;
            Program.LoadJob(dialogue.FileName);
        }

        private void FavouritesListbox_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void FavouritesListbox_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Result = GetJobResult.Open;
            this.Close();
        }
    }

    enum GetJobResult
    {
        Cancel,
        New,
        Open
    }

    class JobControl : Control
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
