using System.Collections.Generic;

namespace AlienInvasion.Server.REST.Models
{
    public class Game
    {
        public Game(IEnumerable<Team> teams)
        {
            Scoreboard = new List<TeamScore>();
            foreach (var team in teams)
                Scoreboard.Add(new TeamScore(team.Name, team.Score));
            Links = new List<RestLink> {RestLink.RegisterTeam()};
        }

        public IList<TeamScore> Scoreboard { get; private set; }
        public IList<RestLink> Links { get; private set; } 
    }
}