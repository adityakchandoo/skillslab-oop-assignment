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
        private readonly AppUserRepo _appUserRepo;
        public UserService(AppUserRepo appUserRepo)
        {
            _appUserRepo = appUserRepo;
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
                db_user.Status = UserStatusEnum.Unverified;
                db_user.Role = UserRoleEnum.Employee;

                _appUserRepo.Insert(db_user);

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
        public IEnumerable<AppUser> GetUsersManagedBy(string UserId)
        {
            return _appUserRepo.GetUsersManagedBy(UserId);
        }
    }
}
