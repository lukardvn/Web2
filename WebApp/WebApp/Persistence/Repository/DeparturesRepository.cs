using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using WebApp.Models.DomainEntities;

namespace WebApp.Persistence.Repository
{
    public class DeparturesRepository : Repository<Departures,int> , IDeparturesRepository
    {
        public DeparturesRepository(DbContext context) : base(context)
        {
        }
        //dodati kasnije metode sa include-om i ostalo sto treba samo za ovaj tip a nema nasledjeno
    }
}