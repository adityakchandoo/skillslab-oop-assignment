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

namespace DataLayer.Repository
{
    public class AppUserRepo : DataAccessLayer<AppUser>, IAppUserRepo
    {
        private readonly IDbConnection _conn;
        public AppUserRepo(IDbContext dbContext) : base(dbContext)
        {
            _conn = dbContext.GetConn();
        }

        public AppUserDetails GetUserByUsername(string username)
        {
            try
            {
                string sql = @" SELECT AppUser.*, UserRole.RoleId As Role, Role.Name AS RoleName, UserManager.ManagerId FROM AppUser
                                INNER JOIN UserRole ON AppUser.UserId = UserRole.UserId
                                INNER JOIN Role ON Role.RoleId = UserRole.RoleId
                                LEFT JOIN UserManager ON AppUser.UserId = UserManager.UserId
                                WHERE AppUser.Username = @Username;";

                using (IDbCommand cmd = _conn.CreateCommand())
                {
                    cmd.CommandText = sql;
                    DbHelper.AddParameterWithValue(cmd, "@Username", username);

                    using (IDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            return DbHelper.ConvertToObject<AppUserDetails>(reader);
                        }
                    }
                    return null;
                }
            }
            catch (Exception ex) { throw ex; }
        }

        public bool IsRecordExists(string column, string value)
        {
            try
            {
                // var column is coming from service layer not from user input
                string sql = $"SELECT COALESCE((SELECT 1 FROM AppUser WHERE {column} = @value), 0);";

                using (IDbCommand cmd = _conn.CreateCommand())
                {
                    cmd.CommandText = sql;
                    DbHelper.AddParameterWithValue(cmd, "@column", column);
                    DbHelper.AddParameterWithValue(cmd, "@value", value);

                    var result = (int)cmd.ExecuteScalar() == 1 ? true : false;

                    return result;
                }
            }
            catch (Exception ex) { throw ex; }
        }

        public AppUser GetUserManager(int UserId)
        {
            var user = base.GetMany(
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
                new Dictionary<string, object>() { { "UserId", UserId } }
            );

            if (user.Count > 0)
            {
                return user[0];
            }

            return null;
        }
        public int CreateUserReturningID(AppUser appUser)
        {
            try
            {
                string sql = @"INSERT INTO [dbo].[AppUser] (UserName, Password, FirstName, LastName, Email, DOB, NIC, MobileNumber, CreatedOn, Status, DepartmentId) 
                               OUTPUT Inserted.UserId 
                               VALUES (@UserName, @Password, @FirstName, @LastName, @Email, @DOB, @NIC, @MobileNumber, @CreatedOn, @Status, @DepartmentId)";


                using (IDbCommand cmd = _conn.CreateCommand())
                {
                    cmd.CommandText = sql;

                    DbHelper.AddParameterWithValue(cmd, "@UserName", appUser.UserName);
                    DbHelper.AddParameterWithValue(cmd, "@Password", appUser.Password);
                    DbHelper.AddParameterWithValue(cmd, "@FirstName", appUser.FirstName);
                    DbHelper.AddParameterWithValue(cmd, "@LastName", appUser.LastName);
                    DbHelper.AddParameterWithValue(cmd, "@Email", appUser.Email);
                    DbHelper.AddParameterWithValue(cmd, "@DOB", appUser.DOB);
                    DbHelper.AddParameterWithValue(cmd, "@NIC", appUser.NIC);
                    DbHelper.AddParameterWithValue(cmd, "@MobileNumber", appUser.MobileNumber);
                    DbHelper.AddParameterWithValue(cmd, "@CreatedOn", appUser.CreatedOn);
                    DbHelper.AddParameterWithValue(cmd, "@Status", appUser.Status);

                    if (appUser.DepartmentId == -1)
                    {
                        DbHelper.AddParameterWithValue(cmd, "@DepartmentId", DBNull.Value);                        
                    }
                    else
                    {
                        DbHelper.AddParameterWithValue(cmd, "@DepartmentId", appUser.DepartmentId);
                    }

                    return (int)cmd.ExecuteScalar();
                }

            }
            catch (Exception ex) { throw ex; }
        }

        public IEnumerable<AppUser> GetAllUsersByRole(UserRoleEnum userRoleEnum)
        {
            return base.GetMany(
                "SELECT * FROM [dbo].[AppUser] INNER JOIN UserRole ON UserRole.UserId = AppUser.UserId WHERE RoleId = @RoleId;",
                new Dictionary<string, object>() { { "RoleId", (int)userRoleEnum } }
            );
        }

        public IEnumerable<AppUser> GetAllUsersByManager(int ManagerId)
        {
            return base.GetMany(
                "SELECT * FROM [dbo].[AppUser] INNER JOIN UserManager ON AppUser.UserId = UserManager.UserId WHERE UserManager.ManagerId = @ManagerId;",
                new Dictionary<string, object>() { { "ManagerId", ManagerId } }
            );
        }

        public IEnumerable<AppUser> GetAllUsersByManagerAndStatus(int ManagerId, UserStatusEnum userStatusEnum)
        {
            return base.GetMany(
                "SELECT * FROM [dbo].[AppUser] INNER JOIN UserManager ON AppUser.UserId = UserManager.UserId WHERE UserManager.ManagerId = @ManagerId AND AppUser.Status = @Status;",
                new Dictionary<string, object>() { { "ManagerId", ManagerId }, { "Status", (int)userStatusEnum } }
            );
        }

    }
}
