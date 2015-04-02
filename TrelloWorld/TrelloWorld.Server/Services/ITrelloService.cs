using System.Threading.Tasks;

namespace TrelloWorld.Server.Services
{
    public interface ITrelloService
    {
        Task AddComment(string rawComment);
    }
}