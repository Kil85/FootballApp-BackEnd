using System.Reflection.Metadata.Ecma335;

namespace FootballResultsApi.Entities
{
    public class Country
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Flag { get; set; }
        public List<League> Leagues { get; set; } = new List<League>();
    }
}
