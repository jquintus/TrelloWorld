namespace TrelloWorld.Server.Services
{
    using System.Net.Http;
    using System.Threading.Tasks;

    public interface ITrelloController
    {
        Task<HttpResponseMessage> Get();

        Task<HttpResponseMessage> Post(dynamic value);
    }
}