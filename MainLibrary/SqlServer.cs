using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainLibrary
{
    public class SqlServer : IDbContext ,IDisposable
    {

        private SqlConnection connection;

        public SqlServer(string connectionString)
        {
            connection = new SqlConnection(connectionString);
            connection.Open();

        }

        public SqlConnection GetConn()
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
