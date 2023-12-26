using DataLayer.Generic;
using DataLayer.Repository.Interfaces;
using Entities.DbCustom;
using Entities.DbModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repository
{
    public class UserRoleRepo : DataAccessLayer<UserRole>, IUserRoleRepo
    {
        private readonly SqlConnection _conn;
        public UserRoleRepo(IDbContext dbContext) : base(dbContext)
        {
            _conn = dbContext.GetConn();
        }

        public async Task<IEnumerable<UserRoleAssigned>> GetUserRolesAssignedAsync(int UserId)
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
                using (SqlCommand cmd = new SqlCommand(sql, _conn))
                {
                    cmd.Parameters.AddWithValue("@UserId", UserId);

                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
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

        public async Task DeleteUserRoleAsync(int UserId, int RoleId)
        {
            try
            {
                string sql = @"DELETE FROM UserRole WHERE UserId = @UserId AND RoleId = @RoleId;";

                using (SqlCommand cmd = new SqlCommand(sql, _conn))
                {
                    cmd.Parameters.AddWithValue("@UserId", UserId);
                    cmd.Parameters.AddWithValue("@RoleId", RoleId);

                    await cmd.ExecuteScalarAsync();
                }

            }
            catch (Exception ex) { throw ex; }
        }
    }
}
