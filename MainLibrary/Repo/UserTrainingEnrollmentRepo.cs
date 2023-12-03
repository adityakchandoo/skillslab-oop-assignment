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
    public class UserTrainingEnrollmentRepo : IUserTrainingEnrollmentRepo
    {
        private readonly IDbConnection _conn;
        public UserTrainingEnrollmentRepo(IDbContext dbContext)
        {
            _conn = dbContext.GetConn();
        }

        public void CreateUserTrainingEnrollment(UserTrainingEnrollment userTrainingEnrollment)
        {
            string sql = "INSERT INTO [dbo].[UserTrainingEnrollment] (UserId, TrainingId, ApplyDate, EnrolledDate, Status) VALUES (@UserId, @TrainingId, @ApplyDate, @EnrolledDate, @Status);";

            using (IDbCommand cmd = _conn.CreateCommand())
            {
                cmd.CommandText = sql;

                MyExtensions.AddParameterWithValue(cmd, "@UserId", userTrainingEnrollment.UserId);
                MyExtensions.AddParameterWithValue(cmd, "@TrainingId", userTrainingEnrollment.TrainingId);
                MyExtensions.AddParameterWithValue(cmd, "@ApplyDate", userTrainingEnrollment.ApplyDate);
                MyExtensions.AddParameterWithValue(cmd, "@EnrolledDate", userTrainingEnrollment.EnrolledDate == null ? DBNull.Value : (object)userTrainingEnrollment.EnrolledDate);
                MyExtensions.AddParameterWithValue(cmd, "@Status", userTrainingEnrollment.Status);

                cmd.ExecuteNonQuery();
            }
        }


        public void DeleteUserTrainingEnrollment(int userTrainingEnrollmentId)
        {
            string sql = "DELETE FROM [dbo].[UserTrainingEnrollment] WHERE UserTrainingEnrollmentId = @UserTrainingEnrollmentId";

            using (IDbCommand cmd = _conn.CreateCommand())
            {
                cmd.CommandText = sql;

                MyExtensions.AddParameterWithValue(cmd, "@UserTrainingEnrollmentId", userTrainingEnrollmentId);

                cmd.ExecuteNonQuery();
            }
        }

        public IEnumerable<UserTrainingEnrollment> GetAllUserTrainingEnrollments()
        {
            string sql = "SELECT * FROM [dbo].[UserTrainingEnrollment];";

            List<UserTrainingEnrollment> results = new List<UserTrainingEnrollment>();

            using (IDbCommand cmd = _conn.CreateCommand())
            {
                cmd.CommandText = sql;

                using (IDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        results.Add(MyExtensions.ConvertToObject<UserTrainingEnrollment>(reader));
                    }
                }
            }
            return results;
        }


        public UserTrainingEnrollment GetUserTrainingEnrollment(int userTrainingEnrollmentId)
        {
            string sql = "SELECT * FROM [dbo].[UserTrainingEnrollment] WHERE UserTrainingEnrollmentId = @UserTrainingEnrollmentId;";

            using (IDbCommand cmd = _conn.CreateCommand())
            {
                cmd.CommandText = sql;
                MyExtensions.AddParameterWithValue(cmd, "@UserTrainingEnrollmentId", userTrainingEnrollmentId);

                using (IDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        return MyExtensions.ConvertToObject<UserTrainingEnrollment>(reader);
                    }
                }
            }
            return null;
        }


        public void UpdateUserTrainingEnrollment(UserTrainingEnrollment userTrainingEnrollment)
        {
            string sql = "UPDATE [dbo].[UserTrainingEnrollment] SET UserId = @UserId, TrainingId = @TrainingId, ApplyDate = @ApplyDate, EnrolledDate = @EnrolledDate, Status = @Status WHERE UserTrainingEnrollmentId = @UserTrainingEnrollmentId;";

            using (IDbCommand cmd = _conn.CreateCommand())
            {
                cmd.CommandText = sql;

                MyExtensions.AddParameterWithValue(cmd, "@UserId", userTrainingEnrollment.UserId);
                MyExtensions.AddParameterWithValue(cmd, "@TrainingId", userTrainingEnrollment.TrainingId);
                MyExtensions.AddParameterWithValue(cmd, "@ApplyDate", userTrainingEnrollment.ApplyDate);
                MyExtensions.AddParameterWithValue(cmd, "@EnrolledDate", userTrainingEnrollment.EnrolledDate);
                MyExtensions.AddParameterWithValue(cmd, "@Status", userTrainingEnrollment.Status);
                MyExtensions.AddParameterWithValue(cmd, "@UserTrainingEnrollmentId", userTrainingEnrollment.UserTrainingEnrollmentId);

                cmd.ExecuteNonQuery();
            }
        }

    }
}
