﻿using BusinessLayer.Services.Interfaces;
using Entities.DbCustom;
using Entities.DTO;
using Entities.Enums;
using Entities.FormDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using WebApp.Helpers;

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
            this.Session.Abandon();
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Authenticate(UserLoginFormDTO form)
        {
            AuthenticateResponse authResponse = await _userService.AuthenticateUserAsync(form);

            if (!authResponse.IsLoginSuccessful)
            {
                Response.StatusCode = 400;
                return Json(new { authResponse.Error });
            }

            this.Session["UserId"] = authResponse.AppUser.UserId;
            this.Session["Name"] = authResponse.AppUser.FirstName + " " + authResponse.AppUser.LastName;

            return Json(new { status = "ok", redirectPath = "/User/SelectRole" });

        }

        public async Task<ActionResult> SelectRole()
        {
            if (this.Session["UserId"] == null)
                return RedirectToAction("Login");

            IEnumerable<AppUserRole> userRoles = await _userService.GetRolesByUserIdAsync((int)this.Session["UserId"]);

            if (userRoles.Count() == 0)
            {
                throw new ArgumentException("User has no role");

            }
            else if (userRoles.Count() == 1)
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
            return Json(new { status = "ok", redirectPath = $"/Dash/{appUserRole.RoleName}Dash" });
        }

        public ActionResult Logout()
        {
            this.Session.Abandon();
            return new RedirectResult("~/User/Login");
        }

        public async Task<ActionResult> Register()
        {
            ViewBag.Managers = await _userService.GetAllUsersByTypeAsync(UserRoleEnum.Manager);
            ViewBag.Departments = await _departmentService.GetAllDepartmentsAsync();
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> RegisterPost(RegisterFormDTO form)
        {
            await _userService.RegisterAsync(form);
            return Json(new { status = "ok" });
        }

        [HttpPost]
        public async Task<ActionResult> UsernameCheck(string Username)
        {
            bool result = await _userService.IsUsernameExistsAsync(Username);
            return Content((!result).ToString().ToLower());
        }

        [HttpPost]
        public async Task<ActionResult> EmailCheck(string Email)
        {
            bool result = await _userService.IsEmailExistsAsync(Email);
            return Content((!result).ToString().ToLower());
        }

        [HttpPost]
        public async Task<ActionResult> NicCheck(string NIC)
        {
            bool result = await _userService.IsNICExistsAsync(NIC);
            return Content((!result).ToString().ToLower());
        }


        [AuthorizePermission("user.profile")]
        public async Task<ActionResult> MyProfile()
        {
            int UserId = (int)this.Session["UserId"];
            ViewBag.User = await _userService.GetUserAsync(UserId);
            ViewBag.Manager = await _userService.GetUserManagerAsync(UserId);
            return View();
        }

        [AuthorizePermission("user.profile")]
        [HttpPost]
        public async Task<ActionResult> UpdateProfile(UpdateProfileDTO updateProfileDTO)
        {
            await _userService.UpdateProfileAsync(
                    (int)this.Session["UserId"],
                    updateProfileDTO
                );
            return Json(new { status = "ok" });
        }

        [AuthorizePermission("user.profile")]
        [HttpPost]
        public async Task<ActionResult> UpdatePassword(UpdatePasswordDTO updatePasswordDTO)
        {
            await _userService.UpdatePasswordAsync(
                    (int)this.Session["UserId"],
                    updatePasswordDTO
                );

            return Json(new { status = "ok" });
        }

        public ActionResult Zero()
        {
            var zero = 0;
            var sdf = 3 / zero;
            return Content("ok");
        }

        public async Task<ActionResult> Test()
        {
            await _userService.TestAsync();
            return Content("ok");
        }

    }
}