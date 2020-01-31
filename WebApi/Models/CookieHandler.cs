using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace WebApi.Models
{
    public static class CookieHandler
    {
        public static void CreateCookie(string userName, string userData, string cookieName)
        {
            FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket
                       (
                       1, userName, DateTime.Now, DateTime.Now.AddMinutes(15), false, userData
                       );

            string enTicket = FormsAuthentication.Encrypt(authTicket);

            HttpCookie faCookie = new HttpCookie(cookieName, enTicket);
            HttpContext.Current.Response.Cookies.Add(faCookie);
        }
        public static void DeleteCookie(string cookieName)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[cookieName];
            cookie.Expires = DateTime.Now.AddYears(-1);
        }

        public static string GetUserNameFromCookie(string cookieName)
        {
             HttpCookie cookie = HttpContext.Current.Request.Cookies["LoginCookie"];
            FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(cookie.Value);
            string[] cookieData = authTicket.UserData.Split(' ');
            return cookieData[0];
        }
        public static string GetUserRoleFromCookie(string cookieName)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies["LoginCookie"];
            FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(cookie.Value);
            string[] cookieData = authTicket.UserData.Split(' ');

            return cookieData[1];
        }


    }
}