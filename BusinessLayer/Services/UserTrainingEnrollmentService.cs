using BusinessLayer.Services.Interfaces;
using DataLayer.Repository;
using DataLayer.Repository.Interfaces;
using Entities.DbModels;
using Entities.DTO;
using Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services
{
    public class UserTrainingEnrollmentService : IUserTrainingEnrollmentService
    {
        IUserTrainingEnrollmentRepo _userTrainingEnrollmentRepo;

        public UserTrainingEnrollmentService(IUserTrainingEnrollmentRepo userTrainingEnrollmentRepo)
        {
            _userTrainingEnrollmentRepo = userTrainingEnrollmentRepo;
        }

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

        public UserTrainingEnrollment GetUserTrainingEnrollmentByUserTraining(string userId, int trainingId)
        {
            return _userTrainingEnrollmentRepo.GetUserTrainingEnrollmentByUserTraining(userId, trainingId);
        }

        public IEnumerable<TrainingEnrollmentDetails> GetUserTrainingEnrollmentInfo(string userId, int trainingId)
        {
            return _userTrainingEnrollmentRepo.GetUserTrainingEnrollmentInfo(userId, trainingId);
        }

        public void ProcessTrainingRequest(string targetUserId, int targetTrainingId, bool isApproved)
        {
            var userEnrollment = _userTrainingEnrollmentRepo.GetUserTrainingEnrollmentByUserTraining(targetUserId, targetTrainingId);

            userEnrollment.Status = isApproved ? EnrollStatusEnum.Approved : EnrollStatusEnum.Rejected;

            _userTrainingEnrollmentRepo.Update(userEnrollment);


            // TODO: Send Mail

        }
    }
}
