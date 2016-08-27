using System;
using System.Collections.Generic;
using System.Data;

namespace WindowsFormsApplication2
{
    class BL
    {
        internal List<Movie> GetMovies()
        {
            List<Movie> movies = new List<Movie>();

            DAL objDal = new DAL();
            DataTable dt = objDal.GetData("Select Name, Year, FullPath From Movie");

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

        internal void SaveMovies(List<Movie> listMoview)
        {
            //Save to Db
        }
    }
}
