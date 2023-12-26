using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebApp.Helpers;

namespace WebApp.Controllers
{
    public partial class DashController : Controller
    {
        // GET: Home
        [AuthorizePermission("admin.dash")]
        public async Task<ActionResult> AdminDash()
        {
            return View("~/Views/Admin/Dashboard.cshtml");
        }
    }
}