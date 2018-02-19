using System;
using System.Web;

namespace Bidding.Web.Helpers
{
    public class CookieHelper
    {
        public static readonly string COOKIE_USERID = "UserID";
        public static readonly string COOKIE_EMAIL = "Email";
        public static readonly string COOKIE_GROUPS = "Groups";
        public static readonly string COOKIE_ACCESSIBLE_GROUPS = "AccessibleGroups";
        public static readonly string COOKIE_NAME = "Name";

        public static void SetCookieValue(HttpRequestBase request, HttpResponseBase response, string key, string value,
                                          string domainName = null, bool persist = true, int year = 1, int days = 0, int hours = 0)
        {
            if (request.Url.Authority.Contains("localhost"))
            {
                domainName = null;
            }
            var cookie = request.Cookies.Get(key);
            if (cookie != null)
            {
                if (!string.IsNullOrEmpty(domainName)) cookie.Domain = domainName;
                cookie.Value = value;
            }
            else if (response != null)
            {
                cookie = !string.IsNullOrEmpty(domainName)
                    ? new HttpCookie(key, value) { Domain = domainName }
                    : new HttpCookie(key, value);
            }

            if (cookie != null && response != null)
            {
                if (persist)
                {
                    if (year > 0)
                    {
                        cookie.Expires = DateTime.Now.AddYears(year);
                    }
                    if (days > 0)
                    {
                        cookie.Expires = DateTime.Now.AddDays(days);
                    }
                    if (hours > 0)
                    {
                        cookie.Expires = DateTime.Now.AddHours(hours);
                    }
                }
                response.Cookies.Set(cookie);
            }
        }

        public static string GetCookieValue(HttpRequestBase request, HttpResponseBase response, string key)
        {
            string cookieValue = string.Empty;
            if (request == null && response == null) return cookieValue;
            HttpCookie cookie = null;
            //we look in the request
            //response takes precedence
            if (response != null && response.Cookies != null && Array.IndexOf(response.Cookies.AllKeys, key) >= 0)
            {
                cookie = response.Cookies.Get(key);
            }
            else if (request != null && request.Cookies != null && Array.IndexOf(request.Cookies.AllKeys, key) >= 0)
            {
                cookie = request.Cookies.Get(key);
            }

            if (cookie != null && !string.IsNullOrEmpty(cookie.Value))
            {
                cookieValue = cookie.Value;
            }

            return cookieValue;
        }
    }
}