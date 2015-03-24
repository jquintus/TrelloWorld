using System;
using System.Threading.Tasks;
using TrelloNet;

namespace TrelloWorld.Server.Services
{
    public class TrelloService
    {
        private readonly IAsyncTrello _trello;

        public TrelloService(IAsyncTrello trello)
        {
            if (trello == null) throw new ArgumentNullException();
            _trello = trello;
        }

        public async Task AddComment(string rawComment)
        {
            string cardId = ParseId(rawComment);
            if (string.IsNullOrWhiteSpace(cardId)) return;

            var card = await _trello.Cards.WithId(cardId);

            if (card == null) return;
            await _trello.Cards.AddComment(card, rawComment);
        }

        private string ParseId(string rawComment)
        {
            return null;
        }

    }
}