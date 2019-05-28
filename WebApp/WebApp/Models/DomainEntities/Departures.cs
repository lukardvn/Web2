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

        public int DayInWeek { get; set; }

        public string TimeTable { get; set; }

        public virtual ICollection<TransportLine> TransportLines { get; set; }
    }
}