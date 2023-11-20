using MainLibrary.DAL;
using MainLibrary.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApp.Controllers
{
    public class UserController : Controller
    {
        public ActionResult Login(UserLoginFormDTO user)
        {

            var userService = new UserService();

            try
            {
                userService.Login(user);
            }
            catch
            {
                TempData["login_status"] = "Failed";
            }

            return View();
        }
    }
}