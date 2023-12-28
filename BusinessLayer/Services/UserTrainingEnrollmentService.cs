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

        public async Task CreateUserTrainingEnrollmentAsync(UserTrainingEnrollment userTrainingEnrollment)
        {
            await _userTrainingEnrollmentRepo.Insert(userTrainingEnrollment);
        }

        public async Task DeleteUserTrainingEnrollmentAsync(int userTrainingEnrollmentId)
        {
            await _userTrainingEnrollmentRepo.Delete(new UserTrainingEnrollment() { UserTrainingEnrollmentId = userTrainingEnrollmentId });
        }

        public async Task<IEnumerable<UserTrainingEnrollment>> GetAllUserTrainingEnrollmentsAsync()
        {
            return await _userTrainingEnrollmentRepo.GetMany();
        }

        public async Task<UserTrainingEnrollment> GetUserTrainingEnrollmentAsync(int userTrainingEnrollmentId)
        {
            return await _userTrainingEnrollmentRepo.GetByPKAsync(userTrainingEnrollmentId);

        }

        public async Task UpdateUserTrainingEnrollmentAsync(UserTrainingEnrollment userTrainingEnrollment)
        {
            await _userTrainingEnrollmentRepo.Update(userTrainingEnrollment);
        }

        public async Task<UserTrainingEnrollment> GetUserTrainingEnrollmentAsync(int userId, int trainingId)
        {
            return await _userTrainingEnrollmentRepo.GetUserTrainingEnrollmentAsync(userId, trainingId);
        }

        public async Task<IEnumerable<TrainingEnrollmentDetails>> GetUserTrainingEnrollmentInfoAsync(int userId, int trainingId)
        {
            return await _userTrainingEnrollmentRepo.GetUserTrainingEnrollmentInfoAsync(userId, trainingId);
        }

        public async Task ProcessTrainingRequestAsync(int targetUserId, int targetTrainingId, bool isApproved, string declineReason)
        {
            if (!isApproved && string.IsNullOrEmpty(declineReason))
            {
                throw new ArgumentNullException("Must Have decline reason, if not approved");
            }

            var userEnrollment = await _userTrainingEnrollmentRepo.GetUserTrainingEnrollmentAsync(targetUserId, targetTrainingId);

            userEnrollment.ManagerApprovalStatus = isApproved ? EnrollStatusEnum.Approved : EnrollStatusEnum.Rejected;
            userEnrollment.EnrolledDate = DateTime.Now;
            userEnrollment.DeclineReason = string.IsNullOrEmpty(declineReason) ? null : declineReason;

            await _userTrainingEnrollmentRepo.Update(userEnrollment);

            var user = await _appUserRepo.GetByPKAsync(targetUserId);
            var training = await _trainingRepo.GetByPKAsync(targetTrainingId);

            _ = _notificationService.NotifyTrainingRequestProcessAsync(user.Email, training.Name, isApproved, declineReason);

        }
    }
}
