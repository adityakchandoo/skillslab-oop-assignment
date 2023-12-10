using DataLayer.Generic;
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
        IEnumerable<AppUser> GetAllUsersByType(UserRoleEnum userRoleType);
        IEnumerable<AppUser> GetUsersByManager(string UserId);
        IEnumerable<AppUser> GetUsersByManagerAndStatus(string UserId, UserStatusEnum userStatusEnum);
    }
}