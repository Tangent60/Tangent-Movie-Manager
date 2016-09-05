using NLog;
using System;
using System.Configuration;
using System.Data;
using System.Data.SQLite;

namespace WindowsFormsApplication2
{
    class DAL
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private SQLiteConnection _sqlConn;
        private SQLiteCommand _sqlCmd;
        private SQLiteDataAdapter _sqlDataAdapter;
        private DataSet _dataSet = new DataSet();
        private DataTable _dataTable = new DataTable();

        private void SetConnection()
        {
            string connString = ConfigurationManager.AppSettings["ConnString"];
            _sqlConn = new SQLiteConnection(connString);
        }

        public void ExecuteQuery(string txtQuery)
        {
            SetConnection();

            _sqlConn.Open();
            _sqlCmd = _sqlConn.CreateCommand();
            _sqlCmd.CommandText = txtQuery;
            _sqlCmd.ExecuteNonQuery();
            _sqlConn.Close();
        }

        public DataTable GetData(string commandText)
        {
            SetConnection();

            try
            {
                _sqlConn.Open();
                _sqlCmd = _sqlConn.CreateCommand();
                _sqlDataAdapter = new SQLiteDataAdapter(commandText, _sqlConn);
                _dataSet.Reset();
                _sqlDataAdapter.Fill(_dataSet);
                _dataTable = _dataSet.Tables[0];
            }
            catch (Exception ex)
            {
                logger.Warn(ex);
                CreateTableMovie();
                GetData(commandText);
            }
            finally
            {
                _sqlConn.Close();
            }

            return _dataTable;
        }

        public void CreateTableMovie()
        {
            string query ="CREATE TABLE 'Movie' (	'Name'	TEXT,	'Year'	INTEGER,	'FullPath'	TEXT)";
            ExecuteQuery(query);
        }
    }
}
