using MainLibrary.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApp.Helpers
{
    public class ManagerSession : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.Session["UserId"] == null ||
                filterContext.HttpContext.Session["Role"] == null ||
                (UserRoleType)filterContext.HttpContext.Session["Role"] != UserRoleType.Manager)
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