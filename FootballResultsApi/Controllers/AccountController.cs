using FootballResultsApi.Models;
using FootballResultsApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FootballResultsApi.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost("register")]
        public ActionResult Register([FromBody] LoginUserDto registerUser)
        {
            var userId = _accountService.Register(registerUser);

            return Created($"/api/account/{userId}", null);
        }

        [HttpPost("login")]
        public ActionResult Login([FromBody] LoginUserDto login)
        {
            var jwtToken = _accountService.Login(login);

            return Ok(jwtToken);
        }

        //[HttpGet("all")]
        //[Authorize("admin")]
        //public ActionResult All()
        //{
        //    var result = _accountService.GetAllUsers();

        //    return Ok(result);
        //}

        //[HttpGet("{id}")]
        //[Authorize]
        //public ActionResult Get([FromRoute] int id)
        //{
        //    var user = _accountService.GetUserById(id);
        //    return Ok(user);
        //}
    }
}
