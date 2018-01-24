using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Bidding.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

           routes.MapRoute(
                name: "Account",
                url: "Account/Login",
                defaults: new { controller = "Account", action = "Login", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Logout",
                url: "Account/Logout",
                defaults: new { controller = "Account", action = "Logout", id = UrlParameter.Optional }
            );

            routes.MapRoute("Default", "{*url}",
                new { controller = "Home", action = "Index" }, new[] { "Bidding.Web.Controllers" }
            );
            
 
        }
    }
}
