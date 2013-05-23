using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using CentipedeInterfaces;


namespace Centipede
{
    public partial class EditFavourites : Form
    {
        public EditFavourites()
        {
            InitializeComponent();

            this.UpdateBinding();
        }

        private void UpdateBinding()
        {
            StringCollection listOfFavouriteJobs = Properties.Settings.Default.ListOfFavouriteJobs;
            this.FavouriteJobsGridView.DataSource = listOfFavouriteJobs.OfType<String>().Select(s => new
                                                                                                     {
                                                                                                         Filename = s
                                                                                                     }).ToList();
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = openFileDialog1.ShowDialog(this);
            if (dialogResult != DialogResult.OK)
            {
                return;
            }
            foreach (string fileName in this.openFileDialog1.FileNames)
            {
                AddFavourite(fileName);
            }
        }

        private void AddFavourite(string fileName)
        {
            //CentipedeJob job = new CentipedeJob(fileName);

            //this._favouriteJobs.Favourites.AddFavouritesRow(job.Name, fileName);

            Properties.Settings.Default.ListOfFavouriteJobs.Add(fileName);
            this.UpdateBinding();
        }

        private void RemoveButton_Click(object sender, EventArgs e)
        {
            foreach (int index in from DataGridViewRow selectedRow in FavouriteJobsGridView.SelectedRows
                                  select selectedRow.Index)
            {
                try
                {
                    Properties.Settings.Default.ListOfFavouriteJobs.RemoveAt(index);
                    this.UpdateBinding();

                }
                catch (Exception exception)
                {
                    Debug.WriteLine(exception.Message);
                }
            }
        }

        public static String GetFaveFilename()
        {
            String filename = Properties.Settings.Default.FavouritesFile;
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
            //Properties.Settings.Default.ListOfFavouriteJobs = new StringCollection();
            //Properties.Settings.Default.ListOfFavouriteJobs.AddRange(_favouriteJobs.Favourites.Select(row => row.Filename));

        }

        //private void MoveUpButton_Click(object sender, EventArgs e)
        //{
        //    FavouriteJobsGridView.SelectedRows.Cast<DataGridViewRow>()
        //                         .Select<DataGridViewRow, FavouriteJobs.FavouritesRow>(
        //                                                                               r =>
        //                                                                               r.DataBoundItem as
        //                                                                               FavouriteJobs.FavouritesRow)
        //                         .Single()
        //                         .MoveDown();
        //}

        //private void MoveDownButton_Click(object sender, EventArgs e)
        //{
        //    FavouriteJobsGridView.SelectedRows.Cast<DataGridViewRow>()
        //                         .Select<DataGridViewRow, FavouriteJobs.FavouritesRow>(
        //                                                                               r =>
        //                                                                               r.DataBoundItem as
        //                                                                               FavouriteJobs.FavouritesRow)
        //                         .Single()
        //                         .MoveUp();
        //}

    }
}
