using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Bidding.Web.Helpers;

namespace Bidding.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var userId = CookieHelper.GetCookieValue(Request, Response, CookieHelper.COOKIE_USERID);
            if( string.IsNullOrEmpty(userId))
            {
                string url = "~/Account/Login";
                return new RedirectResult(url);
            }

            return View();
        }
    }
}