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
        AuthenticateResponse AuthenticateUser(UserLoginFormDTO form);
        void ForgetPass();
        void ResetPass();
        void Register(RegisterFormDTO reg);
        bool IsUserIdExists(string UserId);
        void ConfirmAccount(string UserId);
        IEnumerable<User> ExportSelectedEmployees();
    }
}
