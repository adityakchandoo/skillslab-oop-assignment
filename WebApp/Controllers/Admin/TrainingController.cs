﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MainLibrary.Service;
using MainLibrary.Service.Interfaces;

namespace WebApp.Controllers.Admin
{
    [RoutePrefix("Admin/Training")]
    public class TrainingController : Controller
    {
        ITrainingService _trainingService;

        public TrainingController(ITrainingService trainingService)
        {
            _trainingService = trainingService;
        }


        // GET: Training
        [Route("Manage")]
        public ActionResult Manage()
        {
            ViewBag.PageTag = "train-manage";

            ViewBag.Trainings = _trainingService.GetAllTraining();

            return View("~/Views/Admin/TrainingManage.cshtml");
        }
    }
}