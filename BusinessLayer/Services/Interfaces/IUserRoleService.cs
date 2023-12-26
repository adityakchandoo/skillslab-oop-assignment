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
        Task<IEnumerable<UserRoleAssigned>> GetUserRolesAssignedAsync(int UserId);
        Task EditUserRolesAsync(int UserId, UserRoleAssigned[] userRolesAssigned);
    }
}
