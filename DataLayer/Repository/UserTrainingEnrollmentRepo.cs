﻿using DataLayer.Generic;
using DataLayer.Repository.Interfaces;
using Entities;
using Entities.AppLogger;
using Entities.DbModels;
using Entities.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace DataLayer.Repository
{
    public class UserTrainingEnrollmentRepo : DataAccessLayer<UserTrainingEnrollment>, IUserTrainingEnrollmentRepo
    {
        ILogger _logger;
        private readonly SqlConnection _conn;
        public UserTrainingEnrollmentRepo(ILogger logger, IDbContext dbContext) : base(logger, dbContext)
        {
            _logger = logger;
            _conn = dbContext.GetConn();
        }

        public async Task CreateEnrollmentWithAttachments(UserTrainingEnrollment enrollment, DataTable attachment)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("CreateEnrollmentWithAttachment", _conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@UserId", enrollment.UserId);
                    cmd.Parameters.AddWithValue("@TrainingId", enrollment.TrainingId);
                    cmd.Parameters.AddWithValue("@ApplyDate", enrollment.ApplyDate);
                    cmd.Parameters.AddWithValue("@ManagerApprovalStatus", enrollment.ManagerApprovalStatus);
                    cmd.Parameters.AddWithValue("@EnrollStatus", enrollment.EnrollStatus);
                    var dt = cmd.Parameters.AddWithValue("@EnrollmentAttachment", attachment);
                    dt.SqlDbType = SqlDbType.Structured;

                    await cmd.ExecuteNonQueryAsync();
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw new DbErrorException("Database Error");
                throw;
            }

        }

        public async Task<UserTrainingEnrollment> GetUserTrainingEnrollmentAsync(int targetUserId, int targetTrainingId)
        {
            var userEnrollment = await base.GetMany(
                "SELECT * FROM UserTrainingEnrollment WHERE UserId = @UserId AND TrainingId = @TrainingId",
                new Dictionary<string, object>() { { "UserId", targetUserId }, { "TrainingId", targetTrainingId } }
                );

            if (userEnrollment.Count() > 0)
                return userEnrollment.First();
            else
                return null;

        }

        public async Task<IEnumerable<TrainingEnrollmentDetails>> GetUserTrainingEnrollmentInfoAsync(int userId, int trainingId)
        {
            var trainingEnrollmentDetails = new List<TrainingEnrollmentDetails>();

            string sql = @" SELECT 
                                    epa.EnrollmentPrerequisiteAttachmentId,
                                    epa.OriginalFilename,
                                    epa.FileKey,
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
                using (SqlCommand cmd = new SqlCommand(sql, _conn))
                {
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    cmd.Parameters.AddWithValue("@TrainingId", trainingId);

                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        while (reader.Read())
                        {
                            trainingEnrollmentDetails.Add(DbHelper.ConvertToObject<TrainingEnrollmentDetails>(reader));
                        }
                    }
                }
                return trainingEnrollmentDetails;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw new DbErrorException("Database Error");
                throw;
            }
        }

    }
}
