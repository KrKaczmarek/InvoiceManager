using NLog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using WebApi.Models;

namespace WebApi.MVCControllers
{
    [MyAuthorization("Admin", "Pracownik")]
    public class CountryController : Controller
    {
        private Logger logger = LogManager.GetCurrentClassLogger();
       static IEnumerable<CountryViewModel> Countries;       
        public ActionResult CountryIndex()
        {

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Request.Url.GetLeftPart(UriPartial.Authority));
                var responseTask = client.GetAsync("api/Country");
                responseTask.Wait();



                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IList<CountryViewModel>>();
                    readTask.Wait();
                    Countries = readTask.Result;


                }
                else
                {
                    Countries = Enumerable.Empty<CountryViewModel>();
                    ModelState.AddModelError(string.Empty, "No countries in db");
                }
                ViewData["error"] = TempData["error"];
                return View(Countries);
            }
        }
        public ActionResult CreateCountry()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateCountry(CountryViewModel country)
        {
            if(CheckIfDuplicatedId(country))
            return RedirectToAction("CreateCountry");
            try
            {
                if (ModelState.IsValid)
                {
                    using (var client = new HttpClient())
                    {
                        client.BaseAddress = new Uri(Request.Url.GetLeftPart(UriPartial.Authority));
                        var postTask = client.PostAsJsonAsync<CountryViewModel>("api/country", country);
                        postTask.Wait();

                        var result = postTask.Result;
                        if (result.IsSuccessStatusCode)
                        {
                            logger.Info(Environment.NewLine + "Country" + country.CountryId.ToString() + " added by " + CookieHandler.GetUserNameFromCookie("LoginCookie") + " " + DateTime.Now);
                            return RedirectToAction("CountryIndex");
                        }
                    }
                }
            }
            catch (DataException)
            {
                ModelState.AddModelError(string.Empty, "Server Error.");
            }

            ModelState.AddModelError(string.Empty, "Cannot create country");
            return View(country);
        }

        private bool CheckIfDuplicatedId(CountryViewModel country)
        {
            if (Countries.Where(c => c.CountryId.Equals(country.CountryId, StringComparison.OrdinalIgnoreCase)).SingleOrDefault() != null)
            {
                TempData["error"] = "Cannot create. ID must be unique.";
                return true;                
            }
            return false;
        }
        public ActionResult EditCountry(string id)
        {
            IEnumerable<CountryViewModel> country = null;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Request.Url.GetLeftPart(UriPartial.Authority));

                var responseTask = client.GetAsync("api/country?id=" + id.ToString());
                responseTask.Wait();

                var result = responseTask.Result;

                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IList<CountryViewModel>>();
                    readTask.Wait();
                    country = readTask.Result;
                }
            }

            return View(country.SingleOrDefault());
        }

        [HttpPost]
        public ActionResult EditCountry(CountryViewModel country)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using (var client = new HttpClient())
                    {
                        client.BaseAddress = new Uri(Request.Url.GetLeftPart(UriPartial.Authority));


                        var putTask = client.PutAsJsonAsync<CountryViewModel>("api/country", country);
                        putTask.Wait();

                        var result = putTask.Result;
                        if (result.IsSuccessStatusCode)
                        {
                            logger.Info(Environment.NewLine + "Country" + country.CountryId.ToString() + " edited by " + CookieHandler.GetUserNameFromCookie("LoginCookie") + " " + DateTime.Now);
                            return RedirectToAction("CountryIndex");
                        }
                    }
                }
            }catch(DataException)
            {

            }
            return View(country);
        }

        public ActionResult DeleteCountry(string id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Request.Url.GetLeftPart(UriPartial.Authority));


                var deleteTask = client.DeleteAsync("api/country/" + id.ToString());
                deleteTask.Wait();

                var result = deleteTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    logger.Info(Environment.NewLine + "Country" + id.ToString() + " deleted by " + CookieHandler.GetUserNameFromCookie("LoginCookie") + " " + DateTime.Now);
                    return RedirectToAction("CountryIndex");
                }
            }
            TempData["error"] = "Cannot delete. Country used in other records.";
            return RedirectToAction("CountryIndex");
        }
    }
}