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
using System.Reflection;
using MainLibrary.Repo.Interfaces;
using System.Data.SqlClient;
using MainLibrary.Services;

namespace WebApp.Controllers
{
    public class UserController : Controller
    {

        IUserService _userService;
        IUserRepo _userrepo;
        public UserController(IUserService userService, IUserRepo userRepo)
        {
            _userService = userService;
            _userrepo = userRepo;
        }


        [HttpGet]
        public ActionResult Login()
        {
            //            return Content(_userrepo.GetUser("mcb").Email);

            return View();
        }

        [HttpPost]
        public ActionResult LoginPost(UserLoginFormDTO form)
        {
            if(ModelState.IsValid)
            {
                try
                {
                    User user;
                    bool loginOK = _userService.CheckLogin(form, out user);

                    if(!loginOK)
                    {
                        TempData["Error"] = "Login/Password Error";
                        return View("Login");
                    }

                    this.Session["UserId"] = user.UserId;

                    if (user.Role == UserRoleType.Admin)
                    {
                        this.Session["Role"] = "Admin";
                        return Redirect("/Admin");
                    }
                    else if(user.Role == UserRoleType.Manager)
                    {
                        this.Session["Role"] = "Manager";
                        return Redirect("/Manager");
                    }
                    else if (user.Role == UserRoleType.Employee)
                    {
                        this.Session["Role"] = "Employee";
                        return Redirect("/Employee");
                    }

                } 
                catch (Exception ex)
                {
                    TempData["Error"] = ex.Message;
                    return View("SiteInfo");
                }
                
            }

            return RedirectToAction("Login");
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult RegisterPost(RegisterFormDTO form)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _userService.Register(form);
                    TempData["Success"] = "User Successfully Registered";
                    return View("SiteInfo");
                }
                
                catch (Exception ex)
                {
                    TempData["Error"] = ex.Message;
                    return View("SiteInfo");
                }
            }
            return View("Register", form);
        }

        public ActionResult Test()
        {
            var testService = new TestService();


            testService.DoSomething();

            return Content("");
        }

    }
}