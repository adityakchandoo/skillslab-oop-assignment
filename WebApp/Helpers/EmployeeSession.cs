using Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApp.Helpers
{
    public class EmployeeSession : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.Session["UserId"] == null ||
                filterContext.HttpContext.Session["Role"] == null ||
                filterContext.HttpContext.Session["Name"] == null ||
                (UserRoleEnum)filterContext.HttpContext.Session["Role"] != UserRoleEnum.Employee)
            {
                filterContext.Controller.TempData["Error"] = "You are not logged in or you do not have the nessary permission to access this page";
                filterContext.Result = new RedirectResult("~/User/Login"); // redirect to login action
            }
            else
            {
                filterContext.Controller.ViewBag.Name = filterContext.HttpContext.Session["Name"].ToString();
                // continue normal execution 
            }
        }
    }
}