using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading.Tasks;
using System.Web.Mvc;
using BusinessLayer.Services;
using BusinessLayer.Services.Interfaces;
using Entities.DbModels;
using Entities.DTO;
using Entities.Enums;
using Entities.FormDTO;
using WebApp.Helpers;

namespace WebApp.Controllers
{
    public partial class TrainingController : Controller
    {
        [AuthorizePermission("training.dash")]
        public async Task<ActionResult> ViewDash()
        {
            // TODO: User Session
            //int UserId = 10;
            int UserId = (int)this.Session["UserId"];

            if (Request.QueryString["q"] == "my")
            {
                ViewBag.Trainings = await _trainingService.GetTrainingEnrolledByUserAsync(UserId);
            }
            else if (Request.QueryString["q"] == "all")
            {
                ViewBag.Trainings = await _trainingService.GetAllTrainingAsync(UserId);
            }
            else if (Request.QueryString["q"] == "approved")
            {
                ViewBag.Trainings = await _trainingService.GetTrainingEnrolledByUserAsync(UserId, EnrollStatusEnum.Approved);
            }
            else if (Request.QueryString["q"] == "pending")
            {
                ViewBag.Trainings = await _trainingService.GetTrainingEnrolledByUserAsync(UserId, EnrollStatusEnum.Pending);
            }
            else
            {
                // Same as my
                ViewBag.Trainings = await _trainingService.GetTrainingEnrolledByUserAsync(UserId);
            }

            return View("~/Views/Employee/TrainingCardView.cshtml");

        }

        [AuthorizePermission("training.apply")]
        public async Task<ActionResult> Apply(int id)
        {
            // TODO: User Session
            //string UserId = 1;
            int UserId = (int)this.Session["UserId"];

            var enrollement = await _userTrainingEnrollmentService.GetUserTrainingEnrollmentAsync(UserId, id);

            if (enrollement != null && (enrollement.EnrollStatus == EnrollStatusEnum.Pending || enrollement.ManagerApprovalStatus == EnrollStatusEnum.Pending))
            {
                Response.StatusCode = 400;
                return Content("Already Applied!");
            }

            ViewBag.trainingId = id;
            ViewBag.Training = await _trainingService.GetTrainingAsync(id);
            ViewBag.Prerequisites = await _prerequisiteService.GetPrerequisitesByTrainingAsync(id);

            return View("~/Views/Employee/ApplyTraining.cshtml");
        }

        [AuthorizePermission("training.apply")]
        [HttpPost]
        public async Task<ActionResult> ApplyPost(int trainingId)
        {
            // TODO: User Session
            //string UserId = 1;
            int UserId = (int)this.Session["UserId"];

            List<UploadFileStore> uploadFiles = new List<UploadFileStore>();

            foreach (string key in Request.Files.AllKeys)
            {
                uploadFiles.Add(new UploadFileStore() { FileId = int.Parse(key), FileName = Request.Files.Get(key).FileName, FileContent = Request.Files.Get(key).InputStream }); ;
            }

            await _trainingService.ApplyTrainingAsync(UserId, trainingId, uploadFiles);

            return Json(new { status = "ok" });
        }

        
    }
}