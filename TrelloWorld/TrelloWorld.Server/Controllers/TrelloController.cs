using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;
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

        public HttpResponseMessage Get()
        {
            var response = new HttpResponseMessage();

            string content = "Setup complete.";

            if (string.IsNullOrWhiteSpace(_config.Key))
            {
                content = @"<h1>Make sure to configure the Trello app key in Azure </h1>
                  <h2> Getting the App Key</h2>
                  The app key can be found on <a href='https://trello.com/app-key'> Trello's Developer Page </a>  <p/>
                  Paste the value found in the <b>Key</b> section. <p/>

                  <a href='https://trello.com/app-key'>
                      <img src='Assets/Trello_Developer_Page.png' alt=""Trello's Developer Page"" border='1' />
                  </a>

                  <h2> Setting the App Key</h2>
                  Log on to the <a href='https://manage.windowsazure.com/'> Azure Management Portal </a> and go to the configuration tab for this website. <p/>
                  <img src='Assets/Azure_Config.png' border='1' />

                  <p/>

                  Scroll down to the <b> app settings </b> section and enter a new setting <p/>
                  <ul>
                    <li><b>KEY</b>:  Trello.Key</li>
                    <li><b>VALUE</b>:  [the key you got from Trello]</li>
                  </ul>

                  <img src='Assets/Azure_AppSettings.png' border='1' />
                  <p/>

                  <i>Note:  </i> Trello.Token will be filled in durin the next step.

                  <h2> Next Steps</h2>
                  Once you complete this step, refresh the page.  If you did everything right you will see the next steps.

";
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