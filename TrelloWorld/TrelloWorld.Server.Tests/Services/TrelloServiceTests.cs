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
    }
}