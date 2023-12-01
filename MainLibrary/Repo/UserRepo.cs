using MainLibrary.Entities;
using System;
using System.Collections.Generic;
using MainLibrary.Repo.Interfaces;
using System.Data;
using MainLibrary.Helpers;

namespace MainLibrary.Repo
{
    public class UserRepo : IUserRepo
    {
        IDbConnection _conn;
        public UserRepo(IDbContext dbContext)
        {
            _conn = dbContext.GetConn();
        }

        public void CreateUser(User user)
        {
            string sql = "INSERT INTO [dbo].[AppUser] (UserId,FirstName,LastName,Password,Email,DOB,NIC,MobileNumber,CreatedOn,Status,Role) VALUES " +
                         "(@UserId,@FirstName,@LastName,@Password,@Email,@DOB,@NIC,@MobileNumber,@CreatedOn,@Status,@Role);";

            using (IDbCommand cmd = _conn.CreateCommand())
            {
                cmd.CommandText = sql;

                MyExtensions.AddParameterWithValue(cmd, "@UserId", user.UserId);
                MyExtensions.AddParameterWithValue(cmd, "@FirstName", user.FirstName);
                MyExtensions.AddParameterWithValue(cmd, "@LastName", user.LastName);
                MyExtensions.AddParameterWithValue(cmd, "@Password", user.Password);
                MyExtensions.AddParameterWithValue(cmd, "@Email", user.Email);
                MyExtensions.AddParameterWithValue(cmd, "@DOB", user.DOB);
                MyExtensions.AddParameterWithValue(cmd, "@NIC", user.NIC);
                MyExtensions.AddParameterWithValue(cmd, "@MobileNumber", user.MobileNumber);
                MyExtensions.AddParameterWithValue(cmd, "@CreatedOn", user.CreatedOn);
                MyExtensions.AddParameterWithValue(cmd, "@Status", user.Status);
                MyExtensions.AddParameterWithValue(cmd, "@Role", user.Role);

                cmd.ExecuteNonQuery();
            }
        }

        public void DeleteUser(string UserId)
        {
            string sql = "DELETE FROM [dbo].[AppUser] WHERE Id = @id";

            using (IDbCommand cmd = _conn.CreateCommand())
            {
                cmd.CommandText = sql;

                MyExtensions.AddParameterWithValue(cmd, "@id", UserId);

                cmd.ExecuteNonQuery();
            }

        }

        public IEnumerable<User> GetAllUsers()
        {
            string sql = "SELECT * FROM [dbo].[AppUser];";

            List<User> results = new List<User>();

            using (IDbCommand cmd = _conn.CreateCommand())
            {
                cmd.CommandText = sql;

                using (IDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        results.Add(MyExtensions.ConvertToObject<User>(reader));
                    }
                }
            }
            return results;
        }


        public User GetUser(string UserId)
        {
            string sql = "SELECT * FROM [dbo].[AppUser] WHERE [dbo].[AppUser].[UserId] = 'mcb';";

            using (IDbCommand cmd = _conn.CreateCommand())
            {
                cmd.CommandText = sql;
                MyExtensions.AddParameterWithValue(cmd, "@id", UserId);

                using (IDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        return MyExtensions.ConvertToObject<User>(reader);
                    }
                }
            }
            return null;
        }

        public void UpdateUser(User user)
        {
            string sql = "UPDATE [dbo].[AppUser] SET FirstName = @FirstName, LastName = @LastName, Password = @Password, Email = @Email, DOB = @DOB, NIC = @NIC, MobileNumber = @MobileNumber, Status = @Status, Role = @Role WHERE UserId = @UserId;";

            using (IDbCommand cmd = _conn.CreateCommand())
            {
                cmd.CommandText = sql;

                MyExtensions.AddParameterWithValue(cmd, "@FirstName", user.FirstName);
                MyExtensions.AddParameterWithValue(cmd, "@LastName", user.LastName);
                MyExtensions.AddParameterWithValue(cmd, "@Password", user.Password);
                MyExtensions.AddParameterWithValue(cmd, "@Email", user.Email);
                MyExtensions.AddParameterWithValue(cmd, "@DOB", user.DOB);
                MyExtensions.AddParameterWithValue(cmd, "@NIC", user.NIC);
                MyExtensions.AddParameterWithValue(cmd, "@MobileNumber", user.MobileNumber);
                MyExtensions.AddParameterWithValue(cmd, "@CreatedOn", user.CreatedOn);
                MyExtensions.AddParameterWithValue(cmd, "@Status", user.Status);
                MyExtensions.AddParameterWithValue(cmd, "@Role", user.Role);

                cmd.ExecuteNonQuery();
            }

        }
    }
}
