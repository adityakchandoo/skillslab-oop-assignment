using MainLibrary.Entities;
using MainLibrary.Helpers;
using MainLibrary.Repo.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainLibrary.Repo
{
    public class DepartmentRepo : IDepartmentRepo
    {
        private readonly IDbConnection _conn;
        public DepartmentRepo(IDbContext dbContext)
        {
            _conn = dbContext.GetConn();
        }

        public Department GetDepartment(int departmentId)
        {
            string sql = "SELECT * FROM [dbo].[Department] WHERE DepartmentId = @DepartmentId;";

            using (IDbCommand cmd = _conn.CreateCommand())
            {
                cmd.CommandText = sql;
                MyExtensions.AddParameterWithValue(cmd, "@DepartmentId", departmentId);

                using (IDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        return MyExtensions.ConvertToObject<Department>(reader);
                    }
                }
            }
            return null;
        }


        public IEnumerable<Department> GetAllDepartments()
        {
            string sql = "SELECT * FROM [dbo].[Department];";

            List<Department> results = new List<Department>();

            using (IDbCommand cmd = _conn.CreateCommand())
            {
                cmd.CommandText = sql;

                using (IDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        results.Add(MyExtensions.ConvertToObject<Department>(reader));
                    }
                }
            }
            return results;
        }


        public void CreateDepartment(Department department)
        {
            string sql = "INSERT INTO [dbo].[Department] (Name, Description) VALUES (@Name, @Description);";

            using (IDbCommand cmd = _conn.CreateCommand())
            {
                cmd.CommandText = sql;

                MyExtensions.AddParameterWithValue(cmd, "@Name", department.Name);
                MyExtensions.AddParameterWithValue(cmd, "@Description", department.Description);

                cmd.ExecuteNonQuery();
            }
        }

        public void DeleteDepartment(int departmentId)
        {
            string sql = "DELETE FROM [dbo].[Department] WHERE DepartmentId = @DepartmentId";

            using (IDbCommand cmd = _conn.CreateCommand())
            {
                cmd.CommandText = sql;

                MyExtensions.AddParameterWithValue(cmd, "@DepartmentId", departmentId);

                cmd.ExecuteNonQuery();
            }
        }

        public void UpdateDepartment(Department department)
        {
            string sql = "UPDATE [dbo].[Department] SET Name = @Name, Description = @Description WHERE DepartmentId = @DepartmentId;";

            using (IDbCommand cmd = _conn.CreateCommand())
            {
                cmd.CommandText = sql;

                MyExtensions.AddParameterWithValue(cmd, "@Name", department.Name);
                MyExtensions.AddParameterWithValue(cmd, "@Description", department.Description);
                MyExtensions.AddParameterWithValue(cmd, "@DepartmentId", department.DepartmentId);

                cmd.ExecuteNonQuery();
            }
        }


    }
}
