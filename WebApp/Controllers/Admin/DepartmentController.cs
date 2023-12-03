using MainLibrary.DTO;
using MainLibrary.Service;
using MainLibrary.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApp.Controllers.Admin
{
    [RoutePrefix("Admin")]
    public class DepartmentController : Controller
    {
        private readonly IDepartmentService _dapartmentService;
        public DepartmentController(IDepartmentService dapartmentService)
        {
            _dapartmentService = dapartmentService;
        }

        [Route("Departments")]
        public ActionResult Index()
        {

            ViewBag.PageTag = "departments";

            ViewBag.Departments = _dapartmentService.GetAllDepartments();

            return View("~/Views/Admin/Departments.cshtml");
        }

        [Route("AddDepartment")]
        public ActionResult AddDepartment(DepartmentDTO department)
        {
            if (!ModelState.IsValid)
            {
                Response.StatusCode = 400;
                return Json(ModelState);
            }

            try
            {
                _dapartmentService.AddDepartment(department);

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