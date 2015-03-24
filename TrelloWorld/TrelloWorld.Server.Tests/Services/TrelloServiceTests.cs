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
        private Mock<IAsyncTrello> _trelloMock;
        private TrelloService _service;

        [Test]
        [Ignore]
        public async Task AddComment_RawCommentIsNull_DoesNothing()
        {
            // Act
            await _service.AddComment(null);

            // Assert
            _trelloMock.Verify(t => t.Cards.AddComment(It.IsAny<ICardId>(), It.IsAny<string>()), Times.Never);

        }

        [SetUp]
        public void SetUp()
        {
            _trelloMock = new Mock<IAsyncTrello>();
            _service = new TrelloService(_trelloMock.Object);
        }

        [Test]
        [TestCase("Comments but no id", null)]
        [TestCase("The word trello but no id", null)]
        [TestCase("Trello()", null)]
        [TestCase("", null)]

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
    }
}
