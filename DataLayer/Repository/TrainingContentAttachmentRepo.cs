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
    public class TrainingContentAttachmentRepo : DataAccessLayer<TrainingContentAttachment>, ITrainingContentAttachmentRepo
    {
        private readonly IDbConnection _conn;
        public TrainingContentAttachmentRepo(IDbContext dbContext) : base(dbContext)
        {
            _conn = dbContext.GetConn();
        }
    }
}
