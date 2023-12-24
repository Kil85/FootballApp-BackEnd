namespace FootballResultsApi.Models
{
    public class TeamNFavMatches
    {
        public TeamDto Team { get; set; }
        public List<List<FixtureDto>> Fixtures { get; set; }
    }
}
