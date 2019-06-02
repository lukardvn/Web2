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
    public class DeparturesController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/Departures
        public IQueryable<Departures> GetDepartures()
        {
            return db.Departures;
        }

        // GET: api/Departures/5
        [ResponseType(typeof(Departures))]
        public IHttpActionResult GetDepartures(int id)
        {
            Departures departures = db.Departures.Find(id);
            if (departures == null)
            {
                return NotFound();
            }

            return Ok(departures);
        }

        // PUT: api/Departures/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutDepartures(int id, Departures departures)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != departures.ID)
            {
                return BadRequest();
            }

            db.Entry(departures).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DeparturesExists(id))
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

        // POST: api/Departures
        [ResponseType(typeof(Departures))]
        public IHttpActionResult PostDepartures(Departures departures)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Departures.Add(departures);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = departures.ID }, departures);
        }

        // DELETE: api/Departures/5
        [ResponseType(typeof(Departures))]
        public IHttpActionResult DeleteDepartures(int id)
        {
            Departures departures = db.Departures.Find(id);
            if (departures == null)
            {
                return NotFound();
            }

            db.Departures.Remove(departures);
            db.SaveChanges();

            return Ok(departures);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool DeparturesExists(int id)
        {
            return db.Departures.Count(e => e.ID == id) > 0;
        }
    }
}