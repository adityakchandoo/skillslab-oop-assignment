using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace DataLayer.Generic
{
    public interface IDataAccessLayer<T>
    {
        Task<IEnumerable<T>> GetMany(string sql = "", Dictionary<string, object> parameters = null);
        Task<T> GetByPKAsync(object primaryKeyValue);
        Task<int> Insert(T item);
        Task<int> Update(T item);
        Task<int> Delete(T item);
    }
}
