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
        private readonly IUserTrainingEnrollmentRepo _userTrainingEnrollmentRepo;
        private readonly INotificationService _notificationService;
        private readonly IAppUserRepo _appUserRepo;
        private readonly ITrainingRepo _trainingRepo;

        public UserTrainingEnrollmentService(IUserTrainingEnrollmentRepo userTrainingEnrollmentRepo, INotificationService notificationService, IAppUserRepo appUserRepo, ITrainingRepo trainingRepo)
        {
            _userTrainingEnrollmentRepo = userTrainingEnrollmentRepo;
            _notificationService = notificationService;
            _appUserRepo = appUserRepo;
            _trainingRepo = trainingRepo;
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

        public UserTrainingEnrollment GetUserTrainingEnrollment(int userId, int trainingId)
        {
            return _userTrainingEnrollmentRepo.GetUserTrainingEnrollment(userId, trainingId);
        }

        public IEnumerable<TrainingEnrollmentDetails> GetUserTrainingEnrollmentInfo(int userId, int trainingId)
        {
            return _userTrainingEnrollmentRepo.GetUserTrainingEnrollmentInfo(userId, trainingId);
        }

        public void ProcessTrainingRequest(int targetUserId, int targetTrainingId, bool isApproved)
        {
            var userEnrollment = _userTrainingEnrollmentRepo.GetUserTrainingEnrollment(targetUserId, targetTrainingId);

            userEnrollment.Status = isApproved ? EnrollStatusEnum.Approved : EnrollStatusEnum.Rejected;
            userEnrollment.EnrolledDate = DateTime.Now; 

            _userTrainingEnrollmentRepo.Update(userEnrollment);

            var user = _appUserRepo.GetByPK(targetUserId);
            var training = _trainingRepo.GetByPK(targetTrainingId);

            _notificationService.NotifyTrainingRequestProcess(user.Email, training.Name, isApproved);

        }
    }
}
