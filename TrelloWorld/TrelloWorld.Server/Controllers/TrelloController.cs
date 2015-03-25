using System.Threading.Tasks;
using System.Web.Http;
using TrelloNet;
using TrelloWorld.Server.Services;

namespace TrelloWorld.Server.Controllers
{
    public class TrelloController : ApiController
    {
        private readonly TrelloService _service;

        public TrelloController()
        {
            var config = new Config();
            var trello = new Trello(config.Key);
            trello.Authorize(config.Token);
            _service = new TrelloService(trello.Async);
        }

        // POST: api/Trello
        public async Task Post([FromBody]string value)
        {
            await _service.AddComment(value);
        }
    }
}