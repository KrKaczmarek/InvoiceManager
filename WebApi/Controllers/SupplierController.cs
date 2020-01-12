using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApi.Models;

namespace WebApi.Controllers
{
    public class SupplierController : ApiController
    {
        public IHttpActionResult GetAllSuppliers()
        {
            IList<SupplierViewModel> Suppliers = null;
            using (var ctx = new SklepEntities())
            {
                Suppliers = ctx.Dostawcy.Include("Dostawca_id").Select(I => new SupplierViewModel()
                {
                    SupplierId = I.Dostawca_id,
                    SupplierName = I.Dostawca_nazwa,
                    SupplierCountryId = I.Kraj_id


                }).ToList();
            }
            
            return Ok(Suppliers);
        }
        public IHttpActionResult GetSupplierById(string id)
        {
            IList<SupplierViewModel> Suppliers = null;
            using (var ctx = new SklepEntities())
            {
                Suppliers = ctx.Dostawcy.Include("Dostawca_id").Where(E => E.Dostawca_id == id).Select(I => new SupplierViewModel()
                {
                     SupplierId = I.Dostawca_id,
                    SupplierName = I.Dostawca_nazwa,
                    SupplierCountryId = I.Kraj_id

                }).ToList();
            }

            return Ok(Suppliers);
        }
        public IHttpActionResult PostNewSupplier(SupplierViewModel supplier)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid data.");

            using (var ctx = new SklepEntities())
            {
                ctx.Dostawcy.Add(new Dostawcy()
                {
                    Dostawca_id = supplier.SupplierId,
                    Dostawca_nazwa = supplier.SupplierName,
                    Kraj_id = supplier.SupplierCountryId


                }); ctx.SaveChanges();
            }
            return Ok();
        }

        public IHttpActionResult PutRecipient(SupplierViewModel supplier)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid data.");

            using (var ctx = new SklepEntities())
            {
                var ExistingSupplier = ctx.Dostawcy.Where(I => I.Dostawca_id == supplier.SupplierId).FirstOrDefault();


                if (ExistingSupplier != null)
                {

                    ExistingSupplier.Dostawca_id = supplier.SupplierId;
                    ExistingSupplier.Dostawca_nazwa = supplier.SupplierName;
                    ExistingSupplier.Kraj_id = supplier.SupplierCountryId;

                    ctx.SaveChanges();
                }
                else return NotFound();

            }
            return Ok();
        }

        public IHttpActionResult DeleteSupplierById(string id)
        {

            using (var ctx = new SklepEntities())
            {
                if (ctx.Faktury.Include("Dostawca").Any(K => K.Dostawca_id == id))
                {
                    return BadRequest("Id exists");
                }
                {
                    var supplier = ctx.Dostawcy.Where(I => I.Dostawca_id == id).FirstOrDefault();
                    ctx.Entry(supplier).State = System.Data.Entity.EntityState.Deleted;
                    ctx.SaveChanges();
                }
            }
            return Ok();
        }
    }
}

