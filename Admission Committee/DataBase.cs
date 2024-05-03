using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace Admission_Committee
{
    internal class DataBase
    {
        SqlConnection sqlConnection = new SqlConnection(@"Data Source=KINEXP;Initial Catalog=ПриёмнаяКомиссия;Integrated Security=True");
        //SqlConnection sqlConnection = new SqlConnection(@"Data Source=JUNK;Initial Catalog=ПриёмнаяКомиссия;Integrated Security=True");
        //SqlConnection sqlConnection = new SqlConnection(@"Data Source=KINEXP;Initial Catalog=ПриёмнаяКомиссия;Integrated Security=True");

        public void OpenConnection()
        {
            if (sqlConnection.State == System.Data.ConnectionState.Closed)
                sqlConnection.Open();
        }

        public void CloseConnection()
        {
            if (sqlConnection.State == System.Data.ConnectionState.Open)
                sqlConnection.Close();
        }

        public SqlConnection GetConnection()
        {
            return sqlConnection;
        }
    }
}
