using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using System.Web.Mvc;
using MainLibrary.DTO;
using MainLibrary.Entities;
using MainLibrary.Entities.Types;
using MainLibrary.Service;
using MainLibrary.Services;
using MainLibrary.Services.Interfaces;
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
        private readonly INotificationService _notificationService;

        public TrainingController(ITrainingService trainingService, IUserTrainingEnrollmentService userTrainingEnrollmentService, IUserService userService, INotificationService notificationService)
        {
            _trainingService = trainingService;
            _userTrainingEnrollmentService = userTrainingEnrollmentService;
            _userService = userService;
            _notificationService = notificationService;
        }


        // GET: Training
        [Route("MyTrainings")]
        public ActionResult MyTrainings()
        {
            ViewBag.PageTag = "train-manage";

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
            ViewBag.PageTag = "train-manage";

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

        [Route("ApplyTraining")]
        [HttpPost]
        public ActionResult ApplyTraining(int trainingId)
        {
            try
            {
                const string UserId = "aditya";// (string)this.Session["UserId"];

                UserTrainingEnrollment enrollment = new UserTrainingEnrollment()
                {
                    UserId = UserId,
                    TrainingId = trainingId,
                    Status = EnrollStatusType.Pending
                };

                Training training = _trainingService.GetTraining(trainingId);
                User user = _userService.GetUser(training.ManagerId);

                _userTrainingEnrollmentService.CreateUserTrainingEnrollment(enrollment);


                NotificationDTO notificationDTO = new NotificationDTO()
                {
                    To = user.Email,
                    Subject = "New Student Applied to Training",
                    Body = "Check website"
                };

                _notificationService.Send(notificationDTO);

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