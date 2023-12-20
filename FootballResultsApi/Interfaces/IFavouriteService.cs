using FootballResultsApi.Models;

namespace FootballResultsApi.Interfaces
{
    public interface IFavouriteService
    {
        void AddFavouriteTeam(FavouriteDto favouriteDto);
        void DeleteFavouriteTeam(FavouriteDto favouriteDto);
    }
}
