using MainLibrary.DTO;
using MainLibrary.Entities;
using MainLibrary.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MainLibrary.DAL;
using MainLibrary.Services;

namespace MainLibrary.DAL
{
    public class UserService : IUserService
    {
        public IEnumerable<User> ExportSelectedEmployees()
        {
            throw new NotImplementedException();
        }

        public void ForgetPass()
        {
            throw new NotImplementedException();
        }

        void IUserService.ResetPass()
        {
            throw new NotImplementedException();
        }

        public void Login(UserLoginFormDTO user)
        {

            /*
            var userDAL = new UserDAL();

            var targetUser = userDAL.GetByUsername(user.user);

            if (targetUser != null)
            {
                throw new Exception();
            }

            if (targetUser.Password != user.pass)
            {
                throw new Exception();
            }

            */
        }

        public void Logout()
        {
            throw new NotImplementedException();
        }

        public void Register(User user)
        {
            /*
            var userDAL = new UserDAL();
            var NotificationService = new NotificationService();

            userDAL.CreateUser(user);
            NotificationService.SendNotification(user);
            */
        }

        public void ConfirmAccount(User user)
        {
            throw new NotImplementedException();
        }
    }
}
