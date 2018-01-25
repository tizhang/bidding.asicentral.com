using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using Bidding.Web.Helpers;

namespace Bidding.Web.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Login()
        {
            var user = new UserModel();
            return View(user);
        }

        [HttpGet]
        public ActionResult Logout()
        {
            CookieHelper.SetCookieValue(Request, Response, CookieHelper.COOKIE_USERID, string.Empty);
            return new RedirectResult("/");
        }

        [HttpPost]
        public async Task<ActionResult> Login(UserModel user)
        {
            user.ErrorMessage = "Invalid user email or password";
            var client = new HttpClient();
            var webApiUrl = string.Format("http://local-bidding.asicentral.com/api/login?email={0}&password={1}", user.Email, user.Password);
            var response = await client.GetAsync(webApiUrl);
            var result = response.Content.ReadAsStringAsync().Result;
            var pattern = @"""Id"":(\d+).*?Email"":""(.*?)"",""Groups";
            var match = Regex.Match(result, pattern);
            if( match.Success)
            {
                CookieHelper.SetCookieValue(Request, Response, CookieHelper.COOKIE_USERID, match.Groups[1].Value);
                return new RedirectResult("/");
            }
            return View(user);
        }
    }

    public class UserModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public int UserId { get; set; }
        public string ErrorMessage { get; set; }
    }
}