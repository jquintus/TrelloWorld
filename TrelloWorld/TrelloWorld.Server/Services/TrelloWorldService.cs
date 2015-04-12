using System;
using System.Threading.Tasks;
using TrelloNet;

namespace TrelloWorld.Server.Services
{
    using Models;

    public class TrelloWorldService : ITrelloWorldService
    {
        private readonly IAsyncTrello _asyncTrello;
        private readonly ITrello _trello;

        public TrelloWorldService(ITrello trello, IAsyncTrello asyncTrello)
        {
            if (asyncTrello == null) throw new ArgumentNullException();
            if (trello == null) throw new ArgumentNullException();

            _trello = trello;
            _asyncTrello = asyncTrello;
        }

        public string AuthUrl
        {
            get { return _trello.GetAuthorizationUrl("TrelloWorld", Scope.ReadWrite, Expiration.Never).ToString(); }
        }

        public async Task AddComment(Commit commit)
        {
            try
            {
                if (commit == null || string.IsNullOrWhiteSpace(commit.CardId)) return;

                var card = await _asyncTrello.Cards.WithId(commit.CardId);

                if (card == null) return;
                await _asyncTrello.Cards.AddComment(card, commit.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }
    }
}