using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace WebApp.Helpers
{
    public class GlobalExceptionHandler : IExceptionFilter
    {
        public void OnException(ExceptionContext filterContext)
        {
            if (filterContext.ExceptionHandled)
                return;

            filterContext.ExceptionHandled = true;

            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                filterContext.HttpContext.Response.StatusCode = GetStatuscode(filterContext.Exception);
                filterContext.Result = new JsonResult { Data = new { Error = filterContext.Exception.Message } };
                return;
            }
            else
            {
                filterContext.HttpContext.Response.StatusCode = GetStatuscode(filterContext.Exception);
                filterContext.Result = new ViewResult { ViewName = "~/Views/Other/GenericErrorPage.cshtml", TempData = new TempDataDictionary() { { "Message", filterContext.Exception.Message } }  };
                return;
            }            
        }

        private int GetStatuscode(Exception ex)
        {
            if (ex is ArgumentException)
            {
                return (int)HttpStatusCode.BadRequest;
            }
            else if (ex is DbErrorException)
            {
                return (int)HttpStatusCode.InternalServerError;
            }
            else
            {
                return (int)HttpStatusCode.InternalServerError;
            }
        }
    }
}