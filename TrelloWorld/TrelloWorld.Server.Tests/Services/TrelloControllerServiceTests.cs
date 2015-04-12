namespace TrelloWorld.Server.Tests.Services
{
    using Models;
    using Moq;
    using NUnit.Framework;
    using Server.Config;
    using Server.Services;
    using System.Collections.Generic;
    using System.Dynamic;
    using System.Threading.Tasks;

    [TestFixture]
    public class TrelloControllerServiceTests
    {
        private TrelloControllerService _controller;
        private Mock<IMarkdownService> _md;
        private Mock<ICommitParser> _parser;
        private Mock<ITrelloWorldService> _service;
        private Settings _settings;

        [Test]
        public async Task Post_CommitListEmpty_AddCommitNotCalled()
        {
            // Assemble
            dynamic value = new ExpandoObject();
            value.commits = new List<dynamic>();

            // Act
            await _controller.Post(value);

            // Assert
            _service.Verify(s => s.AddComment(It.IsAny<Commit>()), Times.Never);
        }

        [Test]
        public async Task Post_NoCommits_Throws()
        {
            // Assemble
            dynamic value = new ExpandoObject();

            // Act
            await _controller.Post(value);

            // Assert
            _service.Verify(s => s.AddComment(It.IsAny<Commit>()), Times.Never());
        }

        [Test]
        public async Task Post_NullBody_DoesNothing()
        {
            // Act
            await _controller.Post(null);

            // Assert
            _service.Verify(s => s.AddComment(It.IsAny<Commit>()), Times.Never());
        }

        [Test]
        public async Task Post_TwoCommitsWithMessage_CallsAddCommitTwice()
        {
            // Assemble
            var commits = new List<Commit>
            {
                new Commit {Message = "msg1"},
                new Commit {Message = "msg2"},
            };
            dynamic input = new ExpandoObject();

            _parser.Setup(p => p.Parse(It.IsAny<object>())).Returns(commits);

            // Act
            await _controller.Post(input);

            // Assert
            _service.Verify(s => s.AddComment(It.IsAny<Commit>()), Times.Exactly(2));
            _service.Verify(s => s.AddComment(commits[0]), Times.Once);
            _service.Verify(s => s.AddComment(commits[1]), Times.Once);
        }

        [SetUp]
        public void SetUp()
        {
            _service = new Mock<ITrelloWorldService>();
            _md = new Mock<IMarkdownService>();
            _settings = new Settings();
            _parser = new Mock<ICommitParser>();
            _controller = new TrelloControllerService(_settings, _service.Object, _md.Object, _parser.Object);
        }
    }
}