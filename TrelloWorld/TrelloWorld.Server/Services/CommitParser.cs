namespace TrelloWorld.Server.Services
{
    using Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;
    using TrelloWorld.Server.Config;

    public class CommitParser : ICommitParser
    {
        private readonly Regex _idRegex;
        private readonly Settings _settings;

        public CommitParser(Settings settings)
        {
            _settings = settings;
            _idRegex = new Regex(settings.CardIdRegex, RegexOptions.IgnoreCase);
        }

        public IEnumerable<Commit> Parse(dynamic push)
        {
            try
            {
                if (push != null)
                {
                    IEnumerable<Commit> commits = UnSafeParse(push);
                    return commits.Where(c => c != null).ToList();  // We want to materialize the enumerable within this try/catch, not later.
                }
                else
                {
                    return Enumerable.Empty<Commit>();
                }
            }
            catch
            {
                return Enumerable.Empty<Commit>();
            }
        }

        public string ParseId(string rawComment)
        {
            if (string.IsNullOrWhiteSpace(rawComment)) return null;

            var match = _idRegex.Match(rawComment);

            if (match.Success && match.Groups.Count > 1)
            {
                string id = match.Groups["cardId"].Value;
                return id.Trim();
            }
            return null;
        }

        private string ParseBranch(dynamic push)
        {
            try
            {
                string branch = push.@ref;
                branch = branch.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries)
                    .LastOrDefault();
                return branch ?? string.Empty;
            }
            catch
            {
                return string.Empty;
            }
        }

        private Commit ParseCommit(dynamic commit, string branch)
        {
            try
            {
                string gitUrl = commit.url;
                string rawMsg = commit.message;
                string cardId = ParseId(rawMsg);
                string formattedMessage = FormatMessage(rawMsg, gitUrl);

                return new Commit
                {
                    Branch = branch,
                    CardId = cardId,
                    Message = formattedMessage,
                    CommitUrl = gitUrl,
                };
            }
            catch
            {
                return null;
            }
        }

        private string FormatMessage(string rawMsg, string gitUrl)
        {
            if (!_settings.IncludeCardId)
            {
                rawMsg = _idRegex.Replace(rawMsg, string.Empty);
            }
            if (_settings.IncludeLinkToCommit)
            {
                rawMsg += Environment.NewLine + gitUrl;
            }
            return rawMsg.Trim();
        }

        private IEnumerable<Commit> UnSafeParse(dynamic push)
        {
            dynamic commits = push.commits;
            string branch = ParseBranch(push);

            foreach (dynamic commit in commits)
            {
                yield return ParseCommit(commit, branch);
            }
        }
    }
}