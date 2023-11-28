using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using MainLibrary.Repo.Interfaces;
using MainLibrary.Entities;
using MainLibrary.Entities.Types;
using System.Data.SqlClient;
using System.Reflection;

namespace MainLibrary.Repo
{
    public class TrainingRepo : ITrainingRepo
    {
        DbContext _dbContext;
        public TrainingRepo(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void CreateTraining(Training training)
        {

            string insertQuery = "INSERT INTO [dbo].[Training] (Name,Description,Treshhold,Deadline,ManagerId,PreferedDepartmentId) VALUES " +
                "(Name,Description,Treshhold,Deadline,ManagerId,PreferedDepartmentId)";

            SqlCommand cmd = new SqlCommand(insertQuery, _dbContext.GetConn());

            cmd.Parameters.AddWithValue("@Name", training.Name);
            cmd.Parameters.AddWithValue("@Description", training.Description);
            cmd.Parameters.AddWithValue("@Treshhold", training.Treshhold);
            cmd.Parameters.AddWithValue("@Deadline", training.Deadline);
            cmd.Parameters.AddWithValue("@ManagerId", training.ManagerId);
            cmd.Parameters.AddWithValue("@PreferedDepartmentId", training.PreferedDepartmentId);


            cmd.ExecuteNonQuery();

            throw new NotImplementedException();
        }

        public void DeleteTraining(int id)
        {
            string delQuery = "DELETE FROM [dbo].[Training] WHERE TrainingId = @id";

            SqlCommand cmd = new SqlCommand(delQuery, _dbContext.GetConn());
            cmd.Parameters.AddWithValue("@id", id);

            cmd.ExecuteNonQuery();
        }

        public IEnumerable<TrainingDetails> GetAllTraining()
        {
            List<TrainingDetails> results = new List<TrainingDetails>();

            using (SqlCommand command = new SqlCommand(
                "SELECT [Training].*, AppUser.FirstName, AppUser.LastName, Department.Name AS Dname FROM [dbo].[Training] " +
                "INNER JOIN AppUser ON Training.ManagerId = AppUser.UserId " +
                "LEFT JOIN Department ON Training.PreferedDepartmentId = Department.DepartmentId;", _dbContext.GetConn()))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        // Map the data from the SqlDataReader to your model
                        TrainingDetails model = new TrainingDetails
                        {
                            TrainingId = (int)reader["TrainingId"],
                            Name = (string)reader["Name"],
                            Description = (string)reader["Description"],
                            Treshhold = (int)reader["Treshhold"],
                            Deadline = DateTime.Parse(reader["Deadline"].ToString()),
                            ManagerId = (string)reader["ManagerId"],
                            //PreferedDepartmentId = (int)reader["PreferedDepartmentId"],
                            //DepartmentName = (string)reader["Dname"] ?? "",
                            ManagerName = (string)reader["FirstName"] + (string)reader["LastName"],
                        };

                        results.Add(model);
                    }
                }
            }
            return results;
        }
        public Training GetTraining(int id)
        {
            Training user = new Training();

            using (SqlCommand command = new SqlCommand("SELECT * FROM [dbo].[AppUser] WHERE TrainingId = @TrainingId;", _dbContext.GetConn()))
            {
                command.Parameters.AddWithValue("@TrainingId", id);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        user.TrainingId = (int)reader["TrainingId"];
                        user.Name = (string)reader["Name"];
                        user.Description = (string)reader["Description"];
                        user.Treshhold = (int)reader["PassTreshholdword"];
                        user.Deadline = DateTime.Parse(reader["Deadline"].ToString());
                        user.ManagerId = (string)reader["ManagerId"];
                        user.PreferedDepartmentId = (int)reader["PreferedDepartmentId"];

                        return user;
                    }
                }
            }


            return null;

            throw new NotImplementedException();
        }

        public IEnumerable<Training> GetTrainingManagedByUser(string UserId)
        {
            List<Training> results = new List<Training>();

            using (SqlCommand command = new SqlCommand("SELECT * FROM [dbo].[Training] WHERE ManagerId = @ManagerId;", _dbContext.GetConn()))
            {
                command.Parameters.AddWithValue("@UserId", UserId);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        // Map the data from the SqlDataReader to your model
                        Training model = new Training
                        {
                            TrainingId = (int)reader["TrainingId"],
                            Name = (string)reader["FirstName"],
                            Description = (string)reader["Description"],
                            Treshhold = (int)reader["Treshhold"],
                            Deadline = DateTime.Parse(reader["Deadline"].ToString()),
                            PreferedDepartmentId = (int)reader["PreferedDepartmentId"],
                        };

                        results.Add(model);
                    }
                }
            }
            return results;
        }

        public IEnumerable<Training> GetTrainingEnrolledByUser(string UserId)
        {
            List<Training> results = new List<Training>();

            using (SqlCommand command = new SqlCommand("SELECT [dbo].[Training].* FROM [dbo].[Training] INNER JOIN [dbo].[UserTrainingEnrollment] ON " +
                                                       "[dbo].[Training].[TrainingId] = [dbo].[UserTrainingEnrollment].[TrainingId] WHERE " +
                                                       "[dbo].[UserTrainingEnrollment].[UserId] = @UserId;", _dbContext.GetConn()))
            {
                command.Parameters.AddWithValue("@UserId", UserId);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        // Map the data from the SqlDataReader to your model
                        Training model = new Training
                        {
                            TrainingId = (int)reader["TrainingId"],
                            Name = (string)reader["FirstName"],
                            Description = (string)reader["Description"],
                            Treshhold = (int)reader["Treshhold"],
                            Deadline = DateTime.Parse(reader["Deadline"].ToString()),
                            PreferedDepartmentId = (int)reader["PreferedDepartmentId"],
                        };

                        results.Add(model);
                    }
                }
            }
            return results;
        }

        public void UpdateTraining(Training training)
        {
            string updateQuery = "UPDATE [dbo].[Training] SET Name = @Name, Description = @Description, Treshhold = @Treshhold, Deadline = @Deadline, ManagerId = @ManagerId, " +
                "PreferedDepartmentId = @PreferedDepartmentId WHERE YourPrimaryKeyColumn = @YourPrimaryKeyValue;";

            SqlCommand cmd = new SqlCommand(updateQuery, _dbContext.GetConn());

            cmd.Parameters.AddWithValue("@Name", training.Name);
            cmd.Parameters.AddWithValue("@Description", training.Description);
            cmd.Parameters.AddWithValue("@Treshhold", training.Treshhold);
            cmd.Parameters.AddWithValue("@Deadline", training.Deadline);
            cmd.Parameters.AddWithValue("@ManagerId", training.ManagerId);
            cmd.Parameters.AddWithValue("@PreferedDepartmentId", training.PreferedDepartmentId);

            cmd.ExecuteNonQuery();
            throw new NotImplementedException();
        }
    }
}
