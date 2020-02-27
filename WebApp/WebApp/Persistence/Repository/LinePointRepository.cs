using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using WebApp.Models.DomainEntities;

namespace WebApp.Persistence.Repository
{
    public class LinePointRepository : Repository<LinePoint,int> , ILinePointRepository
    {
        public LinePointRepository(DbContext context) : base(context)
        {
        }
        //dodati kasnije metode sa include-om i ostalo sto treba samo za ovaj tip a nema nasledjeno
        //dosla cica na kolica mladjane, mroao si i ovo da uradis he he 
        new public LinePoint Get(int id)
        {
            return context.Set<LinePoint>().Where(a => a.LinePointID == id).Include("TransportLine").FirstOrDefault();
        }

        new public IEnumerable<LinePoint> GetAll()
        {
            return context.Set<LinePoint>().Include("TransportLine").ToList();
        }
    }
}