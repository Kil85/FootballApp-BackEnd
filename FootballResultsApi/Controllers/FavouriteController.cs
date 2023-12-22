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

        [HttpPost("league")]
        public ActionResult AddFavouriteLeague([FromBody] FavouriteDto favouriteDto)
        {
            _favourite.AddFavouriteLeague(favouriteDto);

            return Ok();
        }

        [HttpDelete("league")]
        public ActionResult RemoveFavouriteLeague([FromBody] FavouriteDto favouriteDto)
        {
            _favourite.DeleteFavouriteLeague(favouriteDto);

            return Ok();
        }

        [HttpGet("favteams")]
        public ActionResult GetFixturesOfFavTeam([FromQuery] int userId)
        {
            var result = _favourite.GetFixtureListbyFavouriteTeams(userId);
            return Ok(result);
        }
    }
}
