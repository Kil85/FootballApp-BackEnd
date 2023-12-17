using FootballResultsApi.Models;

namespace FootballResultsApi.Interfaces
{
    public interface IFixtureService
    {
        Task StartApp();
        Task<List<List<FixtureDto>>> GetFixtureDtosByDate(string date);
    }
}
