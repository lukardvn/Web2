using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.IO;
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
    //[Authorize]
    public class TicketsController : ApiController
    {
        private IUnitOfWork unitOfWork { get; set; }

        public TicketsController() { }

        public TicketsController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }


        [NonAction]
        private int ConvertStringToInt(string tipKorisnika)
        {
            if (tipKorisnika.Equals("regularan"))
            {
                return 0;
            }else if (tipKorisnika.Equals("student"))
            {
                return 1;
            }
            else if (tipKorisnika.Equals("penzioner"))
            {
                return 2;
            }
            else
            {
                return -1;
            }

        }



        [Route("api/tickets/getCena/{tipKorisnika}/{tipKarte}")]
        [HttpGet]
        [AllowAnonymous]
        public IHttpActionResult GetCena(string tipKorisnika, string tipKarte)
        {
            //0---- regularan
            //1---- student
            //2---- penzioner
            int typeUserInDB = ConvertStringToInt(tipKorisnika);

            int bozeMoj = typeUserInDB + 1;



            UserType userType = unitOfWork.UserType.Find(u => u.TypeOfUser == typeUserInDB).FirstOrDefault();
            if (userType == null)
                return BadRequest("regularan korisnik ne postoji u sistemu");

            Pricelist pricelist = unitOfWork.Pricelist.GetAll().Where(x => x.To == null).FirstOrDefault();
            if (pricelist == null)
                return BadRequest("trenutno ne postoji cenovnik");

            List<PriceFinal> pf = unitOfWork.PriceFinal.GetAll().Where(x => x.PricelistID == pricelist.ID).ToList();
            double tipK = 0;
            switch(tipKorisnika)
            {
                case "regularan": tipK = 1;  break;
                case "student": tipK = 0.8; break;
                case "penzioner": tipK = 0.6; break;

                default: break;
            }

            double cena = 0;
            switch(tipKarte)
            {
                case "regularna": cena = pf[0].Price; break;
                case "dnevna": cena = pf[1].Price; break;
                case "mesecna": cena = pf[2].Price;  break;
                case "godisnja": cena = pf[3].Price; break;

                default: break;
            }
            return Ok(cena * tipK);
        }

        [Route("api/tickets/UserType")]
        [HttpGet]
        public IHttpActionResult GetUserType()
        {
            var userId = User.Identity.GetUserId();
            if (userId != null && userId !="admin")
            {
                var user = this.unitOfWork.User.Get(userId);
                var usertype = user.UserType.TypeOfUser;
                string userstring = string.Empty;
                if(usertype == 1)
                {
                    userstring = "regularan";
                }else if(usertype == 2)
                {
                    userstring = "student";
                }else
                {
                    userstring = "penzioner"; 
                }
                return Ok(userstring);
            } else
            {
                return Ok("anoniman");
            }
        }

        [NonAction]
        private DateTime ExpiresAt(string tipKarte)
        {
            DateTime dateTime = DateTime.Now;
            switch (tipKarte)
            {
                case "regularna":
                    {
                        dateTime = dateTime.AddHours(1);
                        break;
                    }
                case "dnevna":
                    {
                        dateTime = dateTime.AddHours(23 - dateTime.Hour);
                        dateTime = dateTime.AddMinutes(59 - dateTime.Minute);
                        dateTime = dateTime.AddSeconds(59 - dateTime.Second);
                        break;
                    }
                case "mesecna":
                    {
                        var daysInMonth = DateTime.DaysInMonth(dateTime.Year, dateTime.Month);

                        dateTime = dateTime.AddDays(daysInMonth - dateTime.Day);
                        dateTime = dateTime.AddHours(23 - dateTime.Hour);
                        dateTime = dateTime.AddMinutes(59 - dateTime.Minute);
                        dateTime = dateTime.AddSeconds(59 - dateTime.Second);
                        break;
                    }
                case "godisnja":
                    {
                        dateTime = dateTime.AddMonths(12 - dateTime.Month);
                        dateTime = dateTime.AddHours(23 - dateTime.Hour);
                        dateTime = dateTime.AddMinutes(59 - dateTime.Minute);
                        dateTime = dateTime.AddSeconds(59 - dateTime.Second);
                        break;
                    }
            }
            return dateTime;
        }

        [Route("api/tickets/BuyTicket/{karta}/{korisnik}")]
        [HttpPost]
        [Authorize(Roles = "AppUser")]
        [ResponseType(typeof(Ticket))]
        public IHttpActionResult Buy(string karta,string korisnik,PayPalPaymentDetails paymentDetails)
        {
            var userId = User.Identity.GetUserId();

            var user = this.unitOfWork.User.Get(userId);

            var userType = this.unitOfWork.UserType.Get((int)user.UserTypeID);

            Pricelist pricelist = unitOfWork.Pricelist.Find(x => x.To == null).FirstOrDefault();
            if (pricelist == null)
                return BadRequest("trenutno ne postoji cenovnik");

            PriceFinal priceFinal = unitOfWork.PriceFinal.GetAll().Where(z => z.PricelistID == pricelist.ID).FirstOrDefault();
            if (priceFinal == null)
                return BadRequest("trenutno ne postoji cena trenutni cenovnik");

            DateTime dateTimeNow = DateTime.UtcNow;

            var expires = ExpiresAt(karta);
            Ticket ticket = new Ticket()
            {
                BoughtAt = dateTimeNow,
                PayPalPaymentDetails = paymentDetails,
                TicketType = karta,
                UserID = userId,
                Expires = expires
            };

            unitOfWork.Ticket.Add(ticket);
            unitOfWork.Complete();

            PriceFinal p = new PriceFinal()
            {
                Price = userType.Coefficient * priceFinal.Price,
                PricelistID = pricelist.ID,
                Pricelist = pricelist,
                Ticket = ticket
            };

            unitOfWork.PriceFinal.Add(p);
            unitOfWork.Complete();



            ticket.PriceFinal = p;


            unitOfWork.Ticket.Update(ticket);
            unitOfWork.Complete();

            //try
            //{
            //    unitOfWork.Ticket.Add(ticket);
            //    unitOfWork.Complete();
            //}catch(DBConcurrencyException)
            //{
            //    if (!TicketExists(ticket.TicketID))
            //    {
            //        return NotFound();
            //    }
            //    else
            //    {
            //        throw;
            //    }
            //}





            return Ok(ticket);   
        }



        [Route("api/tickets/BuyTicketAnonymus/{email}")]
        [HttpGet]
        [AllowAnonymous]
        [ResponseType(typeof(bool))]
        public IHttpActionResult BuyTicket(string email)
        {
            //0---- regularan
            //1---- student
            //2---- penzioner



            UserType userType = unitOfWork.UserType.Find(u => u.TypeOfUser == 0).FirstOrDefault();
            if (userType == null)
                return BadRequest("regularan korisnik ne postoji u sistemu");

            Pricelist pricelist = unitOfWork.Pricelist.Find(x => x.To == null).FirstOrDefault();
            if (pricelist == null)
                return BadRequest("trenutno ne postoji cenovnik");

            PriceFinal priceFinal = unitOfWork.PriceFinal.GetAll().Where(z => z.PricelistID == pricelist.ID).FirstOrDefault();
            if (priceFinal == null)
                return BadRequest("trenutno ne postoji cena za regularni tip karne i trenutni cenovnik");


            //double cena = userType.Coefficient * priceFinal.Price;
            //priceFinal.Price = userType.Coefficient * priceFinal.Price;
            //unitOfWork.PriceFinal.Update(priceFinal);
            //unitOfWork.Complete();

            Ticket templateTicket = unitOfWork.Ticket.Find(t => t.TicketType == "regularna").FirstOrDefault();

            DateTime bouthtAt = DateTime.Now;

            Ticket buyTicket = new Ticket {
                TicketType = "regularna",
            UserID = "anonymus",
            BoughtAt = bouthtAt,
            Expires = bouthtAt.AddHours(1)
            //buyTicket.PriceFinal = p;
        };
            unitOfWork.Ticket.Add(buyTicket);
            unitOfWork.Complete();
            //{
            //    TicketType = "regularna",
            //    UserID = "regularan",
            //    BoughtAt = bouthtAt,
            //    Expires = bouthtAt.AddHours(1),
            //    //PriceFinal = priceFinal
            //};

            PriceFinal p = new PriceFinal()
            {
                Price = userType.Coefficient * priceFinal.Price,
                PricelistID = pricelist.ID,
                Pricelist = pricelist,
                Ticket = buyTicket
            };

            unitOfWork.PriceFinal.Add(p);
            unitOfWork.Complete();


            //buyTicket.TicketType = "regularna";
            //buyTicket.UserID = "regularan";
            //buyTicket.BoughtAt = bouthtAt;
            //buyTicket.Expires = bouthtAt.AddHours(1);
            buyTicket.PriceFinal = p;

            //templateTicket.BoughtAt = bouthtAt;
            //templateTicket.Expires = bouthtAt.AddHours(1);
            //templateTicket.PriceFinal = priceFinal;
            unitOfWork.Ticket.Update(buyTicket);
            unitOfWork.Complete();

            string appended = $"TicketID:{buyTicket.TicketID}\nUserID:{buyTicket.UserID}\n" +
                $"BoughtAt:{buyTicket.BoughtAt.Value}\nExpires:{buyTicket.Expires.Value}\n" +
                $"Price:{buyTicket.PriceFinal.Price}";


            try
            {
                //unitOfWork.Ticket.Add(buyTicket);
                //unitOfWork.Complete();
                SendEMailHelper.Send(email + ".com","Podaci o Vasoj karti",appended);
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


            return Ok(true);
        }


        [Route("api/tickets/BuyTicketRegular/{email}/{tipKarte}")]
        [HttpGet]
        [AllowAnonymous]
        [ResponseType(typeof(bool))]
        public IHttpActionResult BuyTicketRegular(string email, string tipKarte)
        {
            //0---- regularan
            //1---- student
            //2---- penzioner



            UserType userType = unitOfWork.UserType.Find(u => u.TypeOfUser == 0).FirstOrDefault();
            if (userType == null)
                return BadRequest("regularan korisnik ne postoji u sistemu");

            Pricelist pricelist = unitOfWork.Pricelist.Find(x => x.To == null).FirstOrDefault();
            if (pricelist == null)
                return BadRequest("trenutno ne postoji cenovnik");

            List<PriceFinal> priceFinal = unitOfWork.PriceFinal.GetAll().Where(z => z.PricelistID == pricelist.ID).ToList();
            if (priceFinal == null)
                return BadRequest("trenutno ne postoji cena za regularni tip karne i trenutni cenovnik");


            //double cena = userType.Coefficient * priceFinal.Price;
            //priceFinal.Price = userType.Coefficient * priceFinal.Price;
            //unitOfWork.PriceFinal.Update(priceFinal);
            //unitOfWork.Complete();

            Ticket templateTicket = unitOfWork.Ticket.Find(t => t.TicketType == tipKarte).FirstOrDefault();

            DateTime bouthtAt = DateTime.Now;

            int sati = 0;
            int minuti = 0;
            int sekunde = 0;
            DateTime trenutno =  DateTime.Now;

            sati = 23 - trenutno.Hour;
            minuti = 59 - trenutno.Minute;
            sekunde = 59 - trenutno.Second;
            double cena = 0;

            switch (tipKarte)
            {
                case "regularna": trenutno = trenutno.AddHours(1); cena = priceFinal[0].Price; break;
                case "dnevna": trenutno = trenutno.AddHours(sati); trenutno = trenutno.AddMinutes(minuti); trenutno = trenutno.AddSeconds(sekunde); cena = priceFinal[1].Price; break;
                case "mesecna": trenutno = trenutno.AddHours(sati); trenutno = trenutno.AddMinutes(minuti); trenutno = trenutno.AddSeconds(sekunde); trenutno = trenutno.AddMonths(1); cena = priceFinal[2].Price; break;
                case "godisnja":trenutno =  trenutno.AddHours(sati); trenutno = trenutno.AddMinutes(minuti); trenutno = trenutno.AddSeconds(sekunde); trenutno =  trenutno.AddYears(1); cena = priceFinal[3].Price; break;

                default: break;
            }


            Ticket buyTicket = new Ticket
            {
                TicketType = tipKarte,
                UserID = "anonymus",
                BoughtAt = bouthtAt,
                Expires = trenutno
                //buyTicket.PriceFinal = p;
            };
            unitOfWork.Ticket.Add(buyTicket);
            unitOfWork.Complete();
            //{
            //    TicketType = "regularna",
            //    UserID = "regularan",
            //    BoughtAt = bouthtAt,
            //    Expires = bouthtAt.AddHours(1),
            //    //PriceFinal = priceFinal
            //};

            PriceFinal p = new PriceFinal()
            {
                Price = userType.Coefficient * cena,
                PricelistID = pricelist.ID,
                Pricelist = pricelist,
                Ticket = buyTicket
            };

            unitOfWork.PriceFinal.Add(p);
            unitOfWork.Complete();


            //buyTicket.TicketType = "regularna";
            //buyTicket.UserID = "regularan";
            //buyTicket.BoughtAt = bouthtAt;
            //buyTicket.Expires = bouthtAt.AddHours(1);
            buyTicket.PriceFinal = p;

            //templateTicket.BoughtAt = bouthtAt;
            //templateTicket.Expires = bouthtAt.AddHours(1);
            //templateTicket.PriceFinal = priceFinal;
            unitOfWork.Ticket.Update(buyTicket);
            unitOfWork.Complete();

            string appended = $"TicketID:{buyTicket.TicketID}\n" +
                $"BoughtAt:{buyTicket.BoughtAt.Value}\nExpires:{buyTicket.Expires.Value}\n" +
                $"Price:{buyTicket.PriceFinal.Price}";


            try
            {
                //unitOfWork.Ticket.Add(buyTicket);
                //unitOfWork.Complete();
                SendEMailHelper.Send(email + ".com", "Podaci o Vasoj karti", appended);
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


            return Ok(true);
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
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin,AppUser")]
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
        [Authorize(Roles = "Admin")]
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