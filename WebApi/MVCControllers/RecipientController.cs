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
    public class RecipientController : Controller
    {
        private Logger logger = LogManager.GetCurrentClassLogger();

        static IEnumerable<RecipientViewModel> Recipients;
        RecipientViewModel Recipient = new RecipientViewModel();
        public ActionResult RecipientIndex()
        {

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Request.Url.GetLeftPart(UriPartial.Authority));
                var responseTask = client.GetAsync("api/Recipient");
                responseTask.Wait();



                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IList<RecipientViewModel>>();
                    readTask.Wait();
                    Recipients = readTask.Result;


                }
                else
                {
                    Recipients = Enumerable.Empty<RecipientViewModel>();
                    ModelState.AddModelError(string.Empty, "No recipients in db");
                }
                ViewData["error"] = TempData["error"];
                return View(Recipients);
            }
        }
        public ActionResult CreateRecipient()
        {
            PopulateList();
            return View(Recipient);
        }

        [HttpPost]
        public ActionResult CreateRecipient(RecipientViewModel recipient)
        {
            PopulateList();
            if (CheckIfDuplicatedId(recipient))
                return RedirectToAction("CreateRecipient");
            try
            {
                if (ModelState.IsValid)
                {
                    using (var client = new HttpClient())
                    {
                        client.BaseAddress = new Uri(Request.Url.GetLeftPart(UriPartial.Authority));
                        var postTask = client.PostAsJsonAsync<RecipientViewModel>("api/recipient", recipient);
                        postTask.Wait();

                        var result = postTask.Result;
                        if (result.IsSuccessStatusCode)
                        {
                            logger.Info(Environment.NewLine + "Recipient " + recipient.RecipientId.ToString() + " added by " + CookieHandler.GetUserNameFromCookie("LoginCookie") + " " + DateTime.Now);

                            return RedirectToAction("RecipientIndex");
                        }
                    }
                }
            }
            catch (DataException)
            {
                ModelState.AddModelError(string.Empty, "Cannot create recipient");

            }
            return View(recipient);
        }
        private bool CheckIfDuplicatedId(RecipientViewModel recipient)
        {
            if (Recipients.Where(c => c.RecipientId.Equals(recipient.RecipientId, StringComparison.OrdinalIgnoreCase)).SingleOrDefault() != null)
            {
                TempData["error"] = "Cannot create. ID must be unique.";
                return true;
            }
            return false;
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
            Recipient.CountryList = new SelectList(countries, "CountryId", "CountryName");
        }
        public ActionResult EditRecipient(string id)
        {
            PopulateList();
            IEnumerable<RecipientViewModel> Recipients = null;
            id=id.Trim();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Request.Url.GetLeftPart(UriPartial.Authority));

                var responseTask = client.GetAsync("api/recipient?id=" + id);
                responseTask.Wait();

                var result = responseTask.Result;

                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IList<RecipientViewModel>>();
                    readTask.Wait();

                    Recipients = readTask.Result;
                }
            }
           Recipients.SingleOrDefault().CountryList=Recipient.CountryList;          
            return View(Recipients.SingleOrDefault());
        }

        [HttpPost]
        public ActionResult EditRecipient(RecipientViewModel recipient)
        {
            recipient.CountryList = Recipient.CountryList;
            try
            {
                if (ModelState.IsValid)
                {
                    using (var client = new HttpClient())
                    {
                        client.BaseAddress = new Uri(Request.Url.GetLeftPart(UriPartial.Authority));


                        var putTask = client.PutAsJsonAsync<RecipientViewModel>("api/Recipient", recipient);
                        putTask.Wait();

                        var result = putTask.Result;
                        if (result.IsSuccessStatusCode)
                        {
                            logger.Info(Environment.NewLine + "Recipient " + recipient.RecipientId + " editted by " + CookieHandler.GetUserNameFromCookie("LoginCookie") + " " + DateTime.Now);
                            return RedirectToAction("RecipientIndex");
                        }
                    }
                }
            }catch(DataException)
            {

            }

            PopulateList();
            return View(recipient);
        }

        public ActionResult DeleteRecipient(string id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Request.Url.GetLeftPart(UriPartial.Authority));


                var deleteTask = client.DeleteAsync("api/Recipient/" + id);
                deleteTask.Wait();

                var result = deleteTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    logger.Info(Environment.NewLine + "Recipient " + id + " deleted by " + CookieHandler.GetUserNameFromCookie("LoginCookie") + " " + DateTime.Now);
                    return RedirectToAction("RecipientIndex");
                }
            }
            TempData["error"] = "Cannot delete. Recipient used in invoice.";
            return RedirectToAction("RecipientIndex");
        }
    }
}