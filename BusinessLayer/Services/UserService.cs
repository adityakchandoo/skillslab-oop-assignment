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
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

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

        public async Task<AppUser> GetUserAsync(int UserId)
        {
            return await _appUserRepo.GetByPKAsync(UserId);
        }

        public async Task<AuthenticateResponse> AuthenticateUserAsync(UserLoginFormDTO userLoginFormDTO)
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
                
            }            

            // Beyound the point login is successful
            authenticateResponse.IsLoginSuccessful = true;
            authenticateResponse.AppUser = appUser;

            return authenticateResponse;

        }

        public async Task<IEnumerable<AppUserRole>> GetRolesByUserIdAsync(int UserId)
        {
            return await _appUserRepo.GetRolesByUserIdAsync(UserId);
        }

        public async Task RegisterAsync(RegisterFormDTO registerFormDTO)
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
            if (emailRegex.IsMatch(registerFormDTO.Email))
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

                var InsertedId = await _appUserRepo.CreateUserReturningIDAsync(db_user);


                await _userManagerRepo.Insert(new UserManager() { UserId = InsertedId, ManagerId = registerFormDTO.ManagerId });

                await _userRoleRepo.Insert(new UserRole() { UserId = InsertedId, RoleId = (int)UserRoleEnum.Employee });


                var managerEmail = (await _appUserRepo.GetByPKAsync(registerFormDTO.ManagerId)).Email;

                var employeeName = registerFormDTO.FirstName + " " + registerFormDTO.LastName;

                await _notificationService.NotifyUserRegistrationAsync(managerEmail, employeeName);

            } catch (Exception ex)
            {
                throw ex;
                throw;
            }


        }

        public Task<IEnumerable<AppUser>> ExportSelectedEmployeesAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<AppUsersInlineRoles>> GetAllUsersWithInlineRolesAsync()
        {
            return await _appUserRepo.GetAllUsersWithInlineRolesAsync();
        }

        public async Task<bool> IsUsernameExistsAsync(string value)
        {
            return await _appUserRepo.IsRecordExistsAsync("UserName", value);
        }

        public async Task<bool> IsNICExistsAsync(string value)
        {
            return await _appUserRepo.IsRecordExistsAsync("NIC", value);
        }

        public async Task<bool> IsEmailExistsAsync(string value)
        {
            return await _appUserRepo.IsRecordExistsAsync("Email", value);
        }

        public async Task<IEnumerable<AppUser>> GetAllUsersByTypeAsync(UserRoleEnum userRoleEnum)
        {
            return await _appUserRepo.GetAllUsersByRoleAsync(userRoleEnum);
        }

        public async Task<IEnumerable<AppUser>> GetUsersByManagerAsync(int UserId)
        {
            return await _appUserRepo.GetAllUsersByManagerAsync(UserId);
        }

        public async Task<IEnumerable<AppUser>> GetUsersByManagerAndStatusAsync(int UserId, UserStatusEnum userStatusEnum)
        {
            return await _appUserRepo.GetAllUsersByManagerAndStatusAsync(UserId, userStatusEnum);
        }

        public async Task UpdateProfileAsync(int UserId, UpdateProfileDTO updateProfileDTO)
        {
            if (
                string.IsNullOrEmpty(updateProfileDTO.UserName) ||
                string.IsNullOrEmpty(updateProfileDTO.Email) ||
                string.IsNullOrEmpty(updateProfileDTO.Email)
                )
            {
                throw new ArgumentNullException(nameof(updateProfileDTO));
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
            AppUser user = await _appUserRepo.GetByPKAsync(UserId);
            AppUser userManger = await _appUserRepo.GetUserManagerAsync(UserId);
            user.Status = isApprove ? UserStatusEnum.Registered : UserStatusEnum.Banned;

            await _appUserRepo.Update(user);

            var managerName = userManger.FirstName + " " + userManger.LastName;

            _ = _notificationService.NotifyUserRegistrationProcessAsync(user.Email, managerName, isApprove);

        }

        public async Task<bool> CheckPermissionAsync(int UserId, string permission)
        {
            return await _appUserRepo.CheckPermissionAsync(UserId, permission);
        }
    }
}
