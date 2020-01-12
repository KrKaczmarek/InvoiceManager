using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using WebApi.Models;

namespace WebApi.MVCControllers
{
    public class InvoiceController : Controller
    {
       
        IEnumerable<InvoiceViewModel> Invoices { get; set; }
      
        CreateViewModel create = new CreateViewModel();
        public ActionResult Index(string searchString = "", string Answer = "")
        {

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Request.Url.GetLeftPart(UriPartial.Authority));
                var responseTask = client.GetAsync("api/Invoice");
                responseTask.Wait();



                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IList<InvoiceViewModel>>();
                    readTask.Wait();
                    Invoices = readTask.Result;


                }
                else
                {
                    Invoices = Enumerable.Empty<InvoiceViewModel>();
                    ModelState.AddModelError(string.Empty, "Server error");
                }
                if (!String.IsNullOrEmpty(searchString))
                {
                    if (Answer == "EmployeeName")
                        Invoices = Invoices.Where(s => s.Employee.EmployeeName.Contains(searchString));
                    if (Answer == "DuckType")
                        Invoices = Invoices.Where(s => s.Duck.DuckType.Contains(searchString));
                    if (Answer == "SupplierName")
                        Invoices = Invoices.Where(s => s.Supplier.SupplierName.Contains(searchString));
                    if (Answer == "RecipientName")
                        Invoices = Invoices.Where(s => s.Recipient.RecipientName.Contains(searchString));
                }
            }

         
            return View(Invoices);

        }

        public ActionResult Create()
        {
            create.Invoice = new InvoiceViewModel();
            create.Invoice.InvoiceId = WebApi.Controllers.InvoiceController.NextInvoiceIndex;
            create.Invoice.Date = DateTime.Now;
            PopulateList();
            return View(create);

        }

        public void PopulateList()
        {
            List<EmployeeViewModel> employees = new List<EmployeeViewModel>();
            List<DuckViewModel> ducks = new List<DuckViewModel>();
            List<SupplierViewModel> suppliers = new List<SupplierViewModel>();
            List<RecipientViewModel> recipients = new List<RecipientViewModel>();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Request.Url.GetLeftPart(UriPartial.Authority));
                var responseTaskE = client.GetAsync("api/Employee");
                var responseTaskD = client.GetAsync("api/Duck");
                var responseTaskS = client.GetAsync("api/Supplier");
                var responseTaskR = client.GetAsync("api/Recipient");

                responseTaskE.Wait();
                responseTaskD.Wait();
                responseTaskS.Wait();
                responseTaskR.Wait();

                var resultE = responseTaskE.Result;
                var resultD = responseTaskD.Result;
                var resultS = responseTaskS.Result;
                var resultR = responseTaskR.Result;

                if (resultE.IsSuccessStatusCode)
                {
                    var readTask = resultE.Content.ReadAsAsync<IList<EmployeeViewModel>>();
                    readTask.Wait();
                    employees = readTask.Result.ToList();
                }
                if (resultR.IsSuccessStatusCode)
                {
                    var readTask = resultR.Content.ReadAsAsync<IList<RecipientViewModel>>();
                    readTask.Wait();
                    recipients = readTask.Result.ToList();
                }
                if (resultS.IsSuccessStatusCode)
                {
                    var readTask = resultS.Content.ReadAsAsync<IList<SupplierViewModel>>();
                    readTask.Wait();
                    suppliers = readTask.Result.ToList();
                }
                if (resultD.IsSuccessStatusCode)
                {
                    var readTask = resultD.Content.ReadAsAsync<IList<DuckViewModel>>();
                    readTask.Wait();
                    ducks = readTask.Result.ToList();
                }
                else
                {

                    ModelState.AddModelError(string.Empty, "Server error");
                }
                create.EmployeeList = new SelectList(employees, "EmployeeId", "EmployeeName");
                create.DuckList = new SelectList(ducks, "DuckId", "DuckType");
                create.RecipientList = new SelectList(recipients, "RecipientId", "RecipientName");
                create.SupplierList = new SelectList(suppliers, "SupplierId", "SupplierName");
            }
        }

        [HttpPost]
        public ActionResult Create(CreateViewModel create)
        {
          
                this.create = create;
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(Request.Url.GetLeftPart(UriPartial.Authority));
                    var postTask = client.PostAsJsonAsync("api/invoice", create.Invoice);
                    postTask.Wait();

                    var result = postTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index");
                    }

              
            }
            PopulateList();
            ModelState.AddModelError(string.Empty, "Server Error.");


            return View(create);
        }


        public ActionResult Edit(int id)
        {
            IEnumerable<InvoiceViewModel> invoice = null;
            PopulateList();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Request.Url.GetLeftPart(UriPartial.Authority));

                var responseTask = client.GetAsync("api/invoice?id=" + id.ToString());
                responseTask.Wait();

                var result = responseTask.Result;

                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IList<InvoiceViewModel>>();
                    readTask.Wait();

                    invoice = readTask.Result;
                }
            }
            create.Invoice = invoice.SingleOrDefault();
         
            return View(create);
        }

    

        [HttpPost]
        public ActionResult Edit(CreateViewModel create)
        {
         
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Request.Url.GetLeftPart(UriPartial.Authority));


                var putTask = client.PutAsJsonAsync<InvoiceViewModel>("api/invoice", create.Invoice);
                putTask.Wait();

                var result = putTask.Result;
                if (result.IsSuccessStatusCode)
                {
                  
                    return RedirectToAction("Index");
                }
            }
            PopulateList();
            return View(create);
        }

        public ActionResult Delete(int id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Request.Url.GetLeftPart(UriPartial.Authority));


                var deleteTask = client.DeleteAsync("api/invoice/" + id.ToString());
                deleteTask.Wait();

                var result = deleteTask.Result;
                if (result.IsSuccessStatusCode)
                {

                    return RedirectToAction("Index");
                }
            }

            return RedirectToAction("Index");
        }

    }
}
