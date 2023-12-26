using DataLayer.Generic;
using Entities.DbModels;
using Entities.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repository.Interfaces
{
    public interface IUserTrainingEnrollmentRepo : IDataAccessLayer<UserTrainingEnrollment>
    {
        Task<int> CreateUserTrainingEnrollmentReturningIDAsync(UserTrainingEnrollment userTrainingEnrollment);
        Task<UserTrainingEnrollment> GetUserTrainingEnrollmentAsync(int targetUserId, int targetTrainingId);
        Task<IEnumerable<TrainingEnrollmentDetails>> GetUserTrainingEnrollmentInfoAsync(int userId, int trainingId);
    }
}
