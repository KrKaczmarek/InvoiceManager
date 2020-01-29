using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WebApi.Models;

namespace WebApi.MVCControllers
{
    public class LoginController : Controller
    {
        IEnumerable<UserLoginViewModel> Users { get; set; }
       
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(UserLoginViewModel user)
        {
            if (ValidateUser(user))
            {
                var tempUser=Users.SingleOrDefault(U => U.UserName == user.UserName);
                string userData = user.UserName +" "+ tempUser.UserRole;
                FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket
                       (
                       1, user.UserName, DateTime.Now, DateTime.Now.AddMinutes(15), false, userData
                       );
                
                string enTicket = FormsAuthentication.Encrypt(authTicket);
               
                HttpCookie faCookie = new HttpCookie("LoginCookie", enTicket);
                Response.Cookies.Add(faCookie);

                return RedirectToAction("Index", "Invoice");
            }
            ModelState.AddModelError("Login", "Incorrect login or password");
            return View(user);
        }
        
        public ActionResult LogOff()
        {
            HttpCookie cookie = Request.Cookies["LoginCookie"];
            cookie.Expires = DateTime.Now.AddYears(-1);
            return RedirectToAction("Login", "Login");
        }
        public bool ValidateUser(UserLoginViewModel user)
        {
            if (user.UserName == null || user.UserPassword ==null)
                return false;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Request.Url.GetLeftPart(UriPartial.Authority));
                var responseTask = client.GetAsync("api/Login");
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IList<UserLoginViewModel>>();
                    readTask.Wait();
                    Users = readTask.Result;
                }
                else
                {
                    Users = Enumerable.Empty<UserLoginViewModel>();
                    ModelState.AddModelError(string.Empty, "Server error");
                }
            }
            if (Users.Any(u => u.UserName == user.UserName && u.UserPassword == user.UserPassword))
                return true;
            else
            {               
                return false;
            }
        }
    }
}