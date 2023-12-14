using FootballResultsApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace FootballResultsApi.Controllers
{
    [Route("api/favourite")]
    [ApiController]
    public class FavouriteController : ControllerBase
    {
        [HttpPost("match")]
        public ActionResult AddFavMatch([FromQuery] int userId, [FromQuery] int matchId)
        {
            return null;
        }

        [HttpPost("league")]
        public ActionResult AddFavLeague([FromQuery] int userId, [FromQuery] int leagueId)
        {
            return null;
        }

        [HttpDelete("match")]
        public ActionResult RemoveFavMatch([FromQuery] int userId, [FromQuery] int matchId)
        {
            return null;
        }

        [HttpDelete("league")]
        public ActionResult RemoveFavLeagueh([FromQuery] int userId, [FromQuery] int leagueId)
        {
            return null;
        }

        [HttpPost("player")]
        public ActionResult AddFavPlayer([FromQuery] int userId, [FromQuery] int playerId)
        {
            return null;
        }

        [HttpDelete("player")]
        public ActionResult RemoveFavPlayer([FromQuery] int userId, [FromQuery] int playerId)
        {
            return null;
        }
    }
}
