using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web.Http;
using AlienInvasion.Server.REST.Models;

namespace AlienInvasion.Server.REST.Controllers
{
    public class GameController : ApiController
    {
        private readonly ITeamRepository _teamRepository;

        public GameController()
        {
            _teamRepository = new InMemoryDataRepository();
        }

        [HttpGet]
        public HttpResponseMessage Get()
        {
            var game = new Game(_teamRepository.AllTeams());
            return Request.CreateResponse(HttpStatusCode.OK, game);
        }
    }
}