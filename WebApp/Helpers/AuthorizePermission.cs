using BusinessLayer.Services;
using BusinessLayer.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Unity;

namespace WebApp.Helpers
{
    public class AuthorizePermission : AuthorizeAttribute
    {
        private IUserService _userService;

        private readonly string _feature;

        public AuthorizePermission(string feature)
        {
            _feature = feature;
        }

        [InjectionMethod]
        public void Initialize(IUserService userService)
        {
            _userService = userService;
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext == null)
            {
                throw new ArgumentNullException("filterContext");
            }

            if (filterContext.HttpContext.Session["UserId"] == null ||
                filterContext.HttpContext.Session["Role"] == null ||
                filterContext.HttpContext.Session["Name"] == null)
            {
                filterContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                filterContext.Controller.TempData["Error"] = "You are not logged in";
                filterContext.Result = new RedirectResult("~/User/Login");
                return;
            }

            bool IsAuthorised = _userService.CheckPermission(
                (int)filterContext.HttpContext.Session["UserId"],
                _feature);

            if (IsAuthorised == false)
            {
                filterContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                filterContext.Result = new ViewResult() { ViewName = "~/Views/Other/UnauthorizedErrorPage.cshtml" };
                return;
            }

            // Set the name variable. Used in master layout
            filterContext.Controller.ViewBag.Name = filterContext.HttpContext.Session["Name"].ToString();
        }


    }
}