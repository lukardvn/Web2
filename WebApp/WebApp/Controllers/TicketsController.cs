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
using WebApp.App_Start;
using WebApp.Models.DomainEntities;
using WebApp.Persistence;
using WebApp.Persistence.UnitOfWork;

namespace WebApp.Controllers
{
    public class TicketsController : ApiController
    {
        private IUnitOfWork unitOfWork { get; set; }

        public TicketsController() { }

        public TicketsController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }



        [Route("api/Tickets/BuyTicketAnonymus")]
        [HttpPost]
        [AllowAnonymous]
        [ResponseType(typeof(Ticket))]
        public IHttpActionResult BuyTicket()
        {
            UserType userType = unitOfWork.UserType.Find(u => u.TypeOfUser == 0).FirstOrDefault();
            if (userType == null)
                return BadRequest("regularan korisnik ne postoji u sistemu");

            Pricelist pricelist = unitOfWork.Pricelist.Find(x => x.From <= DateTime.Now && x.To >= DateTime.Now).FirstOrDefault();
            if (pricelist == null)
                return BadRequest("trenutno ne postoji cenovnik");

            PriceFinal priceFinal = unitOfWork.PriceFinal.Find(z => z.ID == pricelist.ID &&z.Ticket.TicketType== "regularna").FirstOrDefault();
            if (priceFinal == null)
                return BadRequest("trenutno ne postoji cena za regularni tip karne i trenutni cenovnik");


            //double cena = userType.Coefficient * priceFinal.Price;
            priceFinal.Price = userType.Coefficient * priceFinal.Price;
            unitOfWork.PriceFinal.Update(priceFinal);
            unitOfWork.Complete();

            Ticket templateTicket = unitOfWork.Ticket.Find(t => t.TicketType == "regularna").FirstOrDefault();

            DateTime bouthtAt = DateTime.Now;

            Ticket buyTicket = new Ticket()
            {
                TicketType = templateTicket.TicketType,
                UserID = templateTicket.UserID,
                BoughtAt = bouthtAt,
                Expires = bouthtAt.AddHours(1),
                PriceFinal = priceFinal
            };

            string appended = $"TicketID:{buyTicket.TicketID}\nUserID:{buyTicket.UserID}\n" +
                $"BoughtAt:{buyTicket.BoughtAt.Value}\nExpires:{buyTicket.Expires.Value}\n" +
                $"Price:{buyTicket.PriceFinal.Price}";


            try
            {
                unitOfWork.Ticket.Add(buyTicket);
                unitOfWork.Complete();
                //SendEMailHelper.Send(appended,)
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TicketExists(buyTicket.TicketID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }


            return Ok(buyTicket);
        }

        /// <summary>
        /// gotove metode
        /// </summary>
        /// <returns></returns>
        // GET: api/Tickets
        public IEnumerable<Ticket> GetTickets()
        {
            return unitOfWork.Ticket.GetAll();
        }

        // GET: api/Tickets/5
        [ResponseType(typeof(Ticket))]
        public IHttpActionResult GetTicket(int id)
        {
            Ticket ticket = unitOfWork.Ticket.Get(id);
            if (ticket == null)
            {
                return NotFound();
            }

            return Ok(ticket);
        }

        // PUT: api/Tickets/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutTicket(int id, Ticket ticket)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != ticket.TicketID)
            {
                return BadRequest();
            }

            

            try
            {
                unitOfWork.Ticket.Update(ticket);
                unitOfWork.Complete();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TicketExists(id))
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

        // POST: api/Tickets
        [ResponseType(typeof(Ticket))]
        public IHttpActionResult PostTicket(Ticket ticket)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                unitOfWork.Ticket.Add(ticket);
                unitOfWork.Complete();
            }
            catch
            {
                throw;
            }

            return CreatedAtRoute("DefaultApi", new { id = ticket.TicketID }, ticket);
        }

        // DELETE: api/Tickets/5
        [ResponseType(typeof(Ticket))]
        public IHttpActionResult DeleteTicket(int id)
        {
            Ticket ticket = unitOfWork.Ticket.Get(id);
            if (ticket == null)
            {
                return NotFound();
            }

            try
            {
                unitOfWork.Ticket.Remove(ticket);
                unitOfWork.Complete();
            }
            catch
            {
                throw;
            }

            return Ok(ticket);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                unitOfWork.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TicketExists(int id)
        {
            return unitOfWork.Ticket.Find(e => e.TicketID == id).Count() > 0;
        }
    }
}