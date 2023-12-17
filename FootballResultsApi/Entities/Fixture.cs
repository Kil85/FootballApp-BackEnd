namespace FootballResultsApi.Entities
{
    public class Fixture
    {
        public int Id { get; set; }
        public string Referee { get; set; }
        public string TimeZone { get; set; }

        //public DateTime Date { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }

        public string Status { get; set; }
        public int? HomeTeamGoals { get; set; }
        public int? AwayTeamGoals { get; set; }
        public int LeagueId { get; set; }
        public virtual League League { get; set; }

        public int MetaDataId { get; set; }
        public virtual MetaData MetaData { get; set; }
        public int HomeTeamId { get; set; }
        public virtual Team HomeTeam { get; set; }
        public int AwayTeamId { get; set; }
        public virtual Team AwayTeam { get; set; }
    }
}
