using BusinessLayer.Services.Interfaces;
using DataLayer.Repository.Interfaces;
using Entities.DbCustom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services
{
    public class UserRoleService : IUserRoleService
    {
        private readonly IUserRoleRepo _userRoleRepo;
        public UserRoleService(IUserRoleRepo userRoleRepo)
        {
            _userRoleRepo = userRoleRepo;
        }

        public async Task<IEnumerable<UserRoleAssigned>> GetUserRolesAssignedAsync(int UserId)
        {
            return await _userRoleRepo.GetUserRolesAssignedAsync(UserId);
        }

        public async Task EditUserRolesAsync(int UserId, UserRoleAssigned[] userRolesAssigned)
        {

            UserRoleAssigned[] oldUserRolesAssigned = (await _userRoleRepo.GetUserRolesAssignedAsync(UserId))
                                                                   .OrderBy(k => k.RoleId)
                                                                   .ToArray();

            UserRoleAssigned[] newUserRolesAssigned = userRolesAssigned.OrderBy(k => k.RoleId).ToArray();

            for (int i = 0; i < oldUserRolesAssigned.Length; i++)
            {
                if (oldUserRolesAssigned[i].IsAssigned == 0 && newUserRolesAssigned[i].IsAssigned == 1)
                {
                    await _userRoleRepo.Insert(new Entities.DbModels.UserRole()
                    {
                        UserId = UserId,
                        RoleId = oldUserRolesAssigned[i].RoleId,
                    });
                }
                else if (oldUserRolesAssigned[i].IsAssigned == 1 && newUserRolesAssigned[i].IsAssigned == 0)
                {
                    await _userRoleRepo.DeleteUserRoleAsync(UserId, oldUserRolesAssigned[i].RoleId);
                }
            }

        }

    }
}
