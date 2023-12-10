using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BusinessLayer.Services.Interfaces;
using Entities.DbModels;
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
        private readonly IPrerequisiteService _prerequisitService;

        public TrainingController(ITrainingService trainingService, IDepartmentService departmentService, IPrerequisiteService prerequisitService)
        {
            _trainingService = trainingService;
            _departmentService = departmentService;
            _prerequisitService = prerequisitService;
        }


        // GET: Training
        [Route("ViewTrainings")]
        public ActionResult ViewTrainings()
        {
            ViewBag.Trainings = _trainingService.GetAllTrainingDetails();

            return View("~/Views/Admin/ViewTrainings.cshtml");
        }

        [Route("Training/{trainingId}")]
        public ActionResult Training(int trainingId)
        {
            ViewBag.Training = _trainingService.GetTraining(trainingId);
            ViewBag.Contents = _trainingService.GetTrainingWithContents(trainingId);

            return View("~/Views/Admin/AdminTraining.cshtml");
        }

        [Route("AddTraining")]
        public ActionResult AddTraining()
        {
            ViewBag.Departments = _departmentService.GetAllDepartments();
            ViewBag.Prerequisites = _prerequisitService.GetAllPrerequisites();

            return View("~/Views/Admin/AddTraining.cshtml");
        }

        [Route("AddTrainingPost")]
        [HttpPost]
        public ActionResult AddTrainingPost(AddTrainingFormDTO training)
        {
            try
            {
                _trainingService.AddTrainingWithTrainingPrerequisite(training);

                return Json(new { status = "ok" });

            }
            catch (Exception ex)
            {
                Response.StatusCode = 400;
                return Json(new { Error = ex.Message });
            }
        }

        [Route("AddTrainingContent/{trainingId}")]
        public ActionResult AddTrainingContent(int trainingId)
        {
            ViewBag.trainingId = trainingId;
            return View("~/Views/Admin/AddTrainingContent.cshtml");
        }

        [Route("AddTrainingContentPost")]
        [HttpPost]
        public ActionResult AddTrainingContentPost(AddTrainingContentDTO addTrainingContentDTO)
        {
            try
            {
                _trainingService.SaveTrainingWithContents(addTrainingContentDTO);

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