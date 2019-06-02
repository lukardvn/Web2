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
    public class StationsOnLinesController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/StationsOnLines
        public IQueryable<StationsOnLine> GetStationsOnLines()
        {
            return db.StationsOnLines;
        }

        // GET: api/StationsOnLines/5
        [ResponseType(typeof(StationsOnLine))]
        public IHttpActionResult GetStationsOnLine(int id)
        {
            StationsOnLine stationsOnLine = db.StationsOnLines.Find(id);
            if (stationsOnLine == null)
            {
                return NotFound();
            }

            return Ok(stationsOnLine);
        }

        // PUT: api/StationsOnLines/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutStationsOnLine(int id, StationsOnLine stationsOnLine)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != stationsOnLine.StationsOnLineID)
            {
                return BadRequest();
            }

            db.Entry(stationsOnLine).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StationsOnLineExists(id))
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

        // POST: api/StationsOnLines
        [ResponseType(typeof(StationsOnLine))]
        public IHttpActionResult PostStationsOnLine(StationsOnLine stationsOnLine)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.StationsOnLines.Add(stationsOnLine);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = stationsOnLine.StationsOnLineID }, stationsOnLine);
        }

        // DELETE: api/StationsOnLines/5
        [ResponseType(typeof(StationsOnLine))]
        public IHttpActionResult DeleteStationsOnLine(int id)
        {
            StationsOnLine stationsOnLine = db.StationsOnLines.Find(id);
            if (stationsOnLine == null)
            {
                return NotFound();
            }

            db.StationsOnLines.Remove(stationsOnLine);
            db.SaveChanges();

            return Ok(stationsOnLine);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool StationsOnLineExists(int id)
        {
            return db.StationsOnLines.Count(e => e.StationsOnLineID == id) > 0;
        }
    }
}