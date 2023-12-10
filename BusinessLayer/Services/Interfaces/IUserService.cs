using Entities.DbModels;
using Entities.DTO;
using Entities.Enums;
using Entities.FormDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services.Interfaces
{
    public interface IUserService
    {
        AuthenticateResponse AuthenticateUser(UserLoginFormDTO form);
        IEnumerable<AppUser> GetAllUsersByType(UserRoleEnum userRoleEnum);
        void ForgetPass();
        void ResetPass();
        void Register(RegisterFormDTO reg);
        bool IsUserIdExists(string UserId);
        void ConfirmAccount(string UserId);
        IEnumerable<AppUser> ExportSelectedEmployees();
        AppUser GetUser(string UserId);
        IEnumerable<AppUser> GetUsersByManager(string UserId);
        IEnumerable<AppUser> GetUsersByManagerAndStatus(string UserId, UserStatusEnum userStatusEnum);
        void ProcessNewUser(string userId, bool approve);
    }
}
