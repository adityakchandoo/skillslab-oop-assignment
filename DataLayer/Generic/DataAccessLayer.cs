using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;

namespace DataLayer.Generic
{
    public class DataAccessLayer<T> : IDataAccessLayer<T>
    {
        private readonly IDbConnection _conn;
        public DataAccessLayer(IDbContext dbContext)
        {
            _conn = dbContext.GetConn();
        }

        // CRUDS Ops
        public List<T> GetMany(string sql = "", Dictionary<string,object> parameters = null)
        {
            var result = new List<T>();

            try
            {
                sql = GenerateSelectQuery();
                using (IDbCommand cmd = _conn.CreateCommand())
                {
                    cmd.CommandText = sql;
                    if (parameters != null && parameters.Any())
                    {
                        foreach (var p in parameters)
                        {
                            AddParameterWithValue(cmd, p.Key, p.Value);
                        }
                    }
                    using (IDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            T item = Activator.CreateInstance<T>();
                            Populate(item, reader);
                            result.Add(item);
                        }
                    }
                }
            } catch (Exception e)
            {
                throw;
            }
            return result;
        }

        public T GetByPK(object primaryKeyValue)
        {
            T item = default(T);
            string primaryKeyName;
            string sql = GenerateSelectByPKQuery(out primaryKeyName);

            try
            {
                using (IDbCommand cmd = _conn.CreateCommand())
                {
                    cmd.CommandText = sql;

                    AddParameterWithValue(cmd, primaryKeyName, primaryKeyValue);

                    using (IDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            item = Activator.CreateInstance<T>();
                            Populate(item, reader);
                            return item;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return item;
        }

        public int Insert(T item)
        {
            int rowAdded = -1;

            try
            {
                string sql = GenerateInsertQuery();
                Dictionary<string, object> parameters = GenerateSqlParameters(item);
                using (IDbCommand cmd = _conn.CreateCommand())
                {
                    cmd.CommandText = sql;

                    foreach (var p in parameters)
                    {
                        AddParameterWithValue(cmd, p.Key, p.Value);
                    }

                    rowAdded = cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return rowAdded;
        }
        
        public int Update(T item)
        {
            int rowUpdated = -1;
            try
            {
                string sql = GenerateUpdateQuery();
                Dictionary<string, object> parameters = GenerateSqlParameters(item, true);

                using (IDbCommand cmd = _conn.CreateCommand())
                {
                    cmd.CommandText = sql;

                    foreach (var p in parameters)
                    {
                        AddParameterWithValue(cmd, p.Key, p.Value);
                    }

                    rowUpdated = cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return rowUpdated;
        }

        public int Delete(T item)
        {
            int rowsDeleted = -1;

            try
            {
                string sql = GenerateDeleteQuery();
                Dictionary<string, object> parameters = GenerateSqlParameters(item, includePK: true);
                using (IDbCommand cmd = _conn.CreateCommand())
                {
                    cmd.CommandText = sql;

                    foreach (var p in parameters)
                    {
                        AddParameterWithValue(cmd, p.Key, p.Value);
                    }

                    rowsDeleted = cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return rowsDeleted;
        }

        /// PRIVATE Helper Methods
        /// 
        private static void Populate(T item, IDataReader reader)
        {
            for (int i = 0; i < reader.FieldCount; i++)
            {
                string propertyName = reader.GetName(i);
                PropertyInfo property = typeof(T).GetProperty(propertyName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

                if (property != null && !reader.IsDBNull(i))
                {
                    if (property.PropertyType.IsEnum)
                    {
                        // Handle enums
                        var enumType = property.PropertyType;
                        var enumValue = Enum.ToObject(enumType, reader.GetValue(i));
                        property.SetValue(item, enumValue);
                    }
                    else
                    {
                        // Handle other types
                        Type targetType = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;
                        object value = Convert.ChangeType(reader.GetValue(i), targetType);
                        property.SetValue(item, value);
                    }
                }
            }
        }

        private Dictionary<string, object> GenerateSqlParameters(T item, bool includePK = false)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();

            PropertyInfo[] properties = typeof(T).GetProperties();

            var propLists = includePK ? properties.ToList() : properties.Where(prop => !Attribute.IsDefined(prop, typeof(KeyAttribute)))
            .ToList();

            foreach (PropertyInfo property in propLists)
            {
                if (!property.CanRead) continue;
                parameters.Add(property.Name, property.GetValue(item) ?? DBNull.Value);
            }

            return parameters;
        }

        private string GenerateSelectQuery(bool includeCondition = false, List<string> conditionsProps = null)
        {
            StringBuilder columns = new StringBuilder();
            PropertyInfo[] properties = typeof(T).GetProperties();

            foreach (PropertyInfo property in properties)
            {
                if (!property.CanRead) continue;
                columns.Append(property.Name + ",");
            }
            columns.Remove(columns.Length - 1, 1); // Remove the last comma
            string condition = string.Empty;

            if (includeCondition && conditionsProps != null)
            {
                // build Where clause
                condition = " WHERE ";
            }

            string sql = $"SELECT {columns.ToString()} from {typeof(T).Name} {condition}";
            return sql;
        }

        private string GenerateSelectByPKQuery(out string pk_name)
        {
            string primaryKeyName = null;
            pk_name = null;
            PropertyInfo[] properties = typeof(T).GetProperties();
            foreach (var property in properties)
            {
                if (Attribute.IsDefined(property, typeof(KeyAttribute)))
                {
                    pk_name = property.Name;
                    primaryKeyName = property.Name;
                }
            }
            if (string.IsNullOrEmpty(primaryKeyName))
            {
                throw new InvalidOperationException("No primary key defined for the given type.");
            }

            return $"SELECT * FROM {typeof(T).Name} WHERE {primaryKeyName} = @{primaryKeyName};";
        }

        private string GenerateInsertQuery()
        {
            StringBuilder columns = new StringBuilder();
            StringBuilder values = new StringBuilder();

            PropertyInfo[] properties = typeof(T).GetProperties();
            foreach (PropertyInfo property in properties.Where(prop => !Attribute.IsDefined(prop, typeof(KeyAttribute))))
            {
                if (!property.CanRead) continue;
                columns.Append(property.Name + ",");
                values.Append("@" + property.Name + ",");
            }
            columns.Remove(columns.Length - 1, 1); // Remove the last comma
            values.Remove(values.Length - 1, 1); // Remove the last comma

            string query = $"INSERT INTO {typeof(T).Name} ({columns.ToString()}) VALUES ({values.ToString()})";
            return query;
        }

        private string GenerateUpdateQuery()
        {
            StringBuilder updateColumns = new StringBuilder();

            PropertyInfo[] properties = typeof(T).GetProperties();
            foreach (PropertyInfo property in properties.Where(prop => !Attribute.IsDefined(prop, typeof(KeyAttribute))))
            {
                if (!property.CanRead) continue;
                updateColumns.Append(property.Name + "= @" + property.Name + ",");
            }
            updateColumns.Remove(updateColumns.Length - 1, 1); // Remove the last comma

            PropertyInfo pkField = properties.FirstOrDefault(prop => Attribute.IsDefined(prop, typeof(KeyAttribute)));
            string sql = string.Empty;
            if (pkField != null)
            {
                string condition = $" WHERE {pkField.Name} = @{pkField.Name};";
                sql = $"Update {typeof(T).Name} SET {updateColumns.ToString()} {condition}";
            }

            return sql;
        }

        private string GenerateDeleteQuery()
        {
            StringBuilder whereClause = new StringBuilder();
            PropertyInfo[] properties = typeof(T).GetProperties().Where(prop => Attribute.IsDefined(prop, typeof(KeyAttribute))).ToArray();

            foreach (PropertyInfo property in properties)
            {
                if (!property.CanRead) continue;
                whereClause.Append($"{property.Name}=@{property.Name} AND ");
            }

            // Remove the last 'AND'
            if (whereClause.Length > 0) whereClause.Length -= 5;

            string query = $"DELETE FROM {typeof(T).Name} WHERE {whereClause}";
            return query;
        }
        private void AddParameterWithValue(IDbCommand command, string parameterName, object parameterValue)
        {
            var parameter = command.CreateParameter();
            parameter.ParameterName = "@"+parameterName;
            parameter.Value = parameterValue;
            command.Parameters.Add(parameter);
        }
    }
}