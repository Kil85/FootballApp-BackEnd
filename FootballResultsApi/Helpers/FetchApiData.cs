using FootballResultsApi.Entities;
using FootballResultsApi.Interfaces;
using FootballResultsApi.Models;
using Newtonsoft.Json;

namespace FootballResultsApi.Helpers
{
    public class FetchApiData
    {
        private readonly IFixtureService _fixtureService;

        public FetchApiData(IFixtureService fixtureService)
        {
            _fixtureService = fixtureService;
        }

        public async Task startApp()
        {
            await _fixtureService.StartApp();
        }
    }
}
