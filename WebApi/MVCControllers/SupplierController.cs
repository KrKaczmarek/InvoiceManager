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
    public class SupplierController : Controller
    {
        private Logger logger = LogManager.GetCurrentClassLogger();
        static IEnumerable<SupplierViewModel> Suppliers;
        SupplierViewModel Supplier = new SupplierViewModel();
        public ActionResult SupplierIndex()
        {

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Request.Url.GetLeftPart(UriPartial.Authority));
                var responseTask = client.GetAsync("api/Supplier");
                responseTask.Wait();



                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IList<SupplierViewModel>>();
                    readTask.Wait();
                    Suppliers = readTask.Result;


                }
                else
                {
                    Suppliers = Enumerable.Empty<SupplierViewModel>();
                    ModelState.AddModelError(string.Empty, "No suppliers in db");
                }
                ViewData["error"] = TempData["error"];
                return View(Suppliers);
            }
        }
        public ActionResult CreateSupplier()
        {
            PopulateList();
            return View(Supplier);
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
            Supplier.CountryList = new SelectList(countries, "CountryId", "CountryName");
        }
        [HttpPost]
        public ActionResult CreateSupplier(SupplierViewModel supplier)
        {
            PopulateList();         
            if (CheckIfDuplicatedId(supplier))
                return RedirectToAction("CreateSupplier");
            try
            {
                if (ModelState.IsValid)
                {
                    using (var client = new HttpClient())
                    {
                        client.BaseAddress = new Uri(Request.Url.GetLeftPart(UriPartial.Authority));
                        var postTask = client.PostAsJsonAsync<SupplierViewModel>("api/supplier", supplier);
                        postTask.Wait();

                        var result = postTask.Result;
                        if (result.IsSuccessStatusCode)
                        {
                            logger.Info(Environment.NewLine + "Supplier " + supplier.SupplierId + " added by " + CookieHandler.GetUserNameFromCookie("LoginCookie") + " " + DateTime.Now);

                            return RedirectToAction("SupplierIndex");
                        }
                    }
                }
            }
            catch (DataException)
            {
                ModelState.AddModelError(string.Empty, "Cannot create supplier");
            }

            return View(supplier);
        }
        private bool CheckIfDuplicatedId(SupplierViewModel supplier)
        {
            if (Suppliers.Where(c => c.SupplierId.Equals(supplier.SupplierId, StringComparison.OrdinalIgnoreCase)).SingleOrDefault() != null)
            {
                TempData["error"] = "Cannot create. ID must be unique.";
                return true;
            }
            return false;
        }
        public ActionResult EditSupplier(string id)
        {
            IEnumerable<SupplierViewModel> Suppliers = null;
            PopulateList();
            try
            {
                if (ModelState.IsValid)
                {
                    using (var client = new HttpClient())
                    {
                        client.BaseAddress = new Uri(Request.Url.GetLeftPart(UriPartial.Authority));

                        var responseTask = client.GetAsync("api/supplier?id=" + id);
                        responseTask.Wait();

                        var result = responseTask.Result;

                        if (result.IsSuccessStatusCode)
                        {
                            var readTask = result.Content.ReadAsAsync<IList<SupplierViewModel>>();
                            readTask.Wait();

                            Suppliers = readTask.Result;
                        }
                    }
                }
            }
            catch (DataException) {  }
            Suppliers.SingleOrDefault().CountryList = Supplier.CountryList;
            return View(Suppliers.SingleOrDefault());
        }

        [HttpPost]
        public ActionResult EditSupplier(SupplierViewModel supplier)
        {
            supplier.CountryList = Supplier.CountryList;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Request.Url.GetLeftPart(UriPartial.Authority));


                var putTask = client.PutAsJsonAsync<SupplierViewModel>("api/Supplier", supplier);
                putTask.Wait();

                var result = putTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    logger.Info(Environment.NewLine + "Recipient " + supplier.SupplierId + " idited by " + CookieHandler.GetUserNameFromCookie("LoginCookie") + " " + DateTime.Now);

                    return RedirectToAction("SupplierIndex");
                }
            }
            PopulateList();
            return View(supplier);
        }

        public ActionResult DeleteSupplier(string id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Request.Url.GetLeftPart(UriPartial.Authority));


                var deleteTask = client.DeleteAsync("api/Supplier/" + id);
                deleteTask.Wait();

                var result = deleteTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    logger.Info(Environment.NewLine + "Supplier" + id + " deleted by " + CookieHandler.GetUserNameFromCookie("LoginCookie") + " " + DateTime.Now);

                    return RedirectToAction("SupplierIndex");
                }
            }
            TempData["error"] = "Cannot delete. Supplier used in invoice.";
            return RedirectToAction("SupplierIndex");
        }
    }
}
