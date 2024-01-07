using DataLayer.Generic;
using DataLayer.Repository.Interfaces;
using Entities.AppLogger;
using Entities.DbModels;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace DataLayer.Repository
{
    public class TrainingContentAttachmentRepo : DataAccessLayer<TrainingContentAttachment>, ITrainingContentAttachmentRepo
    {
        ILogger _logger;
        private readonly SqlConnection _conn;
        public TrainingContentAttachmentRepo(ILogger logger, IDbContext dbContext) : base(logger, dbContext)
        {
            _logger = logger;
            _conn = dbContext.GetConn();
        }

        public async Task<IEnumerable<TrainingContentAttachment>> GetAllTrainingContentAttachmentAsync(int trainingContentId)
        {
            return await base.GetMany("SELECT * FROM TrainingContentAttachment WHERE TrainingContentId = @TrainingContentId;",
                                         new Dictionary<string, object> { { "TrainingContentId", trainingContentId } });
        }
    }
}
