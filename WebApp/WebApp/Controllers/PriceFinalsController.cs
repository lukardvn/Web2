using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using WebApp.Models.DomainEntities;
using WebApp.Persistence;

namespace WebApp.Controllers
{
    public class PriceFinalsController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/PriceFinals
        public IQueryable<PriceFinal> GetPriceFinals()
        {
            return db.PriceFinals;
        }

        // GET: api/PriceFinals/5
        [ResponseType(typeof(PriceFinal))]
        public IHttpActionResult GetPriceFinal(int id)
        {
            PriceFinal priceFinal = db.PriceFinals.Find(id);
            if (priceFinal == null)
            {
                return NotFound();
            }

            return Ok(priceFinal);
        }

        // PUT: api/PriceFinals/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutPriceFinal(int id, PriceFinal priceFinal)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != priceFinal.ID)
            {
                return BadRequest();
            }

            db.Entry(priceFinal).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PriceFinalExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/PriceFinals
        [ResponseType(typeof(PriceFinal))]
        public IHttpActionResult PostPriceFinal(PriceFinal priceFinal)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.PriceFinals.Add(priceFinal);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = priceFinal.ID }, priceFinal);
        }

        // DELETE: api/PriceFinals/5
        [ResponseType(typeof(PriceFinal))]
        public IHttpActionResult DeletePriceFinal(int id)
        {
            PriceFinal priceFinal = db.PriceFinals.Find(id);
            if (priceFinal == null)
            {
                return NotFound();
            }

            db.PriceFinals.Remove(priceFinal);
            db.SaveChanges();

            return Ok(priceFinal);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PriceFinalExists(int id)
        {
            return db.PriceFinals.Count(e => e.ID == id) > 0;
        }
    }
}