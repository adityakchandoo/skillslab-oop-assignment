using DataLayer.Generic;
using Entities.DbCustom;
using Entities.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repository.Interfaces
{
    public interface IUserRoleRepo : IDataAccessLayer<UserRole>
    {
        Task<IEnumerable<UserRoleAssigned>> GetUserRolesAssignedAsync(int UserId);
        Task DeleteUserRoleAsync(int UserId, int RoleId);
    }
}
