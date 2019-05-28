using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApp.Models.DomainEntities
{
    public class Location
    {
        public Location()
        {
            Address = new Address();
        }

        public int ID { get; set; }
        [Required]
        public double Longitude { get; set; }
        [Required]
        public double Latitude { get; set; }
        [Required]
        public Address Address { get; set; }


    }
}