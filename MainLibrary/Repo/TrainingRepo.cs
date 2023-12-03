using System;
using System.Collections.Generic;
using MainLibrary.Repo.Interfaces;
using MainLibrary.Entities;
using System.Data;
using MainLibrary.Helpers;

namespace MainLibrary.Repo
{
    public class TrainingRepo : ITrainingRepo
    {
        private readonly IDbConnection _conn;
        public TrainingRepo(IDbContext dbContext)
        {
            _conn = dbContext.GetConn();
        }

        public int CreateTraining(Training training)
        {
            string sql = @"INSERT INTO [dbo].[Training] (Name, Description, Threshold, Deadline, ManagerId, PreferedDepartmentId) 
                           OUTPUT Inserted.TrainingId 
                           VALUES (@Name, @Description, @Threshold, @Deadline, @ManagerId, @PreferedDepartmentId)";


            using (IDbCommand cmd = _conn.CreateCommand())
            {
                cmd.CommandText = sql;
                

                MyExtensions.AddParameterWithValue(cmd, "@Name", training.Name);
                MyExtensions.AddParameterWithValue(cmd, "@Description", training.Description);
                MyExtensions.AddParameterWithValue(cmd, "@Threshold", training.Threshold);
                MyExtensions.AddParameterWithValue(cmd, "@Deadline", training.Deadline);
                MyExtensions.AddParameterWithValue(cmd, "@ManagerId", training.ManagerId);
                MyExtensions.AddParameterWithValue(cmd, "@PreferedDepartmentId", training.PreferedDepartmentId == -1 ? DBNull.Value : (object)training.PreferedDepartmentId);

                return (int)cmd.ExecuteScalar();
            }
        }

        public void DeleteTraining(int TrainingId)
        {
            string sql = "DELETE FROM [dbo].[Training] WHERE TrainingId = @TrainingId";

            using (IDbCommand cmd = _conn.CreateCommand())
            {
                cmd.CommandText = sql;
                MyExtensions.AddParameterWithValue(cmd, "@TrainingId", TrainingId);

                cmd.ExecuteNonQuery();
            }            
        }
        public Training GetTraining(int TrainingId)
        {
            string sql = "SELECT * FROM [dbo].[Training] WHERE TrainingId = @TrainingId;";

            using (IDbCommand cmd = _conn.CreateCommand())
            {
                cmd.CommandText = sql;
                MyExtensions.AddParameterWithValue(cmd, "@TrainingId", TrainingId);
                using (IDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        return MyExtensions.ConvertToObject<Training>(reader);
                    }
                }
            }
            throw new Exception();
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
                        results.Add(MyExtensions.ConvertToObject<TrainingDetails>(reader));
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
                MyExtensions.AddParameterWithValue(cmd, "@UserId", UserId);
                using (IDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        results.Add(MyExtensions.ConvertToObject<TrainingDetails>(reader));
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
                MyExtensions.AddParameterWithValue(cmd, "@ManagerId", UserId);
                using (IDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        results.Add(MyExtensions.ConvertToObject<TrainingDetails>(reader));
                    }
                }
            }

            return results;
        }

        public void UpdateTraining(Training training)
        {
            string sql = "UPDATE [dbo].[Training] SET Name = @Name, Description = @Description, TrainingId = @TrainingId, Deadline = @Deadline, ManagerId = @ManagerId, " +
                         "PreferedDepartmentId = @PreferedDepartmentId WHERE TrainingId = @TrainingId;";

            using (IDbCommand cmd = _conn.CreateCommand())
            {
                cmd.CommandText = sql;

                MyExtensions.AddParameterWithValue(cmd, "@TrainingId", training.TrainingId);
                MyExtensions.AddParameterWithValue(cmd, "@Name", training.Name);
                MyExtensions.AddParameterWithValue(cmd, "@Description", training.Description);
                MyExtensions.AddParameterWithValue(cmd, "@TrainingId", training.TrainingId);
                MyExtensions.AddParameterWithValue(cmd, "@Deadline", training.Deadline);
                MyExtensions.AddParameterWithValue(cmd, "@ManagerId", training.ManagerId);
                MyExtensions.AddParameterWithValue(cmd, "@PreferedDepartmentId", training.PreferedDepartmentId);

                cmd.ExecuteNonQuery();
            }
        }
    }
}
