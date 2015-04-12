namespace TrelloWorld.Server.Services
{
    using System.Web;
    using System.Web.Configuration;
    using TrelloWorld.Server.Config;

    public class SettingsLoader : ISettingsLoader<Settings>
    {
        public Settings Load()
        {
            string root = HttpContext.Current == null
                ? string.Empty
                : HttpContext.Current.Server.MapPath("~/MarkdownViews");

            return new Settings()
            {
                Key = WebConfigurationManager.AppSettings["Trello.Key"],
                Token = WebConfigurationManager.AppSettings["Trello.Token"],
                MarkdownRootPath = root,
            };
        }
    }
}