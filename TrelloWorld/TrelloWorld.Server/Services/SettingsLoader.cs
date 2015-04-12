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
                MarkdownRootPath = root,
                Key = WebConfigurationManager.AppSettings["Trello.Key"],
                Token = WebConfigurationManager.AppSettings["Trello.Token"],
                Branches = ReadList("Trello.Branches"),
                IncludeLinkToCommit = ReadBool("Trello.IncludeLinkToCommit"),
            };
        }

        private bool ReadBool(string key, bool defaultValue = false)
        {
            var stringValue = WebConfigurationManager.AppSettings[key];
            if (string.IsNullOrWhiteSpace(stringValue)) return defaultValue;

            bool value = defaultValue;
            if (bool.TryParse(stringValue, out value))
            {
                return value;
            }
            return defaultValue;
        }

        private List<string> ReadList(string key)
        {
            var value = WebConfigurationManager.AppSettings[key];
            if (string.IsNullOrWhiteSpace(value)) return new List<string>();

            return value.Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries).ToList();
        }
    }
}