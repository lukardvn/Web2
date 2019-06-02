using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using WebApp.Models.DomainEntities;

namespace WebApp.Persistence.Repository
{
    public class StationsOnLineRepository : Repository<StationsOnLine,int> , IStationsOnLineRepository
    {
        public StationsOnLineRepository(DbContext context) : base(context)
        {

        }
    }
}