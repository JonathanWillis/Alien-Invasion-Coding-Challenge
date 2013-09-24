using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AlienInvasion.Server.REST.Models;

namespace AlienInvasion.Server.REST.Controllers
{
    public class TeamController : ApiController
    {
        private readonly ITeamRepository _teamRepository;

        public TeamController()
        {
            _teamRepository = new InMemoryDataRepository();
        }

        [HttpPost]
        public HttpResponseMessage Post([FromBody]CreateTeamRequest request)
        {
            var team = _teamRepository.AddTeam(new Team(request.Name));
            var response = Request.CreateResponse(HttpStatusCode.Created);
            response.Headers.Location = new Uri(string.Format("http://localhost/team/{0}", team.Id));
            return response;
        }

        [HttpGet]
        public HttpResponseMessage Get(string id)
        {
            var team = _teamRepository.GetTeam(id);
            return Request.CreateResponse(HttpStatusCode.OK, team);
        }
    }
}
