using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    internal static class DbHelper
    {
        public static void AddParameterWithValue(this IDbCommand command, string parameterName, object parameterValue)
        {
            var parameter = command.CreateParameter();
            parameter.ParameterName = parameterName;
            parameter.Value = parameterValue;
            command.Parameters.Add(parameter);
        }

        public static T ConvertToObject<T>(this IDataReader rd) where T : class, new()
        {
            Type type = typeof(T);
            var properties = type.GetProperties();

            var t = new T();

            for (int i = 0; i < rd.FieldCount; i++)
            {
                if (!rd.IsDBNull(i))
                {
                    string fieldName = rd.GetName(i);

                    var property = properties.FirstOrDefault(p =>
                        string.Equals(p.Name, fieldName, StringComparison.OrdinalIgnoreCase));

                    if (property != null)
                    {
                        property.SetValue(t, rd.GetValue(i));
                    }
                }
            }

            return t;
        }
    }
}
