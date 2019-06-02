using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using WebApp.Models.DomainEntities;

namespace WebApp.Persistence.Repository
{
    public class StationRepository : Repository<Station,int> , IStationRepository
    {
        public StationRepository(DbContext context) : base(context)
        {

        }
    }
}