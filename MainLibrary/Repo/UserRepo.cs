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
            throw new NotImplementedException();
        }

        public void DeleteUser(int user_id)
        {
            throw new NotImplementedException();
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
                            Id = (int)reader["Id"],
                            FirstName = (string)reader["FirstName"],
                            LastName = (string)reader["LastName"],
                            Username = (string)reader["Username"],
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

        public User GetByUsername(string username)
        {
            throw new NotImplementedException();
        }

        public User GetUser(int user_id)
        {
            throw new NotImplementedException();
        }

        public void UpdateUser(User user)
        {
            throw new NotImplementedException();
        }
    }
}
