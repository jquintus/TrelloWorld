﻿namespace TrelloWorld.Server.Tests.Services
{
    using Models;
    using Newtonsoft.Json.Linq;
    using NUnit.Framework;
    using Server.Config;
    using Server.Services;
    using System.Collections.Generic;
    using System.Dynamic;
    using System.IO;
    using System.Linq;

    [TestFixture]
    public class CommitParserTests
    {
        private const string SIMPLE_JSON = @"{
  'ref': 'refs/heads/branch2',
  'commits': [
    {
      'message': 'My message',
      'url': 'https://github.com/jquintus/spikes/commit/2',
    }
  ]
}";

        private CommitParser _parser;

        private Settings _settings;

        [Test]
        public void Parse_CommitOnBranch2_ReturnsOneCommitOnBranch2()
        {
            // Assemble
            dynamic push = ParseFromFile("CommitOnBranch2.json");
            var expected = new List<Commit>
            {
                new Commit
                {
                    Branch = "branch2",
                    CardId = null,
                    Message = "sample commit message",
                    CommitUrl = @"https://github.com/jquintus/spikes/commit/629b493f34631ddc43f71421a35b2688619647c8",
                }
            };

            // Act
            var actual = _parser.Parse(push);

            // Assemble
            CollectionAssert.AreEqual(expected, actual);
        }

        [Test]
        public void Parse_CommitOnMaster_ReturnsOneCommitOnMaster()
        {
            // Assemble
            dynamic push = ParseFromFile("CommitOnMaster.json");
            var expected = new List<Commit>
            {
                new Commit
                {
                    Branch = "master",
                    CardId = null,
                    Message = "Resharper clean up",
                    CommitUrl = @"https://github.com/jquintus/spikes/commit/ec29f7be4762c722f5dbc452fe2a3735b538fee7",
                }
            };

            // Act
            var actual = _parser.Parse(push);

            // Assemble
            CollectionAssert.AreEqual(expected, actual);
        }

        [Test]
        public void Parse_MultipleCommitsOnMaster_ReturnsOneCommitOnMaster()
        {
            // Assemble
            dynamic push = ParseFromFile("MultipleCommitsOnMaster.json");
            var expected = new List<Commit>
            {
                new Commit
                {
                    Branch = "master",
                    CardId = null,
                    Message = "noop",
                    CommitUrl = @"https://github.com/jquintus/spikes/commit/629b493f34631ddc43f71421a35b2688619647c8",
                },
                new Commit
                {
                    Branch = "master",
                    CardId = null,
                    Message = "Fix capitalization of spikes",
                    CommitUrl = @"https://github.com/jquintus/spikes/commit/3b47b123c94752b95dcff64a8ff03f6934945ea0",
                }
            };

            // Act
            var actual = _parser.Parse(push);

            // Assemble
            CollectionAssert.AreEquivalent(expected, actual);
        }

        [Test]
        public void Parse_NullorEmpty_ReturnEmpty()
        {
            // Act
            var commits = _parser.Parse(null);

            // Assert
            Assert.IsEmpty(commits);
        }

        [Test]
        public void Parse_PoorlyFormated_ReturnsEmpy()
        {
            // Assemble
            ExpandoObject input = new ExpandoObject();

            // Act
            var commits = _parser.Parse(input);

            // Assert
            Assert.IsEmpty(commits);
        }

        [Test]
        [TestCase("Comments but no id", null)]
        [TestCase("The word trello but no id", null)]
        [TestCase("Trello()", null)]
        [TestCase("", null)]
        [TestCase(null, null)]
        [TestCase("Trello(abc)", "abc")]
        [TestCase("trello(abc)", "abc")]
        [TestCase("trello(Abc)", "Abc")]
        [TestCase("trello(123)", "123")]
        [TestCase("trello(abc123)", "abc123")]
        [TestCase("Comments before the id trello(abc)", "abc")]
        [TestCase("Comments before the id trello(abc) and after the id", "abc")]
        [TestCase("trello(abc) comments after the id", "abc")]
        [TestCase("trello( abc )", "abc")]
        [TestCase("trello (abc)", "abc")]
        public void ParseId(string comment, string expected)
        {
            // Act
            string actual = _parser.ParseId(comment);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestCase(@"trello.com/c/(?<cardId>\w+)/", "https://trello.com/c/abc/1-myTrelloCard", "abc")]
        [TestCase(@"trello\s*\(\s*(?<cardId>\w+)\s*\)", "Trello(abc)", "abc")]
        [TestCase(@"trello\s*\(\s*(?<cardId>\w+)\s*\)|trello.com/c/(?<cardId>\w+)/", "https://trello.com/c/abc/1-myTrelloCard", "abc")]
        [TestCase(@"trello\s*\(\s*(?<cardId>\w+)\s*\)|trello.com/c/(?<cardId>\w+)/", "Trello(abc)", "abc")]
        public void ParseId_CustomRegex(string regex, string comment, string expected)
        {
            // Assemble
            var parser = new CommitParser(new Settings { CardIdRegex = regex });

            // Act
            string actual = parser.ParseId(comment);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [Test]
        [TestCase(true, "My message\r\nhttps://github.com/jquintus/spikes/commit/2")]
        [TestCase(false, "My message")]
        public void ParseMessage_IncludeGitUrl_AppendsUrlToMessage(bool includeLinkToCommit, string expected)
        {
            // Assemble
            dynamic push = JObject.Parse(SIMPLE_JSON);
            _settings.IncludeLinkToCommit = includeLinkToCommit;

            // Act
            List<Commit> commits = _parser.Parse(push);

            // Assert
            Assert.AreEqual(expected, commits.First().Message);
        }

        [Test]
        [TestCase(true, "My message\r\nTrello(abc)")]
        [TestCase(false, "My message")]
        public void ParseMessage_IncludeCardId_AppendsUrlToMessage(bool includeCardId, string expected)
        {
            // Assemble
            dynamic push = JObject.Parse(SIMPLE_JSON);
            push.commits[0].message = @"My message
Trello(abc)";
            _settings.IncludeCardId = includeCardId;

            // Act
            List<Commit> commits = _parser.Parse(push);

            // Assert
            Assert.AreEqual(expected, commits.First().Message);
        }

        [SetUp]
        public void SetUp()
        {
            _settings = new Settings
            {
                CardIdRegex = Settings.DEFAULT_CARD_ID_REGEX,
            };
            _parser = new CommitParser(_settings);
        }

        private dynamic ParseFromFile(string path)
        {
            using (var r = new StreamReader(Path.Combine("SampleData", path)))
            {
                var json = r.ReadToEnd();
                return JObject.Parse(json);
            }
        }
    }
}