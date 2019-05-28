using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApp.Models.DomainEntities
{
    public class PriceFinal
    {
        public PriceFinal()
        {
            PriceLists = new HashSet<Pricelist>();
            Tickets = new HashSet<Ticket>();
        }

        public int ID { get; set; }
        [Required]
        public double Price { get; set; }

        public virtual ICollection<Pricelist> PriceLists { get; set; }

        public virtual ICollection<Ticket> Tickets { get; set; }
    }
}