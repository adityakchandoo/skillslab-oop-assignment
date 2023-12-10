using System;
using System.Collections.Generic;
using System.Data;
using DataLayer.Generic;
using DataLayer.Repository.Interfaces;
using Entities.DbModels;
using Entities.Enums;
using DataLayer;

namespace DataLayer.Repository
{
    public class AppUserRepo : DataAccessLayer<AppUser>, IAppUserRepo
    {
        private readonly IDbConnection _conn;
        public AppUserRepo(IDbContext dbContext) : base(dbContext)
        {
            _conn = dbContext.GetConn();
        }
        public IEnumerable<AppUser> GetAllUsersByType(UserRoleEnum userRoleEnum)
        {
            return base.GetMany(
                "SELECT * FROM [dbo].[AppUser] WHERE Role = @Role;",
                new Dictionary<string, object>() { { "Role", (int)userRoleEnum } }
            );
        }

        public IEnumerable<AppUser> GetUsersByManager(string UserId)
        {
            return base.GetMany(
                "SELECT * FROM [dbo].[AppUser] WHERE ManagerId = @ManagerId;",
                new Dictionary<string, object>() { { "ManagerId", UserId } }
            );
        }

        public IEnumerable<AppUser> GetUsersByManagerAndStatus(string UserId, UserStatusEnum userStatusEnum)
        {
            return base.GetMany(
                "SELECT * FROM [dbo].[AppUser] WHERE ManagerId = @ManagerId AND Status = @Status;",
                new Dictionary<string, object>() { { "ManagerId", UserId }, { "Status", userStatusEnum } }
            );
        }

    }
}
