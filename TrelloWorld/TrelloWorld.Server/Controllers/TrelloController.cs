﻿using System.Threading.Tasks;
using System.Web.Http;
using TrelloNet;
using TrelloWorld.Server.Services;

namespace TrelloWorld.Server.Controllers
{
    public class TrelloController : ApiController
    {
        private readonly ITrelloService _service;

        public TrelloController(ITrelloService service = null)
        {
            var config = new Config();
            var trello = new Trello(config.Key);
            trello.Authorize(config.Token);
            _service = service ?? new TrelloService(trello.Async);
        }

        // POST: api/Trello
        public async Task Post([FromBody]dynamic value)
        {
            dynamic commits = value.commits;

            foreach (dynamic comit in commits)
            {
                string msg = comit.message;
                await _service.AddComment(msg);
            }
        }
    }
}