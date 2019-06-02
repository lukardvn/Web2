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
    public class TransportLinesController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/TransportLines
        public IQueryable<TransportLine> GetTransportLines()
        {
            return db.TransportLines;
        }

        // GET: api/TransportLines/5
        [ResponseType(typeof(TransportLine))]
        public IHttpActionResult GetTransportLine(int id)
        {
            TransportLine transportLine = db.TransportLines.Find(id);
            if (transportLine == null)
            {
                return NotFound();
            }

            return Ok(transportLine);
        }

        // PUT: api/TransportLines/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutTransportLine(int id, TransportLine transportLine)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != transportLine.TransportLineID)
            {
                return BadRequest();
            }

            db.Entry(transportLine).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TransportLineExists(id))
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

        // POST: api/TransportLines
        [ResponseType(typeof(TransportLine))]
        public IHttpActionResult PostTransportLine(TransportLine transportLine)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.TransportLines.Add(transportLine);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = transportLine.TransportLineID }, transportLine);
        }

        // DELETE: api/TransportLines/5
        [ResponseType(typeof(TransportLine))]
        public IHttpActionResult DeleteTransportLine(int id)
        {
            TransportLine transportLine = db.TransportLines.Find(id);
            if (transportLine == null)
            {
                return NotFound();
            }

            db.TransportLines.Remove(transportLine);
            db.SaveChanges();

            return Ok(transportLine);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TransportLineExists(int id)
        {
            return db.TransportLines.Count(e => e.TransportLineID == id) > 0;
        }
    }
}