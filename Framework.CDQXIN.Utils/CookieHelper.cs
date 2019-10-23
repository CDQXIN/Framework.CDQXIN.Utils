using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Framework.CDQXIN.Utils
{
    public class CookieHelper
    {
        public static void Set(string key, string value, int expireMins = 0)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[key];
            if (cookie == null)
                cookie = new HttpCookie(key);
            cookie.Value = value;
            if (expireMins > 0)
                cookie.Expires = DateTime.Now.AddMinutes(expireMins);

            HttpContext.Current.Response.Cookies.Add(cookie);
        }
        public static void Set(string key, string value, string path, string domain, int expireMins = 0)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[key];
            if (cookie == null)
                cookie = new HttpCookie(key);
            cookie.Value = value;
            if (expireMins > 0)
                cookie.Expires = DateTime.Now.AddMinutes(expireMins);
            if (path != "")
            {
                cookie.Path = path;
            }
            if (domain != "")
            {
                cookie.Domain = domain;
            }
            HttpContext.Current.Response.Cookies.Add(cookie);
        }
        public static string Get(string key)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[key];
            if (cookie == null)
                return string.Empty;
            return cookie.Value;
        }

        public static void Remove(string key)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[key];
            if (cookie != null)
            {
                cookie.Expires = DateTime.Now.AddMinutes(-1);
                HttpContext.Current.Response.Cookies.Set(cookie);
            }
        }
        /// <summary>
        /// 登陆
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void SetCookie(string key, string value)
        {
            HttpContext.Current.Response.Cookies[key].Value = value;
            HttpContext.Current.Response.Cookies[key].Path = "/";
            HttpContext.Current.Response.Cookies[key].Domain = HttpContext.Current.Request.Url.Host;
            HttpContext.Current.Response.Cookies[key].Expires = DateTime.Now.AddDays(1);
        }
        /// <summary>
        /// 退出
        /// </summary>
        /// <param name="context"></param>
        /// <param name="domain"></param>
        public static void ClearCookie(HttpContext context, string domain = "")
        {
            if (context == null) return;
            int limit = context.Request.Cookies.Count;
            for (int i = 0; i < limit; i++)
            {
                var aCookie = context.Request.Cookies[i];
                aCookie.HttpOnly = true;
                aCookie.Expires = DateTime.Now.AddDays(-24);
                aCookie.Domain = domain;//.Url.Host; //必须设置Domain，否则清除不了Cookie，此方法只清除本域名的Cookie
                context.Response.SetCookie(aCookie);
            }
        }
    }
}
