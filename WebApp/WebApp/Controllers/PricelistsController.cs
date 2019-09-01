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
        private IUnitOfWork unitOfWork { get; set; }

        public PricelistsController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }



        public PricelistsController() { }

        //trenutni cenovnik
        [Route("api/Pricelists/Current")]
        [HttpGet]
        [ResponseType(typeof(ICollection<Pricelist>))]
        [AllowAnonymous]
        public IEnumerable<Pricelist> GetCurrentPriceHistories()
        {
            return unitOfWork.Pricelist.GetAll().Where(x => x.From < DateTime.Now && x.To > DateTime.Now);
        }

        [Route("api/priceList/getPriceList")]
        [HttpGet] 
        [AllowAnonymous]
        public List<int> GetPriceList()
        {
            IQueryable<Pricelist> lista = unitOfWork.Pricelist.GetAll().AsQueryable();
            List<int> retVal = new List<int>();

            foreach (var it in lista)
            {
                // ne moze se brisati trenutni cenovnik, stari mogu
                if (it.To != null)
                {
                    retVal.Add(it.ID);
                }
            }

            return retVal;
        }

        [Route("api/priceList/getTrenutniCenovnik")]
        [HttpGet]
        [AllowAnonymous]
        public CenovnikEdit GetTrenutniCenovnik()
        {
            IQueryable<Pricelist> lista = unitOfWork.Pricelist.GetAll().AsQueryable();
            CenovnikEdit retVal = new CenovnikEdit();

            foreach (var it in lista)
            {
                // ne moze se brisati trenutni cenovnik, stari mogu
                if (it.To == null)
                {
                    IQueryable<PriceFinal> pf = unitOfWork.PriceFinal.GetAll().Where(x => x.PricelistID == it.ID).AsQueryable();
                    PriceFinal[] pf2 = pf.ToArray();
                    retVal.ID = it.ID;
                    retVal.CenaRegularna = pf2[0].Price;
                    retVal.CenaDnevna = pf2[1].Price;
                    retVal.CenaMesecna = pf2[2].Price;
                    retVal.CenaGodisnja = pf2[3].Price;

                    return retVal;
                }
            }

            return retVal;
        }

        // GET: api/Pricelists
        [AllowAnonymous]
        public IEnumerable<Pricelist> GetPricelists()
        {
            return unitOfWork.Pricelist.GetAll();
        }

        [AllowAnonymous]
        [Route("api/priceList/deleteCenovnik/{id}")]
        public IHttpActionResult DeleteCenovnik(int id)
        {
            IQueryable<PriceFinal> cene = unitOfWork.PriceFinal.GetAll().AsQueryable();
            foreach (var it in cene)
            {
                if (it.PricelistID == id)
                {
                    unitOfWork.PriceFinal.Remove(it);
                }
            }

            Pricelist pl = unitOfWork.Pricelist.Get(id);
            unitOfWork.Pricelist.Remove(pl);
            unitOfWork.Complete();

            return Ok(true);
        }

        [AllowAnonymous]
        [Route("api/priceList/updateCenovnik")]
        public IHttpActionResult UpdateCenovnik(CenovnikEdit cenovnik)
        {
            IQueryable<Pricelist> lista = unitOfWork.Pricelist.GetAll().AsQueryable();

            foreach (var it in lista)
            {
                // ne moze se brisati trenutni cenovnik, stari mogu
                if (it.To == null)
                {
                    IQueryable<PriceFinal> pf = unitOfWork.PriceFinal.Find(x => x.PricelistID == it.ID).AsQueryable();
                    PriceFinal[] pf2 = pf.ToArray();
                    pf2[0].Price = cenovnik.CenaRegularna;
                    pf2[1].Price = cenovnik.CenaDnevna;
                    pf2[2].Price = cenovnik.CenaMesecna;
                    pf2[3].Price = cenovnik.CenaGodisnja;

                    unitOfWork.PriceFinal.Update(pf2[0]);
                    unitOfWork.PriceFinal.Update(pf2[1]);
                    unitOfWork.PriceFinal.Update(pf2[2]);
                    unitOfWork.PriceFinal.Update(pf2[3]);

                    unitOfWork.Complete();

                    return Ok(true);
                }
            }

            return Ok();
        }

        [AllowAnonymous]
        [Route("api/priceList/newCenovnik")]
        public IHttpActionResult NewCenovnik(CenovnikEdit cenovnik)
        {

            Pricelist temp1 = unitOfWork.Pricelist.GetAll().Where(x => x.To == null).FirstOrDefault();
            temp1.To = DateTime.Now;
            unitOfWork.Pricelist.Update(temp1);
            unitOfWork.Complete();

            Pricelist pl = new Pricelist();
            pl.From = DateTime.Now;
            pl.To = null;

            unitOfWork.Pricelist.Add(pl);
            unitOfWork.Complete();

            Pricelist temp = unitOfWork.Pricelist.GetAll().Where(x => x.To == null).FirstOrDefault();

            PriceFinal pf1 = new PriceFinal();
            pf1.PricelistID = temp.ID;
            pf1.Price = cenovnik.CenaRegularna;
            pf1.Pricelist = temp;
            Ticket ticketRegular = new Ticket() { TicketType = "regularna", UserID = "anonymus" };
            unitOfWork.Ticket.Add(ticketRegular);
            unitOfWork.Complete();
            pf1.Ticket = unitOfWork.Ticket.GetAll().Last();
            unitOfWork.PriceFinal.Add(pf1);
            unitOfWork.Complete();


            PriceFinal pf2 = new PriceFinal();
            pf2.PricelistID = temp.ID;
            pf2.Price = cenovnik.CenaDnevna;
            pf2.Pricelist = temp;
            Ticket ticketRegular2 = new Ticket() { TicketType = "dnevna", UserID = "anonymus" };
            unitOfWork.Ticket.Add(ticketRegular2);
            unitOfWork.Complete();
            pf2.Ticket = unitOfWork.Ticket.GetAll().Last();
            unitOfWork.PriceFinal.Add(pf2);
            unitOfWork.Complete();


            PriceFinal pf3 = new PriceFinal();
            pf3.PricelistID = temp.ID;
            pf3.Price = cenovnik.CenaMesecna;
            pf3.Pricelist = temp;
            Ticket ticketRegular3 = new Ticket() { TicketType = "mesecna", UserID = "anonymus" };
            unitOfWork.Ticket.Add(ticketRegular3);
            unitOfWork.Complete();
            pf3.Ticket = unitOfWork.Ticket.GetAll().Last();
            unitOfWork.PriceFinal.Add(pf3);
            unitOfWork.Complete();

            PriceFinal pf4 = new PriceFinal();
            pf4.PricelistID = temp.ID;
            pf4.Price = cenovnik.CenaGodisnja;
            pf4.Pricelist = temp;
            Ticket ticketRegular4 = new Ticket() { TicketType = "godisnja", UserID = "anonymus" };
            unitOfWork.Ticket.Add(ticketRegular4);
            unitOfWork.Complete();
            pf4.Ticket = unitOfWork.Ticket.GetAll().Last();
            unitOfWork.PriceFinal.Add(pf4);
            unitOfWork.Complete();

            unitOfWork.Complete();

            return Ok(true);
        }

        // GET: api/Pricelists/5
        [ResponseType(typeof(Pricelist))]
        [Route("api/pricelists/getSelectedPricelist/{id}")]
        //[Authorize(Roles ="Admin")]
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
        [Route("api/pricelists/editPricelist/{id}")]
        //[Authorize(Roles = "Admin")]
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

            var price = unitOfWork.Pricelist.Find(x => x.ID == pricelist.ID).FirstOrDefault();


            try
            {
                //var priceFinal = unitOfWork.PriceFinal.GetAll().Where(x => x.PricelistID == price.ID);

                //price.PriceFinals = priceFinal.ToList();

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
        //[Authorize(Roles = "Admin")]
        public IHttpActionResult PostPricelist(Pricelist pricelist)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var lastPricelist = unitOfWork.Pricelist.GetAll().Last();

                pricelist.From = lastPricelist.To.Value;

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
        [Authorize(Roles = "Admin")]
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