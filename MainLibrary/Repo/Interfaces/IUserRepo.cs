using MainLibrary.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainLibrary.Repo.Interfaces
{
    public interface IUserRepo
    {
        IEnumerable<User> GetAllUsers();
        User GetUser(int UserId);
        void CreateUser(User user);
        void UpdateUser(User user);
        void DeleteUser(int UserId);
    }
}
