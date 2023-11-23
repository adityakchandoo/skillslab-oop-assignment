using MainLibrary.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using MainLibrary.Repo.Interfaces;
using MainLibrary.Entities.Types;


namespace MainLibrary.Repo
{
    public class UserRepo : IUserRepo
    {
        DbContext _dbContext;
        public UserRepo(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void CreateUser(User user)
        {
            string insertQuery = "INSERT INTO [dbo].[AppUser] (FirstName,LastName,Password,Email,DOB,NIC,MobileNumber,Status,UserType) VALUES " +
                "(@FirstName,@LastName,@Username,@Email,@DOB,@NIC,@MobileNumber,@Status,@UserType)";

            SqlCommand cmd = new SqlCommand(insertQuery, _dbContext.GetConn());

            cmd.Parameters.AddWithValue("@FirstName", user.FirstName);
            cmd.Parameters.AddWithValue("@LastName", user.LastName);
            cmd.Parameters.AddWithValue("@Password", user.Password);
            cmd.Parameters.AddWithValue("@Email", user.Email);
            cmd.Parameters.AddWithValue("@DOB", user.DOB);
            cmd.Parameters.AddWithValue("@NIC", user.NIC);
            cmd.Parameters.AddWithValue("@MobileNumber", user.MobileNumber);
            cmd.Parameters.AddWithValue("@Status", user.Status);
            cmd.Parameters.AddWithValue("@UserType", user.UserType);

            cmd.ExecuteNonQuery();

        }

        public void DeleteUser(int UserId)
        {
            string delQuery = "DELETE FROM [dbo].[AppUser] WHERE Id = @id";

            SqlCommand cmd = new SqlCommand(delQuery, _dbContext.GetConn());
            cmd.Parameters.AddWithValue("@id", user_id);

            cmd.ExecuteNonQuery();
        }

        public IEnumerable<User> GetAllUsers()
        {
            List<User> results = new List<User>();

            using (SqlCommand command = new SqlCommand("SELECT * FROM [dbo].[AppUser];", _dbContext.GetConn()))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        // Map the data from the SqlDataReader to your model
                        User model = new User
                        {
                            Id = (string)reader["UserId"],
                            FirstName = (string)reader["FirstName"],
                            LastName = (string)reader["LastName"],
                            Password = (string)reader["Password"],
                            Email = (string)reader["Email"],
                            DOB = DateTime.Parse(reader["DOB"].ToString()),
                            NIC = (string)reader["NIC"],
                            MobileNumber = (string)reader["MobileNumber"],
                            Status = (StatusType)(int)reader["Status"],
                            UserType = (UserType)(int)reader["UserType"],
                        };

                        results.Add(model);
                    }
                }
            }
            return results;
        }


        public User GetUser(int UserId)
        {

            User user = new User();

            using (SqlCommand command = new SqlCommand("SELECT * FROM [dbo].[AppUser] WHERE UserId = @UserId;", _dbContext.GetConn()))
            {
                command.Parameters.AddWithValue("@UserId", user_id);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        user.UserId = (string)reader["UserId"];
                        user.FirstName = (string)reader["FirstName"];
                        user.LastName = (string)reader["LastName"];
                        user.Password = (string)reader["Password"];
                        user.Email = (string)reader["Email"];
                        user.DOB = DateTime.Parse(reader["DOB"].ToString());
                        user.NIC = (string)reader["NIC"];
                        user.MobileNumber = (string)reader["MobileNumber"];
                        user.Status = (StatusType)(int)reader["Status"];
                        user.UserType = (UserType)(int)reader["UserType"];

                        return user;
                    }
                }
            }

            return null;
        }

        public void UpdateUser(User user)
        {

            string updateQuery = "UPDATE [dbo].[AppUser] SET FirstName = @FirstName, LastName = @LastName, Password = @Password, Email = @Email, DOB = @DOB, NIC = @NIC, MobileNumber = @MobileNumber, Status = @Status, UserType = @UserType WHERE UserId = @UserId";

            SqlCommand cmd = new SqlCommand(updateQuery, _dbContext.GetConn());

            cmd.Parameters.AddWithValue("@FirstName", user.FirstName);
            cmd.Parameters.AddWithValue("@LastName", user.LastName);
            cmd.Parameters.AddWithValue("@Password", user.Password);
            cmd.Parameters.AddWithValue("@Email", user.Email);
            cmd.Parameters.AddWithValue("@DOB", user.DOB);
            cmd.Parameters.AddWithValue("@NIC", user.NIC);
            cmd.Parameters.AddWithValue("@MobileNumber", user.MobileNumber);
            cmd.Parameters.AddWithValue("@Status", user.Status);
            cmd.Parameters.AddWithValue("@UserType", user.UserType);

            cmd.ExecuteNonQuery();

        }
    }
}
