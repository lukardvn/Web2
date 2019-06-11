using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApp.Models.DomainEntities
{
    public class PriceFinal
    {

        public int ID { get; set; }
        [Required]
        public double Price { get; set; }

        //da li lista ili samo 0..1 ? 
        //public virtual ICollection<Pricelist> PriceLists { get; set; }

        public int PricelistID { get; set; }

        public virtual Pricelist Pricelist { get; set; }

        //veza obicna

        //public virtual ICollection<Ticket> Tickets { get; set; }
        public int TicketID { get; set; }

        [Required]
        public virtual Ticket Ticket { get; set; }
    }
}