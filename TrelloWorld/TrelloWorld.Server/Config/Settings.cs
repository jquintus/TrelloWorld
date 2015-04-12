namespace TrelloWorld.Server.Config
{
    using System.Collections.Generic;

    public class Settings
    {
        public string Key { get; set; }

        public string Token { get; set; }

        public string MarkdownRootPath { get; set; }

        public List<string> Branches { get; set; }
    }
}