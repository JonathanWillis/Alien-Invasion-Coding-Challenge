using System.Collections.Generic;
using System.Linq;
using AlienInvasion.Server.REST.Controllers;

namespace AlienInvasion.Server.REST.Models
{
    public interface ITeamRepository
    {
        IEnumerable<Team> AllTeams();
        Team AddTeam(Team team);
        Team GetTeam(string id);
    }

    public class InMemoryDataRepository : ITeamRepository
    {
        private static IList<Team> _teams;

        static InMemoryDataRepository()
        {
            _teams = new List<Team>();
            _teams.Add(new Team("Team REST", CityName.London, "0"));
        }

        public IEnumerable<Team> AllTeams()
        {
            return _teams;
        }

        public Team AddTeam(Team team)
        {
            _teams.Add(team);
            return team;
        }

        public Team GetTeam(string id)
        {
            return _teams.First(x => x.Id.Equals(id));
        }
    }
}