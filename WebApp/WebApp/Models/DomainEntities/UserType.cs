using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Models.DomainEntities
{
    public class UserType
    {
        public int ID { get; set; }

        public int TypeOfUser { get; set; }

        public double Coefficient{ get; set; }

        public virtual ICollection<ApplicationUser> ApplicationUsers { get; set; }
    }
}