using System;
using System.Data.SqlClient;

namespace DataLayer
{
    public class SqlServer : IDbContext, IDisposable
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
