using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApi.Models;

namespace WebApi.Controllers
{
    public class RecipientController : ApiController
    {
       
        public IHttpActionResult GetAllRecipients()
        {
            IList<RecipientViewModel> Recipients = null;
            using (var ctx = new SklepEntities())
            {
                Recipients = ctx.Odbiorcy.Include("Odbiorca_id").Select(I => new RecipientViewModel()
                {
                  RecipientId=I.Odbiorca_id,
                  RecipientName=I.Odbiorca_nazwa,
                  RecipientCountryId=I.Kraj_id


                }).ToList();
            }
       
            return Ok(Recipients);
        }
        public IHttpActionResult GetRecipientById(string id)
        {
            IList<RecipientViewModel> Recipients = null;
            using (var ctx = new SklepEntities())
            {
                Recipients = ctx.Odbiorcy.Include("Odbiorca_id").Where(E => E.Odbiorca_id == id).Select(I => new RecipientViewModel()
                {
                    RecipientId = I.Odbiorca_id,
                    RecipientName = I.Odbiorca_nazwa,
                    RecipientCountryId = I.Kraj_id

                }).ToList();
            }

            return Ok(Recipients);
        }
        public IHttpActionResult PostNewRecipient(RecipientViewModel recipient)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid data.");

            using (var ctx = new SklepEntities())
            {
                ctx.Odbiorcy.Add(new Odbiorcy()
                {
                    Odbiorca_id = recipient.RecipientId,
                     Odbiorca_nazwa = recipient.RecipientName,
                     Kraj_id = recipient.RecipientCountryId
                                       

                }); ctx.SaveChanges();
            }
            return Ok();
        }

        public IHttpActionResult PutRecipient(RecipientViewModel recipient)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid data.");

            using (var ctx = new SklepEntities())
            {
                var ExistingRecipient = ctx.Odbiorcy.Where(I => I.Odbiorca_id == recipient.RecipientId).FirstOrDefault();


                if (ExistingRecipient != null)
                {

                    ExistingRecipient.Odbiorca_id = recipient.RecipientId;
                    ExistingRecipient.Odbiorca_nazwa = recipient.RecipientName;
                    ExistingRecipient.Kraj_id = recipient.RecipientCountryId;

                    ctx.SaveChanges();
                }
                else return NotFound();

            }
            return Ok();
        }

        public IHttpActionResult DeleteRecipientById(string id)
        {
            
            using (var ctx = new SklepEntities())
            {
                if (ctx.Faktury.Include("Odbiorca_id").Any(K => K.Odbiorca_id == id))
                {
                    return BadRequest("Id exists");
                }
                {
                    var recipient = ctx.Odbiorcy.Where(I => I.Odbiorca_id == id).FirstOrDefault();
                    ctx.Entry(recipient).State = System.Data.Entity.EntityState.Deleted;
                    ctx.SaveChanges();
                }
            }
            return Ok();
        }
    }
}

