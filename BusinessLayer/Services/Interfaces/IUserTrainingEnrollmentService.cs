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
        void CreateUserTrainingEnrollment(UserTrainingEnrollment userTrainingEnrollment);
        void DeleteUserTrainingEnrollment(int userTrainingEnrollmentId);
        IEnumerable<UserTrainingEnrollment> GetAllUserTrainingEnrollments();
        UserTrainingEnrollment GetUserTrainingEnrollment(int userTrainingEnrollmentId);
        UserTrainingEnrollment GetUserTrainingEnrollment(int userId, int trainingId);
        void UpdateUserTrainingEnrollment(UserTrainingEnrollment userTrainingEnrollment);
        IEnumerable<TrainingEnrollmentDetails> GetUserTrainingEnrollmentInfo(int userId, int trainingId);
        void ProcessTrainingRequest(int targetUserId, int targetTrainingId, bool approve);
    }
}
