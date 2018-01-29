using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;
using Bidding.Web.Helpers;
using Newtonsoft.Json;

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
            var userObject = JsonConvert.DeserializeObject<UserModel>(result);
            if( userObject != null)
            {
                CookieHelper.SetCookieValue(Request, Response, CookieHelper.COOKIE_USERID, userObject.Id.ToString());
                CookieHelper.SetCookieValue(Request, Response, CookieHelper.COOKIE_NAME, userObject.Name);
                if ( userObject.Groups != null && userObject.Groups.Count > 0)
                {
                    CookieHelper.SetCookieValue(Request, Response, CookieHelper.COOKIE_GROUPS, string.Join(",", userObject.Groups));
                }
                return new RedirectResult("/");
            }

            return View(user);
        }
    }

    public class UserModel
    {
        public string Email { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public int Id { get; set; }
        public string ErrorMessage { get; set; }

        public List<string> Groups { get; set; }
    }
}