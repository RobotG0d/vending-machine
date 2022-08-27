using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MvpMatch.Challenges.VendingMachine.API.Controllers
{
    [RoutePrefix("values")]
    public class ValuesController : ApiController
    {
        // GET api/values
        [HttpGet]
        [Route("")]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet]
        [Route("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        [Route("")]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut]
        [Route("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete]
        [Route("{id}")]
        public void Delete(int id)
        {
        }
    }
}
