using System;
using System.Web.Mvc;
using Entities.FormDTO;
using Entities.DTO;
using BusinessLayer.Services.Interfaces;
using Entities.Enums;
using DataLayer.Repository.Interfaces;
using WebApp.Helpers;
using BusinessLayer.Services;
using System.Web;
using System.Net;
using System.Collections.Generic;
using Entities.DbCustom;
using System.Linq;
using Entities.DbModels;

namespace WebApp.Controllers
{
    public partial class UserController : Controller
    {

        private readonly IUserService _userService;
        private readonly IUserRoleService _userRoleService;
        private readonly IDepartmentService _departmentService;
        public UserController(IUserService userService, IDepartmentService departmentService, IUserRoleService userRoleService)
        {
            _userService = userService;
            _departmentService = departmentService;
            _userRoleService = userRoleService;
        }


        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Authenticate(UserLoginFormDTO form)
        {
            try
            {
                AuthenticateResponse authResponse = _userService.AuthenticateUser(form);

                if (!authResponse.IsLoginSuccessful)
                {
                    Response.StatusCode = 400;
                    return Json(new { authResponse.Error });
                }                

                this.Session["UserId"] = authResponse.AppUser.UserId;                
                this.Session["Name"] = authResponse.AppUser.FirstName + " " + authResponse.AppUser.LastName;

                return Json(new { status = "ok", redirectPath = "/User/SelectRole" });

            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(new { Error = ex.Message });
            }

        }

        public ActionResult SelectRole()
        {
            if (this.Session["UserId"] == null)
                return RedirectToAction("Login");

            IEnumerable<AppUserRole> userRoles = _userService.GetRolesByUserId((int)this.Session["UserId"]);

            if (userRoles.Count() == 1)
            {
                this.Session["Role"] = userRoles.First().RoleId;
                return new RedirectResult($"~/Dash/{userRoles.First().RoleName}Dash");
            }
            else
            {
                ViewBag.Roles = userRoles;
                return View();
            }
        }

        [HttpPost]
        public ActionResult SelectRolePost(AppUserRole appUserRole)
        {
            this.Session["Role"] = appUserRole.RoleId;
            return new RedirectResult($"~/Dash/{appUserRole.RoleName}Dash");
        }

        public ActionResult Logout()
        {
            this.Session.Abandon();
            return new RedirectResult("~/User/Login");
        }

        public ActionResult Register()
        {
            ViewBag.Managers = _userService.GetAllUsersByType(UserRoleEnum.Manager);
            ViewBag.Departments = _departmentService.GetAllDepartments();
            return View();
        }

        [HttpPost]
        public ActionResult RegisterPost(RegisterFormDTO form)
        {
            try
            {
                _userService.Register(form);
                return Json(new { status = "ok" });
            }

            catch (Exception ex)
            {
                Response.StatusCode = 400;
                return Json(new { Error = ex.Message });
            }

        }

        [HttpPost]
        public ActionResult UsernameCheck(string Username)
        {
            return Content((!_userService.IsUsernameExists(Username)).ToString().ToLower());
        }

        [HttpPost]
        public ActionResult EmailCheck(string Email)
        {
            return Content((!_userService.IsEmailExists(Email)).ToString().ToLower());
        }

        [HttpPost]
        public ActionResult NicCheck(string NIC)
        {
            return Content((!_userService.IsNICExists(NIC)).ToString().ToLower());
        }


        [AuthorizePermission("user.profile")]
        public ActionResult MyProfile()
        {
            ViewBag.User = _userService.GetUser((int)this.Session["UserId"]);
            return View();
        }

        [AuthorizePermission("user.profile")]
        [HttpPost]
        public ActionResult UpdateProfile(UpdateProfileDTO updateProfileDTO)
        {
            

            try
            {
                _userService.UpdateProfile(
                    (int)this.Session["UserId"],
                    updateProfileDTO
                );
                return Json(new { status = "ok" });
            }

            catch (Exception ex)
            {
                Response.StatusCode = 400;
                return Json(new { Error = ex.Message });
            }

        }

        [AuthorizePermission("user.profile")]
        [HttpPost]
        public ActionResult UpdatePassword(UpdatePasswordDTO updatePasswordDTO)
        {
            _userService.UpdatePassword(
                    (int)this.Session["UserId"],
                    updatePasswordDTO
                );

            return Json(new { status = "ok" });
        }

    }
}