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
    public class PricelistsController : ApiController
    {
        private IUnitOfWork unitOfWork;

        public PricelistsController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }



        // GET: api/Pricelists
        public IEnumerable<Pricelist> GetPricelists()
        {
            return unitOfWork.Pricelist.GetAll();
        }

        // GET: api/Pricelists/5
        [ResponseType(typeof(Pricelist))]
        public IHttpActionResult GetPricelist(int id)
        {
            Pricelist pricelist = unitOfWork.Pricelist.Get(id);
            if (pricelist == null)
            {
                return NotFound();
            }

            return Ok(pricelist);
        }

        // PUT: api/Pricelists/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutPricelist(int id, Pricelist pricelist)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != pricelist.ID)
            {
                return BadRequest();
            }

            

            try
            {
                unitOfWork.Pricelist.Update(pricelist);
                unitOfWork.Complete();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PricelistExists(id))
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

        // POST: api/Pricelists
        [ResponseType(typeof(Pricelist))]
        public IHttpActionResult PostPricelist(Pricelist pricelist)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                unitOfWork.Pricelist.Add(pricelist);
                unitOfWork.Complete();
            }
            catch
            {
                throw;
            }

            return CreatedAtRoute("DefaultApi", new { id = pricelist.ID }, pricelist);
        }

        // DELETE: api/Pricelists/5
        [ResponseType(typeof(Pricelist))]
        public IHttpActionResult DeletePricelist(int id)
        {
            Pricelist pricelist = unitOfWork.Pricelist.Get(id);
            if (pricelist == null)
            {
                return NotFound();
            }

            try
            {
                unitOfWork.Pricelist.Remove(pricelist);
                unitOfWork.Complete();
            }
            catch
            {
                throw;
            }

            return Ok(pricelist);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                unitOfWork.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PricelistExists(int id)
        {
            return unitOfWork.Pricelist.Find(e => e.ID == id).Count() > 0;
        }
    }
}