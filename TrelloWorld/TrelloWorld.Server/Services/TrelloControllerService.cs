namespace TrelloWorld.Server.Services
{
    using Config;
    using Models;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;
    using System.Web.Http;

    public class TrelloControllerService : ITrelloController
    {
        private readonly Settings _config;
        private readonly IMarkdownService _md;
        private readonly ICommitParser _parser;
        private readonly ITrelloWorldService _service;

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
            IEnumerable<Commit> commits = _parser.Parse(value);

            foreach (var commit in commits.Where(FilterBranch))
            {
                await _service.AddComment(commit);
            }
        }

        private bool FilterBranch(Commit c)
        {
            if (!_config.Branches.Any()) return true;

            return _config.Branches.Contains(c.Branch);
        }
    }
}