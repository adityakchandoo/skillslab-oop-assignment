using Entities.DbModels;
using Entities.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services.Interfaces
{
    public interface IUserTrainingEnrollmentService
    {
        Task CreateUserTrainingEnrollmentAsync(UserTrainingEnrollment userTrainingEnrollment);
        Task DeleteUserTrainingEnrollmentAsync(int userTrainingEnrollmentId);
        Task<IEnumerable<UserTrainingEnrollment>> GetAllUserTrainingEnrollmentsAsync();
        Task<UserTrainingEnrollment> GetUserTrainingEnrollmentAsync(int userTrainingEnrollmentId);
        Task<UserTrainingEnrollment> GetUserTrainingEnrollmentAsync(int userId, int trainingId);
        Task UpdateUserTrainingEnrollmentAsync(UserTrainingEnrollment userTrainingEnrollment);
        Task<IEnumerable<TrainingEnrollmentDetails>> GetUserTrainingEnrollmentInfoAsync(int userId, int trainingId);
        Task ProcessTrainingRequestAsync(int targetUserId, int targetTrainingId, bool approve);
    }
}
