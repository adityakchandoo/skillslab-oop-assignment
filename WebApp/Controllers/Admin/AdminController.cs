using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApp.Controllers.Admin
{
    [RoutePrefix("Admin")]
    public class AdminController : Controller
    {
        [Route("")]
        // GET: Home
        public ActionResult Index()
        {
            return Content("Admin Home");
        }
    }
}