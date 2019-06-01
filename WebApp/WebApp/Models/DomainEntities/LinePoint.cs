using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Models.DomainEntities
{
    public class LinePoint
    {
        public int LinePointID { get; set; }

        public double X { get; set; }

        public double Y { get; set; }

        public int TransportLineID { get; set; }

        public virtual TransportLine TransportLine { get; set; }

    }
}