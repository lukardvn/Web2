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
    public class PricelistsController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/Pricelists
        public IQueryable<Pricelist> GetPricelists()
        {
            return db.Pricelists;
        }

        // GET: api/Pricelists/5
        [ResponseType(typeof(Pricelist))]
        public IHttpActionResult GetPricelist(int id)
        {
            Pricelist pricelist = db.Pricelists.Find(id);
            if (pricelist == null)
            {
                return NotFound();
            }

            return Ok(pricelist);
        }

        // PUT: api/Pricelists/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutPricelist(int id, Pricelist pricelist)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != pricelist.ID)
            {
                return BadRequest();
            }

            db.Entry(pricelist).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PricelistExists(id))
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

        // POST: api/Pricelists
        [ResponseType(typeof(Pricelist))]
        public IHttpActionResult PostPricelist(Pricelist pricelist)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Pricelists.Add(pricelist);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = pricelist.ID }, pricelist);
        }

        // DELETE: api/Pricelists/5
        [ResponseType(typeof(Pricelist))]
        public IHttpActionResult DeletePricelist(int id)
        {
            Pricelist pricelist = db.Pricelists.Find(id);
            if (pricelist == null)
            {
                return NotFound();
            }

            db.Pricelists.Remove(pricelist);
            db.SaveChanges();

            return Ok(pricelist);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PricelistExists(int id)
        {
            return db.Pricelists.Count(e => e.ID == id) > 0;
        }
    }
}