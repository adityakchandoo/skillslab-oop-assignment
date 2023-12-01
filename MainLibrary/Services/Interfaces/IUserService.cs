using MainLibrary.DTO;
using MainLibrary.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainLibrary.Service.Interfaces
{
    public interface IUserService
    {
        bool CheckLogin(UserLoginFormDTO form, out User user);
        void ForgetPass();
        void ResetPass();
        void Register(RegisterFormDTO reg);
        void ConfirmAccount(string UserId);
        IEnumerable<User> ExportSelectedEmployees();
    }
}
