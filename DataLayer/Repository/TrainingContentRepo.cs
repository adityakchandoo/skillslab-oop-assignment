using DataLayer.Generic;
using DataLayer.Repository.Interfaces;
using Entities;
using Entities.AppLogger;
using Entities.DbModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repository
{
    public class TrainingContentRepo : DataAccessLayer<TrainingContent>, ITrainingContentRepo
    {
        ILogger _logger;
        private readonly SqlConnection _conn;
        public TrainingContentRepo(ILogger logger, IDbContext dbContext) : base(logger, dbContext)
        {
            _logger = logger;
            _conn = dbContext.GetConn();
        }

        public async Task<int> CreateTrainingContentReturningIDAsync(TrainingContent trainingContent)
        {
            try
            {
                string sql = @"INSERT INTO [dbo].[TrainingContent] (TrainingId, Name, Description, PostDate) 
                               OUTPUT Inserted.TrainingContentId 
                               VALUES (@TrainingId, @Name, @Description, @PostDate)";


                using (SqlCommand cmd = new SqlCommand(sql, _conn))
                {
                    cmd.Parameters.AddWithValue("@TrainingId", trainingContent.TrainingId);
                    cmd.Parameters.AddWithValue("@Name", trainingContent.Name);
                    cmd.Parameters.AddWithValue("@Description", trainingContent.Description);
                    cmd.Parameters.AddWithValue("@PostDate", trainingContent.PostDate);

                    return (int)(await cmd.ExecuteScalarAsync());
                }

            }
            catch (Exception ex) 
            {
                _logger.LogError(ex);
                throw new DbErrorException("Database Error");
                throw;
            }
        }
    }
}
