using System.Web.Mvc;
using WebApp.Helpers;

namespace WebApp.Controllers
{
    public partial class DashController : Controller
    {
        // GET: Home
        [AuthorizePermission("admin.dash")]
        public ActionResult AdminDash()
        {
            return RedirectToAction("ViewAll", "Training");
        }
    }
}