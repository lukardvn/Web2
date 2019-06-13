using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using WebApp.Models.DomainEntities;

namespace WebApp.Persistence.Repository
{
    public class TransportLineRepository : Repository<TransportLine,string> , ITransportLineRepository
    {
        public TransportLineRepository(DbContext context) : base(context)
        {

        }
        new public IEnumerable<TransportLine> GetAll()
        {
            return context.Set<TransportLine>().Include("Stations").ToList();
        }
    }
}