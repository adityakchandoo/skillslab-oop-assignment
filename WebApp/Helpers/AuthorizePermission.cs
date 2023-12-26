using BusinessLayer.Services.Interfaces;
using System;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
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

            //bool IsAuthorised = _userService.CheckPermissionAsync(
            //    (int)filterContext.HttpContext.Session["UserId"],
            //    _feature).GetAwaiter().GetResult();

            bool IsAuthorised = Task.Run(() => _userService.CheckPermissionAsync(
                (int)filterContext.HttpContext.Session["UserId"],
                _feature)).Result;


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