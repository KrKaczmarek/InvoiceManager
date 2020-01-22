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
        private readonly string[] allowedUsers;
        public MyAuthorizationAttribute(params string[] users)
        {
            allowedUsers = users;
        }
        

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            bool authorize = false;
            string id;
            HttpCookie cookie = HttpContext.Current.Request.Cookies["LoginCookie"];
            
            if (cookie != null)
            {
                FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(cookie.Value);
                id=authTicket.UserData;
                foreach(var i in allowedUsers)
                {
                    if (i == id)
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