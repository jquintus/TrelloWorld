namespace TrelloWorld.Server.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
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
                Branches = ReadList("Trello.Branches"),
                MarkdownRootPath = root,
            };
        }

        private List<string> ReadList(string key)
        {
            var value = WebConfigurationManager.AppSettings[key];
            if (string.IsNullOrWhiteSpace(value)) return new List<string>();

            return value.Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries).ToList();
        }
    }
}