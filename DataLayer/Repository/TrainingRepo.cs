using System;
using System.Collections.Generic;
using System.Data;
using DataLayer;
using DataLayer.Generic;
using DataLayer.Repository.Interfaces;
using Entities.DbCustom;
using Entities.DbModels;
using Entities.DTO;
using Entities.Enums;

namespace DataLayer.Repository
{
    public class TrainingRepo : DataAccessLayer<Training>, ITrainingRepo
    {
        private readonly IDbConnection _conn;
        public TrainingRepo(IDbContext dbContext) : base(dbContext)
        {
            _conn = dbContext.GetConn();
        }

        public int CreateTrainingReturningID(Training training)
        {
            try
            {
                string sql = @"INSERT INTO [dbo].[Training] (Name, Description, MaxSeat, Deadline, PreferedDepartmentId) 
                               OUTPUT Inserted.TrainingId 
                               VALUES (@Name, @Description, @MaxSeat, @Deadline, @PreferedDepartmentId)";


                using (IDbCommand cmd = _conn.CreateCommand())
                {
                    cmd.CommandText = sql;

                    DbHelper.AddParameterWithValue(cmd, "@Name", training.Name);
                    DbHelper.AddParameterWithValue(cmd, "@Description", training.Description);
                    DbHelper.AddParameterWithValue(cmd, "@MaxSeat", training.MaxSeat);
                    DbHelper.AddParameterWithValue(cmd, "@Deadline", training.Deadline);
                    DbHelper.AddParameterWithValue(cmd, "@PreferedDepartmentId", training.PreferedDepartmentId == -1 ? DBNull.Value : (object)training.PreferedDepartmentId);

                    return (int)cmd.ExecuteScalar();
                }

            } catch (Exception ex) { throw ex; }
        }
        public IEnumerable<TrainingStatus> GetAllTraining(int UserId)
        {
            List<TrainingStatus> results = new List<TrainingStatus>();

            string sql = @"SELECT 
                            T.*,
                            CASE 
                                WHEN UTE.UserId IS NULL THEN 0
                                ELSE UTE.Status
                            END AS EnrollmentStatus
                            FROM 
                              Training T
                            LEFT JOIN 
                              UserTrainingEnrollment UTE ON T.TrainingId = UTE.TrainingId AND UTE.UserId = @UserId;";

            try
            {
                using (IDbCommand cmd = _conn.CreateCommand())
                {
                    cmd.CommandText = sql;
                    DbHelper.AddParameterWithValue(cmd, "@UserId", UserId);

                    using (IDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            results.Add(DbHelper.ConvertToObject<TrainingStatus>(reader));
                        };

                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
                throw;
            }

            return results;
        }
        public IEnumerable<TrainingStatus> GetTrainingEnrolledByUser(int UserId, EnrollStatusEnum status)
        {
            List<TrainingStatus> results = new List<TrainingStatus>();

            string sql = $@"SELECT 
                            T.*,
                            UTE.Status AS EnrollmentStatus
                            FROM 
                              Training T
                            LEFT JOIN 
                              UserTrainingEnrollment UTE ON T.TrainingId = UTE.TrainingId
                            WHERE UTE.UserId = @UserId AND UTE.Status = @Status;";
            try
            {
                using (IDbCommand cmd = _conn.CreateCommand())
                {
                    cmd.CommandText = sql;
                    DbHelper.AddParameterWithValue(cmd, "@UserId", UserId);
                    DbHelper.AddParameterWithValue(cmd, "@Status", (int)status);

                    using (IDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            results.Add(DbHelper.ConvertToObject<TrainingStatus>(reader));
                        };

                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
                throw;
            }

            return results;

        }
        public IEnumerable<TrainingStatus> GetTrainingEnrolledByUser(int UserId)
        {
            List<TrainingStatus> results = new List<TrainingStatus>();

            string sql = $@"SELECT 
                            T.*,
                            UTE.Status AS EnrollmentStatus
                            FROM 
                              Training T
                            LEFT JOIN 
                              UserTrainingEnrollment UTE ON T.TrainingId = UTE.TrainingId
                            WHERE UTE.UserId = @UserId;";
            try
            {
                using (IDbCommand cmd = _conn.CreateCommand())
                {
                    cmd.CommandText = sql;
                    DbHelper.AddParameterWithValue(cmd, "@UserId", UserId);

                    using (IDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            results.Add(DbHelper.ConvertToObject<TrainingStatus>(reader));
                        };

                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
                throw;
            }

            return results;

        }
        public IEnumerable<TrainingEnrollCount> GetAllTrainingWithEnrollCount()
        {
            List<TrainingEnrollCount> results = new List<TrainingEnrollCount>();

            // AllTrainingWithDepartmentName is a view

            string sql = @"SELECT Training.*, A.DepartmentName, A.NumberOfEmployeesEnrolled FROM Training 
                           LEFT JOIN AllTrainingWithDepartmentName A ON Training.TrainingId = A.TrainingId";

            try
            {
                using (IDbCommand cmd = _conn.CreateCommand())
                {
                    cmd.CommandText = sql;

                    using (IDataReader reader = cmd.ExecuteReader())
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
                throw ex;
                throw;
            }

            return results;
        }
        public IEnumerable<UserTraining> GetUserTrainingByStatusAndManagerId(EnrollStatusEnum enrollStatusEnum, int UserId)
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
                            AND UTE.Status = @EnrollStatusEnum
                            ";

            try
            {
                using (IDbCommand cmd = _conn.CreateCommand())
                {
                    cmd.CommandText = sql;

                    DbHelper.AddParameterWithValue(cmd, "@ManagerId", UserId);
                    DbHelper.AddParameterWithValue(cmd, "@EnrollStatusEnum", (int)enrollStatusEnum);


                    using (IDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            pendingUserTraining.Add(DbHelper.ConvertToObject<UserTraining>(reader));
                        }
                    }
                }
                return pendingUserTraining;

            }
            catch (Exception ex) { throw ex; }

        }

    }
}
