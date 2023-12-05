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
    public class TrainingPrerequisiteRepo : DataAccessLayer<TrainingPrerequisite>, ITrainingPrerequisiteRepo
    {
        private readonly IDbConnection _conn;
        public TrainingPrerequisiteRepo(IDbContext dbContext) : base(dbContext)
        {
            _conn = dbContext.GetConn();
        }
    }
}
