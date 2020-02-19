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
            string role = CookieHandler.GetUserRoleFromCookie("LoginCookie");
            
            if (role != null)
            {
               
                foreach(var i in allowedRoles)
                {
                    if (i == role)
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