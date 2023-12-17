﻿using DataLayer.Generic;
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
        AppUserDetails GetUserByUsername(string username);
        bool IsRecordExists(string column, string value);
        AppUser GetUserManager(int UserId);
        int CreateUserReturningID(AppUser appUser);
        IEnumerable<AppUser> GetAllUsersByRole(UserRoleEnum userRoleEnum);
        IEnumerable<AppUser> GetAllUsersByManager(int ManagerId);
        IEnumerable<AppUser> GetAllUsersByManagerAndStatus(int ManagerId, UserStatusEnum userStatusEnum);

    }
}