using MainLibrary.DTO;
using MainLibrary.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainLibrary.Services.Interfaces
{
    internal interface IUserService
    {
        void Login(UserLoginFormDTO user);
        void Logout();
        void ForgetPass();
        void ResetPass();
        void Register(User user);
        void ConfirmAccount(User user);
        IEnumerable<User> ExportSelectedEmployees();
    }
}
