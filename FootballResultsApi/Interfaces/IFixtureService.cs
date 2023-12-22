using FootballResultsApi.Models;

namespace FootballResultsApi.Interfaces
{
    public interface IFixtureService
    {
        Task StartApp();
        List<List<FixtureDto>> GetFixtureDtosByDate(string date);
        Task checkDepricatedData();
    }
}
