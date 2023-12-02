using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApp.Helpers;

namespace WebApp.Controllers.Manager
{    
    [RoutePrefix("Manager")]
    [ManagerSession]
    public class ManagerController : Controller
    {
        [Route("")]
        // GET: Home
        public ActionResult Index()
        {
            return Content("Manager Home");

        }
    }
}