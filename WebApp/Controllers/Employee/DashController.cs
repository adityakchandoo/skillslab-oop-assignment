using System.Web.Mvc;
using WebApp.Helpers;

namespace WebApp.Controllers
{
    public partial class DashController : Controller
    {
        [AuthorizePermission("employee.dash")]
        public ActionResult EmployeeDash()
        {
            return RedirectToAction("ViewDash", "Training");
        }
    }
}