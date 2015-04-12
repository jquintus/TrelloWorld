namespace TrelloWorld.Server.Config
{
    using System.Collections.Generic;

    public class Settings
    {
        public const string DEFAULT_CARD_ID_REGEX = @"trello\s*\(\s*(?<cardId>\w+)\s*\)";

        public List<string> Branches { get; set; }

        public string CardIdRegex { get; set; }

        public bool IncludeLinkToCommit { get; set; }

        public string Key { get; set; }

        public string MarkdownRootPath { get; set; }

        public bool IncludeCardId{ get; set; }

        public string Token { get; set; }
    }
}