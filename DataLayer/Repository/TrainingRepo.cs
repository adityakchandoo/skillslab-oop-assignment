using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using DataLayer;
using DataLayer.Generic;
using DataLayer.Repository.Interfaces;
using Entities;
using Entities.AppLogger;
using Entities.DbCustom;
using Entities.DbModels;
using Entities.DTO;
using Entities.Enums;

namespace DataLayer.Repository
{
    public class TrainingRepo : DataAccessLayer<Training>, ITrainingRepo
    {
        private ILogger _logger;
        private readonly SqlConnection _conn;
        public TrainingRepo(ILogger logger, IDbContext dbContext) : base(logger, dbContext)
        {
            _logger = logger;
            _conn = dbContext.GetConn();
        }

        public async Task<int> CreateTrainingReturningIDAsync(Training training)
        {
            try
            {
                string sql = @"INSERT INTO [dbo].[Training] (Name, Description, MaxSeat, Deadline, PreferedDepartmentId) 
                               OUTPUT Inserted.TrainingId 
                               VALUES (@Name, @Description, @MaxSeat, @Deadline, @PreferedDepartmentId)";


                using (SqlCommand cmd = new SqlCommand(sql, _conn))
                {
                    cmd.Parameters.AddWithValue("@Name", training.Name);
                    cmd.Parameters.AddWithValue("@Description", training.Description);
                    cmd.Parameters.AddWithValue("@MaxSeat", training.MaxSeat);
                    cmd.Parameters.AddWithValue("@Deadline", training.Deadline);
                    cmd.Parameters.AddWithValue("@PreferedDepartmentId", training.PreferedDepartmentId == -1 ? DBNull.Value : (object)training.PreferedDepartmentId);

                    return (int)(await cmd.ExecuteScalarAsync());
                }

            } catch (Exception ex) 
            {
                _logger.LogError(ex);
                throw new DbErrorException("Database Error");
                throw;
            }
        }
        public async Task<IEnumerable<TrainingEnrollCount>> GetAllTrainingWithEnrollCountAsync()
        {
            List<TrainingEnrollCount> results = new List<TrainingEnrollCount>();

            // AllTrainingWithDepartmentName is a view

            string sql = @"SELECT Training.*, A.* FROM Training 
                           LEFT JOIN AllTrainingWithDepartmentName A ON Training.TrainingId = A.TrainingId
                           WHERE Training.IsActive = 1
                           ORDER BY Training.CreatedOn";

            try
            {
                using (SqlCommand cmd = new SqlCommand(sql, _conn))
                {
                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        while (reader.Read())
                        {
                            results.Add(DbHelper.ConvertToObject<TrainingEnrollCount>(reader));
                        };                  
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw new DbErrorException("Database Error");
                throw;
            }

            return results;
        }
        public async Task<IEnumerable<TrainingWithUserStatus>> GetAllTrainingAsync(int UserId)
        {
            List<TrainingWithUserStatus> results = new List<TrainingWithUserStatus>();

            string sql = @"SELECT 
                            T.*,
                            CASE 
                                WHEN UTE.UserId IS NULL THEN 0
                                ELSE UTE.ManagerApprovalStatus
                            END AS ManagerApprovalStatus,
                            CASE 
                                WHEN UTE.UserId IS NULL THEN 0
                                ELSE UTE.EnrollStatus
                            END AS EnrollStatus
                            FROM 
                              Training T
                            LEFT JOIN 
                              UserTrainingEnrollment UTE ON T.TrainingId = UTE.TrainingId AND UTE.UserId = @UserId
                            WHERE T.IsActive = 1";

            try
            {
                using (SqlCommand cmd = new SqlCommand(sql, _conn))
                {
                    cmd.Parameters.AddWithValue("@UserId", UserId);

                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        while (reader.Read())
                        {
                            results.Add(DbHelper.ConvertToObject<TrainingWithUserStatus>(reader));
                        };

                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw new DbErrorException("Database Error");
                throw;
            }

            return results;
        }
        public async Task<IEnumerable<TrainingWithUserStatus>> GetTrainingEnrolledByUserAsync(int UserId, EnrollStatusEnum status)
        {
            List<TrainingWithUserStatus> results = new List<TrainingWithUserStatus>();

            string sql = $@"SELECT 
                            T.*,
                            UTE.ManagerApprovalStatus AS ManagerApprovalStatus,
                            UTE.EnrollStatus AS EnrollStatus
                            FROM 
                              Training T
                            INNER JOIN 
                              UserTrainingEnrollment UTE ON T.TrainingId = UTE.TrainingId
                            WHERE UTE.UserId = @UserId AND UTE.ManagerApprovalStatus = @ManagerApprovalStatus AND UTE.EnrollStatus = @EnrollStatus
                            AND T.IsActive = 1;";
            try
            {
                using (SqlCommand cmd = new SqlCommand(sql, _conn))
                {
                    cmd.Parameters.AddWithValue("@UserId", UserId);
                    cmd.Parameters.AddWithValue("@ManagerApprovalStatus", (int)status);
                    cmd.Parameters.AddWithValue("@EnrollStatus", (int)status);

                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        while (reader.Read())
                        {
                            results.Add(DbHelper.ConvertToObject<TrainingWithUserStatus>(reader));
                        };

                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw new DbErrorException("Database Error");
                throw;
            }

            return results;

        }
        public async Task<IEnumerable<TrainingWithUserStatus>> GetTrainingEnrolledByUserAsync(int UserId)
        {
            List<TrainingWithUserStatus> results = new List<TrainingWithUserStatus>();

            string sql = $@"SELECT 
                            T.*,
                            UTE.ManagerApprovalStatus AS ManagerApprovalStatus,
                            UTE.EnrollStatus AS EnrollStatus
                            FROM 
                              Training T
                            INNER JOIN 
                              UserTrainingEnrollment UTE ON T.TrainingId = UTE.TrainingId
                            WHERE UTE.UserId = @UserId
                            AND T.IsActive = 1;";
            try
            {
                using (SqlCommand cmd = new SqlCommand(sql, _conn))
                {
                    cmd.Parameters.AddWithValue("@UserId", UserId);

                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        while (reader.Read())
                        {
                            results.Add(DbHelper.ConvertToObject<TrainingWithUserStatus>(reader));
                        };

                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw new DbErrorException("Database Error");
                throw;
            }

            return results;

        }

        public async Task<IEnumerable<UserTraining>> GetUserTrainingByStatusAndManagerIdAsync(EnrollStatusEnum enrollStatusEnum, int UserId)
        {
            var pendingUserTraining = new List<UserTraining>();

            string sql = $@"SELECT
                                UTE.UserId as 'UserId',
                                AU.UserName as 'UserName',
	                            UTE.TrainingId as 'TrainingId',
                                AU.FirstName + ' ' + AU.LastName as 'Name',
                                T.Name as 'TrainingName',
                                AU.Email,
                                AU.MobileNumber,
                                T.Deadline as 'TrainingDeadline'
                            FROM Training T
                            JOIN UserTrainingEnrollment UTE ON T.TrainingId = UTE.TrainingId
                            JOIN AppUser AU ON UTE.UserId = AU.UserId
                            JOIN UserManager UM ON AU.UserId = UM.UserId
                            WHERE UM.ManagerId = @ManagerId
                            AND UTE.ManagerApprovalStatus = @EnrollStatusEnum
                            ";

            try
            {
                using (SqlCommand cmd = new SqlCommand(sql, _conn))
                {
                    cmd.Parameters.AddWithValue("@ManagerId", UserId);
                    cmd.Parameters.AddWithValue("@EnrollStatusEnum", (int)enrollStatusEnum);

                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        while (reader.Read())
                        {
                            pendingUserTraining.Add(DbHelper.ConvertToObject<UserTraining>(reader));
                        }
                    }
                }
                return pendingUserTraining;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw new DbErrorException("Database Error");
                throw;
            }

        }

        public async Task<IEnumerable<TrainingEmployeeDetails>> GetAllTrainingEmployeeDetailsByTrainingId(int trainingId)
        {
            List<TrainingEmployeeDetails> results = new List<TrainingEmployeeDetails>();

            string sql = @" SELECT 
                                u.FirstName + ' ' + u.LastName AS EmployeeName,
                                u.Email AS ContactEmail,
                                u.MobileNumber AS ContactNumber,
                                m.FirstName + ' ' + m.LastName AS ManagerName
                            FROM 
                                UserTrainingEnrollment ute
                            INNER JOIN 
                                AppUser u ON ute.UserId = u.UserId
                            LEFT JOIN 
                                UserManager um ON u.UserId = um.UserId
                            LEFT JOIN 
                                AppUser m ON um.ManagerId = m.UserId
                            WHERE 
                                ute.TrainingId = @TrainingId;";

            try
            {
                using (SqlCommand cmd = new SqlCommand(sql, _conn))
                {
                    cmd.Parameters.AddWithValue("@TrainingId", trainingId);

                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        while (reader.Read())
                        {
                            results.Add(DbHelper.ConvertToObject<TrainingEmployeeDetails>(reader));
                        };

                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw new DbErrorException("Database Error");
                throw;
            }

            return results;

        }

        public async Task SoftDeleteTrainingAsync(int trainingId)
        {
            try
            {
                string sql = @" UPDATE Training
                                SET IsActive = 0
                                WHERE TrainingId = @TrainingId;";


                using (SqlCommand cmd = new SqlCommand(sql, _conn))
                {
                    cmd.Parameters.AddWithValue("@TrainingId", trainingId);

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

        public async Task AutoProcess()
        {
            try
            {
                string sql = $"EXEC AutomaticProcess";
                SqlCommand cmd = new SqlCommand(sql, _conn);

                await cmd.ExecuteNonQueryAsync();
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
