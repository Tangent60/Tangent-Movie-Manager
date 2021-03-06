﻿using NLog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace WindowsFormsApplication2
{
    public partial class frmMain : Form
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public frmMain()
        {
            InitializeComponent();
            LoadData();

            mlblInfo.Text = "Found " + dataGridView1.RowCount + " records.";
        }

        #region Events

        private void Form1_Load(object sender, EventArgs e)
        {
            SetGridStyle();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string savedFolderPath = GetSavedSettings();
            string selectedFolder;

            selectedFolder = OpenFile(savedFolderPath);
            
            LoadDataFromDisk(selectedFolder);

            if (savedFolderPath != selectedFolder)
                SaveSettings(selectedFolder);

            mlblInfo.Text = "Found " + dataGridView1.RowCount + " records.";
        }
        
        private void btnRefresh_Click(object sender, EventArgs e)
        {
        }

        private void mbtnRefresh_Click(object sender, EventArgs e)
        {
            LoadData(mtxtSearch.Text);

            mlblInfo.Text = "Found " + dataGridView1.RowCount + " records.";
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveToDb();
        }
        
        private void btnDeleteDuplicates_Click(object sender, EventArgs e)
        {
            DeleteDuplicates();
        }
        
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            logger.Trace("Inside dataGridView1_CellContentClick");

            var senderGrid = (DataGridView)sender;

            if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn &&
                e.RowIndex >= 0)
            {
                object fullPath = dataGridView1.Rows[e.RowIndex].Cells[nameof(Movie.FullPath)].Value;

                logger.Trace("fullPath="+ fullPath);

                System.Diagnostics.Process.Start(fullPath.ToString());
            }
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

        private void LoadData(string filter = null)
        {
            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            LogMethodInsideTrace(methodName);

            DateTime dtStart = DateTime.Now;

            BL objBL = new BL();
            List<Movie> listMovies = null;

            try
            {
                listMovies = objBL.GetMovies();

                if(filter != null)
                    listMovies = FilterMovies(listMovies, filter);

                dataGridView1.DataSource = listMovies;

                logger.Info(GetMessageTotalTimeTaken(methodName: methodName, dtStart: dtStart));
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception Inside " + methodName);
            }
        }

        private void LoadDataFromDisk(string selectedFolder)
        {
            DateTime dtStartTime = DateTime.Now;
            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            LogMethodInsideTrace(methodName);

            string[] strFiles = Directory.GetFiles(selectedFolder, "*", SearchOption.AllDirectories);

            List<Movie> listMovies = new List<Movie>();
            Movie objMovie = new Movie();

            string[] fileExtExcluded = Utility.GetNonVideoExtensions();
            string[] fileExtVideos = Utility.GetVideoExtensions();

            fileExtExcluded = (from str in fileExtExcluded
                               select str.ToUpper()).ToArray();

            fileExtVideos = (from str in fileExtVideos
                             select str.ToUpper()).ToArray();

            foreach (string file in strFiles)
            {
                string extension = Path.GetExtension(file);
                extension = extension.ToUpper();

                if (fileExtExcluded.Contains(extension))
                    continue;

                if (!fileExtVideos.Contains(extension))
                    logger.Warn("Unsupported Extension - " + extension);

                logger.Debug(file);

                string fileName = Path.GetFileName(file);

                objMovie = new Movie();

                objMovie.Name = Path.GetFileNameWithoutExtension(file);
                objMovie.FullPath = file;
                objMovie.Year = GetNumber(fileName);

                listMovies.Add(objMovie);
            }
            
            dataGridView1.DataSource = FilterMovies(listMovies, mtxtSearch.Text);

            logger.Info(GetMessageTotalTimeTaken(methodName, dtStartTime));
        }

        private List<Movie> FilterMovies(List<Movie> listMovies, string searchText)
        {
            List<Movie> filteredMovies;
            List<Movie> moviesFilteredOnName = new List<Movie>();
            List<Movie> moviesFilteredOnYear = new List<Movie>();

            var myRegex = new Regex(@searchText, RegexOptions.IgnoreCase);

            if (!string.IsNullOrEmpty(mtxtSearch.Text))
            {
                moviesFilteredOnName = listMovies.Where(x => myRegex.IsMatch(x.Name)).ToList();

                moviesFilteredOnYear = listMovies.Where(x => myRegex.IsMatch(x.Year.ToString())).ToList();
                
                filteredMovies = moviesFilteredOnName
                                            .Union(moviesFilteredOnYear)
                                            .ToList();
            }
            else
            {
                filteredMovies = listMovies.ToList();
            }

            return filteredMovies;
        }

        private void SaveToDb()
        {
            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;

            LogMethodInsideTrace(methodName);

            try
            {
                ThreadStart testThreadStart = new ThreadStart(this.DoDbSaving);
                Thread testThread = new Thread(testThreadStart);

                testThread.Start();
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Movie Saving FAILED!");
            }
        }

        private void DoDbSaving()
        {
            DateTime dtStartTime = DateTime.Now;
            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            List<Movie> listMovie = GetMoviesFromGrid();
            BL objBL = new BL();
            objBL.SaveMovies(listMovie);

            logger.Info(GetMessageTotalTimeTaken(methodName, dtStartTime));
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
                if(dataGridView1.Columns[nameof(Movie.Year)] != null)
                    dataGridView1.Columns[nameof(Movie.Year)].Width = 40;

                if (dataGridView1.Columns[nameof(Movie.FullPath)] != null)
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

        private string OpenFile(string savedFolderPath)
        {
            string selectedFolder = null;

            if(!string.IsNullOrEmpty(savedFolderPath))
                folderBrowserDialog1.SelectedPath = savedFolderPath;

            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
                selectedFolder = folderBrowserDialog1.SelectedPath;

            return selectedFolder;
        }

        private string GetSavedSettings()
        {
            string savedFolderName = BL.FolderPath;
            return savedFolderName;
        }

        private void SaveSettings(string selectedFolder)
        {
            Utility.UpdateSetting(BL.FOLDER_PATH, selectedFolder);
        }
        #endregion
    }
}
