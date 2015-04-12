namespace TrelloWorld.Server.Services
{
    using System.Net.Http;
    using System.Threading.Tasks;

    public interface ITrelloController
    {
        Task<HttpResponseMessage> Get();
        Task Post(dynamic value);
    }
}