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
    public class TransportLinesController : ApiController
    {
        private IUnitOfWork unitOfWork { get; set; }

        public TransportLinesController() { }

        public TransportLinesController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        //prikaz svih linija
        [AllowAnonymous]
        [Route("api/transportlines/getAllLines")]
        public List<string> GetAllLines()
        {
            IQueryable<TransportLine> linije = unitOfWork.TransportLine.GetAll().AsQueryable();
            List<string> BrojeviLinija = new List<string>();
            foreach (TransportLine l in linije)
            {
                BrojeviLinija.Add(l.TransportLineID);
            }
            return BrojeviLinija;
        }

        [AllowAnonymous]
        [Route("api/transportlines/getAllLinesForUpdate")]
        public List<Linije> GetAllLinesForUpdate()
        {
            IQueryable<TransportLine> linije = unitOfWork.TransportLine.GetAll().AsQueryable();
            List<Linije> BrojeviLinija = new List<Linije>();
            foreach (TransportLine l in linije)
            {
                Linije lin = new Linije();
                lin.TransportLineID = l.TransportLineID;
                lin.FromTo = l.FromTo;
                BrojeviLinija.Add(lin);
            }
            return BrojeviLinija;
        }

        [AllowAnonymous]
        [ResponseType(typeof(string))]
        [Route("api/transportLines/deleteLine/{id}")]
        public IHttpActionResult DeleteLine(string id)
        {
            TransportLine st = unitOfWork.TransportLine.Get(id);
            if (st == null)
            {
                return NotFound();
            }
            IQueryable<StationsOnLine> stanice = unitOfWork.StationsOnLine.GetAll().AsQueryable();
            foreach (var it in stanice)
            {
                if (it.TransportLineID == st.TransportLineID)
                {
                    unitOfWork.StationsOnLine.Remove(it);
                }
            }

            unitOfWork.TransportLine.Remove(st);
            unitOfWork.Complete();

            return Ok("obrisana linija");
        }

        [HttpPost]
        [Route("api/transportlines/addStationInLine/")]
        [AllowAnonymous]
        public IHttpActionResult AddStationInLine(StaniceNaLiniji bravo)
        {
            StationsOnLine st = new StationsOnLine();
            st.StationID = bravo.StationID;
            st.TransportLineID = bravo.TransportLineID;
            unitOfWork.StationsOnLine.Add(st);
            unitOfWork.Complete();

            //var line = unitOfWork.TransportLine.Find(x => x.TransportLineID == id).FirstOrDefault();

            //try
            //{
            //    unitOfWork.Station.Add(externalStation);
            //    unitOfWork.Complete();
            //}
            //catch (DBConcurrencyException)
            //{

            //}

            //try
            //{
            //    line.Stations.Add(externalStation);
            //    unitOfWork.StationsOnLine.Add(new StationsOnLine()
            //    {
            //        StationID = externalStation.StationID,
            //        TransportLineID = line.TransportLineID
            //    });
            //    unitOfWork.Complete();
            //}
            //catch (DBConcurrencyException)
            //{

            //}



            return Ok("uspesno dodato");
        }

        [AllowAnonymous]
        [Route("api/transportLines/dodajLiniju/")]
        public IHttpActionResult DodajLiniju(Linije linija)
        {

            TransportLine tl = new TransportLine();
            tl.TransportLineID = linija.TransportLineID;
            tl.FromTo = linija.FromTo;

            unitOfWork.TransportLine.Add(tl);
            unitOfWork.Complete();

            return Ok("Dodato");
        }

        [AllowAnonymous]
        [Route("api/transportLines/updateLiniju/")]
        public IHttpActionResult UpdateLiniju(Linije linija)
        {

            TransportLine tl = unitOfWork.TransportLine.Get(linija.TransportLineID);
            tl.FromTo = linija.FromTo;

            unitOfWork.TransportLine.Update(tl);
            unitOfWork.Complete();

            return Ok("Dodato");
        }


        [HttpPut]
        [Route("api/transportlines/editStationInLine/{id}")]
        [ResponseType(typeof(void))]
        [Authorize(Roles = "Admin")]
        public IHttpActionResult EditStationInLine(string id, Station externalStation)
        {
            var line = unitOfWork.TransportLine.Find(x => x.TransportLineID == id).FirstOrDefault();

            var stat = unitOfWork.Station.Find(s => s.StationID == externalStation.StationID).FirstOrDefault();
            stat.Name = externalStation.Name;
            stat.X = externalStation.X;
            stat.Y = externalStation.Y;
            stat.Address = externalStation.Address;
            try
            {
                unitOfWork.Station.Update(stat);
                unitOfWork.Complete();
            }
            catch (DBConcurrencyException)
            {

            }



            //return StatusCode(HttpStatusCode.NoContent);
            return Ok(stat);
        }

        [ResponseType(typeof(ICollection<Station>))]
        [HttpGet]
        [Route("api/transportlines/GetStationsForTransportLine/{id}")]
        [AllowAnonymous]
        public IHttpActionResult GetStationsForTransportLine(string id)
        {
            TransportLine transportLine = unitOfWork.TransportLine.Get(id);
            if (transportLine == null)
            {
                return NotFound();
            }


            return Ok(transportLine.Stations.ToList());
        }


        [ResponseType(typeof(TransportLine))]
        [Route("api/transportlines/deleteStationOnLine/{id}")]
        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public IHttpActionResult DeleteStationOnTransportLine(string id,Station externalStation)
        {
            var line = unitOfWork.TransportLine.Find(x => x.TransportLineID == id).FirstOrDefault();

            var stat = unitOfWork.Station.Find(s => s.StationID == externalStation.StationID).FirstOrDefault();



            try
            {
                //line.Stations.Remove(stat);
                unitOfWork.Station.Remove(stat);
                unitOfWork.Complete();
            }
            catch
            {
                throw;
            }

            return Ok(line);
        }


        // GET: api/TransportLines
        [AllowAnonymous]
        public IEnumerable<TransportLine> GetTransportLines()
        {
            return unitOfWork.TransportLine.GetAll();
        }

        [Route("api/TransportLines/WithBuses")]
        [HttpGet]
        [AllowAnonymous]
        public IEnumerable<TransportLine> GetLinesWithBuses()
        {
            return unitOfWork.TransportLine.GetAll().Where(x => x.Vehicles.Count > 0);
        }

        // GET: api/TransportLines/5
        [ResponseType(typeof(TransportLine))]
        [AllowAnonymous]
        public IHttpActionResult GetTransportLine(string id)
        {
            TransportLine transportLine = unitOfWork.TransportLine.Get(id);
            if (transportLine == null)
            {
                return NotFound();
            }

            return Ok(transportLine);
        }

        // PUT: api/TransportLines/5
        [ResponseType(typeof(void))]
        [Authorize(Roles = "Admin")]
        public IHttpActionResult PutTransportLine(string id, TransportLine transportLine)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != transportLine.TransportLineID)
            {
                return BadRequest();
            }

            

            try
            {
                unitOfWork.TransportLine.Update(transportLine);
                unitOfWork.Complete();
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
        [Authorize(Roles = "Admin")]
        public IHttpActionResult PostTransportLine(TransportLine transportLine)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                unitOfWork.TransportLine.Add(transportLine);
                unitOfWork.Complete();
            }
            catch
            {
                throw;
            }

            return CreatedAtRoute("DefaultApi", new { id = transportLine.TransportLineID }, transportLine);
        }

        // DELETE: api/TransportLines/5
        [ResponseType(typeof(TransportLine))]
        [Authorize(Roles = "Admin")]
        public IHttpActionResult DeleteTransportLine(string id)
        {
            TransportLine transportLine = unitOfWork.TransportLine.Get(id);
            if (transportLine == null)
            {
                return NotFound();
            }

            try
            {
                unitOfWork.TransportLine.Remove(transportLine);
                unitOfWork.Complete();
            }
            catch
            {
                throw;
            }

            return Ok(transportLine);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                unitOfWork.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TransportLineExists(string id)
        {
            return unitOfWork.TransportLine.Find(e => e.TransportLineID == id).Count() > 0;
        }
    }
}