using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Document.Entities;
using ShCore.Utility;

namespace WebApiSample.Controllers
{
    public class ValuesController : ApiController
    {
        // GET api/values
        public IEnumerable<Device> Get()
        {
            return Device.GetAllData();
        }

        // GET api/values/5
        public Device Get(int id)
        {
            return Singleton<Device>.Inst.GetByID(id);
        }

        // POST api/values
        public IHttpActionResult Post(Device device)
        {
            device.Save();
            return Ok();
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete]
        public void Delete(int id)
        {
            var obj = Singleton<Device>.Inst.GetByID(id);
            var device = new Device { ID = id };
            device.Delete();
        }
    }
}
