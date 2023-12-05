using System;
using System.Collections.Generic;
using System.Data;
using DataLayer;
using DataLayer.Generic;
using DataLayer.Repository.Interfaces;
using Entities.DbCustom;
using Entities.DbModels;

namespace DataLayer.Repository
{
    public class TrainingRepo : DataAccessLayer<Training>, ITrainingRepo
    {
        private readonly IDbConnection _conn;
        public TrainingRepo(IDbContext dbContext) : base(dbContext)
        {
            _conn = dbContext.GetConn();
        }

        public IEnumerable<TrainingDetails> GetAllTraining()
        {
            string sql = @"SELECT [Training].*, CONCAT(AppUser.FirstName,' ', AppUser.LastName) AS ManagerName, Department.Name AS DepartmentName FROM [dbo].[Training] 
                         INNER JOIN AppUser ON Training.ManagerId = AppUser.UserId 
                         LEFT JOIN Department ON Training.PreferedDepartmentId = Department.DepartmentId;";


            List<TrainingDetails> results = new List<TrainingDetails>();

            using (IDbCommand cmd = _conn.CreateCommand())
            {
                cmd.CommandText = sql;

                using (IDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        results.Add(DbHelper.ConvertToObject<TrainingDetails>(reader));
                    }
                }
            }
            return results;
        }

        public IEnumerable<TrainingDetails> GetTrainingEnrolledByUser(string UserId)
        {
            string sql = @"SELECT [dbo].[Training].* FROM [dbo].[Training] INNER JOIN [dbo].[UserTrainingEnrollment] ON 
                         [dbo].[Training].[TrainingId] = [dbo].[UserTrainingEnrollment].[TrainingId] 
                         WHERE [dbo].[UserTrainingEnrollment].[UserId] = @UserId;";


            List<TrainingDetails> results = new List<TrainingDetails>();

            using (IDbCommand cmd = _conn.CreateCommand())
            {
                cmd.CommandText = sql;
                DbHelper.AddParameterWithValue(cmd, "@UserId", UserId);
                using (IDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        results.Add(DbHelper.ConvertToObject<TrainingDetails>(reader));
                    }
                }
            }

            return results;
        }

        public IEnumerable<TrainingDetails> GetTrainingManagedByUser(string UserId)
        {
            string sql = "SELECT * FROM [dbo].[Training] WHERE ManagerId = @ManagerId;";

            List<TrainingDetails> results = new List<TrainingDetails>();

            using (IDbCommand cmd = _conn.CreateCommand())
            {
                cmd.CommandText = sql;
                DbHelper.AddParameterWithValue(cmd, "@ManagerId", UserId);
                using (IDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        results.Add(DbHelper.ConvertToObject<TrainingDetails>(reader));
                    }
                }
            }

            return results;
        }


    }
}
