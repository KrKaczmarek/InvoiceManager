using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApi.Models;

namespace WebApi.Controllers
{
    public class DuckController : ApiController
    {
       public static int NextDuckIndex = 0;
        public IHttpActionResult GetAllDucks()
        {
            IList<DuckViewModel> Ducks = null;
            using (var ctx = new SklepEntities())
            {
                Ducks = ctx.Kaczki.Include("Kaczka_id").Select(I => new DuckViewModel()
                {
                    DuckId = I.Kaczka_id,
                    DuckType = I.Rodzaj,
                    DuckPrice = I.Cena,
                    DuckCountryId = I.Kraj_id


                }).ToList();
            }
            NextDuckIndex = Ducks.Last().DuckId + 1;
            return Ok(Ducks);
        }
        public IHttpActionResult GetDuckById(int id)
        {
            IList<DuckViewModel> Ducks = null;
            using (var ctx = new SklepEntities())
            {
                Ducks = ctx.Kaczki.Include("Kaczka_id").Where(E => E.Kaczka_id == id).Select(I => new DuckViewModel()
                {
                    DuckId = I.Kaczka_id,
                    DuckType = I.Rodzaj,
                    DuckPrice = I.Cena,
                    DuckCountryId = I.Kraj_id

                }).ToList();
            }

            return Ok(Ducks);
        }
        public IHttpActionResult PostNewDuck(DuckViewModel duck)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid data.");

            using (var ctx = new SklepEntities())
            {
                ctx.Kaczki.Add(new Kaczki()
                {
                    Kaczka_id = NextDuckIndex,
                    Rodzaj = duck.DuckType,
                    Cena = duck.DuckPrice,
                    Kraj_id = duck.DuckCountryId

                }); ctx.SaveChanges();
            }
            return Ok();
        }

        public IHttpActionResult PutDuck(DuckViewModel duck)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid data.");

            using (var ctx = new SklepEntities())
            {
                var ExistingDuck = ctx.Kaczki.Where(I => I.Kaczka_id == duck.DuckId).FirstOrDefault();


                if (ExistingDuck != null)
                {

                    ExistingDuck.Rodzaj = duck.DuckType;
                    ExistingDuck.Cena = duck.DuckPrice;
                    ExistingDuck.Kraj_id = duck.DuckCountryId;

                    ctx.SaveChanges();
                }
                else return NotFound();

            }
            return Ok();
        }

        public IHttpActionResult DeleteDuckById(int id)
        {
            if (id < 0)
                return BadRequest("Invalid id");

            using (var ctx = new SklepEntities())
            {
                if (ctx.Faktury.Include("Kaczka_id").Any(K => K.Kaczka_id == id))
                {
                    return BadRequest("Id exists");
                }
                else
                {
                    var duck = ctx.Kaczki.Where(I => I.Kaczka_id == id).FirstOrDefault();
                    ctx.Entry(duck).State = System.Data.Entity.EntityState.Deleted;
                    ctx.SaveChanges();
                }
            }
            return Ok();
        }
    }
}

