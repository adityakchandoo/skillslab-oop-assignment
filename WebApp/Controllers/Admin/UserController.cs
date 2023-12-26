using BusinessLayer.Services.Interfaces;
using Entities.DbCustom;
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
        // GET: User
        [AuthorizePermission("user.viewall")]
        public async Task<ActionResult> ViewAll()
        {
            ViewBag.Title = "Manage All Users";
            ViewBag.Users = await _userService.GetAllUsersWithInlineRolesAsync();
            return View("~/Views/Admin/AllUserWithRoles.cshtml");
        }

        public async Task<ActionResult> GetUserRoles(int id)
        {
            // TODO: Check if used anyware
            return Json((await _userRoleService.GetUserRolesAssignedAsync(id)), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Route("User/EditUserRole/{id}")]
        public async Task<ActionResult> EditUserRoles(int id, List<UserRoleAssigned> newUserRolesAssigned)
        {
            await _userRoleService.EditUserRolesAsync(id, newUserRolesAssigned.ToArray());

            return Json(new { Ok = "ok" }, JsonRequestBehavior.AllowGet);
        }


    }
}