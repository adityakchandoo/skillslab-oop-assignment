using MainLibrary.DTO;
using MainLibrary.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainLibrary.Services.Interfaces
{
    public interface IUserService
    {
        AuthenticateResponse AuthenticateUser(UserLoginFormDTO form);
        IEnumerable<User> GetAllUsersByType(UserRoleType userRoleType);
        void ForgetPass();
        void ResetPass();
        void Register(RegisterFormDTO reg);
        bool IsUserIdExists(string UserId);
        void ConfirmAccount(string UserId);
        IEnumerable<User> ExportSelectedEmployees();
        User GetUser(string UserId);
    }
}
