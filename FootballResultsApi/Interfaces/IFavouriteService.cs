using FootballResultsApi.Entities;
using FootballResultsApi.Models;

namespace FootballResultsApi.Interfaces
{
    public interface IFavouriteService
    {
        void AddFavouriteTeam(FavouriteDto favouriteDto);
        void DeleteFavouriteTeam(FavouriteDto favouriteDto);
        void AddFavouriteLeague(FavouriteDto favouriteDto);
        void DeleteFavouriteLeague(FavouriteDto favouriteDto);
        List<TeamNFavMatches> GetFixtureListbyFavouriteTeams(int userId);
        List<List<FixtureDto>> GetFixtureListbyFavouriteLeagues(int userId, string date);
    }
}
