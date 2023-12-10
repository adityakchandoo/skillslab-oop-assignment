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
    public class TrainingContentRepo : DataAccessLayer<TrainingContent>, ITrainingContentRepo
    {
        private readonly IDbConnection _conn;
        public TrainingContentRepo(IDbContext dbContext) : base(dbContext)
        {
            _conn = dbContext.GetConn();
        }

        public int CreateTrainingContentReturningID(TrainingContent trainingContent)
        {
            try
            {
                string sql = @"INSERT INTO [dbo].[TrainingContent] (TrainingId, Name, Description, PostDate) 
                               OUTPUT Inserted.TrainingContentId 
                               VALUES (@TrainingId, @Name, @Description, @PostDate)";


                using (IDbCommand cmd = _conn.CreateCommand())
                {
                    cmd.CommandText = sql;

                    DbHelper.AddParameterWithValue(cmd, "@TrainingId", trainingContent.TrainingId);
                    DbHelper.AddParameterWithValue(cmd, "@Name", trainingContent.Name);
                    DbHelper.AddParameterWithValue(cmd, "@Description", trainingContent.Description);
                    DbHelper.AddParameterWithValue(cmd, "@PostDate", trainingContent.PostDate);

                    return (int)cmd.ExecuteScalar();
                }

            }
            catch (Exception ex) { throw ex; }
        }
    }
}
