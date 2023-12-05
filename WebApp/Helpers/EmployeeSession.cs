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
                (UserRoleEnum)filterContext.HttpContext.Session["Role"] != UserRoleEnum.Employee)
            {
                filterContext.Result = new RedirectResult("~/User/Login"); // redirect to login action
            }
            else
            {
                // continue normal execution 
            }
        }
    }
}