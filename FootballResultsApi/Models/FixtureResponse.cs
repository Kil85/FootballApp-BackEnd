using System.Reflection.Metadata.Ecma335;

namespace FootballResultsApi.Models
{
    public class FixtureResponse
    {
        public string Get { get; set; }
        public Parameters Parameters { get; set; }
        public List<object> Errors { get; set; }
        public int Results { get; set; }
        public Paging Paging { get; set; }
        public List<FixtureData> Response { get; set; }
    }

    public class Parameters
    {
        public string League { get; set; }
        public string Season { get; set; }
        public string Date { get; set; }
    }

    public class Paging
    {
        public int Current { get; set; }
        public int Total { get; set; }
    }

    public class FixtureData
    {
        public FixtureDetails Fixture { get; set; }
        public LeagueDetails League { get; set; }
        public TeamDetails Teams { get; set; }
        public Goals Goals { get; set; }
    }

    public class FixtureDetails
    {
        public int Id { get; set; }
        public string Referee { get; set; }
        public string TimeZone { get; set; }
        public DateTime Date { get; set; }
        public Status Status { get; set; }
    }

    public class LeagueDetails
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Country { get; set; }
        public string Logo { get; set; }
        public string Flag { get; set; }
        public int? Season { get; set; }
        public string Round { get; set; }
    }

    public class TeamDetails
    {
        public Team Home { get; set; }
        public Team Away { get; set; }
    }

    public class Team
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Logo { get; set; }
        public bool? Winner { get; set; }
    }

    public class Status
    {
        public string Long { get; set; }
        public string Short { get; set; }
        public int? Elapsed { get; set; }
    }

    public class Goals
    {
        public int? Home { get; set; }
        public int? Away { get; set; }
    }
}
