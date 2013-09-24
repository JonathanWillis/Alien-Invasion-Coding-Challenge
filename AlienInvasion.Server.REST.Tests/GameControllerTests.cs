using System.Linq;
using System.Net;
using System.Net.Http;
using AlienInvasion.Server.REST.Controllers;
using NUnit.Framework;

namespace AlienInvasion.Server.REST.Tests
{
    [TestFixture]
    public class GivenANewGameToCreate
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
            var createdGameid = createResponse.Headers.Location.ToString().Last().ToString();
            var response = controller.Get(createdGameid);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            //Assert.That(response.Actions["Register"].Url, Is.EqualTo("/game/"));
            //Assert.That(response.Links["Briefing"].Alt, Is.EqualTo("Next mission briefing"));

            //Register (Return a briefing) Links - Briefing
            //Briefing (tells you about the current mission (your current state))+ next actions URI
            //RequestAnInvasion (1st wave, list of weapons, list of aliens),  Links - SubmitADefenceStratgy
            //SubmitADefenceStratgy (Returns summary of submitted stratgy, next wave = list of weapons, list of aliens) Links - SubmitADefenceStratgy, Briefing (when completed or failed)
            //GetScoresOfAllTeams

            //GET - Game - (Scoreboard) - (Links - (Register Team - POST Team)

            //POST - Team - (Register - Teamname) - Location to Team/{id}
            //GET - Team (Team Info) - Current Score, Current Level, Team name, (Link - (Briefing - GET City/London), (RequestAnInvasion - POST Invasion), (Current Wave - GET Invasion))

            //POST - Invasion (Start level - TeamId) - Location to invasion/{id}
            //GET - Invasion (Current wave info, aliens & weapons) - (Links - (Defend - POST DefenceStragey))

            //POST - DefenceStragey (DefendWave - Invasion/{id}, Weapons) - Location to DefenceStragey/{id}
            //GET - DefenceStragey (Results) - (Links - (NextWave - GET Invasion/{id}), (Finished - GET Team/{id}))  

            //GET - City/{name} - (Briefing)
            //GET - Alien/{size} - (Description)
            //GET - Weapon/{type} - (Description)

        }
    }
}
