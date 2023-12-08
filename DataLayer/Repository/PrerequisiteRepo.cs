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
        public IEnumerable<Prerequisite> GetPrerequisitesByTraining(int training)
        {
            var sql = @"SELECT Prerequisite.* FROM Prerequisite INNER JOIN TrainingPrerequisite ON Prerequisite.PrerequisiteId = TrainingPrerequisite.PrerequisiteId WHERE TrainingPrerequisite.TrainingId = @TrainingId";
            
            var param = new Dictionary<string, object>();
            param.Add("TrainingId", training);

            return base.GetMany(sql, param);
        }

    }
}
