using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApp.Models.DomainEntities
{
    public class Station
    {
        public Station()
        {
            TransportLines = new HashSet<TransportLine>();
            Location = new Location();
        }

        public int StationID { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public Location Location { get; set; }
        public virtual ICollection<TransportLine> TransportLines { get; set; }
    }
}