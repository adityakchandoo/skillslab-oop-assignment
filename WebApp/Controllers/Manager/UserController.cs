using BusinessLayer.Services;
using BusinessLayer.Services.Interfaces;
using Entities.DbModels;
using Entities.FormDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApp.Helpers;

namespace WebApp.Controllers
{
    public partial class UserController : Controller
    {
        [AuthorizePermission("user.viewallemployee")]
        public ActionResult ViewAllEmployee()
        {
            ViewBag.Title = "Employees";
            ViewBag.Users = _userService.GetAllUsersByType(Entities.Enums.UserRoleEnum.Employee);
            return View("~/Views/Manager/UserTableView.cshtml");
        }

        [AuthorizePermission("user.viewsubordinates")]
        public ActionResult ViewSubordinates()
        {
            // TODO: User Session
            //int UserId = 4;
            int UserId = (int)this.Session["UserId"];


            ViewBag.Title = "My Employees";
            ViewBag.Users = _userService.GetUsersByManager(UserId);
            return View("~/Views/Manager/UserTableView.cshtml");
        }

        [AuthorizePermission("user.viewpendingsubordinates")]
        public ActionResult ViewPendingSubordinates()
        {
            // TODO: User Session
            //int UserId = 4;
            int UserId = (int)this.Session["UserId"];


            ViewBag.Title = "Pending Employees";
            ViewBag.Employees = _userService.GetUsersByManagerAndStatus(UserId, Entities.Enums.UserStatusEnum.Pending);
            return View("~/Views/Manager/PendingEmployees.cshtml");
        }

        [AuthorizePermission("user.viewpendingsubordinates")]
        [HttpPost]
        public ActionResult PendingSubordinateAction(int userId, bool approve)
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