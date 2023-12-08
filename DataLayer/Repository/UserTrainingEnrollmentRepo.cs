using DataLayer;
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
    }
}
