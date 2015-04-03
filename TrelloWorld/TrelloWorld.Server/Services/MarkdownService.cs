using MarkdownSharp;
using System.IO;
using System.Threading.Tasks;

namespace TrelloWorld.Server.Services
{
    public class MarkdownService
    {
        private readonly string _rootPath;

        public MarkdownService(string rootPath)
        {
            _rootPath = rootPath;
        }
        public async Task<string> GetConfigureAppKey()
        {
            return await GetMarkdown(Path.Combine(_rootPath, "ConfigureAppKey.md"));
        }

        private static async Task<string> GetMarkdown(string fileName)
        {
            Markdown md = new Markdown();

            using (var reader = new StreamReader(fileName))
            {
                var raw = await reader.ReadToEndAsync();
                return md.Transform(raw);
            }
        }
    }
}