using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace PartyService.Controllers
{
    public class EventController : ApiController
    {
        // GET: api/Event
        public Task<IHttpActionResult> Get()
        {
        }

        // GET: api/Event/5
        public Task<IHttpActionResult> Get(int id)
        {
        }

        // POST: api/Event
        public Task<IHttpActionResult> Post([FromBody]string value)
        {
        }

        // PUT: api/Event/5
        public Task<IHttpActionResult> Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Event/5
        public Task<IHttpActionResult> Delete(int id)
        {
        }
    }
}
