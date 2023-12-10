using DataLayer;
using DataLayer.Generic;
using DataLayer.Repository.Interfaces;
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
    public class UserTrainingEnrollmentRepo : DataAccessLayer<UserTrainingEnrollment>, IUserTrainingEnrollmentRepo
    {
        private readonly IDbConnection _conn;
        public UserTrainingEnrollmentRepo(IDbContext dbContext) : base(dbContext)
        {
            _conn = dbContext.GetConn();
        }

        public int CreateUserTrainingEnrollmentReturningID(UserTrainingEnrollment userTrainingEnrollment)
        {
            try
            {
                string sql = @"INSERT INTO [dbo].[UserTrainingEnrollment] 
                               (UserId, TrainingId, ApplyDate, Status) 
                               OUTPUT Inserted.UserTrainingEnrollmentId 
                               VALUES (@UserId, @TrainingId, @ApplyDate, @Status)";


                using (IDbCommand cmd = _conn.CreateCommand())
                {
                    cmd.CommandText = sql;

                    DbHelper.AddParameterWithValue(cmd, "@UserId", userTrainingEnrollment.UserId);
                    DbHelper.AddParameterWithValue(cmd, "@TrainingId", userTrainingEnrollment.TrainingId);
                    DbHelper.AddParameterWithValue(cmd, "@ApplyDate", userTrainingEnrollment.ApplyDate);
                    DbHelper.AddParameterWithValue(cmd, "@Status", userTrainingEnrollment.Status);

                    return (int)cmd.ExecuteScalar();
                }

            } catch (Exception ex)
            {
                throw;
            }

        }

        public UserTrainingEnrollment GetUserTrainingEnrollmentByUserTraining(string targetUserId, int targetTrainingId)
        {
            var userEnrollment = base.GetMany(
                "SELECT * FROM UserTrainingEnrollment WHERE UserId = @UserId AND TrainingId = @TrainingId",
                new Dictionary<string, object>() { { "UserId", targetUserId }, { "TrainingId", targetTrainingId } }
                );

            if (userEnrollment.Count == 1)
                return userEnrollment[0];
            else if (userEnrollment.Count == 0)
                return null;
            else
                throw new Exception("UserTrainingEnrollment Record Error");
            
        }

        public IEnumerable<TrainingEnrollmentDetails> GetUserTrainingEnrollmentInfo(string userId, int trainingId)
        {
            var trainingEnrollmentDetails = new List<TrainingEnrollmentDetails>();

            string sql = @" SELECT 
                                    epa.EnrollmentPrerequisiteAttachmentId,
                                    epa.OriginalFilename,
                                    epa.SystemFilename,
                                    tpr.TrainingId,
                                    tpr.PrerequisiteId,
                                    p.Name as PrerequisiteName
                                FROM 
                                    UserTrainingEnrollment ute
                                INNER JOIN 
                                    EnrollmentPrerequisiteAttachment epa ON ute.UserTrainingEnrollmentId = epa.EnrollmentId
                                INNER JOIN 
                                    TrainingPrerequisite tpr ON epa.TrainingPrerequisiteId = tpr.TrainingPrerequisiteId
                                INNER JOIN 
                                    Prerequisite p ON tpr.PrerequisiteId = p.PrerequisiteId
                                WHERE 
                                    ute.UserId = @UserId
                                    AND ute.TrainingId = @TrainingId";

            try
            {
                using (IDbCommand cmd = _conn.CreateCommand())
                {
                    cmd.CommandText = sql;

                    DbHelper.AddParameterWithValue(cmd, "@UserId", userId);
                    DbHelper.AddParameterWithValue(cmd, "@TrainingId", trainingId);


                    using (IDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            trainingEnrollmentDetails.Add(DbHelper.ConvertToObject<TrainingEnrollmentDetails>(reader));
                        }
                    }
                }
                return trainingEnrollmentDetails;

            }
            catch (Exception ex) { throw ex; }
        }

    }
}
