using BusinessLayer.Services.Interfaces;
using DataLayer.Repository;
using DataLayer.Repository.Interfaces;
using Entities.DbCustom;
using Entities.DbModels;
using Entities.DTO;
using Entities.Enums;
using Entities.FormDTO;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace BusinessLayer.Services
{
    public class UserService : IUserService
    {
        private readonly IAppUserRepo _appUserRepo;
        private readonly IUserManagerRepo _userManagerRepo;
        private readonly IUserRoleRepo _userRoleRepo;
        private readonly INotificationService _notificationService;
        public UserService(IAppUserRepo appUserRepo, IUserManagerRepo userManagerRepo, IUserRoleRepo userRoleRepo, INotificationService notificationService)
        {
            _appUserRepo = appUserRepo;
            _userManagerRepo = userManagerRepo;
            _userRoleRepo = userRoleRepo;
            _notificationService = notificationService;
        }

        public AppUser GetUser(int UserId)
        {
            return _appUserRepo.GetByPK(UserId);
        }

        public AuthenticateResponse AuthenticateUser(UserLoginFormDTO userLoginFormDTO)
        {
            // Validate Username
            if (string.IsNullOrEmpty(userLoginFormDTO.Username))
            {
                throw new ArgumentException("Enter Username");
            }

            // Validate Password
            if (string.IsNullOrEmpty(userLoginFormDTO.Password))
            {
                throw new ArgumentException("Enter Password");
            }

            AuthenticateResponse authenticateResponse = new AuthenticateResponse();
            AppUserDetails appUser = _appUserRepo.GetUserByUsername(userLoginFormDTO.Username);

            if (appUser == null)
            {
                authenticateResponse.IsLoginSuccessful = false;
                return authenticateResponse;
            }

            if (PasswordHasher.VerifySHA256Hash(userLoginFormDTO.Password, appUser.Password))
            {
                authenticateResponse.IsLoginSuccessful = true;
                authenticateResponse.RedirectPath = "/" + appUser.RoleName;
                authenticateResponse.AppUser = appUser;
            }
            else
            {
                authenticateResponse.IsLoginSuccessful = false;
            }

            return authenticateResponse;

        }

        public void Register(RegisterFormDTO registerFormDTO)
        {
            // Validate UserId
            if (string.IsNullOrEmpty(registerFormDTO.UserName))
            {
                throw new ArgumentException("Enter UserName");
            }

            // Validate FirstName
            if (string.IsNullOrEmpty(registerFormDTO.FirstName))
            {
                throw new ArgumentException("Enter FirstName");
            }

            // Validate LastName
            if (string.IsNullOrEmpty(registerFormDTO.LastName))
            {
                throw new ArgumentException("Enter LastName");
            }

            // Validate Email
            var emailRegex = new Regex("/^(([^<>()[\\]\\\\.,;:\\s@\"]+(\\.[^<>()[\\]\\\\.,;:\\s@\"]+)*)|(\".+\"))@((\\[[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\])|(([a-zA-Z\\-0-9]+\\.)+[a-zA-Z]{2,}))$/");
            if (emailRegex.IsMatch(registerFormDTO.NIC))
            {
                throw new ArgumentException("Enter a valid Email");
            }

            // Validate NIC
            var nicRegex = new Regex("^.{1}[0-9]{6}.{7}$");
            if (string.IsNullOrEmpty(registerFormDTO.NIC) || !nicRegex.IsMatch(registerFormDTO.NIC))
            {
                throw new ArgumentException("Invalid NIC");
            }

            // Validate MobileNumber
            if (string.IsNullOrEmpty(registerFormDTO.MobileNumber))
            {
                throw new ArgumentException("Enter MobileNumber");
            }

            // Validate Password and Confirm Password
            if (string.IsNullOrEmpty(registerFormDTO.Pass1))
            {
                throw new ArgumentException("Enter Password");
            }
            if (registerFormDTO.Pass1 != registerFormDTO.Pass2)
            {
                throw new ArgumentException("Confirm Pass Not Same");
            }

            try
            {
                AppUser db_user = new AppUser
                {
                    UserName = registerFormDTO.UserName,
                    FirstName = registerFormDTO.FirstName,
                    LastName = registerFormDTO.LastName,
                    Email = registerFormDTO.Email,
                    Password = PasswordHasher.GenerateSHA256Hash(registerFormDTO.Pass1),
                    DOB = registerFormDTO.DOB,
                    NIC = registerFormDTO.NIC,
                    MobileNumber = registerFormDTO.MobileNumber,
                    DepartmentId = registerFormDTO.DepartmentId,
                    Status = UserStatusEnum.Pending
                };

                var InsertedId = _appUserRepo.CreateUserReturningID(db_user);


                _userManagerRepo.Insert(new UserManager() { UserId = InsertedId, ManagerId = registerFormDTO.ManagerId });

                _userRoleRepo.Insert(new UserRole() { UserId = InsertedId, RoleId = (int)UserRoleEnum.Employee });


                var managerEmail = _appUserRepo.GetByPK(registerFormDTO.ManagerId).Email;

                var employeeName = registerFormDTO.FirstName + " " + registerFormDTO.LastName;

                _notificationService.NotifyUserRegistration(managerEmail, employeeName);

            } catch (Exception ex)
            {
                throw ex;
                throw;
            }


        }

        public IEnumerable<AppUser> ExportSelectedEmployees()
        {
            throw new NotImplementedException();
        }

        public bool IsUsernameExists(string value)
        {
            return _appUserRepo.IsRecordExists("UserName", value);
        }

        public bool IsNICExists(string value)
        {
            return _appUserRepo.IsRecordExists("NIC", value);
        }

        public bool IsEmailExists(string value)
        {
            return _appUserRepo.IsRecordExists("Email", value);
        }

        public IEnumerable<AppUser> GetAllUsersByType(UserRoleEnum userRoleEnum)
        {
            return _appUserRepo.GetAllUsersByRole(userRoleEnum);
        }

        public IEnumerable<AppUser> GetUsersByManager(int UserId)
        {
            return _appUserRepo.GetAllUsersByManager(UserId);
        }

        public IEnumerable<AppUser> GetUsersByManagerAndStatus(int UserId, UserStatusEnum userStatusEnum)
        {
            return _appUserRepo.GetAllUsersByManagerAndStatus(UserId, userStatusEnum);
        }

        public void ProcessNewUser(int UserId, bool isApprove)
        {
            AppUser user = _appUserRepo.GetByPK(UserId);
            AppUser userManger = _appUserRepo.GetUserManager(UserId);
            user.Status = isApprove ? UserStatusEnum.Registered : UserStatusEnum.Banned;

            _appUserRepo.Update(user);

            var managerName = userManger.FirstName + " " + userManger.LastName;

            _notificationService.NotifyUserRegistrationProcess(user.Email, managerName, isApprove);

        }
    }
}
