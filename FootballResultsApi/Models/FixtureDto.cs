using FootballResultsApi.Entities;

namespace FootballResultsApi.Models
{
    public class FixtureDto
    {
        public int Id { get; set; }
        public string Referee { get; set; }
        public string TimeZone { get; set; }
        public DateTime Date { get; set; }
        public string Status { get; set; }
        public int? HomeTeamGoals { get; set; }
        public int? AwayTeamGoals { get; set; }
        public LeagueDto League { get; set; }
        public TeamDto HomeTeam { get; set; }
        public TeamDto AwayTeam { get; set; }
    }
}
