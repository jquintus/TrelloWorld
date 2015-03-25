using System.Web.Configuration;

namespace TrelloWorld.Server.Services
{
    public class Config
    {
        public Config()
        {
            Key = WebConfigurationManager.AppSettings["Trello.Key"];
            Token = WebConfigurationManager.AppSettings["Trello.Token"];
        }

        public string Key { get; set; }

        public string Token { get; set; }
    }
}