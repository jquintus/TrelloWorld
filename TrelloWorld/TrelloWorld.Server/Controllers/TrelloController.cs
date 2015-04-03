using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using MarkdownSharp;
using TrelloNet;
using TrelloWorld.Server.Services;

namespace TrelloWorld.Server.Controllers
{
    public class TrelloController : ApiController
    {
        private readonly string _authUrl;
        private readonly Config _config;
        private readonly ITrelloService _service;

        public TrelloController()
        {
            _config = new Config();
            var trello = new Trello(_config.Key);
            trello.Authorize(_config.Token);

            _authUrl = trello.GetAuthorizationUrl("TrelloWorld", Scope.ReadWrite, Expiration.Never).ToString();
            _service = new TrelloService(trello.Async);
        }

        public TrelloController(ITrelloService service)
        {
            var config = new Config();
            _service = service;
        }

        public async Task<HttpResponseMessage> Get()
        {
            var response = new HttpResponseMessage();

            string content = "Setup complete.";

            if (string.IsNullOrWhiteSpace(_config.Key))
            {
                content = await new MarkdownService(HttpContext.Current.Server.MapPath("~/MarkdownViews")).GetConfigureAppKey();
            }
            else if (string.IsNullOrWhiteSpace(_config.Token))
            {
                content = string.Format("Get your secret token from <a herf='{0}'> Trello </a> ", _authUrl);
            }

            response.Content = new StringContent(string.Format("<html><body>{0}</body></html>", content));
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
