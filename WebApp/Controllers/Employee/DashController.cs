using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApp.Helpers;

namespace WebApp.Controllers
{
    public partial class DashController : Controller
    {
        [AuthorizePermission("employee.dash")]
        public ActionResult EmployeeDash()
        {
            return View("~/Views/Employee/Dashboard.cshtml");
        }
    }
}