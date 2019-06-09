﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApp.Models.DomainEntities
{
    //ako se bude za vecu ocenu radilo, pracenje vozila
    public class Vehicle
    {

        public int VehicleID { get; set; }

        public double X { get; set; }

        public double Y { get; set; }

        [Required]
        public string TransportLineID { get; set; }

        public virtual TransportLine TransportLine { get; set; }
    }
}