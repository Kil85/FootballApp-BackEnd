using FootballResultsApi.Entities;

namespace FootballResultsApi.Helpers
{
    public class FetchApiData
    {
        private readonly HttpData _data;
        private readonly FootballResultsDbContext _context;
        private readonly ILogger _logger;

        public FetchApiData(
            HttpData data,
            FootballResultsDbContext context,
            ILogger<FetchApiData> logger
        )
        {
            _data = data;
            _context = context;
            _logger = logger;
        }

        public async Task FeachFixtures()
        {
            using (var client = new HttpClient())
            {
                var uriBuilder = new UriBuilder(_data.BaseLink + "/fixtures");
                var query = System.Web.HttpUtility.ParseQueryString(uriBuilder.Query);
                DateOnly currentDate = DateOnly.FromDateTime(DateTime.Now);
                string formattedDate = currentDate.ToString("yyyy-MM-dd");

                query["date"] = formattedDate; // Dodawanie parametrów
                query["season"] = currentDate.Year.ToString();
                uriBuilder.Query = query.ToString();
                string urlWithParams = uriBuilder.ToString();

                client.DefaultRequestHeaders.Add(_data.ApiKeyPropName, _data.API_KEY);
                client.DefaultRequestHeaders.Add(_data.HostPropName, _data.Host);
                Console.WriteLine(urlWithParams);
                Console.WriteLine(formattedDate);

                HttpResponseMessage response = await client.GetAsync(urlWithParams);

                if (response.IsSuccessStatusCode)
                {
                    // Pobranie zawartości odpowiedzi jako ciąg znaków
                    string responseBody = await response.Content.ReadAsStringAsync();

                    // Tutaj możesz przetworzyć odpowiedź (np. deserializacja JSON)
                    _logger.LogInformation(responseBody);
                    Console.WriteLine(responseBody);
                }
                else
                {
                    Console.WriteLine("Błąd w żądaniu HTTP. Status kod: " + response.StatusCode);
                }
            }
        }
    }
}
