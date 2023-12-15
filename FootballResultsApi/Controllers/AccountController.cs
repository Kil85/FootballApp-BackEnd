using AutoMapper;
using FootballResultsApi.Entities;
using FootballResultsApi.Interfaces;
using FootballResultsApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.SqlServer.Server;
using RestSharp;

namespace FootballResultsApi.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly FootballResultsDbContext _context;
        private readonly IMapper _mapper;

        public AccountController(
            IAccountService accountService,
            FootballResultsDbContext context,
            IMapper mapper
        )
        {
            _accountService = accountService;
            _context = context;
            _mapper = mapper;
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

        [HttpGet("/mecze")]
        public ActionResult GetUserById([FromQuery] string date)
        {
            if (DateTime.TryParse(date, out DateTime parsedDate))
            {
                DateOnly dateOnly = new DateOnly(parsedDate.Year, parsedDate.Month, parsedDate.Day);
                //var time = DateOnly.FromDateTime(DateTime.Now).AddDays(-1);
                var result = _context.Fixtures
                    .Include(r => r.League)
                    .Include(m => m.MetaData)
                    .Include(h => h.HomeTeam)
                    .Include(h => h.AwayTeam)
                    .ToList();

                var a = result.FindAll(f => f.MetaData.FixtureDate == dateOnly);

                var r = _mapper.Map<List<FixtureDto>>(a);

                return Ok(r);
            }
            else
            {
                Console.WriteLine("Nieudane parsowanie daty.");
            }
            return BadRequest();
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
}
