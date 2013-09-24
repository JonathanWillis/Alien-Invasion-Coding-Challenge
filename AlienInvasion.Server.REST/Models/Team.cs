using System.Collections.Generic;
using AlienInvasion.Server.REST.Controllers;

namespace AlienInvasion.Server.REST.Models
{
    public class Team
    {
        private static int teamId = 1;

        public string Id { get; private set; }
        public string Name { get; private set; }
        public string Level { get; private set; }
        public string Score { get; private set; }
        public IList<RestLink> Links { get; private set; }

        public Team(string name, string level, string score)
        {
            Id = NextTeamId();
            Name = name;
            Level = level;
            Score = score;
            Links = new List<RestLink> {RestLink.RequestInvasion(), RestLink.Briefing(Level)};
        }

        public Team(string name)
        {
            Id = NextTeamId(); 
            Name = name;
            Level = CityName.London;
            Score = "0";
            Links = new List<RestLink> { RestLink.RequestInvasion(), RestLink.Briefing(Level) };
        }

        private static string NextTeamId()
        {
            return teamId++.ToString();
        }
    }
}