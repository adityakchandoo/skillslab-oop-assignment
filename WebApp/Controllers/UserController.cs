using MainLibrary.DAL;
using MainLibrary.DAL.Interfaces;
using MainLibrary.DTO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApp.Controllers
{
    public class UserController : Controller
    {

        UserRepo _userDAL;
        public UserController(UserRepo userDAL)
        {
            _userDAL = userDAL;
        }

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
        public ActionResult Res()
        {

            string output = JsonConvert.SerializeObject(_userDAL.GetAllUsers());

            return Content(output);
        }
    }
}