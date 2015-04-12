namespace TrelloWorld.Server.Services
{
    using Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;

    public interface ICommitParser
    {
        IEnumerable<Commit> Parse(dynamic push);
    }

    public class CommitParser : ICommitParser
    {
        private readonly Regex _idRegex;

        public CommitParser()
        {
            _idRegex = new Regex(@"trello\(\s*(\w+)\s*\)", RegexOptions.IgnoreCase);
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
                string id = match.Groups[1].Value;
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
                string msg = commit.message;
                string gitUrl = commit.url;

                return new Commit
                {
                    Branch = branch,
                    CardId = ParseId(msg),
                    Message = msg,
                    CommitUrl = gitUrl,
                };
            }
            catch
            {
                return null;
            }
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