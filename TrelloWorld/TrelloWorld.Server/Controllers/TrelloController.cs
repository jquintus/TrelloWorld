using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace TrelloWorld.Server.Controllers
{
    public class TrelloController : ApiController
    {

        // POST: api/Trello
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Trello/5
        public void Put(int id, [FromBody]string value)
        {
        }

    }
}
