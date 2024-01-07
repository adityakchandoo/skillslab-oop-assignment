using DataLayer.Generic;
using Entities.AppLogger;
using Entities.DbModels;
using System.Data.SqlClient;

namespace DataLayer.Repository.Interfaces
{
    public class FeedbackRepo : DataAccessLayer<Feedback>, IFeedbackRepo
    {
        ILogger _logger;
        private readonly SqlConnection _conn;
        public FeedbackRepo(ILogger logger, IDbContext dbContext) : base(logger, dbContext)
        {
            _logger = logger;
            _conn = dbContext.GetConn();
        }
    }
}
