using System.Data;
using System.Data.SQLite;

namespace WindowsFormsApplication2
{
    class DAL
    {
        private SQLiteConnection _sqlConn;
        private SQLiteCommand _sqlCmd;
        private SQLiteDataAdapter _sqlDataAdapter;
        private DataSet _dataSet = new DataSet();
        private DataTable _dataTable = new DataTable();

        private void SetConnection()
        {
            _sqlConn = new SQLiteConnection
                ("Data Source=DemoT.db;Version=3;New=False;Compress=True;");
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

            _sqlConn.Open();
            _sqlCmd = _sqlConn.CreateCommand();
            _sqlDataAdapter = new SQLiteDataAdapter(commandText, _sqlConn);
            _dataSet.Reset();
            _sqlDataAdapter.Fill(_dataSet);
            _dataTable = _dataSet.Tables[0];            
            _sqlConn.Close();

            return _dataTable;
        }
    }
}
