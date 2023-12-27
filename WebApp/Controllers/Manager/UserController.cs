using BusinessLayer.Services;
using BusinessLayer.Services.Interfaces;
using Entities.DbModels;
using Entities.FormDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebApp.Helpers;

namespace WebApp.Controllers
{
    public partial class UserController : Controller
    {
        [AuthorizePermission("user.viewallemployee")]
        public async Task<ActionResult> ViewAllEmployee()
        {
            ViewBag.Title = "Employees";
            ViewBag.Users = await _userService.GetAllUsersByTypeAsync(Entities.Enums.UserRoleEnum.Employee);
            return View("~/Views/Manager/UserTableView.cshtml");
        }

        [AuthorizePermission("user.viewsubordinates")]
        public async Task<ActionResult> ViewSubordinates()
        {
            // TODO: User Session
            //int UserId = 4;
            int UserId = (int)this.Session["UserId"];


            ViewBag.Title = "My Employees";
            ViewBag.Users = await _userService.GetUsersByManagerAsync(UserId);
            return View("~/Views/Manager/UserTableView.cshtml");
        }

        [AuthorizePermission("user.viewpendingsubordinates")]
        public async Task<ActionResult> ViewPendingSubordinates()
        {
            // TODO: User Session
            //int UserId = 4;
            int UserId = (int)this.Session["UserId"];


            ViewBag.Title = "Pending Employees";
            ViewBag.Employees = await _userService.GetUsersByManagerAndStatusAsync(UserId, Entities.Enums.UserStatusEnum.Pending);
            return View("~/Views/Manager/PendingEmployees.cshtml");
        }

        [AuthorizePermission("user.viewpendingsubordinates")]
        [HttpPost]
        public async Task<ActionResult> PendingSubordinateAction(int userId, bool approve)
        {
            await _userService.ProcessNewUserAsync(userId, approve);

            return Json(new { status = "ok" });

        }
    }
}