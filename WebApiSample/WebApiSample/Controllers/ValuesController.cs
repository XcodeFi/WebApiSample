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
    [RoutePrefix("api/devices")]
    public class ValuesController : ApiController
    {
        // GET api/values
        [Route("getall")]
        [HttpGet]
        public IEnumerable<Device> Get()
        {
            return Device.GetAllData();
        }

        // GET api/values/5
        [Route("getid/{id:int}",Name ="getid")]
        public Device Get(int id)
        {
            var result= Singleton<Device>.Inst.GetByID(id);
            if (result == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }
            return result;
        }

        // GET api/values/5
        //[Route("testcontraints/{id:nonzero}")]
        public Device GetContraints(int id)
        {
            var result = Singleton<Device>.Inst.GetByID(id);
            if (result == null) 
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }
            return result;
        }

        [Route("test/{*date:datetime}")]
        public DateTime Get(DateTime date)
        {
            if (date == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }
            return date;
        }

        // POST api/values
        [Route("create")]
        public HttpResponseMessage Post(Device device)
        {
            if (ModelState.IsValid)
            {
                device.Save();
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, device);
                response.Headers.Location = new Uri(Url.Link("getid", new { id = device.ID }));
                return response;
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete]
        public HttpResponseMessage Delete(int id)
        {
            var obj = Singleton<Device>.Inst.GetByID(id);
            if (obj==null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound);
            }
            var device = new Device { ID = id };
            device.Delete();
        }
    }
}
