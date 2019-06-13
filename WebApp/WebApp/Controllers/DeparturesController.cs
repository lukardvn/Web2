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
using WebApp.Persistence.UnitOfWork;

namespace WebApp.Controllers
{
    //[AllowAnonymous]
    public class DeparturesController : ApiController
    {
        private IUnitOfWork unitOfWork;
        private ApplicationDbContext context = new ApplicationDbContext();

        public DeparturesController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public DeparturesController() { }


        // GET: api/Departures
        [AllowAnonymous]
        public IEnumerable<Departures> GetDepartures()
        {
            return unitOfWork.Departures.GetAll();
            //return context.Departures;
        }

        // GET: api/Departures/5
        [ResponseType(typeof(Departures))]
        [AllowAnonymous]
        public IHttpActionResult GetDepartures(int id)
        {
            Departures departures = unitOfWork.Departures.Get(id);
            if (departures == null)
            {
                return NotFound();
            }

            return Ok(departures);
        }

        // PUT: api/Departures/5
        [ResponseType(typeof(void))]
        [Authorize(Roles = "Admin")]
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


            try
            {
                //bez dto, ako bude vremena ubacicu,ne bi trebalo da se menja bilo sta ovako ali ajde
                var findExistingDeparture = unitOfWork.Departures.Get(departures.ID);
                if (findExistingDeparture == null)
                    return NotFound();
                //menjanje tabele i linije na tabeli
                findExistingDeparture.TimeTable = departures.TimeTable;
                findExistingDeparture.TransportLineID = departures.TransportLineID;

                unitOfWork.Departures.Update(findExistingDeparture);
                unitOfWork.Complete();
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
        [Authorize(Roles = "Admin")]
        public IHttpActionResult PostDepartures(Departures departures)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {

                departures.ValidFrom = DateTime.Now;
                unitOfWork.Departures.Add(departures);
                unitOfWork.Complete();
            }
            catch
            {
                throw;
            }

            return CreatedAtRoute("DefaultApi", new { id = departures.ID }, departures);
        }

        // DELETE: api/Departures/5
        [ResponseType(typeof(Departures))]
        [Authorize(Roles = "Admin")]
        public IHttpActionResult DeleteDepartures(int id)
        {
            Departures departures = unitOfWork.Departures.Get(id);
            if (departures == null)
            {
                return NotFound();
            }

            try
            {
                unitOfWork.Departures.Remove(departures);
                unitOfWork.Complete();
            }
            catch
            {
                throw;
            }

            return Ok(departures);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                unitOfWork.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool DeparturesExists(int id)
        {
            return unitOfWork.Departures.Find(d => d.ID == id).Count() > 0;
        }
    }
}