using Moq;
using NUnit.Framework;
using System.Threading.Tasks;
using TrelloNet;
using TrelloWorld.Server.Services;

namespace TrelloWorld.Server.Tests.Services
{
    [TestFixture]
    public class TrelloServiceTests
    {
        private Mock<IAsyncCards> _cardsMock;
        private TrelloWorldService _service;
        private Mock<IAsyncTrello> _asyncTrelloMock;
        private Mock<ITrello> _TrelloMock;

        [Test]
        public async Task AddComment_CommentContainsTrelloId_CommentAdded()
        {
            // Assemble
            string comment = "This is my comment Trello(123)";
            Card card = new Card();
            _cardsMock.Setup(c => c.WithId("123")).Returns(Task.FromResult(card));

            // Act
            await _service.AddComment(comment);

            // Assert
            _cardsMock.Verify(c => c.AddComment(card, comment), Times.Once());
        }

        [Test]
        public async Task AddComment_CommentContainsTrelloId_RequestsCardWithThatId()
        {
            // Act
            await _service.AddComment("Trello(abc)");

            // Assert
            _cardsMock.Verify(c => c.WithId("abc"), Times.Once());
        }

        [Test]
        public async Task AddComment_NullCardReturnedFromTrello_AddCommentNotCalled()
        {
            // Assemble
            _cardsMock.Setup(c => c.WithId("abc")).Returns<Card>(null);

            // Act
            await _service.AddComment("Trello(No card for id)");

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
        public void ParseId(string comment, string expected)
        {
            // Act
            string actual = _service.ParseId(comment);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [SetUp]
        public void SetUp()
        {
            _cardsMock = new Mock<IAsyncCards>();
            _asyncTrelloMock = new Mock<IAsyncTrello>();
            _TrelloMock = new Mock<ITrello>();

            _asyncTrelloMock.SetupGet(t => t.Cards).Returns(_cardsMock.Object);

            _service = new TrelloWorldService(_TrelloMock.Object, _asyncTrelloMock.Object);
        }
    }
}