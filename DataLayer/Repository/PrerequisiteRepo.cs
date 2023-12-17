using DataLayer;
using DataLayer.Generic;
using DataLayer.Repository.Interfaces;
using Entities.DbCustom;
using Entities.DbModels;
using Entities.DTO;
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
        //public IEnumerable<PrerequisiteDetails> GetPrerequisitesByTraining2(int training)
        //{
        //    var sql = @"SELECT Prerequisite.*, TrainingPrerequisite.TrainingPrerequisiteId FROM Prerequisite INNER JOIN TrainingPrerequisite ON Prerequisite.PrerequisiteId = TrainingPrerequisite.PrerequisiteId WHERE TrainingPrerequisite.TrainingId = @TrainingId";
            
        //    var param = new Dictionary<string, object>();
        //    param.Add("TrainingId", training);

        //    return base.GetMany(sql, param);
        //}

        public IEnumerable<PrerequisiteDetails> GetPrerequisitesByTraining(int training)
        {
            var prerequisiteDetails = new List<PrerequisiteDetails>();

            string sql = @"SELECT Prerequisite.*, TrainingPrerequisite.TrainingPrerequisiteId FROM Prerequisite INNER JOIN TrainingPrerequisite ON Prerequisite.PrerequisiteId = TrainingPrerequisite.PrerequisiteId WHERE TrainingPrerequisite.TrainingId = @TrainingId";

            try
            {
                using (IDbCommand cmd = _conn.CreateCommand())
                {
                    cmd.CommandText = sql;

                    DbHelper.AddParameterWithValue(cmd, "@TrainingId", training);

                    using (IDataReader reader = cmd.ExecuteReader())
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
