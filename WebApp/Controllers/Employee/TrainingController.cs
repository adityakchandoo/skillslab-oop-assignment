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
    [EmployeeSession]
    [RoutePrefix("Employee")]
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
            // TODO: User Session
            //string UserId = 1;
            int UserId = (int)this.Session["UserId"];

            try
            {

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

        [Route("Training/{trainingId}")]
        public ActionResult Training(int trainingId)
        {
            try
            {
                ViewBag.Training = _trainingService.GetTraining(trainingId);
                ViewBag.Contents = _trainingService.GetTrainingWithContents(trainingId);

                return View("~/Views/Employee/Training.cshtml");

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
            try
            {
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
            // TODO: User Session
            //string UserId = 1;
            int UserId = (int)this.Session["UserId"];

            try
            {
                var enrollement = _userTrainingEnrollmentService.GetUserTrainingEnrollment(UserId, trainingId);

                if (enrollement != null && enrollement.Status == EnrollStatusEnum.Pending)
                {
                    Response.StatusCode = 400;
                    return Content("Already Applied!");
                }
            
                ViewBag.trainingId = trainingId;
                ViewBag.Training = _trainingService.GetTraining(trainingId);
                ViewBag.Prerequisites = _prerequisiteService.GetPrerequisitesByTraining(trainingId);

                return View("~/Views/Employee/ApplyTraining.cshtml");
            }
            catch (Exception ex)
            {
                Response.StatusCode = 400;
                return Content("Error");
            }
        }

        [Route("ApplyTrainingPost")]
        [HttpPost]
        public ActionResult ApplyTrainingPost(int trainingId)
        {
            // TODO: User Session
            //string UserId = 1;
            int UserId = (int)this.Session["UserId"];

            List<UploadFileStore> uploadFiles = new List<UploadFileStore>();

            try
            {
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