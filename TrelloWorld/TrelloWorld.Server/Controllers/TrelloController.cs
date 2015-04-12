using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using TrelloWorld.Server.Services;

namespace TrelloWorld.Server.Controllers
{
    using Config;

    public class TrelloController : ApiController, ITrelloController
    {
        private readonly ITrelloController _service;

        public TrelloController()
        {
            _service = TrelloModule.GetRoot();
        }

        public async Task<HttpResponseMessage> Get()
        {
            return await _service.Get();
        }

        public async Task Post([FromBody]dynamic value)
        {
            await _service.Post(value);
        }
    }
}