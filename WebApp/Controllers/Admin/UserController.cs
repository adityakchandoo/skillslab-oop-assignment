using BusinessLayer.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApp.Helpers;

namespace WebApp.Controllers.Admin
{
    [AdminSession]
    [RoutePrefix("Admin")]
    public class UserController : Controller
    {
        IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        // GET: User
        [Route("ViewAdmins")]
        public ActionResult ViewAdmins()
        {
            ViewBag.Title = "Admins";
            ViewBag.Users = _userService.GetAllUsersByType(Entities.Enums.UserRoleEnum.Admin);
            return View("~/Views/Admin/UserTableView.cshtml");
        }

        [Route("ViewManagers")]
        public ActionResult ViewManagers()
        {
            ViewBag.Title = "Managers";
            ViewBag.Users = _userService.GetAllUsersByType(Entities.Enums.UserRoleEnum.Manager);
            return View("~/Views/Admin/UserTableView.cshtml");
        }

        [Route("ViewEmployees")]
        public ActionResult ViewEmployees()
        {
            ViewBag.Title = "Employees";
            ViewBag.Users = _userService.GetAllUsersByType(Entities.Enums.UserRoleEnum.Employee);
            return View("~/Views/Admin/UserTableView.cshtml");
        }
    }
}