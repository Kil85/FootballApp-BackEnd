using System.ComponentModel.DataAnnotations.Schema;

namespace FootballResultsApi.Entities
{
    public class Team
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Logo { get; set; }

        //public int VenueId { get; set; }

        //public virtual Venue Venue { get; set; }
        public List<Fixture> Fixtures { get; set; }
        public int LeagueId { get; set; }
        public virtual League League { get; set; }
    }
}
