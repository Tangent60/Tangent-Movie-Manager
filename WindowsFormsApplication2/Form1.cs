using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Linq;
using System.Data.Linq.SqlClient;
using System.Threading;

namespace WindowsFormsApplication2
{
    public partial class Form1 : Form
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public Form1()
        {
            InitializeComponent();
            LoadData();

            lblInfo.Text = "Found " + dataGridView1.RowCount + " records.";
        }

        #region Public Events

        private void Form1_Load(object sender, EventArgs e)
        {
            SetGridStyle();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            LoadDataFromDisk();

            lblInfo.Text = "Found " + dataGridView1.RowCount + " records.";
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveToDb();
        }
        
        private void btnDeleteDuplicates_Click(object sender, EventArgs e)
        {
            DeleteDuplicates();
        }

        #endregion

        #region Private Methods

        private int GetNumber(string text)
        {
            int year = 0;
            string pattern = @"\d{4}";// @"^(19|20)\d{2}"
            var regex = new Regex(pattern);
            if (regex.IsMatch(text))
            {
                var myCapturedText = regex.Match(text).Value;
                Int32.TryParse(myCapturedText, out year);
            }

            return year;
        }

        private void LoadData()
        {
            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            LogMethodInsideTrace(methodName);

            DateTime dtStart = DateTime.Now;

            BL objBL = new BL();
            List<Movie> listMovies = null;

            try
            {
                listMovies = objBL.GetMovies();
                dataGridView1.DataSource = listMovies;

                logger.Info(GetMessageTotalTimeTaken(methodName: methodName, dtStart: dtStart));
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception Inside " + methodName);
            }
        }

        private void LoadDataFromDisk()
        {
            DateTime dtStartTime = DateTime.Now;
            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            LogMethodInsideTrace(methodName);
            //Directory dir = new Directory;
            //dir.
            //F:\Movies\2016
            //string path = @"F:\Movies\2016";
            string path = @"F:\Movies";
            string[] strFiles = Directory.GetFiles(path, "*", SearchOption.AllDirectories);

            List<Movie> listMovies = new List<Movie>();
            Movie objMovie = new Movie();

            string[] fileExtExcluded = new string[] { ".txt", ".nfo", ".db", ".jpg", ".png" };

            foreach (string file in strFiles)
            {
                if (fileExtExcluded.Contains(Path.GetExtension(file)))
                    continue;

                logger.Debug(file);

                string fileName = Path.GetFileName(file);

                objMovie = new Movie();

                objMovie.Name = Path.GetFileNameWithoutExtension(file);
                objMovie.FullPath = file;
                objMovie.Year = GetNumber(fileName);

                listMovies.Add(objMovie);
            }

            List<Movie> filteredMovies;

            filteredMovies = listMovies.ToList();

            string searchText = txtSearch.Text;

            var myRegex = new Regex(@searchText);

            if (!string.IsNullOrEmpty(txtSearch.Text))
                filteredMovies = listMovies.Where(x => myRegex.IsMatch(x.Name)).ToList();

            dataGridView1.DataSource = filteredMovies;

            logger.Info(GetMessageTotalTimeTaken(methodName, dtStartTime));
        }

        private void SaveToDb()
        {
            DateTime dtStartTime = DateTime.Now;
            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;

            LogMethodInsideTrace(methodName);

            try
            {
                ThreadStart testThreadStart = new ThreadStart(new Form1().DoDbSaving);
                Thread testThread = new Thread(testThreadStart);

                testThread.Start();
                
                logger.Debug("Saved Movies to Disk Successfully.");
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Movie Saving FAILED!");
            }

            logger.Info(GetMessageTotalTimeTaken(methodName, dtStartTime));
        }

        private void DoDbSaving()
        {
            List<Movie> listMovie = GetMoviesFromGrid();
            BL objBL = new BL();
            objBL.SaveMovies(listMovie);
        }

        private void DeleteDuplicates()
        {
            logger.Trace("Inside DeleteDuplicates()");

            List<Movie> listMovie = GetMoviesFromGrid();

            BL objBL = new BL();
            try
            {
                objBL.DeleteDuplicates(listMovie);
                logger.Debug("Saved Movies to Disk Successfully.");
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Movie Saving FAILED!");
            }
        }

        private string GetValue(object obj)
        {
            if (obj == null)
                return null;
            else
                return obj.ToString();
        }

        private int? GetValueInt(object obj)
        {
            if (obj == null)
                return null;
            else
            {
                int temp = 0;
                try
                {
                    temp = Convert.ToInt32(obj);
                }
                catch (Exception ex)
                {
                    logger.Error(ex, "String=" + obj.ToString());
                }
                return temp;
            }
        }

        private List<Movie> GetMoviesFromGrid()
        {
            List<Movie> listMovie = new List<Movie>();

            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                Movie objMovie = new Movie();

                string columnName = null;

                columnName = nameof(objMovie.Name);
                objMovie.Name = GetValue(dataGridView1.Rows[i].Cells[columnName].Value);

                columnName = nameof(objMovie.Year);
                objMovie.Year = GetValueInt(dataGridView1.Rows[i].Cells[columnName].Value);

                columnName = nameof(objMovie.FullPath);
                objMovie.FullPath = GetValue(dataGridView1.Rows[i].Cells[columnName].Value);

                listMovie.Add(objMovie);
            }

            return listMovie;
        }

        private void SetGridStyle()
        {
            if (dataGridView1.Columns.Count > 0)
            {
                dataGridView1.Columns[nameof(Movie.Year)].Width = 40;
                dataGridView1.Columns[nameof(Movie.FullPath)].Width = 200;
            }
        }

        private string GetMessageTotalTimeTaken(string methodName, DateTime dtStart)
        {
            return methodName + ", Total Time Taken = " + (DateTime.Now - dtStart).TotalSeconds + " secs";
        }

        private void LogMethodInsideTrace(string methodName)
        {
            logger.Trace("Inside " + methodName);
        }
        #endregion

        private void contextMenuStrip1_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {

        }
    }
}
