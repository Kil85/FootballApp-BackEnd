using System.ComponentModel.DataAnnotations.Schema;

namespace FootballResultsApi.Entities
{
    public class League
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Logo { get; set; }
        public List<Team> Teams { get; set; }
        public string Country { get; set; }
        public List<Fixture> Fixtures { get; set; }
    }
}
