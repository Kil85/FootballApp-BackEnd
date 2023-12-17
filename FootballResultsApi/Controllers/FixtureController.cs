using FootballResultsApi.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FootballResultsApi.Controllers
{
    [Route("api/fixture")]
    [ApiController]
    public class FixtureController : ControllerBase
    {
        private readonly IFixtureService _fixtureService;

        public FixtureController(IFixtureService fixtureService)
        {
            _fixtureService = fixtureService;
        }

        [HttpGet]
        public ActionResult GetFixturesByDate([FromQuery] string date)
        {
            var dtos = _fixtureService.GetFixtureDtosByDate(date);
            return Ok(dtos);
        }
    }
}
