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
            string sql = "SELECT * FROM [dbo].[AppUser] WHERE Role = @Role;";

            List<AppUser> results = new List<AppUser>();

            using (IDbCommand cmd = _conn.CreateCommand())
            {
                cmd.CommandText = sql;
                DbHelper.AddParameterWithValue(cmd, "@Role", (int)userRoleEnum);

                using (IDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        results.Add(DbHelper.ConvertToObject<AppUser>(reader));
                    }
                }
            }
            return results;
        }

        // to cancel/move
        public IEnumerable<AppUser> GetUsersManagedBy(string UserId)
        {
            return base.GetMany(
                "SELECT * FROM [dbo].[AppUser] WHERE ManagerId = @ManagerId;",
                new Dictionary<string, object>() { { "ManagerId", UserId } }
            );
        }

    }
}
