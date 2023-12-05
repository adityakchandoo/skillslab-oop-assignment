using DataLayer;
using DataLayer.Generic;
using DataLayer.Repository.Interfaces;
using Entities.DbModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repository
{
    public class PrerequisiteRepo : DataAccessLayer<Prerequisite>, IPrerequisiteRepo
    {
        private readonly IDbConnection _conn;
        public PrerequisiteRepo(IDbContext dbContext) : base(dbContext)
        {
            _conn = dbContext.GetConn();
        }

    }
}
