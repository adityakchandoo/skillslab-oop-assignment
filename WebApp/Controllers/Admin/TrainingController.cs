using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MainLibrary.Entities;
using MainLibrary.Service;
using MainLibrary.Service.Interfaces;

namespace WebApp.Controllers.Admin
{
    [RoutePrefix("Admin")]
    public class TrainingController : Controller
    {
        ITrainingService _trainingService;

        public TrainingController(ITrainingService trainingService)
        {
            _trainingService = trainingService;
        }


        // GET: Training
        [Route("ViewTrainings")]
        public ActionResult ViewTrainings()
        {
            ViewBag.PageTag = "train-manage";

            ViewBag.Trainings = _trainingService.GetAllTraining();

            return View("~/Views/Admin/TrainingManage.cshtml");
        }

        [Route("AddTraining")]
        public ActionResult AddTraining()
        {
            return View("~/Views/Admin/TrainingAdd.cshtml");
        }

        [Route("AddTrainingPost")]
        public ActionResult AddTrainingPost(Training training)
        {
            return View("~/Views/Admin/TrainingAdd.cshtml");
        }
    }
}