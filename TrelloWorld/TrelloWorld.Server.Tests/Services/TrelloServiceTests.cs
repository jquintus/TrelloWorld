using Moq;
using NUnit.Framework;
using System.Threading.Tasks;
using TrelloNet;
using TrelloWorld.Server.Services;

namespace TrelloWorld.Server.Tests.Services
{
    using Models;
    using Server.Config;

    [TestFixture]
    public class TrelloServiceTests
    {
        private Mock<IAsyncTrello> _asyncTrelloMock;
        private Mock<IAsyncCards> _cardsMock;
        private TrelloWorldService _service;
        private Mock<ITrello> _TrelloMock;

        [Test]
        public async Task AddComment_CommentContainsTrelloId_CommentAdded()
        {
            // Assemble
            Commit commit = new Commit
            {
                Message = "This is my comment Trello(123)",
                CardId = "123"
            };

            Card card = new Card();
            _cardsMock.Setup(c => c.WithId("123")).Returns(Task.FromResult(card));

            // Act
            await _service.AddComment(commit);

            // Assert
            _cardsMock.Verify(c => c.AddComment(card, commit.Message), Times.Once());
        }

        [Test]
        public async Task AddComment_CommentContainsTrelloId_RequestsCardWithThatId()
        {
            // Assemble
            Commit commit = new Commit
            {
                Message = "This is my comment Trello(123)",
                CardId = "123"
            };

            // Act
            await _service.AddComment(commit);

            // Assert
            _cardsMock.Verify(c => c.WithId("123"), Times.Once());
        }

        [Test]
        public async Task AddComment_NullCardReturnedFromTrello_AddCommentNotCalled()
        {
            // Assemble
            _cardsMock.Setup(c => c.WithId("abc")).Returns<Card>(null);

            // Act
            await _service.AddComment(new Commit());

            // Assert
            _cardsMock.Verify(c => c.AddComment(It.IsAny<Card>(), It.IsAny<string>()), Times.Never());
        }

        [Test]
        public async Task AddComment_RawCommentIsNull_DoesNothing()
        {
            // Act
            await _service.AddComment(null);

            // Assert
            _cardsMock.Verify(c => c.AddComment(It.IsAny<ICardId>(), It.IsAny<string>()), Times.Never);
        }


        [Test]
        public void Ctor_CallsAuthorize()
        {
            // Assemble
            var settings = new Settings
            {
                Token = "My Token",
            };

            var asyncTrelloMock = new Mock<IAsyncTrello>();
            var trelloMock = new Mock<ITrello>();


            // Act
            new TrelloWorldService(trelloMock.Object, asyncTrelloMock.Object, settings);

            // Assert
            trelloMock.Verify(m => m.Authorize(settings.Token), Times.Once);
        }

        private Settings _settings;
        [SetUp]
        public void SetUp()
        {
            _cardsMock = new Mock<IAsyncCards>();
            _asyncTrelloMock = new Mock<IAsyncTrello>();
            _TrelloMock = new Mock<ITrello>();
            _settings = new Settings();
            _asyncTrelloMock.SetupGet(t => t.Cards).Returns(_cardsMock.Object);

            _service = new TrelloWorldService(_TrelloMock.Object, _asyncTrelloMock.Object, _settings);
        }
    }
}