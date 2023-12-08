using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace DataLayer.Generic
{
    public interface IDataAccessLayer<T>
    {
        List<T> GetMany(string sql = "", Dictionary<string, object> parameters = null);
        T GetByPK(object primaryKeyValue);
        int Insert(T item);
        int Update(T item);
        int Delete(T item);
    }
}
