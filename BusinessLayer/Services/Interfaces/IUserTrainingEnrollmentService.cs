using Entities.DbModels;
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
        void UpdateUserTrainingEnrollment(UserTrainingEnrollment userTrainingEnrollment);
    }
}
