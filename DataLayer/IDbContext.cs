using System.Data.SqlClient;

namespace DataLayer
{
    public interface IDbContext
    {
        SqlConnection GetConn();
    }
}
