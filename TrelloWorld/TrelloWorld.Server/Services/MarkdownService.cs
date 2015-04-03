using MarkdownSharp;
using System.IO;
using System.Threading.Tasks;

namespace TrelloWorld.Server.Services
{
    public interface IMarkdownService
    {
        Task<string> GetConfigureAppKey();
    }

    public class MarkdownService : IMarkdownService
    {
        private readonly Markdown _md;
        private readonly string _rootPath;

        public MarkdownService(string rootPath)
        {
            _rootPath = rootPath;
            _md = new Markdown();
        }

        public async Task<string> GetConfigureAppKey()
        {
            return await GetMarkdown(Path.Combine(_rootPath, "ConfigureAppKey.md"));
        }

        private async Task<string> GetMarkdown(string fileName)
        {
            using (var reader = new StreamReader(fileName))
            {
                var raw = await reader.ReadToEndAsync();
                return _md.Transform(raw);
            }
        }
    }
}