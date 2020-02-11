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
    public class InvoiceController : Controller
    {
      
        IEnumerable<Faktury> Invoices { get; set; }

        UnitOfWork unitOfWork = new UnitOfWork();
        InvoiceViewModel invoiceToEdit = new InvoiceViewModel();
        private static int NextInvoiceIndex;
        public ActionResult Index(string searchString = "", string Answer = "")
        {

            var config = new MapperConfiguration(cfg => cfg.AddProfile(new MapperProfile()));
            var mapper = config.CreateMapper();

            if (!String.IsNullOrEmpty(searchString))
            {
                if (Answer == "EmployeeName")
                    Invoices = unitOfWork.InvoiceRepository.Get(s => s.Pracownicy.Imie.Contains(searchString));
                if (Answer == "DuckType")
                    Invoices = unitOfWork.InvoiceRepository.Get(s => s.Kaczki.Rodzaj.Contains(searchString));
                if (Answer == "SupplierName")
                    Invoices = unitOfWork.InvoiceRepository.Get(s => s.Dostawcy.Dostawca_nazwa.Contains(searchString));
                if (Answer == "RecipientName")
                    Invoices = unitOfWork.InvoiceRepository.Get(s => s.Odbiorcy.Odbiorca_nazwa.Contains(searchString));
            }
            else
            {
                Invoices = unitOfWork.InvoiceRepository.Get(includeProperties: "");
            }

            var inv = mapper.Map<IEnumerable<InvoiceViewModel>>(Invoices);


            foreach (var i in inv) // add employee,recipient,supplier,duck data to invoice
            {
                i.Employee = mapper.Map<EmployeeViewModel>(unitOfWork.EmployeeRepository.GetByID(i.EmployeeId));
                i.Recipient = mapper.Map<RecipientViewModel>(unitOfWork.RecipientRepository.GetByID(i.RecipientId));
                i.Supplier = mapper.Map<SupplierViewModel>(unitOfWork.SupplierRepository.GetByID(i.SupplierId));
                i.Duck = mapper.Map<DuckViewModel>(unitOfWork.DuckRepository.GetByID(i.DuckId));
            }
            NextInvoiceIndex = inv.Last().InvoiceId + 1;
            return View(inv.ToList());


        }

        public ActionResult Create()
        {
            invoiceToEdit = new InvoiceViewModel();
            invoiceToEdit.InvoiceId = NextInvoiceIndex;
            invoiceToEdit.Date = DateTime.Now;
            PopulateDropDownList();
            return View(invoiceToEdit);

        }

        public void PopulateDropDownList()
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
                invoiceToEdit.EmployeeList = new SelectList(employees, "EmployeeId", "EmployeeName");
                invoiceToEdit.DuckList = new SelectList(ducks, "DuckId", "DuckType");
                invoiceToEdit.RecipientList = new SelectList(recipients, "RecipientId", "RecipientName");
                invoiceToEdit.SupplierList = new SelectList(suppliers, "SupplierId", "SupplierName");
            }
        }

        [HttpPost]
        public ActionResult Create(InvoiceViewModel create)
        {
            this.invoiceToEdit =create;
                     
            PopulateDropDownList();
            var config = new MapperConfiguration(cfg => cfg.AddProfile(new MapperProfile()));
            var mapper = config.CreateMapper();
            invoiceToEdit.Date = DateTime.Now;
            try
            {
                if (ModelState.IsValid)
                {
                    unitOfWork.InvoiceRepository.Insert(mapper.Map<Faktury>(invoiceToEdit));
                    unitOfWork.Save();
                    return RedirectToAction("Index");
                  
                }
            }
            catch (DataException)
            {
                ModelState.AddModelError(string.Empty, "Server Error.");
            
            }

            return View(invoiceToEdit);

        }



        public ActionResult Edit(int id)
        {


            var courses = unitOfWork.InvoiceRepository.GetByID(id);
            var config = new MapperConfiguration(cfg => cfg.AddProfile(new MapperProfile()));
            var mapper = config.CreateMapper();



            invoiceToEdit = mapper.Map<InvoiceViewModel>(courses);
            PopulateDropDownList();
            return View(invoiceToEdit);

        }



        [HttpPost]
        public ActionResult Edit(InvoiceViewModel invoiceToEdit)
        {
            PopulateDropDownList();
            var config = new MapperConfiguration(cfg => cfg.AddProfile(new MapperProfile()));
            var mapper = config.CreateMapper();
            try
            {
                if (ModelState.IsValid)
                {
                    unitOfWork.InvoiceRepository.Update(mapper.Map<Faktury>(invoiceToEdit));
                    unitOfWork.Save();
                    //TODO: logger
                    return RedirectToAction("Index");
                }
            }
            catch (DataException)
            {
                //TODO: logger
            }


            return View(invoiceToEdit);
            

        }

        public ActionResult Delete(int id)
        {          
       
            unitOfWork.InvoiceRepository.Delete(id);
            unitOfWork.Save();
            //TODO: logger

            return RedirectToAction("Index");
        }
        public ActionResult UnAuthorized()
        {
            return View();
        }
    }
}
