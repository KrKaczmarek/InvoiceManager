using System;
using System.Collections.Generic;
using System.Linq;

using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;

namespace WebApi.Models
{
    public class MyAuthorizationAttribute:AuthorizeAttribute
    {
        private readonly string[] allowedRoles;
        public MyAuthorizationAttribute(params string[] roles)
        {
            allowedRoles = roles;
        }
        

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            bool authorize = false;
            string[] cookieData;
            HttpCookie cookie = HttpContext.Current.Request.Cookies["LoginCookie"];
            
            if (cookie != null)
            {
                FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(cookie.Value);
                cookieData = authTicket.UserData.Split(' ');
                foreach(var i in allowedRoles)
                {
                    if (i == cookieData[1])
                        authorize= true;
                }
               
            }
            return authorize;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new RedirectToRouteResult(
               new RouteValueDictionary
               {
                    { "controller", "Invoice" },
                    { "action", "UnAuthorized" }
               });
        }
    }
}