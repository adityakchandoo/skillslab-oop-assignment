using MainLibrary.Entities;
using MainLibrary.Repo.Interfaces;
using MainLibrary.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainLibrary.Services
{
    public class UserTrainingEnrollmentService : IUserTrainingEnrollmentService
    {
        IUserTrainingEnrollmentRepo _userTrainingEnrollmentRepo;
        public UserTrainingEnrollmentService(IUserTrainingEnrollmentRepo userTrainingEnrollmentRepo) 
        {
            _userTrainingEnrollmentRepo = userTrainingEnrollmentRepo;
        }
        public void CreateUserTrainingEnrollment(UserTrainingEnrollment userTrainingEnrollment)
        {
            _userTrainingEnrollmentRepo.CreateUserTrainingEnrollment(userTrainingEnrollment);
        }

        public void DeleteUserTrainingEnrollment(int userTrainingEnrollmentId)
        {
            _userTrainingEnrollmentRepo.DeleteUserTrainingEnrollment(userTrainingEnrollmentId);
        }

        public IEnumerable<UserTrainingEnrollment> GetAllUserTrainingEnrollments()
        {
            return _userTrainingEnrollmentRepo.GetAllUserTrainingEnrollments();
        }

        public UserTrainingEnrollment GetUserTrainingEnrollment(int userTrainingEnrollmentId)
        {
            return _userTrainingEnrollmentRepo.GetUserTrainingEnrollment(userTrainingEnrollmentId);

        }

        public void UpdateUserTrainingEnrollment(UserTrainingEnrollment userTrainingEnrollment)
        {
            _userTrainingEnrollmentRepo.UpdateUserTrainingEnrollment(userTrainingEnrollment);
        }
    }
}
