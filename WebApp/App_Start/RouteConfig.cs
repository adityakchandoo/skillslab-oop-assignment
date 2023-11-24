using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace WebApp
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Admin",
                url: "admin/{controller}/{action}/{id}",
                defaults: new { controller = "Dash", action = "Index", id = UrlParameter.Optional },
                new string[] { "WebApp.Controllers.Admin" }
            );

            routes.MapRoute(
                name: "Manager",
                url: "manager/{controller}/{action}/{id}",
                defaults: new { controller = "Dash", action = "Index", id = UrlParameter.Optional },
                new string[] { "WebApp.Controllers.Manager" }
            );

            routes.MapRoute(
                name: "Employee",
                url: "employee/{controller}/{action}/{id}",
                defaults: new { controller = "Dash", action = "Index", id = UrlParameter.Optional },
                new string[] { "WebApp.Controllers.Employee" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );


        }
    }
}
