namespace AlienInvasion.Server.REST.Models
{
    public class RestLink
    {
        public string Rel { get; set; }
        public string Url { get; set; }

        private RestLink(string rel, string url)
        {
            Rel = rel;
            Url = url;
        }

        public static RestLink RegisterTeam()
        {
            return new RestLink("Register", @"http:\\localhost\team");
        }

        public static RestLink RequestInvasion()
        {
            return new RestLink("RequestInvasion", @"http:\\localhost\Invasion");
        }

        public static RestLink CurrentWave(string id)
        {
            return new RestLink("CurrentWave", string.Format(@"http:\\localhost\Invasion\{0}", id));
        }

        public static RestLink Briefing(string city)
        {
            return new RestLink("Briefing", string.Format(@"http:\\localhost\city\{0}", city));
        }
    }
}