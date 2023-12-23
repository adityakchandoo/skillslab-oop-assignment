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

        public IEnumerable<UserRoleAssigned> GetUserRolesAssigned(int UserId)
        {
            return _userRoleRepo.GetUserRolesAssigned(UserId);
        }

        public void EditUserRoles(int UserId, UserRoleAssigned[] userRolesAssigned)
        {
            UserRoleAssigned[] oldUserRolesAssigned = _userRoleRepo.GetUserRolesAssigned(UserId)
                                                                   .OrderBy(k => k.RoleId)
                                                                   .ToArray();

            UserRoleAssigned[] newUserRolesAssigned = userRolesAssigned.OrderBy(k => k.RoleId).ToArray();

            for (int i = 0; i < oldUserRolesAssigned.Length; i++)
            {
                if (oldUserRolesAssigned[i].IsAssigned == 0 && newUserRolesAssigned[i].IsAssigned == 1)
                {
                    _userRoleRepo.Insert(new Entities.DbModels.UserRole()
                    {
                        UserId = UserId,
                        RoleId = oldUserRolesAssigned[i].RoleId,
                    });
                }
                else if (oldUserRolesAssigned[i].IsAssigned == 1 && newUserRolesAssigned[i].IsAssigned == 0)
                {
                    _userRoleRepo.DeleteUserRole(UserId, oldUserRolesAssigned[i].RoleId);
                }
            }

        }

    }
}
