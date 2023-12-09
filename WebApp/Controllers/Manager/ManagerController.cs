using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApp.Helpers;

namespace WebApp.Controllers.Manager
{    
    //[ManagerSession]
    [RoutePrefix("Manager")]
    public class ManagerController : Controller
    {
        [Route("")]
        // GET: Home
        public ActionResult Index()
        {
            return View("~/Views/Manager/Dashboard.cshtml");

        }
    }
}