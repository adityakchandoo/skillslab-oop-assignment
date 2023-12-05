using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BusinessLayer.Services.Interfaces;
using Entities.Enums;
using Entities.FormDTO;
using WebApp.Helpers;

namespace WebApp.Controllers.Admin
{
    [RoutePrefix("Admin")]
    //[AdminSession]
    public class TrainingController : Controller
    {
        private readonly ITrainingService _trainingService;
        private readonly IDepartmentService _departmentService;
        private readonly IUserService _userService;
        private readonly IPrerequisiteService _prerequisitService;

        public TrainingController(ITrainingService trainingService, IDepartmentService departmentService, IUserService userService, IPrerequisiteService prerequisitService)
        {
            _trainingService = trainingService;
            _departmentService = departmentService;
            _userService = userService;
            _prerequisitService = prerequisitService;
        }


        // GET: Training
        [Route("ViewTrainings")]
        public ActionResult ViewTrainings()
        {
            ViewBag.PageTag = "train-manage";

            ViewBag.Trainings = _trainingService.GetAllTraining();

            return View("~/Views/Admin/ViewTrainings.cshtml");
        }

        [Route("AddTraining")]
        public ActionResult AddTraining()
        {
            ViewBag.PageTag = "train-add";

            ViewBag.Departments = _departmentService.GetAllDepartments();
            ViewBag.Managers = _userService.GetAllUsersByType(UserRoleEnum.Manager);
            ViewBag.Prerequisites = _prerequisitService.GetAllPrerequisites();

            return View("~/Views/Admin/AddTraining.cshtml");
        }

        [Route("AddTrainingPost")]
        [HttpPost]
        public ActionResult AddTrainingPost(TrainingDTO training)
        {
            try
            {
                _trainingService.AddTrainingAndTrainingPrerequisite(training);

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