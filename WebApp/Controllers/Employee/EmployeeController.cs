using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApp.Helpers;

namespace WebApp.Controllers.Employee
{
    [EmployeeSession]
    [RoutePrefix("Employee")]
    public class EmployeeController : Controller
    {

        [Route("")]
        // GET: Home
        public ActionResult Index()
        {
            return View("~/Views/Employee/Dashboard.cshtml");
        }
    }
}