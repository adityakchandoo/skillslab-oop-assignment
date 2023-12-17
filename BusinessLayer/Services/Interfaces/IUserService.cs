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
        AppUser GetUser(int UserId);
        AuthenticateResponse AuthenticateUser(UserLoginFormDTO form);
        void Register(RegisterFormDTO reg);
        IEnumerable<AppUser> GetAllUsersByType(UserRoleEnum userRoleEnum);
        IEnumerable<AppUser> ExportSelectedEmployees();
        bool IsUsernameExists(string value);
        bool IsNICExists(string value);
        bool IsEmailExists(string value);
        IEnumerable<AppUser> GetUsersByManager(int UserId);
        IEnumerable<AppUser> GetUsersByManagerAndStatus(int UserId, UserStatusEnum userStatusEnum);
        void ProcessNewUser(int userId, bool approve);
    }
}
