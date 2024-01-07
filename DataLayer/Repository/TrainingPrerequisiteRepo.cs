using DataLayer.Generic;
using DataLayer.Repository.Interfaces;
using Entities.AppLogger;
using Entities.DbModels;
using System.Data.SqlClient;

namespace DataLayer.Repository
{
    public class TrainingPrerequisiteRepo : DataAccessLayer<TrainingPrerequisite>, ITrainingPrerequisiteRepo
    {
        ILogger _logger;
        private readonly SqlConnection _conn;
        public TrainingPrerequisiteRepo(ILogger logger, IDbContext dbContext) : base(logger, dbContext)
        {
            _logger = logger;
            _conn = dbContext.GetConn();
        }
    }
}
