using AlienInvasion.Server.REST.Models;

namespace AlienInvasion.Server.REST.Controllers
{
    public class City
    {
        public string Name { get; set; }
        public string Briefing { get; set; }

        private City(string name, string briefing)
        {
            Name = name;
            Briefing = briefing;
        }

        public static City London()
        {
            return new City(CityName.London,"The London Briefing");
        }

        public static City Rome()
        {
            return new City(CityName.Rome, "The Rome Briefing");
        }
    }
}