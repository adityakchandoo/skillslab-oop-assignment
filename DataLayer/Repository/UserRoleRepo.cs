using DataLayer.Generic;
using DataLayer.Repository.Interfaces;
using Entities;
using Entities.AppLogger;
using Entities.DbCustom;
using Entities.DbModels;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace DataLayer.Repository
{
    public class UserRoleRepo : DataAccessLayer<UserRole>, IUserRoleRepo
    {
        ILogger _logger;
        private readonly SqlConnection _conn;
        public UserRoleRepo(ILogger logger, IDbContext dbContext) : base(logger, dbContext)
        {
            _logger = logger;
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
                _logger.LogError(ex);
                throw new DbErrorException("Database Error");
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
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw new DbErrorException("Database Error");
                throw;
            }
        }
    }
}
