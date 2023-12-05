using BusinessLayer.Services.Interfaces;
using DataLayer.Repository;
using Entities.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services
{
    public class UserTrainingEnrollmentService : IUserTrainingEnrollmentService
    {
        UserTrainingEnrollmentRepo _userTrainingEnrollmentRepo;
        public UserTrainingEnrollmentService(UserTrainingEnrollmentRepo userTrainingEnrollmentRepo) 
        {
            _userTrainingEnrollmentRepo = userTrainingEnrollmentRepo;
        }
        public void CreateUserTrainingEnrollment(UserTrainingEnrollment userTrainingEnrollment)
        {
            _userTrainingEnrollmentRepo.Insert(userTrainingEnrollment);
        }

        public void DeleteUserTrainingEnrollment(int userTrainingEnrollmentId)
        {
            _userTrainingEnrollmentRepo.Delete(new UserTrainingEnrollment() { UserTrainingEnrollmentId = userTrainingEnrollmentId });
        }

        public IEnumerable<UserTrainingEnrollment> GetAllUserTrainingEnrollments()
        {
            return _userTrainingEnrollmentRepo.GetMany();
        }

        public UserTrainingEnrollment GetUserTrainingEnrollment(int userTrainingEnrollmentId)
        {
            return _userTrainingEnrollmentRepo.GetByPK(userTrainingEnrollmentId);

        }

        public void UpdateUserTrainingEnrollment(UserTrainingEnrollment userTrainingEnrollment)
        {
            _userTrainingEnrollmentRepo.Update(userTrainingEnrollment);
        }
    }
}
