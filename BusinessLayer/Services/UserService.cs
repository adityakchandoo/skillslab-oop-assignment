using BusinessLayer.Other;
using BusinessLayer.Services.Interfaces;
using DataLayer.Repository.Interfaces;
using Entities.AppLogger;
using Entities.DbCustom;
using Entities.DbModels;
using Entities.DTO;
using Entities.Enums;
using Entities.FormDTO;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BusinessLayer.Services
{
    public class UserService : IUserService
    {
        private ILogger _logger;
        private readonly IAppUserRepo _appUserRepo;
        private readonly IUserManagerRepo _userManagerRepo;
        private readonly IUserRoleRepo _userRoleRepo;
        private readonly INotificationService _notificationService;
        public UserService(ILogger logger, IAppUserRepo appUserRepo, IUserManagerRepo userManagerRepo, IUserRoleRepo userRoleRepo, INotificationService notificationService)
        {
            _logger = logger;
            _appUserRepo = appUserRepo;
            _userManagerRepo = userManagerRepo;
            _userRoleRepo = userRoleRepo;
            _notificationService = notificationService;
        }

        public async Task<AppUser> GetUserAsync(int UserId)
        {
            try
            {
                if (UserId <= 0)
                {
                    throw new ArgumentException("UserId must be a positive integer.");
                }

                return await _appUserRepo.GetByPKAsync(UserId);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw new ArgumentException("Validation Error");
                throw;
            }
        }

        public async Task<AuthenticateResponse> AuthenticateUserAsync(UserLoginFormDTO userLoginFormDTO)
        {
            try
            {
                // Validate Username
                if (string.IsNullOrEmpty(userLoginFormDTO.Username))
                {
                    throw new ArgumentNullException("Enter Username");
                }

                // Validate Password
                if (string.IsNullOrEmpty(userLoginFormDTO.Password))
                {
                    throw new ArgumentNullException("Enter Password");
                }

                AuthenticateResponse authenticateResponse = new AuthenticateResponse();
                AppUser appUser = await _appUserRepo.GetUserByUsernameAsync(userLoginFormDTO.Username);

                if (appUser == null)
                {
                    authenticateResponse.IsLoginSuccessful = false;
                    authenticateResponse.Error = "Invalid User/Pass";
                    return authenticateResponse;
                }

                if (appUser.Status != UserStatusEnum.Registered)
                {
                    authenticateResponse.IsLoginSuccessful = false;
                    authenticateResponse.Error = "User not comfirmed";
                    return authenticateResponse;
                }

                if (PasswordHasher.VerifySHA256Hash(userLoginFormDTO.Password, appUser.Password) == false)
                {
                    authenticateResponse.IsLoginSuccessful = false;
                    authenticateResponse.Error = "Invalid User/Pass";
                    return authenticateResponse;

                }

                // Beyound the point login is successful
                authenticateResponse.IsLoginSuccessful = true;
                authenticateResponse.AppUser = appUser;

                return authenticateResponse;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw ex;
                throw;
            }

        }

        public async Task<IEnumerable<AppUserRole>> GetRolesByUserIdAsync(int UserId)
        {
            try
            {
                if (UserId <= 0)
                {
                    throw new ArgumentException("UserId must be a positive integer.");
                }

                return await _appUserRepo.GetRolesByUserIdAsync(UserId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw ex;
                throw;
            }
        }

        public async Task RegisterAsync(RegisterFormDTO registerFormDTO)
        {
            try
            {
                // Validate UserName
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
                if (CommonValidations.IsEmailValid(registerFormDTO.Email) == false)
                {
                    throw new ArgumentException("Enter a valid Email");
                }

                // Validate NIC
                if (string.IsNullOrEmpty(registerFormDTO.NIC) || CommonValidations.IsNICValid(registerFormDTO.NIC) == false)
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

                if (CommonValidations.IsDOBValid(registerFormDTO.DOB) == false)
                {
                    throw new ArgumentException("User must be at least 16 years old.");
                }

                if (CommonValidations.IsPhoneNumberValid(registerFormDTO.MobileNumber) == false)
                {
                    throw new ArgumentException("Input must be 8 digits if it starts with 5, otherwise 7 digits.");
                }

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

                await _appUserRepo.RegisterWithRoleAndManager(db_user, registerFormDTO.ManagerId);

                //var InsertedId = await _appUserRepo.CreateUserReturningIDAsync(db_user);
                //await _userManagerRepo.Insert(new UserManager() { UserId = InsertedId, ManagerId = registerFormDTO.ManagerId });
                //await _userRoleRepo.Insert(new UserRole() { UserId = InsertedId, RoleId = (int)UserRoleEnum.Employee });

                // Send Notification
                var managerEmail = (await _appUserRepo.GetByPKAsync(registerFormDTO.ManagerId)).Email;
                var employeeName = registerFormDTO.FirstName + " " + registerFormDTO.LastName;

                await _notificationService.NotifyUserRegistrationAsync(managerEmail, employeeName);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw ex;
                throw;
            }


        }

        public async Task<IEnumerable<AppUsersInlineRoles>> GetAllUsersWithInlineRolesAsync()
        {
            return await _appUserRepo.GetAllUsersWithInlineRolesAsync();
        }

        public async Task<bool> IsUsernameExistsAsync(string value)
        {
            try
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentNullException("Enter UserName");
                }

                return await _appUserRepo.IsRecordExistsAsync("UserName", value);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw ex;
                throw;
            }
        }

        public async Task<bool> IsNICExistsAsync(string value)
        {
            try
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentNullException("Enter NIC");
                }

                return await _appUserRepo.IsRecordExistsAsync("NIC", value);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw ex;
                throw;
            }
        }

        public async Task<bool> IsEmailExistsAsync(string value)
        {
            try
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentNullException("Enter Valid Email");
                }

                return await _appUserRepo.IsRecordExistsAsync("Email", value);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw ex;
                throw;
            }
        }

        public async Task<IEnumerable<AppUser>> GetAllUsersByTypeAsync(UserRoleEnum userRoleEnum)
        {
            try
            {
                return await _appUserRepo.GetAllUsersByRoleAsync(userRoleEnum);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw ex;
                throw;
            }
        }

        public async Task<IEnumerable<AppUser>> GetUsersByManagerAsync(int UserId)
        {
            try
            {
                if (UserId <= 0)
                {
                    throw new ArgumentException("UserId must be a positive integer.");
                }

                return await _appUserRepo.GetAllUsersByManagerAsync(UserId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw ex;
                throw;
            }
        }

        public async Task<IEnumerable<AppUser>> GetUsersByManagerAndStatusAsync(int UserId, UserStatusEnum userStatusEnum)
        {
            try
            {
                if (UserId <= 0)
                {
                    throw new ArgumentException("UserId must be a positive integer.");
                }

                return await _appUserRepo.GetAllUsersByManagerAndStatusAsync(UserId, userStatusEnum);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw ex;
                throw;
            }
        }

        public async Task UpdateProfileAsync(int UserId, UpdateProfileDTO updateProfileDTO)
        {
            try
            {
                if (UserId <= 0)
                {
                    throw new ArgumentException("UserId must be a positive integer.");
                }

                if (
                    string.IsNullOrEmpty(updateProfileDTO.UserName) ||
                    string.IsNullOrEmpty(updateProfileDTO.Email) ||
                    string.IsNullOrEmpty(updateProfileDTO.MobileNumber)
                )
                {
                    throw new ArgumentNullException(nameof(updateProfileDTO));
                }

                if (CommonValidations.IsPhoneNumberValid(updateProfileDTO.MobileNumber) == false)
                {
                    throw new ArgumentException("Input must be 8 digits if it starts with 5, otherwise 7 digits.");
                }

                // Validate Email
                var emailRegex = new Regex("/^(([^<>()[\\]\\\\.,;:\\s@\"]+(\\.[^<>()[\\]\\\\.,;:\\s@\"]+)*)|(\".+\"))@((\\[[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\])|(([a-zA-Z\\-0-9]+\\.)+[a-zA-Z]{2,}))$/");
                if (emailRegex.IsMatch(updateProfileDTO.Email))
                {
                    throw new ArgumentException("Enter a valid Email");
                }

                AppUser user = await _appUserRepo.GetByPKAsync(UserId);

                user.UserName = updateProfileDTO.UserName;
                user.Email = updateProfileDTO.Email;
                user.MobileNumber = updateProfileDTO.MobileNumber;

                await _appUserRepo.Update(user);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw ex;
                throw;
            }


        }

        public async Task UpdatePasswordAsync(int UserId, UpdatePasswordDTO updatePasswordDTO)
        {
            if (
                string.IsNullOrEmpty(updatePasswordDTO.OldPassword) ||
                string.IsNullOrEmpty(updatePasswordDTO.NewPassword) ||
                string.IsNullOrEmpty(updatePasswordDTO.ConfirmNewPassword)
                )
            {
                throw new ArgumentNullException(nameof(updatePasswordDTO));
            }

            if (!updatePasswordDTO.NewPassword.Equals(updatePasswordDTO.ConfirmNewPassword))
            {
                throw new ArgumentException("Confirm pass is not same");
            }

            AppUser user = await _appUserRepo.GetByPKAsync(UserId);

            if (PasswordHasher.VerifySHA256Hash(updatePasswordDTO.OldPassword, user.Password) == false)
            {
                throw new ArgumentException("Current pass is not correct");
            }

            user.Password = PasswordHasher.GenerateSHA256Hash(updatePasswordDTO.NewPassword);

            await _appUserRepo.Update(user);
        }

        public async Task ProcessNewUserAsync(int UserId, bool isApprove)
        {
            try
            {
                if (UserId <= 0)
                {
                    throw new ArgumentException("UserId must be a positive integer.");
                }

                AppUser user = await _appUserRepo.GetByPKAsync(UserId);
                AppUser userManger = await _appUserRepo.GetUserManagerAsync(UserId);

                if (userManger == null)
                {
                    throw new ArgumentException("This User has no manager");
                }

                user.Status = isApprove ? UserStatusEnum.Registered : UserStatusEnum.Banned;

                await _appUserRepo.Update(user);

                var managerName = userManger.FirstName + " " + userManger.LastName;

                _ = _notificationService.NotifyUserRegistrationProcessAsync(user.Email, managerName, isApprove);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw ex;
                throw;
            }
        }

        public async Task SoftDeleteAppUser(int UserId)
        {
            try
            {
                if (UserId <= 0)
                {
                    throw new ArgumentException("UserId must be a positive integer.");
                }

                await _appUserRepo.SoftDeleteAppUser(UserId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw ex;
                throw;
            }
        }

        public async Task<bool> CheckPermissionAsync(int UserId, string permission)
        {
            return await _appUserRepo.CheckPermissionAsync(UserId, permission);
        }

        public async Task<AppUser> GetUserManagerAsync(int UserId)
        {
            return await _appUserRepo.GetUserManagerAsync(UserId);
        }

        public async Task TestAsync()
        {
            await _appUserRepo.TestAsync();
        }
    }
}
