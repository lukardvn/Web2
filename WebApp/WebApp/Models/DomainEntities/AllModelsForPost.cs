using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Models.DomainEntities
{
    public class AllModelsForPost
    {
    }

    public class Stanica
    {
        public string Naziv { get; set; }
        public string Adresa { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
    }

    public class StanicaUpdate
    {
        public int ID { get; set; }
        public string Naziv { get; set; }
        public string Adresa { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
    }



    public class StaniceNaLiniji
    {
        public string TransportLineID { get; set; }
        public int StationID { get; set; }
    }

    public class RedVoznje
    {
        public string TransportLineID { get; set; }
        public string TimeTable { get; set; }
    }

    public class CenovnikEdit
    {
        public int ID { get; set; }
        public double CenaRegularna { get; set; }
        public double CenaDnevna { get; set; }
        public double CenaMesecna { get; set; }
        public double CenaGodisnja { get; set; }
    }

    public class Osoba
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string Email { get; set; }
        public DateTime Date { get; set; }
        public string Tip { get; set; }
        public string Adress { get; set; }
    }

    

    public class RedVoznjeUpdate
    {
        public int ID { get; set; }
        public string TransportLineID { get; set; }
        public string TimeTable { get; set; }
    }

    public class Linije
    {
        public string TransportLineID { get; set; }
        public string FromTo { get; set; }
    }
}