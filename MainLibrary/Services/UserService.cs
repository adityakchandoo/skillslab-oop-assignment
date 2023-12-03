using MainLibrary.DTO;
using MainLibrary.Entities;
using MainLibrary.Entities.Types;
using MainLibrary.Helpers;
using MainLibrary.Repo;
using MainLibrary.Repo.Interfaces;
using MainLibrary.Services.Interfaces;
using System;
using System.Collections.Generic;


namespace MainLibrary.Service
{
    public class UserService : IUserService
    {
        private readonly IUserRepo _userRepo;
        public UserService(IUserRepo userRepo)
        {
            _userRepo = userRepo;
        }


        public IEnumerable<User> ExportSelectedEmployees()
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
            User user = _userRepo.GetUser(form.Username);

            if (PasswordHasher.VerifySHA256Hash(form.Password, user.Password))
            {
                authenticateResponse.IsLoginSuccessful = true;
                authenticateResponse.RedirectPath = "/"+user.Role.ToString();
                authenticateResponse.user = user;
            }
            else
            {
                authenticateResponse.IsLoginSuccessful = false;
            }

            return authenticateResponse;

        }
        public void Register(RegisterFormDTO reg)
        {

            User db_user = new User();
            db_user.UserId = reg.UserId;
            db_user.FirstName = reg.FirstName;
            db_user.LastName = reg.LastName;
            db_user.Email = reg.Email;
            db_user.Password = PasswordHasher.GenerateSHA256Hash(reg.Pass1);
            db_user.Email = reg.Email;
            db_user.DOB = reg.DOB;
            db_user.NIC = reg.NIC;
            db_user.MobileNumber = reg.MobileNumber;
            db_user.Status = UserStatusType.Unverified;
            db_user.Role = UserRoleType.Employee;

            _userRepo.CreateUser(db_user);

        }

        public bool IsUserIdExists(string UserId)
        {
            User user = _userRepo.GetUser(UserId);

            // User Should not exits in database
            if(user == null) { return true; } else { return false; }

        }

        public void ConfirmAccount(string UserId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<User> GetAllUsersByType(UserRoleType userRoleType)
        {
            return _userRepo.GetAllUsersByType(userRoleType);
        }

        public User GetUser(string UserId)
        {
            return _userRepo.GetUser(UserId);
        }
    }
}
