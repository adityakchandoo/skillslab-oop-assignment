using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading.Tasks;
using System.Web.Mvc;
using BusinessLayer.Services;
using BusinessLayer.Services.Interfaces;
using Entities.DbCustom;
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
            int UserId = (int)this.Session["UserId"];
            int pg = int.Parse(Request.QueryString["pg"] ?? "1");

            if (pg < 1)
                pg = 1;

            ViewBag.Pg = pg;

            if (Request.QueryString["q"] == "my")
            {
                TrainingWithUserStatusPG training = await _trainingService.GetTrainingEnrolledByUserAsync(UserId, pg);
                ViewBag.Trainings = training.trainingWithUserStatus;
                ViewBag.Pages = training.totalPages;
            }
            else if (Request.QueryString["q"] == "all")
            {
                TrainingWithUserStatusPG training = await _trainingService.GetAllTrainingAsync(UserId, pg);
                ViewBag.Trainings = training.trainingWithUserStatus;
                ViewBag.Pages = training.totalPages;
            }
            else if (Request.QueryString["q"] == "approved")
            {
                TrainingWithUserStatusPG training = await _trainingService.GetTrainingEnrolledByUserAsync(UserId, EnrollStatusEnum.Approved, pg);
                ViewBag.Trainings = training.trainingWithUserStatus;
                ViewBag.Pages = training.totalPages;
            }
            else if (Request.QueryString["q"] == "pending")
            {
                TrainingWithUserStatusPG training = await _trainingService.GetTrainingEnrolledByUserAsync(UserId, EnrollStatusEnum.Pending, pg);
                ViewBag.Trainings = training.trainingWithUserStatus;
                ViewBag.Pages = training.totalPages;
            }
            else
            {
                // Same as my
                TrainingWithUserStatusPG training = await _trainingService.GetTrainingEnrolledByUserAsync(UserId, pg);
                ViewBag.Trainings = training.trainingWithUserStatus;
                ViewBag.Pages = training.totalPages;
            }            

            return View("~/Views/Employee/TrainingCardView.cshtml");

        }

        [AuthorizePermission("training.apply")]
        public async Task<ActionResult> Apply(int id)
        {
            int UserId = (int)this.Session["UserId"];

            // To Check if user has already applied
            await _userTrainingEnrollmentService.GetUserTrainingEnrollmentAsync(UserId, id);

            ViewBag.trainingId = id;
            ViewBag.Training = await _trainingService.GetTrainingAsync(id);
            ViewBag.Prerequisites = await _prerequisiteService.GetPrerequisitesByTrainingAsync(id);

            return View("~/Views/Employee/ApplyTraining.cshtml");
        }

        [AuthorizePermission("training.apply")]
        [HttpPost]
        public async Task<ActionResult> ApplyPost(int trainingId)
        {
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