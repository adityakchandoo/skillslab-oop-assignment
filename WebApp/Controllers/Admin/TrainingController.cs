using BusinessLayer.Services.Interfaces;
using Entities.Enums;
using Entities.FormDTO;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Web.Mvc;
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
        private readonly IStorageService _storageService;

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
            int UserId = (int)this.Session["UserId"];

            // To Check if user has already applied
            var enrollement = await _userTrainingEnrollmentService.GetUserTrainingEnrollmentAsync(UserId, id);

            if ((int)this.Session["Role"] != (int)UserRoleEnum.Admin && enrollement == null)
            {
                throw new ArgumentException("Not Applied!");
            }

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

        //[AuthorizePermission("training.edit")]
        public async Task<ActionResult> Edit(int id)
        {
            ViewBag.TrainingId = id;
            ViewBag.Training = await _trainingService.GetTrainingAsync(id);

            ViewBag.Departments = await _departmentService.GetAllDepartmentsAsync();
            ViewBag.Prerequisites = await _prerequisiteService.GetAllPrerequisitesByTrainingAsync(id);

            return View("~/Views/Admin/EditTraining.cshtml");
        }

        public async Task<ActionResult> EditPost(AddTrainingFormDTO training)
        {
            await _trainingService.EditTrainingAsync(training);

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
        public async Task<ActionResult> AddContentPost(AddTrainingContentDTO addTrainingContentDTO)
        {
            await _trainingService.SaveTrainingContentWithAttachmentAsync(addTrainingContentDTO);

            return Json(new { status = "ok" });
        }

        [AuthorizePermission("training.exportemp")]
        public async Task<ActionResult> ExportEmployees(int id)
        {
            Stream csv = await _trainingService.ExportSelectedEmployeesAsync(id);

            return File(csv, "text/csv", $"Exported_Employees{DateTime.Now.ToString("yyyyMMdd")}_{id}.csv");
        }

        [AuthorizePermission("training.deltraining")]
        public async Task<ActionResult> DelTraining(int id)
        {
            await _trainingService.SoftDeleteTrainingAsync(id);

            return Redirect(Request.UrlReferrer.ToString());
        }

        [AuthorizePermission("training.deltrainingcontent")]
        public async Task<ActionResult> DelTrainingContent(int id)
        {
            await _trainingService.SoftDeleteTrainingContentAsync(id);

            return Redirect(Request.UrlReferrer.ToString());
        }

        [AuthorizePermission("training.autoprocess")]
        public async Task<ActionResult> AutoProcess()
        {
            await _trainingService.AutoProcess();

            return Json(new { status = "ok" }, JsonRequestBehavior.AllowGet);
        }



    }
}