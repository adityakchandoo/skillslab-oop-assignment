using BusinessLayer.Services.Interfaces;
using DataLayer.Repository.Interfaces;
using Entities.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApp.Helpers;

namespace WebApp.Controllers
{

    public partial class TrainingController : Controller
    {
        [AuthorizePermission("training.viewrequests")]
        public ActionResult ViewRequests()
        {
            // TODO: User Session
            //int UserId = 4;
            int UserId = (int)this.Session["UserId"];



            ViewBag.TrainingRequests = _trainingService.GetTrainingPendingForManager(UserId);
            return View("~/Views/Manager/TrainingRequests.cshtml");
        }

        [AuthorizePermission("training.processrequests")]
        [Route("Training/ProcessRequest/{UserId}/{TrainingId}")]
        public ActionResult ProcessRequest(int UserId, int TrainingId)
        {
            ViewBag.User = _userService.GetUser(UserId);
            ViewBag.Training = _trainingService.GetTraining(TrainingId);
            ViewBag.Attachments = _userTrainingEnrollmentService.GetUserTrainingEnrollmentInfo(UserId, TrainingId);

            return View("~/Views/Manager/TrainingProcess.cshtml");
        }

        [AuthorizePermission("training.processrequests")]
        [HttpPost]
        public ActionResult ProcessRequestAction(int targetUserId, int targetTrainingId, bool approve)
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