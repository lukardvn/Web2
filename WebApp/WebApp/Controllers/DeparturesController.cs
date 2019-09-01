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

        [AllowAnonymous]
        [ResponseType(typeof(string))]
        [Route("api/departures/deleteDepartures/{id}")]
        public IHttpActionResult DeleteDepartures(int id)
        {
            Departures dep = unitOfWork.Departures.Get(id);
            if (dep == null)
            {
                return NotFound();
            }

            unitOfWork.Departures.Remove(dep);
            unitOfWork.Complete();

            return Ok("obrisan red");
        }

        [AllowAnonymous]
        [Route("api/departures/getAllDepartures")]
        public List<int> GetAllDepartures()
        {
            IQueryable<Departures> dep = unitOfWork.Departures.GetAll().AsQueryable();
            List<int> redovi = new List<int>();
            foreach (Departures l in dep)
            {
                redovi.Add(l.ID);
            }
            return redovi;
        }

        [AllowAnonymous]
        [Route("api/departures/getAllDeparturesForUpdate")]
        public List<RedVoznjeUpdate> GetAllDeparturesForUpdate()
        {
            IQueryable<Departures> dep = unitOfWork.Departures.GetAll().AsQueryable();
            List<RedVoznjeUpdate> redovi = new List<RedVoznjeUpdate>();
            foreach (Departures l in dep)
            {
                RedVoznjeUpdate rv = new RedVoznjeUpdate();
                rv.ID = l.ID;
                rv.TransportLineID = l.TransportLineID;
                rv.TimeTable = l.TimeTable;
                redovi.Add(rv);
            }
            return redovi;
        }


        [AllowAnonymous]
        [Route("api/departures/updateDeparture/")]
        public IHttpActionResult UpdateRedVoznje(RedVoznjeUpdate rv)
        {

            Departures tl = unitOfWork.Departures.Get(rv.ID);
            tl.TimeTable = rv.TimeTable;
            tl.TransportLineID = rv.TransportLineID;
           


            unitOfWork.Departures.Update(tl);
            unitOfWork.Complete();

            return Ok("Dodato");
        }

        //dodavanje reda voznje
        [AllowAnonymous]
        [Route("api/departures/postDeparture/")]
        public IHttpActionResult PostDeparture(RedVoznje rv)
        {
            Departures dep = new Departures();
            dep.TimeTable = rv.TimeTable;
            dep.TransportLineID = rv.TransportLineID;
            dep.ValidFrom = DateTime.Now;
            unitOfWork.Departures.Add(dep);
            unitOfWork.Complete();

            return Ok("ok");
        }

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

        //bice ujedno i za brisanje i za editovanje i za dodavanje novih jer je timetable string pa je svejedno 
        [HttpPut]
        [Route("api/departures/putDeparturesForLine")]
        [Authorize(Roles = "Admin")]
        public IHttpActionResult PutDeparturesForLine(Departures departures)
        {

            var l = unitOfWork.Departures.Find(x => x.TransportLineID == departures.TransportLineID).FirstOrDefault();
            try
            {
                l.TimeTable = departures.TimeTable;
                
                unitOfWork.Departures.Update(l);
                unitOfWork.Complete();
            }catch(DbUpdateConcurrencyException)
            {
                if (!DeparturesExists(departures.ID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return Ok();
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

        [Route("api/departures/getDeparturesByLine/{id}")]
        [HttpGet]
        public IHttpActionResult GetDeparturesByLine(string id)
        {
            //var lines = unitOfWork.TransportLine.Get(id);
            return Ok(unitOfWork.Departures.Find(x => x.TransportLineID == id).FirstOrDefault());
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

                //departures.ValidFrom = DateTime.Now;
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
        //[ResponseType(typeof(Departures))]
        //[Authorize(Roles = "Admin")]
        //public IHttpActionResult DeleteDepartures(int id)
        //{
        //    Departures departures = unitOfWork.Departures.Get(id);
        //    if (departures == null)
        //    {
        //        return NotFound();
        //    }

        //    try
        //    {
        //        unitOfWork.Departures.Remove(departures);
        //        unitOfWork.Complete();
        //    }
        //    catch
        //    {
        //        throw;
        //    }

        //    return Ok(departures);
        //}

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