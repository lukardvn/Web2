using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApp.Models.DomainEntities
{
    public class Ticket
    {

        public int TicketID { get; set; }

        [Required]
        public string TicketType { get; set; }//string to int,its easier to recognize than enums

        //zbog inicijalnog dodavanja u bazu sklonjen required
        public DateTime? BoughtAt { get; set; }

        [Required]
        public string UserID { get; set; }

        public virtual ApplicationUser User { get; set; }

        public DateTime? Expires { get; set; }

        //pricefinal veza?
        //public int? PriceFinalID { get; set; }
        
        public virtual PriceFinal PriceFinal { get; set; }

        public string PayPalPaymentDetailsID { get; set; }
        public virtual PayPalPaymentDetails PayPalPaymentDetails { get; set; }

    }
}