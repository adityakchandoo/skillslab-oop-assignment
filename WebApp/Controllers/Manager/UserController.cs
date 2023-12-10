using BusinessLayer.Services;
using BusinessLayer.Services.Interfaces;
using Entities.DbModels;
using Entities.FormDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApp.Controllers.Manager
{
    [RoutePrefix("Manager")]
    public class UserController : Controller
    {
        IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        // GET: User
        public ActionResult Index()
        {
            return View();
        }

        [Route("AllEmployees")]
        public ActionResult AllEmployees()
        {
            ViewBag.Title = "All Employees";
            ViewBag.Users = _userService.GetAllUsersByType(Entities.Enums.UserRoleEnum.Employee);
            return View("~/Views/Manager/UserTableView.cshtml");
        }

        [Route("MyEmployees")]
        public ActionResult MyEmployees()
        {
            // TODO: User Session
            const string UserId = "aditya";
            //const string UserId = (string)this.Session["UserId"];

            ViewBag.Title = "My Employees";
            ViewBag.Users = _userService.GetUsersByManager(UserId);
            return View("~/Views/Manager/UserTableView.cshtml");
        }

        [Route("PendingEmployees")]
        public ActionResult PendingEmployees()
        {
            // TODO: User Session
            const string UserId = "aditya";
            //const string UserId = (string)this.Session["UserId"];

            ViewBag.Title = "Pending Employees";
            ViewBag.Employees = _userService.GetUsersByManagerAndStatus(UserId, Entities.Enums.UserStatusEnum.Pending);
            return View("~/Views/Manager/PendingEmployees.cshtml");
        }

        [Route("PendingEmployeeAction")]
        [HttpPost]
        public ActionResult PendingEmployeeAction(string userId, bool approve)
        {
            try
            {
                _userService.ProcessNewUser(userId, approve);

                return Json(new { status = "ok" });

            }
            catch (Exception ex)
            {
                Response.StatusCode = 400;
                return Json(new { Error = ex.Message });
            }

        }
    }
}