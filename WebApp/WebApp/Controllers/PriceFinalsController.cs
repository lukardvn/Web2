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
    public class PriceFinalsController : ApiController
    {
        private IUnitOfWork unitOfWork { get; set; }

        public PriceFinalsController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public PriceFinalsController() { }


        //finalne cene za trenutni cenovnik
        [Route("api/PriceFinals/Current")]
        [HttpGet]
        [ResponseType(typeof(ICollection<PriceFinal>))]
        public IEnumerable<PriceFinal> GetCurrentPriceHistories()
        {
            return unitOfWork.PriceFinal.GetAll().Where(x=>x.Pricelist.From <= DateTime.Now && x.Pricelist.To >= DateTime.Now);
        }

        // GET: api/PriceFinals
        public IEnumerable<PriceFinal> GetPriceFinals()
        {
            return unitOfWork.PriceFinal.GetAll();
        }

        // GET: api/PriceFinals/5
        [ResponseType(typeof(PriceFinal))]
        public IHttpActionResult GetPriceFinal(int id)
        {
            PriceFinal priceFinal = unitOfWork.PriceFinal.Get(id);
            if (priceFinal == null)
            {
                return NotFound();
            }

            return Ok(priceFinal);
        }

        // PUT: api/PriceFinals/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutPriceFinal(int id, PriceFinal priceFinal)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != priceFinal.ID)
            {
                return BadRequest();
            }

            

            try
            {
                unitOfWork.PriceFinal.Update(priceFinal);
                unitOfWork.Complete();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PriceFinalExists(id))
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

        // POST: api/PriceFinals
        [ResponseType(typeof(PriceFinal))]
        public IHttpActionResult PostPriceFinal(PriceFinal priceFinal)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                unitOfWork.PriceFinal.Add(priceFinal);
                unitOfWork.Complete();
            }
            catch
            {
                throw;
            }

            return CreatedAtRoute("DefaultApi", new { id = priceFinal.ID }, priceFinal);
        }

        // DELETE: api/PriceFinals/5
        [ResponseType(typeof(PriceFinal))]
        public IHttpActionResult DeletePriceFinal(int id)
        {
            PriceFinal priceFinal = unitOfWork.PriceFinal.Get(id);
            if (priceFinal == null)
            {
                return NotFound();
            }

            try
            {
                unitOfWork.PriceFinal.Add(priceFinal);
                unitOfWork.Complete();
            }
            catch
            {
                throw;
            }

            return Ok(priceFinal);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                unitOfWork.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PriceFinalExists(int id)
        {
            return unitOfWork.PriceFinal.Find(e => e.ID == id).Count() > 0;
        }
    }
}