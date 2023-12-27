using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        public async Task<ActionResult> ViewAll()
        {
            ViewBag.Trainings = await _trainingService.GetAllTrainingWithEnrollCountAsync();

            return View("~/Views/Admin/ViewTrainingTable.cshtml");
        }

        [AuthorizePermission("training.viewone")]
        public async Task<ActionResult> View(int id)
        {
            ViewBag.Training = await _trainingService.GetTrainingAsync(id);
            ViewBag.Contents = await _trainingService.GetTrainingWithContentsAsync(id);

            return View("~/Views/Shared/Training.cshtml");
        }

        [AuthorizePermission("training.add")]
        public async Task<ActionResult> Add()
        {
            ViewBag.Departments = await _departmentService.GetAllDepartmentsAsync();
            ViewBag.Prerequisites = await _prerequisiteService.GetAllPrerequisitesAsync();

            return View("~/Views/Admin/AddTraining.cshtml");
        }

        [AuthorizePermission("training.add")]
        [HttpPost]
        public async Task<ActionResult> AddPost(AddTrainingFormDTO training)
        {
            await _trainingService.AddTrainingWithTrainingPrerequisiteAsync(training);

            return Json(new { status = "ok" });
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
            _trainingService.SaveTrainingWithContentsAsync(addTrainingContentDTO);

            return Json(new { status = "ok" });
        }
    }
}