using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Dynamic;
using System.Threading.Tasks;
using TrelloWorld.Server.Controllers;
using TrelloWorld.Server.Services;

namespace TrelloWorld.Server.Tests.Controllers
{
    [TestFixture]
    public class TrelloControllerTests
    {
        private TrelloController _controller;
        private Mock<ITrelloService> _service;

        [Test]
        public async Task Post_CommitListEmpty_AddCommitNotCalled()
        {
            // Assemble
            dynamic value = new ExpandoObject();
            value.commits = new List<dynamic>();

            // Act
            await _controller.Post(value);

            // Assert
            _service.Verify(s => s.AddComment(It.IsAny<string>()), Times.Never);
        }

        [Test]
        [ExpectedException]
        public async Task Post_NoCommits_Throws()
        {
            // Assemble
            dynamic value = new ExpandoObject();

            // Act
            await _controller.Post(value);
        }

        [Test]
        [ExpectedException]
        public async Task Post_NullBody_Throws()
        {
            // Act
            await _controller.Post(null);
        }

        [Test]
        public async Task Post_OneCommitWithMessage_CallsAddCommitOnce()
        {
            // Assemble
            dynamic value = new ExpandoObject();
            dynamic msg1 = new ExpandoObject();
            msg1.message = "msg1";

            value.commits = new List<dynamic>()
            {
               msg1,
            };

            // Act
            await _controller.Post(value);

            // Assert
            _service.Verify(s => s.AddComment(It.IsAny<string>()), Times.Once);
            _service.Verify(s => s.AddComment("msg1"), Times.Once);
        }

        [Test]
        [ExpectedException]
        public async Task Post_OneCommitWithNoMessage_Throws()
        {
            // Assemble
            dynamic value = new ExpandoObject();
            dynamic msg1 = new ExpandoObject();
            value.commits = new List<dynamic>()
            {
               msg1,
            };

            // Act
            await _controller.Post(value);
        }

        [Test]
        public async Task Post_TwoCommitsWithMessage_CallsAddCommitTwice()
        {
            // Assemble
            dynamic value = new ExpandoObject();
            dynamic msg1 = new ExpandoObject();
            msg1.message = "msg1";

            dynamic msg2 = new ExpandoObject();
            msg2.message = "msg2";

            value.commits = new List<dynamic>()
            {
               msg1,
               msg2,
            };

            // Act
            await _controller.Post(value);

            // Assert
            _service.Verify(s => s.AddComment(It.IsAny<string>()), Times.Exactly(2));
            _service.Verify(s => s.AddComment("msg1"), Times.Once);
            _service.Verify(s => s.AddComment("msg2"), Times.Once);
        }

        [SetUp]
        public void SetUp()
        {
            _service = new Mock<ITrelloService>();
            _controller = new TrelloController(_service.Object);
        }
    }
}