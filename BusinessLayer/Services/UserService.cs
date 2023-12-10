using BusinessLayer.Services.Interfaces;
using DataLayer.Repository;
using DataLayer.Repository.Interfaces;
using Entities.DbModels;
using Entities.DTO;
using Entities.Enums;
using Entities.FormDTO;
using System;
using System.Collections.Generic;


namespace BusinessLayer.Services
{
    public class UserService : IUserService
    {
        private readonly IAppUserRepo _appUserRepo;
        private readonly INotificationService _notificationService;
        public UserService(IAppUserRepo appUserRepo, INotificationService notificationService)
        {
            _appUserRepo = appUserRepo;
            _notificationService = notificationService;
        }


        public IEnumerable<AppUser> ExportSelectedEmployees()
        {
            throw new NotImplementedException();
        }

        public void ForgetPass()
        {
            throw new NotImplementedException();
        }

        void IUserService.ResetPass()
        {
            throw new NotImplementedException();
        }

        public AuthenticateResponse AuthenticateUser(UserLoginFormDTO form)
        {
            AuthenticateResponse authenticateResponse = new AuthenticateResponse();
            AppUser appUser = _appUserRepo.GetByPK(form.Username);

            if (PasswordHasher.VerifySHA256Hash(form.Password, appUser.Password))
            {
                authenticateResponse.IsLoginSuccessful = true;
                authenticateResponse.RedirectPath = "/"+ appUser.Role.ToString();
                authenticateResponse.AppUser = appUser;
            }
            else
            {
                authenticateResponse.IsLoginSuccessful = false;
            }

            return authenticateResponse;

        }
        public void Register(RegisterFormDTO reg)
        {
            try
            {
                AppUser db_user = new AppUser();
                db_user.UserId = reg.UserId;
                db_user.FirstName = reg.FirstName;
                db_user.LastName = reg.LastName;
                db_user.Email = reg.Email;
                db_user.Password = PasswordHasher.GenerateSHA256Hash(reg.Pass1);
                db_user.Email = reg.Email;
                db_user.DOB = reg.DOB;
                db_user.NIC = reg.NIC;
                db_user.MobileNumber = reg.MobileNumber;
                db_user.DepartmentId = reg.DepartmentId == -1 ? null : reg.DepartmentId;
                db_user.ManagerId = reg.ManagerId;
                db_user.Status = UserStatusEnum.Pending;
                db_user.Role = UserRoleEnum.Employee;

                _appUserRepo.Insert(db_user);

                var managerEmail = _appUserRepo.GetByPK(reg.ManagerId).Email;

                var employeeName = reg.FirstName + " " + reg.LastName;

                _notificationService.NotifyUserRegistration(managerEmail, employeeName);

            } catch (Exception ex)
            {
                throw;
            }


        }

        public bool IsUserIdExists(string UserId)
        {
            AppUser appUser = _appUserRepo.GetByPK(UserId);

            // User Should not exits in database
            if(appUser == null) { return true; } else { return false; }

        }

        public void ConfirmAccount(string UserId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<AppUser> GetAllUsersByType(UserRoleEnum userRoleEnum)
        {
            return _appUserRepo.GetAllUsersByType(userRoleEnum);
        }

        public AppUser GetUser(string UserId)
        {
            return _appUserRepo.GetByPK(UserId);
        }
        public IEnumerable<AppUser> GetUsersByManager(string UserId)
        {
            return _appUserRepo.GetUsersByManager(UserId);
        }

        public IEnumerable<AppUser> GetUsersByManagerAndStatus(string UserId, UserStatusEnum userStatusEnum)
        {
            return _appUserRepo.GetUsersByManagerAndStatus(UserId, userStatusEnum);
        }

        public void ProcessNewUser(string userId, bool isApprove)
        {
            AppUser user = _appUserRepo.GetByPK(userId);
            AppUser userManger = _appUserRepo.GetByPK(user.ManagerId);
            user.Status = isApprove ? UserStatusEnum.Registered : UserStatusEnum.Banned;

            _appUserRepo.Update(user);

            var managerName = userManger.FirstName + " " + userManger.LastName;

            _notificationService.NotifyUserRegistrationProcess(user.Email, managerName, isApprove);

        }
    }
}
