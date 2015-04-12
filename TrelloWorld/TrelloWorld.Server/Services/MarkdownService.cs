using MarkdownSharp;
using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace TrelloWorld.Server.Services
{
    using Config;

    public class MarkdownService : IMarkdownService
    {
        private readonly Markdown _md;
        private readonly string _rootPath;

        public MarkdownService(Settings settings)
        {
            _rootPath = settings.MarkdownRootPath;
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

        public async Task<string> GetConfigureTrelloToken(string trelloUrl)
        {
            return await GetMarkdown(md => string.Format(md, trelloUrl));
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

        private async Task<string> GetMarkdown(Func<string, string> preprocessMarkDown = null, [CallerMemberName]string fileName = null)
        {
            fileName = CleanFileName(fileName);

            using (var reader = new StreamReader(fileName))
            {
                var raw = await reader.ReadToEndAsync();
                if (preprocessMarkDown != null) raw = preprocessMarkDown(raw);
                return _md.Transform(raw);
            }
        }
    }
}