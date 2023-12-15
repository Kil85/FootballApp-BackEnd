namespace FootballResultsApi.Entities
{
    public class MetaData
    {
        public int Id { get; set; }
        public DateTime LastRefresh { get; set; }
        public DateOnly FixtureDate { get; set; }
        public List<Fixture> Fixtures { get; set; }
    }
}
