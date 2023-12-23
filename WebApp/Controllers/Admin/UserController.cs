using BusinessLayer.Services.Interfaces;
using Entities.DbCustom;
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
        // GET: User
        [AuthorizePermission("user.viewall")]
        public ActionResult ViewAll()
        {
            ViewBag.Title = "Manage All Users";
            ViewBag.Users = _userService.GetAllUsersWithInlineRoles();
            return View("~/Views/Admin/AllUserWithRoles.cshtml");
        }

        public ActionResult GetUserRoles(int id)
        {
            return Json(_userRoleService.GetUserRolesAssigned(id), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Route("User/EditUserRole/{id}")]
        public ActionResult EditUserRoles(int id, List<UserRoleAssigned> newUserRolesAssigned)
        {
            _userRoleService.EditUserRoles(id, newUserRolesAssigned.ToArray());

            return Json(new { Ok = "ok" }, JsonRequestBehavior.AllowGet);
        }


    }
}