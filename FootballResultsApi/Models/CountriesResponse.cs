namespace FootballResultsApi.Models
{
    public class CountriesResponse
    {
        public string Get { get; set; }
        public List<object> Parameters { get; set; }
        public List<object> Errors { get; set; }
        public int Results { get; set; }
        public Paging Paging { get; set; }
        public List<Country> Response { get; set; }
    }

    public class Country
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public string Flag { get; set; }
    }
}
