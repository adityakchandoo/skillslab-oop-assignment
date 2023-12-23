using Entities.DbCustom;
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
        IEnumerable<AppUsersInlineRoles> GetAllUsersWithInlineRoles();
        IEnumerable<AppUserRole> GetRolesByUserId(int UserId);
        bool IsUsernameExists(string value);
        bool IsNICExists(string value);
        bool IsEmailExists(string value);
        IEnumerable<AppUser> GetUsersByManager(int UserId);
        IEnumerable<AppUser> GetUsersByManagerAndStatus(int UserId, UserStatusEnum userStatusEnum);
        void UpdateProfile(int UserId, UpdateProfileDTO updateProfileDTO);
        void UpdatePassword(int UserId, UpdatePasswordDTO updatePasswordDTO);
        void ProcessNewUser(int userId, bool approve);
        bool CheckPermission(int UserId, string permission);
    }
}
