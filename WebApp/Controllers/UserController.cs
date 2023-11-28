using MainLibrary.Service;
using MainLibrary.Service.Interfaces;
using MainLibrary.DTO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MainLibrary.Entities;

namespace WebApp.Controllers
{
    public class UserController : Controller
    {

        IUserService _userService;
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
        public ActionResult LoginPost(UserLoginFormDTO login)
        {
            if(ModelState.IsValid)
            {
                try
                {
                    User targetUser = _userService.CheckLogin(login);
                    this.Session["UserId"] = targetUser.UserId;

                    if (targetUser.Role == UserRoleType.Admin)
                    {
                        this.Session["Role"] = "Admin";
                        return Redirect("/Admin");
                    }
                    else if(targetUser.Role == UserRoleType.Manager)
                    {
                        this.Session["Role"] = "Manager";
                        return Redirect("/Manager");
                    }
                    else if (targetUser.Role == UserRoleType.Employee)
                    {
                        this.Session["Role"] = "Employee";
                        return Redirect("/Employee");
                    }

                } 
                catch (Exception ex)
                {
                    TempData["Error"] = ex.Message;
                }
                
            }

            return RedirectToAction("Login");
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult RegisterPost(RegisterFormDTO reg)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _userService.Register(reg);
                }
                catch (Exception ex)
                {
                    TempData["Error"] = ex.Message;
                }
            }

            return RedirectToAction("RegisterSuccess");
        }

        public ActionResult RegisterSuccess()
        {
            return View();
        }

    }
}