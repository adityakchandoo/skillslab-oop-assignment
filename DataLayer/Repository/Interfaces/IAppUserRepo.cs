using DataLayer.Generic;
using Entities.DbCustom;
using Entities.DbModels;
using Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repository.Interfaces
{
    public interface IAppUserRepo : IDataAccessLayer<AppUser>
    {
        Task<AppUser> GetUserByUsernameAsync(string username);
        Task<IEnumerable<AppUserRole>> GetRolesByUserIdAsync(int UserId);
        Task<IEnumerable<AppUsersInlineRoles>> GetAllUsersWithInlineRolesAsync();
        Task<bool> IsRecordExistsAsync(string column, string value);
        Task<AppUser> GetUserManagerAsync(int UserId);
        Task<int> CreateUserReturningIDAsync(AppUser appUser);
        Task<bool> CheckPermissionAsync(int UserId, string permission);
        Task<IEnumerable<AppUser>> GetAllUsersByRoleAsync(UserRoleEnum userRoleEnum);
        Task<IEnumerable<AppUser>> GetAllUsersByManagerAsync(int ManagerId);
        Task<IEnumerable<AppUser>> GetAllUsersByManagerAndStatusAsync(int ManagerId, UserStatusEnum userStatusEnum);
        Task SoftDeleteAppUser(int userId);

    }
}