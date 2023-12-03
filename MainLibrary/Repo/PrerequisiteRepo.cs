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
    public class PrerequisiteRepo : IPrerequisiteRepo
    {
        private readonly IDbConnection _conn;
        public PrerequisiteRepo(IDbContext dbContext)
        {
            _conn = dbContext.GetConn();
        }
        public void CreatePrerequisite(Prerequisite prerequisite)
        {
            string sql = "INSERT INTO [dbo].[Prerequisite] (Name, Description) VALUES (@Name, @Description);";

            using (IDbCommand cmd = _conn.CreateCommand())
            {
                cmd.CommandText = sql;

                MyExtensions.AddParameterWithValue(cmd, "@Name", prerequisite.Name);
                MyExtensions.AddParameterWithValue(cmd, "@Description", prerequisite.Description);

                cmd.ExecuteNonQuery();
            }
        }

        public void DeletePrerequisite(int prerequisiteId)
        {
            string sql = "DELETE FROM [dbo].[Prerequisite] WHERE PrerequisiteId = @PrerequisiteId";

            using (IDbCommand cmd = _conn.CreateCommand())
            {
                cmd.CommandText = sql;

                MyExtensions.AddParameterWithValue(cmd, "@PrerequisiteId", prerequisiteId);

                cmd.ExecuteNonQuery();
            }
        }


        public IEnumerable<Prerequisite> GetAllPrerequisites()
        {
            string sql = "SELECT * FROM [dbo].[Prerequisite];";

            List<Prerequisite> results = new List<Prerequisite>();

            using (IDbCommand cmd = _conn.CreateCommand())
            {
                cmd.CommandText = sql;

                using (IDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        results.Add(MyExtensions.ConvertToObject<Prerequisite>(reader));
                    }
                }
            }
            return results;
        }


        public Prerequisite GetPrerequisite(int prerequisiteId)
        {
            string sql = "SELECT * FROM [dbo].[Prerequisite] WHERE PrerequisiteId = @PrerequisiteId;";

            using (IDbCommand cmd = _conn.CreateCommand())
            {
                cmd.CommandText = sql;
                MyExtensions.AddParameterWithValue(cmd, "@PrerequisiteId", prerequisiteId);

                using (IDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        return MyExtensions.ConvertToObject<Prerequisite>(reader);
                    }
                }
            }
            return null;
        }


        public void UpdatePrerequisite(Prerequisite prerequisite)
        {
            string sql = "UPDATE [dbo].[Prerequisite] SET Name = @Name, Description = @Description WHERE PrerequisiteId = @PrerequisiteId;";

            using (IDbCommand cmd = _conn.CreateCommand())
            {
                cmd.CommandText = sql;

                MyExtensions.AddParameterWithValue(cmd, "@Name", prerequisite.Name);
                MyExtensions.AddParameterWithValue(cmd, "@Description", prerequisite.Description);
                MyExtensions.AddParameterWithValue(cmd, "@PrerequisiteId", prerequisite.PrerequisiteId);

                cmd.ExecuteNonQuery();
            }
        }

    }
}
