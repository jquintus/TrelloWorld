using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
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

        public HttpResponseMessage Get()
        {
            var response = new HttpResponseMessage();

            string content = "Setup complete.";

            if (string.IsNullOrWhiteSpace(_config.Key))
            {


                Markdown md = new Markdown();

                content = md.Transform(@"
##  Make sure to configure the Trello app key in Azure  ##

### Getting the App Key ###

The app key can be found on [Trello's Developer Page](https://trello.com/app-key) 

Paste the value found in the **Key** section. 

![Trello's Developer Page](Assets/Trello_Developer_Page.png)

### Setting the App Key ###
Log on to the [Azure Management Portal](https://manage.windowsazure.com/)  and go to the configuration tab for this website. 

![](Assets/Azure_Config.png)


Scroll down to the **app settings** section and enter a new setting 

- **KEY**:  Trello.Key
- **VALUE**:  [the key you got from Trello]

![](Assets/Azure_AppSettings.png)


*Note*: Trello.Token will be filled in durin the next step.

### Next Steps ###
Once you complete this step, refresh the page.  If you did everything right you will see the next steps. 

                    
                    ");
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
