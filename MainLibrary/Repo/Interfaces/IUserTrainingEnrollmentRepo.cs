using MainLibrary.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainLibrary.Repo.Interfaces
{
    public interface IUserTrainingEnrollmentRepo
    {
        void CreateUserTrainingEnrollment(UserTrainingEnrollment userTrainingEnrollment);
        void DeleteUserTrainingEnrollment(int userTrainingEnrollmentId);
        IEnumerable<UserTrainingEnrollment> GetAllUserTrainingEnrollments();
        UserTrainingEnrollment GetUserTrainingEnrollment(int userTrainingEnrollmentId);
        void UpdateUserTrainingEnrollment(UserTrainingEnrollment userTrainingEnrollment);
    }
}
