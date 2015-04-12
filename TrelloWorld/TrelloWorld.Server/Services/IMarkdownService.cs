namespace TrelloWorld.Server.Services
{
    using System.Threading.Tasks;

    public interface IMarkdownService
    {
        Task<string> GetConfigurationComplete();

        Task<string> GetConfigureAppKey();

        Task<string> GetConfigureTrelloToken(string trelloUrl);
    }
}