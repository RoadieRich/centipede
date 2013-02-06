using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml;
using Microsoft.Win32;


namespace Centipede
{
    public partial class EditFavourites : Form
    {

        private readonly FavouriteJobs _favouriteJobs;

        public EditFavourites()
        {
            InitializeComponent();
        }

        public EditFavourites(FavouriteJobs favouriteJobsDataStore)
        {
            _favouriteJobs = favouriteJobsDataStore;
            InitializeComponent();


            favouritesBindingSource.DataSource = favouriteJobsDataStore;
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = openFileDialog1.ShowDialog(this);
            if (dialogResult == DialogResult.OK)
            {
                foreach (string fileName in openFileDialog1.FileNames)
                {
                    AddFavourite(fileName);
                }
            }
        }

        private void AddFavourite(string fileName)
        {
            _favouriteJobs.Favourites.AddFavouritesRow(GetJobName(fileName), fileName);
        }

        private static string GetJobName(string fileName)
        {
            XmlDocument xmlDocument = new XmlDocument();
            using (FileStream file = File.OpenRead(fileName))
            {
                xmlDocument.Load(file);
            }
            string jobName = xmlDocument.OfType<XmlElement>().First(e => e.HasAttribute(@"Title")).GetAttribute(@"Title");
            return jobName;
        }
        
        private void RemoveButton_Click(object sender, EventArgs e)
        {
            foreach (int index in from DataGridViewRow selectedRow in FavouriteJobsGridView.SelectedRows
                                  select selectedRow.Index)
            {
                try
                {
                    _favouriteJobs.Favourites.Rows.RemoveAt(index);
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception);
                }
            }
        }

        public static String GetFaveFilename()
        {
            var filename = Registry.CurrentUser.GetValue(@"Chemineer\Centipede\FavouriteFile") as String;

            if (filename == null)
            {
                Registry.CurrentUser.SetValue(@"Chemineer\Centipede\FavouriteFile", @"%APPDATA%\Centipede\favourites.xml");
                filename = Environment.ExpandEnvironmentVariables(@"%APPDATA%\Centipede\favourites.xml");
            }
            return Environment.ExpandEnvironmentVariables(filename);
        }

        private void EditFavourites_Load(object sender, EventArgs e)
        {

            //try
            //{
            //    using (var file = File.Open(GetFaveFilename(), FileMode.OpenOrCreate))
            //    {
            //        favouriteJobs.Favourites.ReadXml(file);
            //    }
            //}
            //catch (Exception)
            //{
            //    ;
            //}
        }

        private void EditFavourites_FormClosed(object sender, FormClosedEventArgs e)
        {
            _favouriteJobs.Favourites.WriteXml(GetFaveFilename(),true);

            
        }

        private void MoveUpButton_Click(object sender, EventArgs e)
        {
            FavouriteJobsGridView.SelectedRows.Cast<DataGridViewRow>()
                                 .Select<DataGridViewRow, FavouriteJobs.FavouritesRow>(
                                                                                       r =>
                                                                                       r.DataBoundItem as
                                                                                       FavouriteJobs.FavouritesRow)
                                 .Single()
                                 .MoveDown();
        }

        private void MoveDownButton_Click(object sender, EventArgs e)
        {
            FavouriteJobsGridView.SelectedRows.Cast<DataGridViewRow>()
                                 .Select<DataGridViewRow, FavouriteJobs.FavouritesRow>(
                                                                                       r =>
                                                                                       r.DataBoundItem as
                                                                                       FavouriteJobs.FavouritesRow)
                                 .Single()
                                 .MoveUp();
        }

    }

    public struct Job
    {
        public String Filename;
        public String Name;
    }

    partial class Extensions
    {
        public static void MoveUp(this FavouriteJobs.FavouritesRow row)
        {
            row.Move(1);
        }

        public static void MoveDown(this FavouriteJobs.FavouritesRow row)
        {
            row.Move(-1);
        }

        public static void Move(this FavouriteJobs.FavouritesRow row, int delta)
        {
            FavouriteJobs.FavouritesDataTable table = (FavouriteJobs.FavouritesDataTable)row.Table;

            int oldIndex = table.Rows.IndexOf(row);
            if (oldIndex <= 0)
            {
                return;
            }
            table.Rows.Remove(row);
            int newIndex = oldIndex + delta;
            table.Rows.InsertAt(row, newIndex.Clamp(0, table.Rows.Count));

        }



        private static int Clamp(this int i, int min, int max)
        {
            return Math.Min(Math.Max(i, min), max);
        }
    }
}
