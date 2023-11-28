using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApp.Controllers.Admin
{
    public class DashController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            ViewBag.PageTag = "train-manage";




            return View("~/Views/Admin/Dash.cshtml");
        }
    }
}