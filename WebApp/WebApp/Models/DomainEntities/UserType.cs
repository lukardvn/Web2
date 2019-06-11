using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApp.Models.DomainEntities
{
    public class UserType
    {
        [Key]
        public int UserTypeID { get; set; }

        //1---- regularan
        //2---- student
        //3---- penzioner
        //jer ef kada dodaje krece od 1
        public int TypeOfUser { get; set; } //penzioner,student ,regularni

        public double Coefficient{ get; set; }

        public virtual ICollection<ApplicationUser> ApplicationUsers { get; set; }
    }
}