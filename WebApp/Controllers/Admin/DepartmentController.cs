using BusinessLayer.Services.Interfaces;
using Entities.DbModels;
using Entities.FormDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;
using WebApp.Helpers;

namespace WebApp.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly IDepartmentService _dapartmentService;
        public DepartmentController(IDepartmentService dapartmentService)
        {
            _dapartmentService = dapartmentService;
        }

        [AuthorizePermission("department.view")]
        public ActionResult ViewAll()
        {

            ViewBag.Departments = _dapartmentService.GetAllDepartments();

            return View("~/Views/Admin/Departments.cshtml");
        }

        [AuthorizePermission("department.add")]
        [HttpPost]
        public ActionResult AddPost(DepartmentDTO department)
        {

            try
            {
                Department department_db = new Department()
                {
                    Name = department.Name,
                    Description = department.Description,
                };

                _dapartmentService.AddDepartment(department_db);

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