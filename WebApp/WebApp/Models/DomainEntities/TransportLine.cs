using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Models.DomainEntities
{
    public class TransportLine
    {

        public int TransportLineID { get; set; }

        public string FromTo { get; set; }
        public string LineNumber { get; set; }

        public virtual ICollection<Vehicle> Vehicles { get; set; }
        public virtual ICollection<Station> Stations { get; set; }
        //treba?
        public virtual ICollection<LinePoint> LinePoints { get; set; }

    }
}