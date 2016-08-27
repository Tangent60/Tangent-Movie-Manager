using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Linq;
using System.Data.Linq.SqlClient;

namespace WindowsFormsApplication2
{
    public partial class Form1 : Form
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public Form1()
        {
            InitializeComponent();
            LoadData();
        }

        #region Public Events

        private void button1_Click(object sender, EventArgs e)
        {
            LoadDataFromDisk();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveToDb();
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
            logger.Trace("Inside LoadData()");

            BL objBL = new BL();
            List<Movie> listMovies = null;

            try
            {
                listMovies = objBL.GetMovies();
                dataGridView1.DataSource = listMovies;
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception Inside LoadData()");
            }
        }

        private void LoadDataFromDisk()
        {
            logger.Trace("Inside LoadDataFromDisk()");
            //Directory dir = new Directory;
            //dir.
            //F:\Movies\2016
            //string path = @"F:\Movies\2016";
            string path = @"F:\Movies";
            string[] strFiles = Directory.GetFiles(path, "*", SearchOption.AllDirectories);

            List<Movie> listMovies = new List<Movie>();
            Movie objMovie = new Movie();
            foreach (string file in strFiles)
            {
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
        }

        private void SaveToDb()
        {
            logger.Trace("Inside SaveToDb()");

            List<Movie> listMovie = new List<Movie>();

            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                Movie objMovie = new Movie();

                string columnName = null;

                columnName  = Utility.GetPropertyName(() => objMovie.Name);
                objMovie.Name = GetValue(dataGridView1.Rows[i].Cells[columnName]);

                columnName = Utility.GetPropertyName(() => objMovie.Year);
                objMovie.Year= GetValueInt(dataGridView1.Rows[i].Cells[columnName]);

                columnName = Utility.GetPropertyName(() => objMovie.FullPath);
                objMovie.FullPath = GetValue(dataGridView1.Rows[i].Cells[columnName]);

                listMovie.Add(objMovie);
            }

            BL objBL = new BL();
            objBL.SaveMovies(listMovie);
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
        #endregion
    }



}
