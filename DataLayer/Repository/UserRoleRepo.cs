using DataLayer.Generic;
using DataLayer.Repository.Interfaces;
using Entities.DbCustom;
using Entities.DbModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repository
{
    public class UserRoleRepo : DataAccessLayer<UserRole>, IUserRoleRepo
    {
        private readonly IDbConnection _conn;
        public UserRoleRepo(IDbContext dbContext) : base(dbContext)
        {
            _conn = dbContext.GetConn();
        }

        public IEnumerable<UserRoleAssigned> GetUserRolesAssigned(int UserId)
        {
            List<UserRoleAssigned> results = new List<UserRoleAssigned>();

            string sql = @" SELECT 
                                r.RoleId, 
                                r.Name AS RoleName, 
                                CASE 
                                    WHEN ur.UserId IS NULL THEN 0 
                                    ELSE 1 
                                END AS IsAssigned
                            FROM 
                                Role r
                            LEFT JOIN 
                                (SELECT * FROM UserRole WHERE UserId = @UserId) ur ON r.RoleId = ur.RoleId
                            ORDER BY 
                                r.RoleId;";

            try
            {
                using (IDbCommand cmd = _conn.CreateCommand())
                {
                    cmd.CommandText = sql;
                    DbHelper.AddParameterWithValue(cmd, "@UserId", UserId);

                    using (IDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            results.Add(DbHelper.ConvertToObject<UserRoleAssigned>(reader));
                        };

                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
                throw;
            }

            return results;
        }

        public void DeleteUserRole(int UserId, int RoleId)
        {
            try
            {
                string sql = @"DELETE FROM UserRole WHERE UserId = @UserId AND RoleId = @RoleId;";


                using (IDbCommand cmd = _conn.CreateCommand())
                {
                    cmd.CommandText = sql;

                    DbHelper.AddParameterWithValue(cmd, "@UserId", UserId);
                    DbHelper.AddParameterWithValue(cmd, "@RoleId", RoleId);

                    cmd.ExecuteScalar();
                }

            }
            catch (Exception ex) { throw ex; }
        }
    }
}
