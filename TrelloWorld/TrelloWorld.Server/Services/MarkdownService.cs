using MarkdownSharp;
using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace TrelloWorld.Server.Services
{
    public interface IMarkdownService
    {
        Task<string> GetConfigurationComplete();

        Task<string> GetConfigureAppKey();

        Task<string> GetConfigureTrelloToken();
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

        public async Task<string> GetConfigurationComplete()
        {
            return await GetMarkdown();
        }

        public async Task<string> GetConfigureAppKey()
        {
            return await GetMarkdown();
        }

        public async Task<string> GetConfigureTrelloToken()
        {
            return await GetMarkdown();
        }

        private string CleanFileName(string fileName)
        {
            if (fileName == null) throw new ArgumentNullException("fileName");
            if (fileName.StartsWith("Get"))
            {
                fileName = fileName.Substring(3);
            }
            return Path.Combine(_rootPath, fileName + ".md");
        }

        private async Task<string> GetMarkdown([CallerMemberName]string fileName = null)
        {
            fileName = CleanFileName(fileName);

            using (var reader = new StreamReader(fileName))
            {
                var raw = await reader.ReadToEndAsync();
                return _md.Transform(raw);
            }
        }
    }
}