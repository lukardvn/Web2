using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using WebApp.Models.DomainEntities;

namespace WebApp.Persistence.Repository
{
    public class PriceFinalRepository : Repository<PriceFinal,int> , IPriceFinalRepository
    {
        public PriceFinalRepository(DbContext context) : base(context)
        {

        }
    }
}