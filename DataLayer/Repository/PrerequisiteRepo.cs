using DataLayer.Generic;
using DataLayer.Repository.Interfaces;
using Entities;
using Entities.AppLogger;
using Entities.DbCustom;
using Entities.DbModels;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace DataLayer.Repository
{
    public class PrerequisiteRepo : DataAccessLayer<Prerequisite>, IPrerequisiteRepo
    {
        ILogger _logger;
        private readonly SqlConnection _conn;
        public PrerequisiteRepo(ILogger logger, IDbContext dbContext) : base(logger, dbContext)
        {
            _logger = logger;
            _conn = dbContext.GetConn();
        }

        public async Task<IEnumerable<PrerequisiteDetails>> GetPrerequisitesByTrainingAsync(int training)
        {
            var prerequisiteDetails = new List<PrerequisiteDetails>();

            string sql = @"SELECT Prerequisite.*, TrainingPrerequisite.TrainingPrerequisiteId FROM Prerequisite INNER JOIN TrainingPrerequisite ON Prerequisite.PrerequisiteId = TrainingPrerequisite.PrerequisiteId WHERE TrainingPrerequisite.TrainingId = @TrainingId";

            try
            {
                using (SqlCommand cmd = new SqlCommand(sql, _conn))
                {
                    cmd.Parameters.AddWithValue("@TrainingId", training);

                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        while (reader.Read())
                        {
                            prerequisiteDetails.Add(DbHelper.ConvertToObject<PrerequisiteDetails>(reader));
                        }
                    }
                }
                return prerequisiteDetails;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw new DbErrorException("Database Error");
                throw;
            }
        }

        public async Task<IEnumerable<PrerequisiteAvailable>> GetAllPrerequisitesByTrainingAsync(int training)
        {
            var result = new List<PrerequisiteAvailable>();

            string sql = @" SELECT 
                                P.*,
                                CASE 
                                    WHEN TP.TrainingId IS NOT NULL THEN 1
                                    ELSE 0
                                END AS IsAvailable
                            FROM 
                                Prerequisite P
                            LEFT JOIN 
                                TrainingPrerequisite TP ON P.PrerequisiteId = TP.PrerequisiteId AND TP.TrainingId = @TrainingId;
                            ";

            try
            {
                using (SqlCommand cmd = new SqlCommand(sql, _conn))
                {
                    cmd.Parameters.AddWithValue("@TrainingId", training);

                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        while (reader.Read())
                        {
                            result.Add(DbHelper.ConvertToObject<PrerequisiteAvailable>(reader));
                        }
                    }
                }
                return result;

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
