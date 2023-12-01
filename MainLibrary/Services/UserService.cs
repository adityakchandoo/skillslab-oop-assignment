using MainLibrary.DTO;
using MainLibrary.Entities;
using MainLibrary.Entities.Types;
using MainLibrary.Helpers;
using MainLibrary.Repo;
using MainLibrary.Repo.Interfaces;
using MainLibrary.Service.Interfaces;
using System;
using System.Collections.Generic;


namespace MainLibrary.Service
{
    public class UserService : IUserService
    {
        IUserRepo _userRepo;
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

        public bool CheckLogin(UserLoginFormDTO form, out User user)
        {
            user = _userRepo.GetUser(form.user);

            if (user == null) { return false; }

            if (PasswordHasher.VerifyPassword(form.pass, user.Password))
            {
                return true;
            }
            return false;
        }

        public void Register(RegisterFormDTO reg)
        {

            User db_user = new User();
            db_user.UserId = reg.UserId;
            db_user.FirstName = reg.FirstName;
            db_user.LastName = reg.LastName;
            db_user.Email = reg.Email;
            db_user.Password = PasswordHasher.HashPassword(reg.Pass1);
            db_user.Email = reg.Email;
            db_user.DOB = reg.DOB;
            db_user.NIC = reg.NIC;
            db_user.MobileNumber = reg.MobileNumber;
            db_user.Status = UserStatusType.Unverified;
            db_user.Role = UserRoleType.Employee;

            _userRepo.CreateUser(db_user);

        }


        public void ConfirmAccount(string UserId)
        {
            throw new NotImplementedException();
        }
    }
}
