using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using WebApp.Models.DomainEntities;

namespace WebApp.Persistence.Repository
{
    public class PricelistRespository : Repository<Pricelist,int> , IPricelistRepository
    {
        public PricelistRespository(DbContext context) : base(context)
        {

        }
    }
}