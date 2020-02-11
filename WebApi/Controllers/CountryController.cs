using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApi.Models;

namespace WebApi.Controllers
{
    public class CountryController : ApiController
    {
        
        public IHttpActionResult GetAllCountries()
        {
            IList<CountryViewModel> Countries = null;
            using (var ctx = new SklepEntities())
            {
                Countries = ctx.Kraje.Include("Kraj_id").Select(I => new CountryViewModel()
                {
                    CountryId=I.Kraj_id,
                    CountryName=I.Kraj_nazwa


                }).ToList();
            }
          
            return Ok(Countries);
        }
        public IHttpActionResult GetCountryById(string id)
        {
            IList<CountryViewModel> Countries = null;
            using (var ctx = new SklepEntities())
            {
                Countries = ctx.Kraje.Include("Kraje_id").Where(E => E.Kraj_id == id).Select(I => new CountryViewModel()
                {
                   
                    CountryId = I.Kraj_id,
                   CountryName=I.Kraj_nazwa

                }).ToList();
            }

            return Ok(Countries);
        }
        public IHttpActionResult PostNewCountry(CountryViewModel country)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid data.");

            using (var ctx = new SklepEntities())
            {
                ctx.Kraje.Add(new Kraje()
                {

                  Kraj_id = country.CountryId,
                  Kraj_nazwa = country.CountryName,
                

                }); ctx.SaveChanges();
            }
            return Ok();
        }

        public IHttpActionResult PutCountry(CountryViewModel country)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid data.");

            using (var ctx = new SklepEntities())
            {
                var ExistingCountry = ctx.Kraje.Where(I => I.Kraj_id == country.CountryId).FirstOrDefault();


                if (ExistingCountry != null)
                {

                    ExistingCountry.Kraj_id = country.CountryId;
                    ExistingCountry.Kraj_nazwa = country.CountryName;
                

                    ctx.SaveChanges();
                }
                else return NotFound();

            }
            return Ok();
        }

        public IHttpActionResult DeleteCountryById(string id)
        {
         

            using (var ctx = new SklepEntities())
            {
                var country = ctx.Kraje.Where(I => I.Kraj_id == id).FirstOrDefault();
                ctx.Entry(country).State = System.Data.Entity.EntityState.Deleted;
                ctx.SaveChanges();
            }
            return Ok();
        }
    }
}
