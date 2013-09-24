using System.Net;
using System.Net.Http;
using System.Web.Http;
using AlienInvasion.Server.REST.Models;

namespace AlienInvasion.Server.REST.Controllers
{
    public class CityController : ApiController
    {
        [HttpGet]
        public HttpResponseMessage Get(string id)
        {
            City city = null;
            switch (id.ToLower())
            {
                case CityName.London:
                    city = City.London();
                    break;
                case CityName.Rome:
                    city = City.Rome();
                    break;
            }
            return Request.CreateResponse(HttpStatusCode.OK, city);
        }
    }
}
