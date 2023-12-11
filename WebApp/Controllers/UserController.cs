using System;
using System.Web.Mvc;
using Entities.FormDTO;
using Entities.DTO;
using BusinessLayer.Services.Interfaces;
using Entities.Enums;
using DataLayer.Repository.Interfaces;

namespace WebApp.Controllers
{
    [RoutePrefix("User")]
    public class UserController : Controller
    {

        private readonly IUserService _userService;
        private readonly IDepartmentService _departmentService;
        public UserController(IUserService userService, IDepartmentService departmentService)
        {
            _userService = userService;
            _departmentService = departmentService;
        }


        [Route("Login")]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [Route("Authenticate")]
        public ActionResult Authenticate(UserLoginFormDTO form)
        {
            if (!ModelState.IsValid)
            {
                Response.StatusCode = 400;
                return Json(ModelState);
            }

            try
            {
                AuthenticateResponse authResponse = _userService.AuthenticateUser(form);

                if (!authResponse.IsLoginSuccessful)
                {
                    Response.StatusCode = 400;
                    return Json(new { Error = "Invalid User/Pass" });
                }

                if (authResponse.AppUser.Role == UserRoleEnum.Employee &&
                    authResponse.AppUser.Status != UserStatusEnum.Registered)
                {
                    Response.StatusCode = 400;
                    return Json(new { Error = "Employee not comfirmed" });
                }

                this.Session["UserId"] = authResponse.AppUser.UserId;
                this.Session["Role"] = (int)authResponse.AppUser.Role;
                this.Session["Name"] = authResponse.AppUser.FirstName + " " + authResponse.AppUser.LastName;

                return Json(new { status = "ok", redirectPath = authResponse.RedirectPath });

            }
            catch (Exception ex)
            {
                Response.StatusCode = 400;
                return Json(new { Error = ex.Message });
            }

        }

        [Route("Logout")]
        public ActionResult Logout()
        {
            this.Session.Abandon();
            return new RedirectResult("~/User/Login");
        }

        [Route("Register")]
        public ActionResult Register()
        {
            ViewBag.Managers = _userService.GetAllUsersByType(UserRoleEnum.Manager);
            ViewBag.Departments = _departmentService.GetAllDepartments();
            return View();
        }

        [HttpPost]
        [Route("RegisterPost")]
        public ActionResult RegisterPost(RegisterFormDTO form)
        {
            if (!ModelState.IsValid)
            {
                Response.StatusCode = 400;
                return Json(ModelState);
            }

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
        [Route("UserIdCheck")]
        public ActionResult UserIdCheck(string UserId)
        {
            return Content(_userService.IsUserIdExists(UserId).ToString().ToLower());
        }

    }
}