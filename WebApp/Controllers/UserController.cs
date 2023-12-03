using MainLibrary.DTO;
using System;
using System.Web.Mvc;
using MainLibrary.Entities;
using WebApp.Helpers;
using MainLibrary.Services.Interfaces;

namespace WebApp.Controllers
{
    public class UserController : Controller
    {

        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }


        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
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

                this.Session["UserId"] = authResponse.user.UserId;
                this.Session["Role"] = (int)authResponse.user.Role;

                return Json(new { status = "ok", redirectPath = authResponse.RedirectPath });

            }
            catch (Exception ex)
            {
                Response.StatusCode = 400;
                return Json(new { Error = ex.Message });
            }

        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
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
        public ActionResult UserIdCheck(string UserId)
        {
            return Content(_userService.IsUserIdExists(UserId).ToString().ToLower());
        }

    }
}