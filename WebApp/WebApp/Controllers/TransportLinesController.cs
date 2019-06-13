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



        [HttpPost]
        [Route("api/transportlines/addStationInLine/{id}")]
        [ResponseType(typeof(TransportLine))]
        public IHttpActionResult AddStationInLine(string id,Station externalStation)
        {
            var line = unitOfWork.TransportLine.Find(x => x.TransportLineID == id).FirstOrDefault();

            line.Stations.Add(externalStation);
            unitOfWork.Complete();


            return Ok(line);
        }

        [HttpPut]
        [Route("api/transportlines/editStationInLine/{id}")]
        [ResponseType(typeof(void))]
        public IHttpActionResult EditStationInLine(string id, Station externalStation)
        {
            var line = unitOfWork.TransportLine.Find(x => x.TransportLineID == id).FirstOrDefault();

            var stat = unitOfWork.Station.Find(s => s.StationID == externalStation.StationID).FirstOrDefault();
            stat.Name = externalStation.Name;
            stat.X = externalStation.X;
            stat.Y = externalStation.Y;
            stat.Address = externalStation.Address;
            unitOfWork.Complete();


            return StatusCode(HttpStatusCode.NoContent);
        }

        [ResponseType(typeof(ICollection<Station>))]
        [HttpGet]
        [Route("api/transportlines/GetStationsForTransportLine/{id}")]
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
        [Route("api/transportlines/deleIt/{id}")]
        public IHttpActionResult DeleteStationOnTransportLine(string id,Station externalStation)
        {
            var line = unitOfWork.TransportLine.Find(x => x.TransportLineID == id).FirstOrDefault();

            var stat = unitOfWork.Station.Find(s => s.StationID == externalStation.StationID).FirstOrDefault();

            line.Stations.Remove(stat);

            try
            {
                unitOfWork.Complete();
            }
            catch
            {
                throw;
            }

            return Ok(line);
        }


        // GET: api/TransportLines
        public IEnumerable<TransportLine> GetTransportLines()
        {
            return unitOfWork.TransportLine.GetAll();
        }

        // GET: api/TransportLines/5
        [ResponseType(typeof(TransportLine))]
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