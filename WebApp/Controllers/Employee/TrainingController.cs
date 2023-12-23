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

namespace WebApp.Controllers
{
    public partial class TrainingController : Controller
    {
        [AuthorizePermission("training.dash")]
        public ActionResult ViewDash()
        {
            // TODO: User Session
            //int UserId = 10;
            int UserId = (int)this.Session["UserId"];

            try
            {
                if (Request.QueryString["q"] == "my")
                {
                    ViewBag.Trainings = _trainingService.GetTrainingEnrolledByUser(UserId);
                }
                else if (Request.QueryString["q"] == "all")
                {
                    ViewBag.Trainings = _trainingService.GetAllTraining(UserId);
                }
                else if (Request.QueryString["q"] == "approved")
                {
                    ViewBag.Trainings = _trainingService.GetTrainingEnrolledByUser(UserId, EnrollStatusEnum.Approved);
                }
                else if (Request.QueryString["q"] == "pending")
                {
                    ViewBag.Trainings = _trainingService.GetTrainingEnrolledByUser(UserId, EnrollStatusEnum.Pending);
                }
                else
                {
                    // Same as my
                    ViewBag.Trainings = _trainingService.GetTrainingEnrolledByUser(UserId);
                }

                return View("~/Views/Employee/TrainingCardView.cshtml");
            }
            catch (Exception ex)
            {
                Response.StatusCode = 400;
                ViewBag.Error = ex.Message;
                return View("~/Views/Other/GlobalErrorDisplayPage.cshtml");
            }

        }

        [AuthorizePermission("training.apply")]
        public ActionResult Apply(int id)
        {
            // TODO: User Session
            //string UserId = 1;
            int UserId = (int)this.Session["UserId"];

            try
            {
                var enrollement = _userTrainingEnrollmentService.GetUserTrainingEnrollment(UserId, id);

                if (enrollement != null && enrollement.Status == EnrollStatusEnum.Pending)
                {
                    Response.StatusCode = 400;
                    return Content("Already Applied!");
                }
            
                ViewBag.trainingId = id;
                ViewBag.Training = _trainingService.GetTraining(id);
                ViewBag.Prerequisites = _prerequisiteService.GetPrerequisitesByTraining(id);

                return View("~/Views/Employee/ApplyTraining.cshtml");
            }
            catch (Exception ex)
            {
                Response.StatusCode = 400;
                return Content("Error");
            }
        }

        [AuthorizePermission("training.apply")]
        [HttpPost]
        public ActionResult ApplyPost(int trainingId)
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