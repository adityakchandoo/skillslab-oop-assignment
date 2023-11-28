using MainLibrary.DTO;
using MainLibrary.Entities;
using MainLibrary.Entities.Types;
using MainLibrary.Repo;
using MainLibrary.Repo.Interfaces;
using MainLibrary.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MainLibrary.Service
{
    public class UserService : IUserService
    {
        IUserRepo _userRepo;
        public UserService(UserRepo userRepo)
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

        public User CheckLogin(UserLoginFormDTO user)
        {
            var targetUser = _userRepo.GetUser(user.user);          

            if (targetUser != null)
            {
                throw new Exception();
            }

            if (targetUser.Password != user.pass)
            {
                throw new Exception();
            }

            return targetUser;

        }

        public void Register(RegisterFormDTO reg)
        {

            User db_user = new User();
            db_user.UserId = reg.UserId;
            db_user.FirstName = reg.FirstName;
            db_user.LastName = reg.LastName;
            db_user.Email = reg.Email;
            db_user.Password = reg.Password;
            db_user.Email = reg.Email;
            db_user.DOB = reg.DOB;
            db_user.NIC = reg.NIC;
            db_user.MobileNumber = reg.MobileNumber;
            db_user.Status = UserStatusType.Registered;
            db_user.Role = UserRoleType.Employee;

            _userRepo.CreateUser(db_user);

        }


        public void ConfirmAccount(string UserId)
        {
            throw new NotImplementedException();
        }
    }
}
