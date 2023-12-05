using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class SqlServer : IDbContext ,IDisposable
    {

        private SqlConnection connection;

        public SqlServer(string connectionString)
        {
            connection = new SqlConnection(connectionString);
            connection.Open();

        }

        public IDbConnection GetConn()
        {
            return connection;
        }

        public void Dispose()
        {
            if (connection != null)
            {
                connection.Close();
                connection.Dispose();
            }
        }
    }
}
