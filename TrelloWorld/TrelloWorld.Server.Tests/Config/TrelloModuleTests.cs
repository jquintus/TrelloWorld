namespace TrelloWorld.Server.Tests.Config
{
    using NUnit.Framework;
    using Server.Services;
    using TrelloWorld.Server.Config;

    [TestFixture]
    public class TrelloModuleTests
    {
        [Test]
        public void GetRoot_ReturnsControllerService()
        {
            // Act
            ITrelloController service = TrelloModule.GetRoot();

            // Assert
            Assert.IsNotNull(service);
        }
    }
}