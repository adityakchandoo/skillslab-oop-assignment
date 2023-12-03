using MainLibrary.DTO;
using MainLibrary.Entities;
using MainLibrary.Services;
using MainLibrary.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApp.Controllers.Admin
{
    [RoutePrefix("Admin")]
    public class PrerequisiteController : Controller
    {
        private readonly IPrerequisiteService _prerequisiteService;
        public PrerequisiteController(IPrerequisiteService prerequisiteService)
        {
            _prerequisiteService = prerequisiteService;
        }

        [Route("Prerequisites")]
        public ActionResult Index()
        {

            ViewBag.PageTag = "Prerequisite";

            ViewBag.Prerequisites = _prerequisiteService.GetAllPrerequisites();

            return View("~/Views/Admin/Prerequisites.cshtml");
        }

        [Route("AddPrerequisite")]
        public ActionResult AddDepartment(PrerequisiteDTO prerequisite)
        {
            if (!ModelState.IsValid)
            {
                Response.StatusCode = 400;
                return Json(ModelState);
            }

            try
            {
                _prerequisiteService.AddPrerequisite(prerequisite);

                return Json(new { status = "ok" });

            }
            catch (Exception ex)
            {
                Response.StatusCode = 400;
                return Json(new { Error = ex.Message });
            }
        }
    }
}