using BusinessLayer.Services.Interfaces;
using Entities.DbModels;
using Entities.FormDTO;
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
                Prerequisite prerequisite_db = new Prerequisite()
                {
                    Name = prerequisite.Name,
                    Description = prerequisite.Description

                };

                _prerequisiteService.AddPrerequisite(prerequisite_db);

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