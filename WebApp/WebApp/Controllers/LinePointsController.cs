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
    public class LinePointsController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/LinePoints
        public IQueryable<LinePoint> GetLinePoints()
        {
            return db.LinePoints;
        }

        // GET: api/LinePoints/5
        [ResponseType(typeof(LinePoint))]
        public IHttpActionResult GetLinePoint(int id)
        {
            LinePoint linePoint = db.LinePoints.Find(id);
            if (linePoint == null)
            {
                return NotFound();
            }

            return Ok(linePoint);
        }

        // PUT: api/LinePoints/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutLinePoint(int id, LinePoint linePoint)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != linePoint.LinePointID)
            {
                return BadRequest();
            }

            db.Entry(linePoint).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LinePointExists(id))
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

        // POST: api/LinePoints
        [ResponseType(typeof(LinePoint))]
        public IHttpActionResult PostLinePoint(LinePoint linePoint)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.LinePoints.Add(linePoint);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = linePoint.LinePointID }, linePoint);
        }

        // DELETE: api/LinePoints/5
        [ResponseType(typeof(LinePoint))]
        public IHttpActionResult DeleteLinePoint(int id)
        {
            LinePoint linePoint = db.LinePoints.Find(id);
            if (linePoint == null)
            {
                return NotFound();
            }

            db.LinePoints.Remove(linePoint);
            db.SaveChanges();

            return Ok(linePoint);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool LinePointExists(int id)
        {
            return db.LinePoints.Count(e => e.LinePointID == id) > 0;
        }
    }
}