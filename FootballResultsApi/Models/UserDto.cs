namespace FootballResultsApi.Models
{
    public class UserDto
    {
        public string JWT { get; set; }
        public List<int> LeaguesIds { get; set; }
        public List<int> TeamsIds { get; set; }
    }
}
