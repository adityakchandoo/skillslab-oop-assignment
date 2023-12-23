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

namespace WebApp.Controllers
{
    public partial class TrainingController : Controller
    {
        private readonly ITrainingService _trainingService;
        private readonly IUserTrainingEnrollmentService _userTrainingEnrollmentService;
        private readonly IUserService _userService;
        private readonly IPrerequisiteService _prerequisiteService;
        private readonly IDepartmentService _departmentService;

        public TrainingController(
            ITrainingService trainingService,
            IUserTrainingEnrollmentService userTrainingEnrollmentService,
            IUserService userService,
            IPrerequisiteService prerequisiteService,
            IDepartmentService departmentService)
        {
            _trainingService = trainingService;
            _userTrainingEnrollmentService = userTrainingEnrollmentService;
            _userService = userService;
            _prerequisiteService = prerequisiteService;
            _departmentService = departmentService;
        }


        // GET: Training
        [AuthorizePermission("training.viewall")]
        public ActionResult ViewAll()
        {
            ViewBag.Trainings = _trainingService.GetAllTrainingWithEnrollCount();

            return View("~/Views/Admin/ViewTrainingTable.cshtml");
        }

        [AuthorizePermission("training.viewone")]
        public ActionResult View(int id)
        {
            ViewBag.Training = _trainingService.GetTraining(id);
            ViewBag.Contents = _trainingService.GetTrainingWithContents(id);

            return View("~/Views/Shared/Training.cshtml");
        }

        [AuthorizePermission("training.add")]
        public ActionResult Add()
        {
            ViewBag.Departments = _departmentService.GetAllDepartments();
            ViewBag.Prerequisites = _prerequisiteService.GetAllPrerequisites();

            return View("~/Views/Admin/AddTraining.cshtml");
        }

        [AuthorizePermission("training.add")]
        [HttpPost]
        public ActionResult AddPost(AddTrainingFormDTO training)
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

        [AuthorizePermission("training.addcontent")]
        public ActionResult AddContent(int id)
        {
            ViewBag.trainingId = id;
            return View("~/Views/Admin/AddTrainingContent.cshtml");
        }

        [AuthorizePermission("training.addcontent")]
        [HttpPost]
        public ActionResult AddContentPost(AddTrainingContentDTO addTrainingContentDTO)
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