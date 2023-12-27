using System;
using System.Collections.Generic;
using System.Data;
using DataLayer.Generic;
using DataLayer.Repository.Interfaces;
using Entities.DbModels;
using Entities.Enums;
using DataLayer;
using Entities.DbCustom;
using System.Runtime.Remoting.Messaging;
using System.Data.SqlClient;
using System.Collections;
using System.Threading.Tasks;
using System.Linq;
using Entities.AppLogger;
using Entities;

namespace DataLayer.Repository
{
    public class AppUserRepo : DataAccessLayer<AppUser>, IAppUserRepo
    {
        ILogger _logger;
        private readonly SqlConnection _conn;
        public AppUserRepo(ILogger logger, IDbContext dbContext) : base(logger, dbContext)
        {
            _logger = logger;
            _conn = dbContext.GetConn();
        }
        public Task<IEnumerable<AppUser>> GetAllUsersByRoleAsync(UserRoleEnum userRoleEnum)
        {
            return base.GetMany(
                "SELECT * FROM [dbo].[AppUser] INNER JOIN UserRole ON UserRole.UserId = AppUser.UserId WHERE RoleId = @RoleId;",
                new Dictionary<string, object>() { { "@RoleId", (int)userRoleEnum } }
            );
        }

        public Task<IEnumerable<AppUser>> GetAllUsersByManagerAsync(int ManagerId)
        {
            return base.GetMany(
                "SELECT * FROM [dbo].[AppUser] INNER JOIN UserManager ON AppUser.UserId = UserManager.UserId WHERE UserManager.ManagerId = @ManagerId;",
                new Dictionary<string, object>() { { "@ManagerId", ManagerId } }
            );
        }

        public Task<IEnumerable<AppUser>> GetAllUsersByManagerAndStatusAsync(int ManagerId, UserStatusEnum userStatusEnum)
        {
            return base.GetMany(
                "SELECT * FROM [dbo].[AppUser] INNER JOIN UserManager ON AppUser.UserId = UserManager.UserId WHERE UserManager.ManagerId = @ManagerId AND AppUser.Status = @Status;",
                new Dictionary<string, object>() { { "@ManagerId", ManagerId }, { "@Status", (int)userStatusEnum } }
            );
        }

        public async Task<AppUser> GetUserByUsernameAsync(string username)
        {
            try
            {
                string sql = @" SELECT * FROM AppUser WHERE Username = @Username;";

                using (SqlCommand cmd = new SqlCommand(sql, _conn))
                {
                    cmd.Parameters.AddWithValue("@Username", username);

                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        while (reader.Read())
                        {
                            return DbHelper.ConvertToObject<AppUser>(reader);
                        }
                    }
                    return null;
                }
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex);
                throw new DbErrorException("Database Error");
                throw;
            }
        }

        public async Task<IEnumerable<AppUserRole>> GetRolesByUserIdAsync(int UserId)
        {
            List<AppUserRole> results = new List<AppUserRole>();
            try
            {
                string sql = @"SELECT r.RoleId, r.Name AS RoleName FROM UserRole ur INNER JOIN Role r ON ur.RoleId = r.RoleId WHERE ur.UserId = @UserId;";

                using (SqlCommand cmd = new SqlCommand(sql, _conn))
                {
                    cmd.Parameters.AddWithValue("@UserId", UserId);

                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        while (reader.Read())
                        {
                            results.Add(DbHelper.ConvertToObject<AppUserRole>(reader));
                        }
                    }
                    return results;
                }
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex);
                throw new DbErrorException("Database Error");
                throw;
            }
        }
        public async Task<IEnumerable<AppUsersInlineRoles>> GetAllUsersWithInlineRolesAsync()
        {
            List<AppUsersInlineRoles> results = new List<AppUsersInlineRoles>();

            string sql = @"SELECT U.*, R.Roles FROM AppUser U
                           LEFT JOIN UserRolesInline R ON U.UserId = R.UserId;";

            try
            {
                using (SqlCommand cmd = new SqlCommand(sql, _conn))
                {
                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        while (reader.Read())
                        {
                            results.Add(DbHelper.ConvertToObject<AppUsersInlineRoles>(reader));
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

        public async Task<bool> IsRecordExistsAsync(string column, string value)
        {
            try
            {
                // var column is coming from service layer not from user input
                string sql = $"SELECT COALESCE((SELECT 1 FROM AppUser WHERE {column} = @value), 0);";

                using (SqlCommand cmd = new SqlCommand(sql, _conn))
                {
                    cmd.Parameters.AddWithValue("@value", value);

                    var result = (int)(await cmd.ExecuteScalarAsync()) == 1 ? true : false;

                    return result;
                }
            }
            catch (Exception ex) { throw ex; }
        }

        public async Task<AppUser> GetUserManagerAsync(int UserId)
        {
            var user = await base.GetMany(
                @"  SELECT 
                        Manager.* 
                    FROM 
                        AppUser AU
                    INNER JOIN 
                        UserManager UM ON AU.UserId = UM.UserId
                    INNER JOIN 
                        AppUser Manager ON UM.ManagerId = Manager.UserId
                    WHERE 
                        AU.UserId = @UserId;
                    ",
                new Dictionary<string, object>() { { "@UserId", UserId } }
            );

            if (user.Count() > 0)
            {
                return user.First();
            }

            return null;
        }

        public async Task<int> CreateUserReturningIDAsync(AppUser appUser)
        {
            try
            {
                string sql = @"INSERT INTO [dbo].[AppUser] (UserName, Password, FirstName, LastName, Email, DOB, NIC, MobileNumber, CreatedOn, Status, DepartmentId) 
                               OUTPUT Inserted.UserId 
                               VALUES (@UserName, @Password, @FirstName, @LastName, @Email, @DOB, @NIC, @MobileNumber, @CreatedOn, @Status, @DepartmentId)";


                using (SqlCommand cmd = new SqlCommand(sql, _conn))
                {
                    cmd.Parameters.AddWithValue("@UserName", appUser.UserName);
                    cmd.Parameters.AddWithValue("@Password", appUser.Password);
                    cmd.Parameters.AddWithValue("@FirstName", appUser.FirstName);
                    cmd.Parameters.AddWithValue("@LastName", appUser.LastName);
                    cmd.Parameters.AddWithValue("@Email", appUser.Email);
                    cmd.Parameters.AddWithValue("@DOB", appUser.DOB);
                    cmd.Parameters.AddWithValue("@NIC", appUser.NIC);
                    cmd.Parameters.AddWithValue("@MobileNumber", appUser.MobileNumber);
                    cmd.Parameters.AddWithValue("@CreatedOn", appUser.CreatedOn);
                    cmd.Parameters.AddWithValue("@Status", appUser.Status);
                    cmd.Parameters.AddWithValue("@DepartmentId", appUser.DepartmentId);

                    return (int)(await cmd.ExecuteScalarAsync());
                }

            }
            catch (Exception ex) 
            {
                _logger.LogError(ex);
                throw new DbErrorException("Database Error");
                throw;
            }
        }


        public async Task<bool> CheckPermissionAsync(int UserId, string permission)
        {
            try
            {
                string sql = $"EXEC CheckUserPermission @UserId = @UserID, @Permission = @Permission";

                using (SqlCommand cmd = new SqlCommand(sql, _conn))
                {
                    cmd.Parameters.AddWithValue("@UserID", UserId);
                    cmd.Parameters.AddWithValue("@Permission", permission);

                    var result = (int)(await cmd.ExecuteScalarAsync()) == 1 ? true : false;

                    return result;
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
