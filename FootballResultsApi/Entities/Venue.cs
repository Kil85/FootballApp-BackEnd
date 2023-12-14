namespace FootballResultsApi.Entities
{
    public class Venue
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public int FixtureId { get; set; }
    }
}
