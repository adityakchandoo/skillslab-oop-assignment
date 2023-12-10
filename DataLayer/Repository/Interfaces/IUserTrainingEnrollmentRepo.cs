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
        int CreateUserTrainingEnrollmentReturningID(UserTrainingEnrollment userTrainingEnrollment);
        UserTrainingEnrollment GetUserTrainingEnrollmentByUserTraining(string targetUserId, int targetTrainingId);
        IEnumerable<TrainingEnrollmentDetails> GetUserTrainingEnrollmentInfo(string userId, int trainingId);
    }
}
