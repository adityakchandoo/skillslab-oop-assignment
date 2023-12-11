using BusinessLayer.Services.Interfaces;
using DataLayer.Repository.Interfaces;
using Entities.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApp.Helpers;

namespace WebApp.Controllers.Manager
{
    [ManagerSession]
    [RoutePrefix("Manager")]
    public class TrainingController : Controller
    {
        private readonly ITrainingService _trainingService;
        private readonly IUserService _userService;
        private readonly IUserTrainingEnrollmentService _userTrainingEnrollmentService;
        public TrainingController(ITrainingService trainingService, IUserService userService, IUserTrainingEnrollmentService userTrainingEnrollmentService)
        {
            _trainingService = trainingService;
            _userService = userService;
            _userTrainingEnrollmentService = userTrainingEnrollmentService;
        }

        // GET: Training
        public ActionResult Index()
        {
            return View();
        }

        [Route("AllTrainings")]
        public ActionResult AllTrainings()
        {
            ViewBag.Trainings = _trainingService.GetAllTrainingDetails();
            return View("~/Views/Manager/AllTrainings.cshtml");
        }

        [Route("Training/{trainingId}")]
        public ActionResult Training(int trainingId)
        {
            ViewBag.Training = _trainingService.GetTraining(trainingId);
            ViewBag.Contents = _trainingService.GetTrainingWithContents(trainingId);

            return View("~/Views/Manager/Training.cshtml");
        }

        [Route("TrainingRequests")]
        public ActionResult TrainingRequests()
        {
            // TODO: User Session
            //string UserId = "aditya";
            string UserId = this.Session["UserId"].ToString();



            ViewBag.TrainingRequests = _trainingService.GetTrainingPendingForManager(UserId);
            return View("~/Views/Manager/TrainingRequests.cshtml");
        }

        [Route("TrainingProcess/{targetUserId}/{targetTrainingId}")]
        public ActionResult TrainingProcess(string targetUserId, int targetTrainingId)
        {
            ViewBag.User = _userService.GetUser(targetUserId);
            ViewBag.Training = _trainingService.GetTraining(targetTrainingId);
            ViewBag.Attachments = _userTrainingEnrollmentService.GetUserTrainingEnrollmentInfo(targetUserId, targetTrainingId);

            return View("~/Views/Manager/TrainingProcess.cshtml");
        }

        [HttpPost]
        [Route("TrainingRequestsAction")]
        public ActionResult TrainingRequestsAction(string targetUserId, int targetTrainingId, bool approve)
        {
            try
            {
                _userTrainingEnrollmentService.ProcessTrainingRequest(targetUserId, targetTrainingId, approve);

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