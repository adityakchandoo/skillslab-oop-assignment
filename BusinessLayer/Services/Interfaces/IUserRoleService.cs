using Entities.DbCustom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services.Interfaces
{
    public interface IUserRoleService
    {
        IEnumerable<UserRoleAssigned> GetUserRolesAssigned(int UserId);
        void EditUserRoles(int UserId, UserRoleAssigned[] userRolesAssigned);
    }
}
