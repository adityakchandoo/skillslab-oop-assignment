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
        IEnumerable<UserRoleAssigned> GetUserRolesAssigned(int UserId);
        void DeleteUserRole(int UserId, int RoleId);
    }
}
