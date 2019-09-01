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
    public class StationsController : ApiController
    {
        private IUnitOfWork unitOfWork;


        public StationsController() { }
        public StationsController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }



        // GET: api/Stations
        public IEnumerable<Station> GetStations()
        {
            return unitOfWork.Station.GetAll();
        }

        //brisanje stanica sa svim vezama sa linijama
        [AllowAnonymous]
        [ResponseType(typeof(string))]
        [Route("api/stations/deleteStation/{id}")]
        public IHttpActionResult DeleteStation(int id)
        {
            Station st = unitOfWork.Station.Get(id);
            if (st == null)
            {
                return NotFound();
            }
            IQueryable<StationsOnLine> stanice = unitOfWork.StationsOnLine.GetAll().AsQueryable();
            foreach (var it in stanice)
            {
                if(it.StationID == st.StationID)
                {
                    unitOfWork.StationsOnLine.Remove(it);
                }
            }

            unitOfWork.Station.Remove(st);
            unitOfWork.Complete();

            return Ok("obrisan red");
        }

        [AllowAnonymous]
        [Route("api/stations/getAllStations")]
        public List<int> GetAllStations()
        {
            IQueryable<Station> sta = unitOfWork.Station.GetAll().AsQueryable();
            List<int> redovi = new List<int>();
            foreach (Station l in sta)
            {
                redovi.Add(l.StationID);
            }
            return redovi;
        }

        [AllowAnonymous]
        [Route("api/stations/getAllStationsForUpdate")]
        public List<StanicaUpdate> GetAllStationsForUpdate()
        {
            IQueryable<Station> sta = unitOfWork.Station.GetAll().AsQueryable();
            List<StanicaUpdate> redovi = new List<StanicaUpdate>();
            foreach (Station l in sta)
            {
                StanicaUpdate st = new StanicaUpdate();
                st.ID = l.StationID;
                st.Naziv = l.Name;
                st.Adresa = l.Address;
                st.X = l.X;
                st.Y = l.Y;
                redovi.Add(st);
            }
            return redovi;
        }

        [AllowAnonymous]
        [Route("api/stations/updateStanicu/")]
        public IHttpActionResult UpdateStanicu(StanicaUpdate stanica)
        {

            Station tl = unitOfWork.Station.Get(stanica.ID);
            tl.Name = stanica.Naziv;
            tl.Address = stanica.Adresa;
            tl.X = stanica.X;
            tl.Y = stanica.Y;


            unitOfWork.Station.Update(tl);
            unitOfWork.Complete();

            return Ok("Dodato");
        }

        [AllowAnonymous]
        [Route("api/stations/dodajStanicu/")]
        public IHttpActionResult DodajStanicu(Stanica stanica)
        {

            Station st = new Station();
            st.Name = stanica.Naziv;
            st.X = stanica.X;
            st.Y = stanica.Y;
            st.Address = stanica.Adresa;

            unitOfWork.Station.Add(st);
            unitOfWork.Complete();

            return Ok("Dodato");
        }

        // GET: api/Stations/5
        [ResponseType(typeof(Station))]
        public IHttpActionResult GetStation(int id)
        {
            Station station = unitOfWork.Station.Get(id);
            if (station == null)
            {
                return NotFound();
            }

            return Ok(station);
        }

        // PUT: api/Stations/5
        [ResponseType(typeof(void))]
        [Authorize(Roles = "Admin")]
        public IHttpActionResult PutStation(int id, Station station)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != station.StationID)
            {
                return BadRequest();
            }

            

            try
            {
                unitOfWork.Station.Update(station);
                unitOfWork.Complete();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StationExists(id))
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

        // POST: api/Stations
        [ResponseType(typeof(Station))]
        [Authorize(Roles = "Admin")]
        public IHttpActionResult PostStation(Station station)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                unitOfWork.Station.Add(station);
                unitOfWork.Complete();
            }
            catch
            {
                throw;
            }

            return CreatedAtRoute("DefaultApi", new { id = station.StationID }, station);
        }

        // DELETE: api/Stations/5
        //[ResponseType(typeof(Station))]
        //[Authorize(Roles = "Admin")]
        //public IHttpActionResult DeleteStation(int id)
        //{
        //    Station station = unitOfWork.Station.Get(id);
        //    if (station == null)
        //    {
        //        return NotFound();
        //    }

        //    try
        //    {
        //        unitOfWork.Station.Remove(station);
        //        unitOfWork.Complete();
        //    }
        //    catch
        //    {
        //        throw;
        //    }

        //    return Ok(station);
        //}

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                unitOfWork.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool StationExists(int id)
        {
            return unitOfWork.Station.Find(e => e.StationID == id).Count() > 0;
        }
    }
}