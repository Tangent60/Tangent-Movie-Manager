using NLog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;

namespace WindowsFormsApplication2
{
    class BL
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public const string FOLDER_PATH = "FolderPath";

        public static string FolderPath
        {
            get
            {
                return ConfigurationManager.AppSettings[FOLDER_PATH];
            }
        }

        internal List<Movie> GetMovies(string fullPath = null)
        {
            List<Movie> movies = new List<Movie>();

            DAL objDal = new DAL();

            string txtSqlQuery = "SELECT Name, Year, FullPath FROM Movie ";
            if (!string.IsNullOrEmpty(fullPath))
                txtSqlQuery += " WHERE FullPath = '" + fullPath + "'";

            logger.Debug(txtSqlQuery);

            DataTable dt = objDal.GetData(txtSqlQuery);

            foreach (DataRow row in dt.Rows)
            {
                Movie objMovie = new Movie();
                objMovie.Name = row["Name"].ToString();
                objMovie.Year = row["Year"] == null ? 0 : Convert.ToInt32(row["Year"].ToString());
                objMovie.FullPath = row["FullPath"] == null ? null : row["FullPath"].ToString();

                movies.Add(objMovie);
            }

            return movies;
        }

        internal Movie GetMovie(string fullPath)
        {
            fullPath = GetSqlCompatibleText(fullPath);

            List<Movie> movies = GetMovies(fullPath);
            Movie objMovie = new Movie();

            if (movies != null)
            {
                if (movies.Count == 1)
                {
                    objMovie = movies[0];
                }
                else
                    logger.Warn("(" + movies.Count + ") Movies found for fullPath=" + fullPath);
            }
            else
            {
                logger.Debug("Movie not found for fullPath=" + fullPath);
            }

            return objMovie;
        }

        internal bool MovieExists(string fullPath)
        {
            Movie movie = GetMovie(fullPath);

            if (string.IsNullOrEmpty(movie.Name))
                return false;
            else
                return true;
        }

        internal void SaveMovies(List<Movie> listMovies)
        {
            //Save to Db

            foreach (Movie movie in listMovies)
            {
                movie.FullPath = GetSqlCompatibleText(movie.FullPath);

                if (MovieExists(movie.FullPath))
                    UpdateMovie(movie);
                else
                    InsertMovie(movie);
            }
        }

        private void InsertMovie(Movie movie)
        {
            DAL objDal = new DAL();

            movie.Name = GetSqlCompatibleText(movie.Name);
            movie.FullPath = GetSqlCompatibleText(movie.FullPath);

            string txtSqlQuery = "INSERT INTO Movie (" + nameof(movie.Name) + ", " + nameof(movie.Year) + ", " + nameof(movie.FullPath) + ") ";
            txtSqlQuery += string.Format("VALUES ('{0}', {1}, '{2}')",
                        movie.Name, movie.Year, movie.FullPath);

            logger.Debug(txtSqlQuery);

            try
            {
                objDal.ExecuteQuery(txtSqlQuery);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void UpdateMovie(Movie movie)
        {
            DAL objDal = new DAL();

            string txtSqlQuery = "UPDATE Movie ";
            txtSqlQuery += " SET " + nameof(movie.Name) + " = '" + movie.Name + "'";
            txtSqlQuery += ", " +  nameof(movie.Year) + " = " + movie.Year;
            txtSqlQuery += " WHERE " + nameof(movie.FullPath) + " = '" + movie.FullPath + "'";

            logger.Debug(txtSqlQuery);

            try
            {
                objDal.ExecuteQuery(txtSqlQuery);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void DeleteMovie(string fullPath)
        {
            DAL objDal = new DAL();

            Movie movie = new Movie();

            string txtSqlQuery = "DELETE FROM Movie ";
            txtSqlQuery += " WHERE " + nameof(movie.FullPath) + " = '" + fullPath + "'";

            logger.Debug(txtSqlQuery);

            try
            {
                objDal.ExecuteQuery(txtSqlQuery);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        internal void DeleteDuplicates(List<Movie> listMovies)
        {
            foreach (Movie movie in listMovies)
            {
                List<Movie> movies = GetMovies(movie.FullPath);
                //Movie objMovie = new Movie();

                if (movies != null)
                {
                    if (movies.Count >= 1)
                    {
                        DeleteMovie(movie.FullPath);
                        logger.Info("Deleted " + movie.FullPath);
                    }
                }
            }
        }

        private string GetSqlCompatibleText(string text)
        {
            text = text.Replace("''", "'");
            return text.Replace("'", "''");
        }
    }
}
