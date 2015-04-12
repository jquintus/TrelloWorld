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
                CardIdRegex = ReadString("Trello.CardIdRegex", Settings.DEFAULT_CARD_ID_REGEX),
            };
        }

        private bool ReadBool(string key, bool defaultValue = false)
        {
            var stringValue = WebConfigurationManager.AppSettings[key];
            if (string.IsNullOrWhiteSpace(stringValue)) return defaultValue;

            bool value;
            return bool.TryParse(stringValue, out value) 
                ? value 
                : defaultValue;
        }

        private List<string> ReadList(string key)
        {
            var value = WebConfigurationManager.AppSettings[key];
            return string.IsNullOrWhiteSpace(value) 
                ? new List<string>() 
                : value.Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries).ToList();
        }

        private string ReadString(string key, string defaultValue = "")
        {
            var value = WebConfigurationManager.AppSettings[key];
            return string.IsNullOrWhiteSpace(value)
                ? defaultValue
                : value;
        }
    }
}