using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Web.Http.Description;
using WebApp.Hubs;
using WebApp.Models.DomainEntities;
using WebApp.Persistence;
using WebApp.Persistence.UnitOfWork;

namespace WebApp.Controllers
{
    public class VehiclesController : ApiController
    {
        private IUnitOfWork unitOfWork;
        private VehicleHub vehicleHub;
        public VehiclesController() { }

        public VehiclesController(IUnitOfWork unitOfWork,VehicleHub vehicleHub)
        {
            this.unitOfWork = unitOfWork;
            this.vehicleHub = vehicleHub;
        }


        [Route("api/vehicles/updateVehiclePosition")]
        [HttpPost]
        [AllowAnonymous]
        [ResponseType(typeof(void))]
        public IHttpActionResult UpdateVehiclePosition()
        {
            var request = System.Web.HttpContext.Current.Request;
            string data = GetDocumentContents(request);

            var hubContext = GlobalHost.ConnectionManager.GetHubContext<VehicleHub>();
            hubContext.Clients.All.newPositions(data);

            foreach (string busData in data.Split('|'))
            {
                //busData => NS335XY,45.45453,19.345435,12A
                var busDataArray = busData.Split(',');
                Vehicle vehicle = unitOfWork.Vehicle.Get(busDataArray[0]);

                vehicle.X = double.Parse(busDataArray[1]);
                vehicle.Y = double.Parse(busDataArray[2]);
                vehicle.TransportLineID = busDataArray[3];
                unitOfWork.Vehicle.Update(vehicle);
            }
            unitOfWork.Complete();
            return Ok();
        }


        // GET: api/Vehicles
        public IEnumerable<Vehicle> GetVehicles()
        {
            return unitOfWork.Vehicle.GetAll();
        }

        // GET: api/Vehicles/5
        [ResponseType(typeof(Vehicle))]
        public IHttpActionResult GetVehicle(string id)
        {
            Vehicle vehicle = unitOfWork.Vehicle.Get(id);
            if (vehicle == null)
            {
                return NotFound();
            }

            return Ok(vehicle);
        }

        // PUT: api/Vehicles/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutVehicle(string id, Vehicle vehicle)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != vehicle.VehicleID)
            {
                return BadRequest();
            }

            

            try
            {
                unitOfWork.Vehicle.Update(vehicle);
                unitOfWork.Complete();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VehicleExists(id))
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

        // POST: api/Vehicles
        [ResponseType(typeof(Vehicle))]
        public IHttpActionResult PostVehicle(Vehicle vehicle)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                unitOfWork.Vehicle.Add(vehicle);
                unitOfWork.Complete();
            }
            catch
            {
                throw;
            }

            return CreatedAtRoute("DefaultApi", new { id = vehicle.VehicleID }, vehicle);
        }

        // DELETE: api/Vehicles/5
        [ResponseType(typeof(Vehicle))]
        public IHttpActionResult DeleteVehicle(string id)
        {
            Vehicle vehicle = unitOfWork.Vehicle.Get(id);
            if (vehicle == null)
            {
                return NotFound();
            }

            try
            {
                unitOfWork.Vehicle.Remove(vehicle);
                unitOfWork.Complete();
            }
            catch
            {
                throw;
            }

            return Ok(vehicle);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                unitOfWork.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool VehicleExists(string id)
        {
            return unitOfWork.Vehicle.Find(e => e.VehicleID == id).Count() > 0;
        }

        private string GetDocumentContents(System.Web.HttpRequest Request)
        {
            string documentContents;
            using (Stream receiveStream = Request.InputStream)
            {
                using (StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8))
                {
                    documentContents = readStream.ReadToEnd();
                }
            }
            return documentContents;
        }
    }
}