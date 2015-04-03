using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using TrelloNet;
using TrelloWorld.Server.Services;

namespace TrelloWorld.Server.Controllers
{
    public class TrelloController : ApiController
    {
        private readonly string _authUrl;
        private readonly Config _config;
        private readonly IMarkdownService _md;
        private readonly ITrelloService _service;

        public TrelloController()
        {
            _config = new Config();
            var trello = new Trello(_config.Key);
            trello.Authorize(_config.Token);

            _authUrl = trello.GetAuthorizationUrl("TrelloWorld", Scope.ReadWrite, Expiration.Never).ToString();
            _service = new TrelloService(trello.Async);
            _md = new MarkdownService(HttpContext.Current.Server.MapPath("~/MarkdownViews"));
        }

        public TrelloController(ITrelloService service, IMarkdownService md)
        {
            _config = new Config();
            _service = service;
            _md = md;
        }

        public async Task<HttpResponseMessage> Get()
        {
            string content;
            if (string.IsNullOrWhiteSpace(_config.Key))
            {
                content = await _md.GetConfigureAppKey();
            }
            else if (string.IsNullOrWhiteSpace(_config.Token))
            {
                content = await _md.GetConfigureTrelloToken(_authUrl);
            }
            else
            {
                content = await _md.GetConfigurationComplete();
            }

            var response = new HttpResponseMessage();
            response.Content = new StringContent(content);
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/html");
            return response;
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