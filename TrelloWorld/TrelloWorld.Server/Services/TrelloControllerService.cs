namespace TrelloWorld.Server.Services
{
    using Config;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;
    using System.Web.Http;

    public class TrelloControllerService : ITrelloController
    {
        private readonly Settings _config;
        private readonly IMarkdownService _md;
        private readonly ITrelloWorldService _service;
        private readonly ICommitParser _parser;

        public TrelloControllerService(Settings config, ITrelloWorldService service, IMarkdownService md, ICommitParser parser)
        {
            _config = config;
            _service = service;
            _md = md;
            _parser = parser;
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
                content = await _md.GetConfigureTrelloToken(_service.AuthUrl);
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

        public async Task Post([FromBody]dynamic value)
        {
            var commits = _parser.Parse(value);

            foreach (var commit in commits)
            {
                await _service.AddComment(commit);
            }
        }
    }
}