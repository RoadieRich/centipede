using System;
using System.Linq;
using System.Windows.Forms;
using CentipedeInterfaces;


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
            CentipedeJob job = new CentipedeJob(fileName);

            this._favouriteJobs.Favourites.AddFavouritesRow(job.Name, fileName);
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
}
