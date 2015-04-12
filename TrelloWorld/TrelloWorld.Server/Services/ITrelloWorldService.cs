using System.Threading.Tasks;

namespace TrelloWorld.Server.Services
{
    public interface ITrelloWorldService
    {
        Task AddComment(string rawComment);

        string AuthUrl { get; }
    }
}