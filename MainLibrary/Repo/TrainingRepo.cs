using System;
using System.Collections.Generic;
using MainLibrary.Repo.Interfaces;
using MainLibrary.Entities;
using System.Data;
using MainLibrary.Helper;

namespace MainLibrary.Repo
{
    public class TrainingRepo : ITrainingRepo
    {
        IDbConnection _conn;
        public TrainingRepo(IDbContext dbContext)
        {
            _conn = dbContext.GetConn();
        }

        public void CreateTraining(Training training)
        {
            string sql = "INSERT INTO [dbo].[Training] (Name,Description,Treshhold,Deadline,ManagerId,PreferedDepartmentId) VALUES " +
                "(Name,Description,Treshhold,Deadline,ManagerId,PreferedDepartmentId)";

            using (IDbCommand cmd = _conn.CreateCommand())
            {
                cmd.CommandText = sql;

                MyExtensions.AddParameterWithValue(cmd, "@Name", training.Name);
                MyExtensions.AddParameterWithValue(cmd, "@Description", training.Description);
                MyExtensions.AddParameterWithValue(cmd, "@Treshhold", training.Treshhold);
                MyExtensions.AddParameterWithValue(cmd, "@Deadline", training.Deadline);
                MyExtensions.AddParameterWithValue(cmd, "@ManagerId", training.ManagerId);
                MyExtensions.AddParameterWithValue(cmd, "@PreferedDepartmentId", training.PreferedDepartmentId);

                cmd.ExecuteNonQuery();
            }
        }

        public void DeleteTraining(int id)
        {
            string sql = "DELETE FROM [dbo].[Training] WHERE TrainingId = @id";

            using (IDbCommand cmd = _conn.CreateCommand())
            {
                cmd.CommandText = sql;

                MyExtensions.AddParameterWithValue(cmd, "@id", id);

                cmd.ExecuteNonQuery();
            }            
        }
        public Training GetTraining(int id)
        {
            string sql = "SELECT * FROM [dbo].[AppUser] WHERE TrainingId = @TrainingId;";

            using (IDbCommand cmd = _conn.CreateCommand())
            {
                cmd.CommandText = sql;

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
            string sql = "SELECT [Training].*, AppUser.FirstName, AppUser.LastName, Department.Name AS Dname FROM [dbo].[Training] " +
                         "INNER JOIN AppUser ON Training.ManagerId = AppUser.UserId " +
                         "LEFT JOIN Department ON Training.PreferedDepartmentId = Department.DepartmentId;";

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

        public IEnumerable<Training> GetTrainingEnrolledByUser(string UserId)
        {
            string sql = "SELECT [dbo].[Training].* FROM [dbo].[Training] INNER JOIN [dbo].[UserTrainingEnrollment] ON " +
                         "[dbo].[Training].[TrainingId] = [dbo].[UserTrainingEnrollment].[TrainingId] " +
                         "WHERE [dbo].[UserTrainingEnrollment].[UserId] = @UserId;";


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

        public IEnumerable<Training> GetTrainingManagedByUser(string UserId)
        {
            string sql = "SELECT * FROM [dbo].[Training] WHERE ManagerId = @ManagerId;";

            List<Training> results = new List<Training>();

            using (IDbCommand cmd = _conn.CreateCommand())
            {
                cmd.CommandText = sql;

                using (IDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        results.Add(MyExtensions.ConvertToObject<Training>(reader));
                    }
                }
            }

            return results;
        }

        public void UpdateTraining(Training training)
        {
            string sql = "UPDATE [dbo].[Training] SET Name = @Name, Description = @Description, Treshhold = @Treshhold, Deadline = @Deadline, ManagerId = @ManagerId, " +
                         "PreferedDepartmentId = @PreferedDepartmentId WHERE TrainingId = @TrainingId;";

            using (IDbCommand cmd = _conn.CreateCommand())
            {
                cmd.CommandText = sql;

                MyExtensions.AddParameterWithValue(cmd, "@TrainingId", training.TrainingId);
                MyExtensions.AddParameterWithValue(cmd, "@Name", training.Name);
                MyExtensions.AddParameterWithValue(cmd, "@Description", training.Description);
                MyExtensions.AddParameterWithValue(cmd, "@Treshhold", training.Treshhold);
                MyExtensions.AddParameterWithValue(cmd, "@Deadline", training.Deadline);
                MyExtensions.AddParameterWithValue(cmd, "@ManagerId", training.ManagerId);
                MyExtensions.AddParameterWithValue(cmd, "@PreferedDepartmentId", training.PreferedDepartmentId);

                cmd.ExecuteNonQuery();
            }
        }
    }
}
