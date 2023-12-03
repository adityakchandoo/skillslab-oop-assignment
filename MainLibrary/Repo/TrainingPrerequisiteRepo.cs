using MainLibrary.Entities;
using MainLibrary.Helpers;
using MainLibrary.Repo.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainLibrary.Repo
{
    public class TrainingPrerequisiteRepo : ITrainingPrerequisiteRepo
    {
        private readonly IDbConnection _conn;
        public TrainingPrerequisiteRepo(IDbContext dbContext)
        {
            _conn = dbContext.GetConn();
        }

        public void CreateTrainingPrerequisite(TrainingPrerequisite trainingPrerequisite)
        {
            string sql = "INSERT INTO [dbo].[TrainingPrerequisite] (TrainingId, PrerequisiteId) VALUES (@TrainingId, @PrerequisiteId);";

            using (IDbCommand cmd = _conn.CreateCommand())
            {
                cmd.CommandText = sql;

                MyExtensions.AddParameterWithValue(cmd, "@TrainingId", trainingPrerequisite.TrainingId);
                MyExtensions.AddParameterWithValue(cmd, "@PrerequisiteId", trainingPrerequisite.PrerequisiteId);

                cmd.ExecuteNonQuery();
            }
        }


        public void DeleteTrainingPrerequisites(int id)
        {
            string sql = "DELETE FROM [dbo].[TrainingPrerequisite] WHERE TrainingPrerequisiteId = @TrainingPrerequisiteId";

            using (IDbCommand cmd = _conn.CreateCommand())
            {
                cmd.CommandText = sql;

                MyExtensions.AddParameterWithValue(cmd, "@TrainingPrerequisiteId", id);

                cmd.ExecuteNonQuery();
            }
        }


        public IEnumerable<TrainingPrerequisite> GetAllTrainingPrerequisites()
        {
            string sql = "SELECT * FROM [dbo].[TrainingPrerequisite];";

            List<TrainingPrerequisite> results = new List<TrainingPrerequisite>();

            using (IDbCommand cmd = _conn.CreateCommand())
            {
                cmd.CommandText = sql;

                using (IDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        results.Add(MyExtensions.ConvertToObject<TrainingPrerequisite>(reader));
                    }
                }
            }
            return results;
        }


        public TrainingPrerequisite GetTrainingPrerequisite(int id)
        {
            string sql = "SELECT * FROM [dbo].[TrainingPrerequisite] WHERE TrainingPrerequisiteId = @TrainingPrerequisiteId;";

            using (IDbCommand cmd = _conn.CreateCommand())
            {
                cmd.CommandText = sql;
                MyExtensions.AddParameterWithValue(cmd, "@TrainingPrerequisiteId", id);

                using (IDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        return MyExtensions.ConvertToObject<TrainingPrerequisite>(reader);
                    }
                }
            }
            return null;
        }


        public void UpdateTrainingPrerequisite(TrainingPrerequisite trainingPrerequisite)
        {
            string sql = "UPDATE [dbo].[TrainingPrerequisite] SET TrainingId = @TrainingId, PrerequisiteId = @PrerequisiteId WHERE TrainingPrerequisiteId = @TrainingPrerequisiteId;";

            using (IDbCommand cmd = _conn.CreateCommand())
            {
                cmd.CommandText = sql;

                MyExtensions.AddParameterWithValue(cmd, "@TrainingId", trainingPrerequisite.TrainingId);
                MyExtensions.AddParameterWithValue(cmd, "@PrerequisiteId", trainingPrerequisite.PrerequisiteId);
                MyExtensions.AddParameterWithValue(cmd, "@TrainingPrerequisiteId", trainingPrerequisite.TrainingPrerequisiteId);

                cmd.ExecuteNonQuery();
            }
        }

    }
}
