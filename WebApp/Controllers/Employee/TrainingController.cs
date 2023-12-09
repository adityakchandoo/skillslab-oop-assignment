using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Mvc;
using BusinessLayer.Services;
using BusinessLayer.Services.Interfaces;
using Entities.DbModels;
using Entities.DTO;
using Entities.Enums;
using Entities.FormDTO;
using WebApp.Helpers;

namespace WebApp.Controllers.Employee
{
    [RoutePrefix("Employee")]
    //[EmployeeSession]
    public class TrainingController : Controller
    {
        private readonly ITrainingService _trainingService;
        private readonly IUserTrainingEnrollmentService _userTrainingEnrollmentService;
        private readonly IUserService _userService;
        private readonly IPrerequisiteService _prerequisiteService;

        public TrainingController(
            ITrainingService trainingService, 
            IUserTrainingEnrollmentService userTrainingEnrollmentService, 
            IUserService userService, 
            IPrerequisiteService prerequisiteService)
        {
            _trainingService = trainingService;
            _userTrainingEnrollmentService = userTrainingEnrollmentService;
            _userService = userService;
            _prerequisiteService = prerequisiteService;
        }


        // GET: Training
        [Route("MyTrainings")]
        public ActionResult MyTrainings()
        {
            try
            {
                const string UserId = "aditya";// (string)this.Session["UserId"];
                ViewBag.Trainings = _trainingService.GetTrainingEnrolledByUser(UserId);

                return View("~/Views/Employee/MyTrainings.cshtml");
            }
            catch (Exception ex)
            {
                Response.StatusCode = 400;
                ViewBag.Error = ex.Message;
                return View("~/Views/Shared/GlobalErrorDisplayPage.cshtml");
            }

        }

        [Route("Training/{Id}")]
        public ActionResult Training(int Id)
        {
            try
            {
                return Content(Id.ToString());
            }
            catch (Exception ex)
            {
                Response.StatusCode = 400;
                ViewBag.Error = ex.Message;
                return View("~/Views/Shared/GlobalErrorDisplayPage.cshtml");
            }

        }

        [Route("SearchTrainings")]
        public ActionResult SearchTrainings()
        {
            ViewBag.PageTag = "train-manage";

            try
            {
                const string UserId = "aditya";// (string)this.Session["UserId"];
                ViewBag.Trainings = _trainingService.GetAllTraining();

                return View("~/Views/Employee/SearchTrainings.cshtml");

            }
            catch (Exception ex)
            {
                Response.StatusCode = 400;
                ViewBag.Error = ex.Message;
                return View("~/Views/Shared/GlobalErrorDisplayPage.cshtml");
            }
        }

        [Route("ApplyTraining/{trainingId}")]
        public ActionResult ApplyTraining(int trainingId)
        {

            ViewBag.trainingId = trainingId;
            ViewBag.Training = _trainingService.GetTraining(trainingId);
            ViewBag.Prerequisites = _prerequisiteService.GetPrerequisitesByTraining(trainingId);

            return View("~/Views/Employee/ApplyTraining.cshtml");
        }

        [Route("ApplyTrainingPost")]
        [HttpPost]
        public ActionResult ApplyTrainingPost(int trainingId)
        {
            List<UploadFileStore> uploadFiles = new List<UploadFileStore>();

            try
            {
                const string UserId = "aditya";// (string)this.Session["UserId"];

                foreach (string key in Request.Files.AllKeys)
                {
                    uploadFiles.Add(new UploadFileStore() { FileId = int.Parse(key), FileName = Request.Files.Get(key).FileName, FileContent = Request.Files.Get(key).InputStream }); ;
                }

                _trainingService.ApplyTraining(UserId, trainingId, uploadFiles);

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