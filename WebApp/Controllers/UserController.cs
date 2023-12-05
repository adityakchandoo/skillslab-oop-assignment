﻿using System;
using System.Web.Mvc;
using Entities.FormDTO;
using Entities.DTO;
using BusinessLayer.Services.Interfaces;

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

                this.Session["UserId"] = authResponse.AppUser.UserId;
                this.Session["Role"] = (int)authResponse.AppUser.Role;

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