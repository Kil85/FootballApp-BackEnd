using FootballResultsApi.Entities;

namespace FootballResultsApi.Models
{
    public class LeagueDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Logo { get; set; }
        public CountryDto Country { get; set; }
    }
}
