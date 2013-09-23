using System.Linq;
using System.Net;
using System.Net.Http;
using AlienInvasion.Server.REST.Controllers;
using NUnit.Framework;

namespace AlienInvasion.Server.REST.Tests
{
    [TestFixture]
    public class GivenAGameToCreate
    {
        private HttpResponseMessage _result;

        [SetUp]
        public void Setup()
        {
            var subject = new GameController();
            _result = subject.Create();
        }

        [Test]
        public void WhenCreatingANewGameThenStatusCodeReturnedCreated()
        {
            Assert.That(_result.StatusCode, Is.EqualTo(HttpStatusCode.Created));
        }

        [Test]
        public void WhenCreatingANewGameThenLocationHeaderPointsToGameUri()
        {
            StringAssert.EndsWith("/game/1", _result.Headers.Location.ToString());
        }
    }

    [TestFixture]
    public class GivenANonExistantGameToRetrieve
    {
        [Test]
        public void WhenRetrievingThenStatusCodeNotFoundIsReturned()
        {
            var controller = new GameController();
            var response = controller.Get("A");
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }
    }

    [TestFixture]
    public class GivenAnExistingGameToRetrieve
    {
        [Test]
        public void WhenRetrievingThenStatusCodeOkIsReturned()
        {
            var controller = new GameController();
            var createResponse = controller.Create();
            var response = controller.Get(createResponse.Headers.Location.ToString().Last().ToString());
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }
    }
}
