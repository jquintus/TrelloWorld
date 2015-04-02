using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TrelloNet;

namespace TrelloWorld.Server.Services
{
    public class TrelloService : ITrelloService
    {
        private readonly Regex _idRegex;
        private readonly IAsyncTrello _trello;

        public TrelloService(IAsyncTrello trello)
        {
            if (trello == null) throw new ArgumentNullException();
            _trello = trello;
            _idRegex = new Regex(@"trello\(\s*(\w+)\s*\)", RegexOptions.IgnoreCase);
        }

        public async Task AddComment(string rawComment)
        {
            try
            {
                string cardId = ParseId(rawComment);
                if (string.IsNullOrWhiteSpace(cardId)) return;

                var card = await _trello.Cards.WithId(cardId);

                if (card == null) return;
                await _trello.Cards.AddComment(card, rawComment);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }

        public string ParseId(string rawComment)
        {
            if (string.IsNullOrWhiteSpace(rawComment)) return null;

            var match = _idRegex.Match(rawComment);

            if (match.Success && match.Groups.Count > 1)
            {
                string id = match.Groups[1].Value;
                return id.Trim();
            }
            return null;
        }
    }
}