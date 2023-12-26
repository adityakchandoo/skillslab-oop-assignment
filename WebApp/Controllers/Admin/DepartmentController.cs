﻿using BusinessLayer.Services.Interfaces;
using Entities.DbModels;
using Entities.FormDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        public async Task<ActionResult> ViewAll()
        {

            ViewBag.Departments = await _dapartmentService.GetAllDepartmentsAsync();

            return View("~/Views/Admin/Departments.cshtml");
        }

        [AuthorizePermission("department.add")]
        [HttpPost]
        public async Task<ActionResult> AddPost(DepartmentDTO department)
        {
            try
            {
                Department department_db = new Department()
                {
                    Name = department.Name,
                    Description = department.Description,
                };

                await _dapartmentService.AddDepartmentAsync(department_db);

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