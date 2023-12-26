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
        Task<AppUser> GetUserAsync(int UserId);
        Task<AuthenticateResponse> AuthenticateUserAsync(UserLoginFormDTO form);
        Task RegisterAsync(RegisterFormDTO reg);
        Task<IEnumerable<AppUser>> GetAllUsersByTypeAsync(UserRoleEnum userRoleEnum);
        Task<IEnumerable<AppUser>> ExportSelectedEmployeesAsync();
        Task<IEnumerable<AppUsersInlineRoles>> GetAllUsersWithInlineRolesAsync();
        Task<IEnumerable<AppUserRole>> GetRolesByUserIdAsync(int UserId);
        Task<bool> IsUsernameExistsAsync(string value);
        Task<bool> IsNICExistsAsync(string value);
        Task<bool> IsEmailExistsAsync(string value);
        Task<IEnumerable<AppUser>> GetUsersByManagerAsync(int UserId);
        Task<IEnumerable<AppUser>> GetUsersByManagerAndStatusAsync(int UserId, UserStatusEnum userStatusEnum);
        Task UpdateProfileAsync(int UserId, UpdateProfileDTO updateProfileDTO);
        Task UpdatePasswordAsync(int UserId, UpdatePasswordDTO updatePasswordDTO);
        Task ProcessNewUserAsync(int userId, bool approve);
        Task<bool> CheckPermissionAsync(int UserId, string permission);
    }
}
