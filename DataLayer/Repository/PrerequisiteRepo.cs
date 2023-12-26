using DataLayer;
using DataLayer.Generic;
using DataLayer.Repository.Interfaces;
using Entities.DbCustom;
using Entities.DbModels;
using Entities.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repository
{
    public class PrerequisiteRepo : DataAccessLayer<Prerequisite>, IPrerequisiteRepo
    {
        private readonly SqlConnection _conn;
        public PrerequisiteRepo(IDbContext dbContext) : base(dbContext)
        {
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
            catch (Exception ex) { throw ex; }
        }

    }
}
