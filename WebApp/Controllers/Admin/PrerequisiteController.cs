using BusinessLayer.Services.Interfaces;
using Entities.DbModels;
using Entities.FormDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApp.Helpers;

namespace WebApp.Controllers
{
    public class PrerequisiteController : Controller
    {
        private readonly IPrerequisiteService _prerequisiteService;
        public PrerequisiteController(IPrerequisiteService prerequisiteService)
        {
            _prerequisiteService = prerequisiteService;
        }

        [AuthorizePermission("prerequisite.view")]
        public ActionResult ViewAll()
        {
            ViewBag.Prerequisites = _prerequisiteService.GetAllPrerequisites();

            return View("~/Views/Admin/Prerequisites.cshtml");
        }

        [AuthorizePermission("prerequisite.add")]
        [HttpPost]
        public ActionResult AddPost(PrerequisiteDTO prerequisite)
        {
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