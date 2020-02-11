using AutoMapper;
using NLog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using WebApi.Models;
using WebApi.Repository;

namespace WebApi.MVCControllers
{
    [MyAuthorization("Admin", "Pracownik")]
    public class DuckController : Controller
    {
        private Logger logger = LogManager.GetCurrentClassLogger();

        DuckViewModel Duck = new DuckViewModel();
        UnitOfWork unitOfWork = new UnitOfWork();
        private static int NextDuckIndex;
        public ActionResult DuckIndex()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile(new MapperProfile()));
            var mapper = config.CreateMapper();

            var ducks = unitOfWork.DuckRepository.Get(includeProperties: "");
            NextDuckIndex = ducks.Last().Kaczka_id + 1;
            return View(mapper.Map<List<DuckViewModel>>(ducks));

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
        }
        public ActionResult CreateDuck()
        {
            PopulateList();
            Duck.DuckId = NextDuckIndex;
            return View(Duck);
        }

        [HttpPost]
        public ActionResult CreateDuck(DuckViewModel duck)
        {
            PopulateList();
            var config = new MapperConfiguration(cfg => cfg.AddProfile(new MapperProfile()));
            var mapper = config.CreateMapper();

            try
            {
                if (ModelState.IsValid)
                {
                    unitOfWork.DuckRepository.Insert(mapper.Map<Kaczki>(duck));
                    unitOfWork.Save();
                    return RedirectToAction("DuckIndex");
                    //TODO:logger }
                }
            }
            catch (DataException) { }


            ModelState.AddModelError(string.Empty, "Cannot create duck");
            return View(Duck);
        }
        public ActionResult EditDuck(int id)
        {
                  

            var ducks = unitOfWork.DuckRepository.GetByID(id);
            var config = new MapperConfiguration(cfg => cfg.AddProfile(new MapperProfile()));
            var mapper = config.CreateMapper();


            Duck = mapper.Map<DuckViewModel>(ducks);
            PopulateList();
            return View(Duck);
        }

        [HttpPost]
        public ActionResult EditDuck(DuckViewModel duck)
        {
            PopulateList();
            duck.CountryList = Duck.CountryList;
            var config = new MapperConfiguration(cfg => cfg.AddProfile(new MapperProfile()));
            var mapper = config.CreateMapper();
            try
            {
                if (ModelState.IsValid)
                {
                    unitOfWork.DuckRepository.Update(mapper.Map<Kaczki>(duck));
                    unitOfWork.Save();
                    //TODO: logger
                    return RedirectToAction("DuckIndex");

                }
            }
            catch (DataException)
            {

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
                    logger.Info(Environment.NewLine + "Duck" + id.ToString() + " deleted by " + CookieHandler.GetUserNameFromCookie("LoginCookie") + " " + DateTime.Now);

                    return RedirectToAction("DuckIndex");
                }
            }
            TempData["error"] = "Cannot delete. Duck used in invoice.";
            return RedirectToAction("DuckIndex");
        }
    }
}
