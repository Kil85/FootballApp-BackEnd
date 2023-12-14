using FootballResultsApi.Entities;
using FootballResultsApi.Interfaces;
using FootballResultsApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestSharp;

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

            //var jwtToken = new test(_accountService.Login(login));

            return Ok(jwtToken);
        }

        [HttpGet("{id}")]
        public ActionResult GetUserById([FromRoute] string id)
        {
            return null;
        }

        [HttpPatch("edit/{id}")]
        public ActionResult EditUser([FromRoute] string id, [FromBody] User user)
        {
            return null;
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteUser([FromRoute] string id)
        {
            return null;
        }

        [HttpPut("changerole/{id}")]
        public ActionResult ChangeRole([FromRoute] string id, [FromBody] Role role)
        {
            return null;
        }

        [HttpGet("all")]
        public ActionResult GetAll()
        {
            var users = _accountService.getAllUsers();
            return Ok(users.ToList());
        }
    }

    class test
    {
        public string jwt { get; set; }

        public test(string j)
        {
            this.jwt = j;
        }
    }
}
