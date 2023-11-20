using MainLibrary.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MainLibrary.DAL.Interfaces;

namespace MainLibrary.DAL
{
    internal class UserDAL : IUserDAL
    {
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
            throw new NotImplementedException();
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
