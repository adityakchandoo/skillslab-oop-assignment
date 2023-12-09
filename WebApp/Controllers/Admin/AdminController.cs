using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApp.Helpers;

namespace WebApp.Controllers.Admin
{
    //[AdminSession]
    [RoutePrefix("Admin")]
    public class AdminController : Controller
    {
        [Route("")]
        // GET: Home
        public ActionResult Index()
        {
            return View("~/Views/Admin/Dashboard.cshtml");
        }
    }
}