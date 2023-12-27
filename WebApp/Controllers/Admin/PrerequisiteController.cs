using BusinessLayer.Services.Interfaces;
using Entities.DbModels;
using Entities.FormDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        public async Task<ActionResult> ViewAll()
        {
            ViewBag.Prerequisites = await _prerequisiteService.GetAllPrerequisitesAsync();

            return View("~/Views/Admin/Prerequisites.cshtml");
        }

        [AuthorizePermission("prerequisite.add")]
        [HttpPost]
        public async Task<ActionResult> AddPost(PrerequisiteDTO prerequisite)
        {
            Prerequisite prerequisite_db = new Prerequisite()
            {
                Name = prerequisite.Name,
                Description = prerequisite.Description

            };

            await _prerequisiteService.AddPrerequisiteAsync(prerequisite_db);

            return Json(new { status = "ok" });
        }
    }
}