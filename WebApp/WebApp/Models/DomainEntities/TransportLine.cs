using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Models.DomainEntities
{
    public class TransportLine
    {
        public TransportLine()
        {
            Stations = new HashSet<Station>();
        }

        public int TransportLineID { get; set; }

        public virtual ICollection<Departures> Departures { get; set; }
        public virtual ICollection<Station> Stations { get; set; }

    }
}