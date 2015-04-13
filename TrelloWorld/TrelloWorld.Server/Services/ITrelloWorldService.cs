using System.Threading.Tasks;

namespace TrelloWorld.Server.Services
{
    using Models;

    public interface ITrelloWorldService
    {
        Task<string> AddComment(Commit rawComment);

        string AuthUrl { get; }
    }
}