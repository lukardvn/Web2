using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApp.Models.DomainEntities
{
    public class Departures
    {
        public int ID { get; set; }

        //mozda moze i kao poseban tip,ali recimo da su 0,1,2 vrednosti da ne komplikujemo bar za sada
        public int DayInWeek { get; set; }

        public string TimeTable { get; set; }

        //da li da ostane vise ili samo jedna?
        public virtual ICollection<TransportLine> TransportLines { get; set; }
    }
}