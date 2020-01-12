using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApi.Models;

namespace WebApi.Controllers
{
    public class InvoiceController : ApiController
    {
        public static int NextInvoiceIndex=0;
        public IHttpActionResult GetAllInvoice()
        {
            IList<InvoiceViewModel> Invoices = null;
            using (var ctx = new SklepEntities())
            {
                Invoices = ctx.Faktury.Include("Faktura_id").Select(I => new InvoiceViewModel()
                {
                    InvoiceId = I.Faktura_id,
                    RecipientId = I.Odbiorca_id,
                    SupplierId = I.Dostawca_id,
                    Quantity = I.Ilosc,
                    EmployeeId = I.Pracownik_id,
                    DuckId = I.Kaczka_id,

                    Recipient = new RecipientViewModel()
                    {
                        RecipientId = I.Odbiorca_id,
                        RecipientName = I.Odbiorcy.Odbiorca_nazwa
                    },

                    Supplier = new SupplierViewModel()
                    {
                        SupplierId = I.Dostawca_id,
                        SupplierName = I.Dostawcy.Dostawca_nazwa
                    },
                    

                    Duck = new DuckViewModel()
                    {
                        DuckId = I.Kaczka_id,
                        DuckType = I.Kaczki.Rodzaj,
                        DuckPrice = I.Kaczki.Cena

                    },
                    Employee = new EmployeeViewModel()
                    {
                        EmployeeId = I.Pracownik_id,
                        EmployeeName = I.Pracownicy.Imie,
                        EmployeeSurname = I.Pracownicy.Nazwisko
                    }

                }).ToList();
            }

            if (Invoices.Count == 0)
                return NotFound();
          
            NextInvoiceIndex = Invoices.Last().InvoiceId + 1;
            return Ok(Invoices);
        }


        public IHttpActionResult GetInvoiceById(int id)
        {

            IList<InvoiceViewModel> Invoices = null;
            using (var ctx = new SklepEntities())
            {
                Invoices = ctx.Faktury.Include("Faktura_id").Where(I => I.Faktura_id == id).Select(I => new InvoiceViewModel()
                {
                    InvoiceId = I.Faktura_id,
                    RecipientId = I.Odbiorca_id,
                    SupplierId = I.Dostawca_id,
                    Quantity = I.Ilosc,
                    EmployeeId=I.Pracownik_id,
                    DuckId=I.Kaczka_id,
                 
                }).ToList();

            }
            if (Invoices.Count == 0)
                return NotFound();

            return Ok(Invoices);
        }

        public IHttpActionResult PostNewInvoice(InvoiceViewModel invoice)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid data.");

            using (var ctx = new SklepEntities())
            {
                ctx.Faktury.Add(new Faktury()
                {
                    Faktura_id = NextInvoiceIndex,

                    Odbiorca_id = invoice.RecipientId.ToString(),

                    Dostawca_id = invoice.SupplierId.ToString(),

                    Kaczka_id = int.Parse(invoice.DuckId.ToString()),

                    Pracownik_id = int.Parse(invoice.EmployeeId.ToString()),

                    Ilosc = invoice.Quantity,
                    Data_wystawienia = DateTime.Now

                });
                ctx.SaveChanges();
            }
            return Ok();
        }

        public IHttpActionResult PutInvoice(InvoiceViewModel invoice)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid data.");

            using (var ctx = new SklepEntities())
            {
                var ExistingInvoice = ctx.Faktury.Where(I => I.Faktura_id == invoice.InvoiceId).FirstOrDefault();


                if (ExistingInvoice != null)
                {
                    //ExistingInvoice.Faktura_id = invoice.InvoiceId;
                    ExistingInvoice.Dostawca_id = invoice.SupplierId;
                    ExistingInvoice.Odbiorca_id = invoice.RecipientId;
                    ExistingInvoice.Kaczka_id = invoice.DuckId;
                    ExistingInvoice.Pracownik_id = invoice.EmployeeId;
                    ExistingInvoice.Ilosc = invoice.Quantity;
                   // ExistingInvoice.Data_wystawienia = invoice.Date;
                 
                    ctx.SaveChanges();
                }
                else return NotFound();

            }
            return Ok();
        }

        public IHttpActionResult DeleteInvoiceById(int id)
        {
            if (id < 0)
                return BadRequest("Invalid id");

            using (var ctx = new SklepEntities())
            {
                var Invoice = ctx.Faktury.Where(I => I.Faktura_id == id).FirstOrDefault();
                ctx.Entry(Invoice).State = System.Data.Entity.EntityState.Deleted;
                ctx.SaveChanges();
            }
            return Ok();
        }
    }
}
