namespace AlienInvasion.Server.REST.Models
{
    public class TeamScore
    {
        public string Name { get; private set; }
        public string Score { get; private set; }

        public TeamScore(string name, string score)
        {
            Name = name;
            Score = score;
        }
    }
}