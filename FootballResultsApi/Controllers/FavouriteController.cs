using FootballResultsApi.Interfaces;
using FootballResultsApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace FootballResultsApi.Controllers
{
    [Route("api/favourite")]
    [ApiController]
    public class FavouriteController : ControllerBase
    {
        private readonly IFavouriteService _favourite;

        public FavouriteController(IFavouriteService favourite)
        {
            _favourite = favourite;
        }

        [HttpPost("team")]
        public ActionResult AddFavouriteTeam([FromBody] FavouriteDto favouriteDto)
        {
            _favourite.AddFavouriteTeam(favouriteDto);
            return Ok();
        }

        [HttpDelete("team")]
        public ActionResult RemoveFavouriteTeam([FromBody] FavouriteDto favouriteDto)
        {
            _favourite.DeleteFavouriteTeam(favouriteDto);
            return Ok();
        }
    }
}
