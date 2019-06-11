using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApp.Persistence.UnitOfWork;

namespace WebApp.Controllers
{
    //[Authorize]
    public class ValuesController : ApiController
    {
        public ValuesController()
        {
        }

        // GET api/values
        public IEnumerable<string> Get()
        {

            string addr2 = System.Web.Hosting.HostingEnvironment.MapPath(@"~/App_Data/busStopsPointsCorrected1.json");

            string json = "";
            using (StreamReader reader = new StreamReader(addr2))
            {
                json = reader.ReadToEnd();
            }

            dynamic niz = JsonConvert.DeserializeObject(json);

            var b = Enumerable.Count(niz);


            for (int i = 0; i < Enumerable.Count(niz); i++)
            {
                for (int j = 0; j < Enumerable.Count(niz[i]); j++)
                {
                    //string item = niz[i][j];
                    //string[] items = item.Split('|');
                    if (j > 0)
                    {
                        string item = niz[i][j];
                        string[] items = item.Split('|');
                    }
                    else
                    {
                        string item = niz[i][j];
                        string[] items = item.Split('|');
                    }
                }
            }
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
