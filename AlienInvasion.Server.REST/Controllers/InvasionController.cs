using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AlienInvasion.Server.REST.Models;

namespace AlienInvasion.Server.REST.Controllers
{
    public class InvasionController : ApiController
    {
        private readonly ITeamRepository _teamRepository;

        public InvasionController()
        {
            _teamRepository = new InMemoryDataRepository();
        }

        //[HttpPost]
        //public HttpResponseMessage Post()
        //{
        //    var teamId = Request.Headers.GetValues("X-Team").First();
        //    var team = _teamRepository.GetTeam(teamId);
        //    var invasion = team.BeginInvasion();
        //    var response = Request.CreateResponse(HttpStatusCode.Created, invasion);
        //    response.Headers.Location = new Uri(string.Format("http://localhost/invasion/{0}", invasion.Id));
        //    return response;
        //}

        //[HttpGet]
        //public HttpResponseMessage Get()
        //{
        //    //Get invasion - return wave description + link to defenceStragtry
        //}
    }

    //public class Wave
    //{
    //    public List<Weapon> Weapons { get; set; }
    //    public string Description { get; set; }
    //}

    //public class Weapon
    //{
    //    public string Id { get; set; }
    //    public string Type { get; set; }
    //}
}
