using BusinessLayer.Services.Interfaces;
using DataLayer.Repository.Interfaces;
using Entities.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebApp.Helpers;

namespace WebApp.Controllers
{

    public partial class TrainingController : Controller
    {
        [AuthorizePermission("training.viewrequests")]
        public async Task<ActionResult> ViewRequests()
        {
            // TODO: User Session
            //int UserId = 4;
            int UserId = (int)this.Session["UserId"];



            ViewBag.TrainingRequests = await _trainingService.GetTrainingPendingForManagerAsync(UserId);
            return View("~/Views/Manager/TrainingRequests.cshtml");
        }

        [AuthorizePermission("training.processrequests")]
        [Route("Training/ProcessRequest/{UserId}/{TrainingId}")]
        public async Task<ActionResult> ProcessRequest(int UserId, int TrainingId)
        {
            ViewBag.User = await _userService.GetUserAsync(UserId);
            ViewBag.Training = await _trainingService.GetTrainingAsync(TrainingId);
            ViewBag.Attachments = await _userTrainingEnrollmentService.GetUserTrainingEnrollmentInfoAsync(UserId, TrainingId);

            return View("~/Views/Manager/TrainingProcess.cshtml");
        }

        [AuthorizePermission("training.processrequests")]
        [HttpPost]
        public async Task<ActionResult> ProcessRequestAction(int targetUserId, int targetTrainingId, bool approve, string declineReason)
        {
            await _userTrainingEnrollmentService.ProcessTrainingRequestAsync(targetUserId, targetTrainingId, approve, declineReason);

            return Json(new { status = "ok" });

        }


    }
}