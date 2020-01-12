using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using WebApi.Models;

namespace WebApi.MVCControllers
{
    public class DuckController : Controller
    {
        IEnumerable<DuckViewModel> Ducks;
        DuckViewModel Duck = new DuckViewModel();
        public ActionResult DuckIndex()
        {

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Request.Url.GetLeftPart(UriPartial.Authority));
                var responseTask = client.GetAsync("api/Duck");
                responseTask.Wait();



                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IList<DuckViewModel>>();
                    readTask.Wait();
                    Ducks = readTask.Result;


                }
                else
                {
                    Ducks = Enumerable.Empty<DuckViewModel>();
                    ModelState.AddModelError(string.Empty, "No ducks in db");
                }
                ViewData["error"] = TempData["error"];
                return View(Ducks);
            }
        }
        private void PopulateList()
        {
            List<CountryViewModel> countries = new List<CountryViewModel>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Request.Url.GetLeftPart(UriPartial.Authority));
                var responseTask = client.GetAsync("api/country");

                responseTask.Wait();
                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IList<CountryViewModel>>();
                    readTask.Wait();
                    countries = readTask.Result.ToList();
                }
            }
           Duck.CountryList = new SelectList(countries, "CountryId", "CountryName");
            Duck.DuckId = WebApi.Controllers.DuckController.NextDuckIndex;
        }
        public ActionResult CreateDuck()
        {
            PopulateList();
            return View(Duck);
        }
        
        [HttpPost]
        public ActionResult CreateDuck(DuckViewModel duck)
        {
            if (duck.DuckCountryId == null)
            {
                PopulateList();
                duck.CountryList = Duck.CountryList; 
                return View(duck);
            }
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Request.Url.GetLeftPart(UriPartial.Authority));
                var postTask = client.PostAsJsonAsync<DuckViewModel>("api/duck", duck);
                postTask.Wait();

                var result = postTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("DuckIndex");
                }
            }
            PopulateList();
            ModelState.AddModelError(string.Empty, "Cannot create duck");


            return View(duck);
        }
        public ActionResult EditDuck(int id)
        {
            PopulateList();
            IEnumerable<DuckViewModel> Ducks = null;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Request.Url.GetLeftPart(UriPartial.Authority));

                var responseTask = client.GetAsync("api/duck?id=" + id.ToString());
                responseTask.Wait();

                var result = responseTask.Result;

                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IList<DuckViewModel>>();
                    readTask.Wait();

                    Ducks = readTask.Result;
                }
            }
            Ducks.SingleOrDefault().CountryList = Duck.CountryList;
            return View(Ducks.SingleOrDefault());
        }

        [HttpPost]
        public ActionResult EditDuck(DuckViewModel duck)
        {
         
            duck.CountryList = Duck.CountryList;
       
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Request.Url.GetLeftPart(UriPartial.Authority));


                var putTask = client.PutAsJsonAsync<DuckViewModel>("api/duck", duck);
                putTask.Wait();

                var result = putTask.Result;
                if (result.IsSuccessStatusCode)
                {

                    return RedirectToAction("DuckIndex");
                }
            }
            PopulateList();
            return View(duck);
        }

        public ActionResult DeleteDuck(int id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Request.Url.GetLeftPart(UriPartial.Authority));


                var deleteTask = client.DeleteAsync("api/duck/" + id.ToString());
                deleteTask.Wait();

                var result = deleteTask.Result;
                if (result.IsSuccessStatusCode)
                {

                    return RedirectToAction("DuckIndex");
                }
            }
            TempData["error"] = "Cannot delete. Duck used in invoice.";
            return RedirectToAction("DuckIndex");
        }
    }
}
