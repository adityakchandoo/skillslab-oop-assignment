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
            // Workaround The text, ntext, and image data types cannot be compared or sorted, except when using IS NULL or LIKE operator.

            List<TrainingDetails> results = new List<TrainingDetails>();

            List<Training> tr = base.GetMany();

            string sql = @" SELECT 
                                T.TrainingId,
                                D.Name AS DepartmentName,
                                COUNT(UTE.UserId) AS NumberOfEmployeesEnrolled
                            FROM 
                                Training T
                            LEFT JOIN 
                                UserTrainingEnrollment UTE ON T.TrainingId = UTE.TrainingId
                            LEFT JOIN 
                                AppUser AU ON UTE.UserId = AU.UserId
                            LEFT JOIN 
                                Department D ON AU.DepartmentId = D.DepartmentId
                            GROUP BY 
                                T.TrainingId, D.Name
                            ORDER BY 
                                T.TrainingId, D.Name;";

            try
            {
                using (IDbCommand cmd = _conn.CreateCommand())
                {
                    cmd.CommandText = sql;

                    using (IDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Training new_tr = tr.Find(e => e.TrainingId == reader.GetInt32(0));
                            var TrainingExtra = new TrainingExtra();
                            TrainingExtra.TrainingId = reader.GetInt32(0);
                            TrainingExtra.DepartmentName = reader.IsDBNull(1) ? "No" : reader.GetString(1);
                            TrainingExtra.NumberOfEmployeesEnrolled = reader.GetInt32(2);

                            results.Add(new TrainingDetails() { Training = new_tr, TrainingExtra = TrainingExtra });
                        };                          
                        
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
                throw;
            }

            return results;
        }

        public int CreateTrainingReturningID(Training training)
        {
            try
            {
                string sql = @"INSERT INTO [dbo].[Training] (Name, Description, Threshold, Deadline, PreferedDepartmentId) 
                               OUTPUT Inserted.TrainingId 
                               VALUES (@Name, @Description, @Threshold, @Deadline, @PreferedDepartmentId)";


                using (IDbCommand cmd = _conn.CreateCommand())
                {
                    cmd.CommandText = sql;

                    DbHelper.AddParameterWithValue(cmd, "@Name", training.Name);
                    DbHelper.AddParameterWithValue(cmd, "@Description", training.Description);
                    DbHelper.AddParameterWithValue(cmd, "@Threshold", training.Threshold);
                    DbHelper.AddParameterWithValue(cmd, "@Deadline", training.Deadline);
                    DbHelper.AddParameterWithValue(cmd, "@PreferedDepartmentId", training.PreferedDepartmentId == -1 ? DBNull.Value : (object)training.PreferedDepartmentId);

                    return (int)cmd.ExecuteScalar();
                }

            } catch (Exception ex) { throw ex; }
        }

        public IEnumerable<Training> GetTrainingEnrolledByUser(string UserId)
        {
            string sql = $@"SELECT [dbo].[Training].* FROM [dbo].[UserTrainingEnrollment] INNER JOIN [dbo].[Training] ON 
                         [dbo].[Training].[TrainingId] = [dbo].[UserTrainingEnrollment].[TrainingId] 
                         WHERE [dbo].[UserTrainingEnrollment].[Status] = @EnrollStatusEnum AND [dbo].[UserTrainingEnrollment].[UserId] = @UserId;";

            Dictionary<string, object> myparam = new Dictionary<string, object>();

            myparam.Add("EnrollStatusEnum", (int)Entities.Enums.EnrollStatusEnum.Approved);
            myparam.Add("UserId", UserId);

            return base.GetMany(sql, myparam);

        }

    }
}
