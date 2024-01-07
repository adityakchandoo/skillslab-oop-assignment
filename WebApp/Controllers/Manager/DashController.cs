using System.Web.Mvc;
using WebApp.Helpers;

namespace WebApp.Controllers
{

    public partial class DashController : Controller
    {
        [AuthorizePermission("manager.dash")]
        public ActionResult ManagerDash()
        {
            return RedirectToAction("ViewRequests", "Training");
        }
    }
}