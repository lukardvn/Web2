using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApp.Models.DomainEntities
{
    public class Departures
    {
        public Departures() { }

        public int ID { get; set; }

        //public bool Type {get;set;} //gradski ili prigradski ??

        ////mozda moze i kao poseban tip,ali recimo da su 0,1,2 vrednosti da ne komplikujemo bar za sada
        //public int DayInWeek { get; set; } // u timetable ce biti stavljene sve pa kasnije u angularu samo filter
        //jednostavnije je...

        public string TimeTable { get; set; }

        public DateTime ValidFrom { get; set; }

        //da li da ostane vise ili samo jedna?
        public string TransportLineID { get; set; }

        public virtual TransportLine TransportLines { get; set; }
    }
}