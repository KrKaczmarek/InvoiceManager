using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApi.Models;

namespace WebApi.Controllers
{
    public class LoginController : ApiController
    {
        public IHttpActionResult GetUser()
        {
            IList<UserLoginViewModel> Users = null;
            using (var ctx = new SklepEntities())
            {
                Users = ctx.Uzytkownicy.Include("Uzytkownik_id").Select(I => new UserLoginViewModel()
                {                  
                    UserName = I.Uzytkownik_nazwa,
                    UserPassword = I.Uzytkownik_haslo,
                    UserRole= I.Rola,
               
                }).ToList();
            }          
            return Ok(Users);
        }
    }
}
