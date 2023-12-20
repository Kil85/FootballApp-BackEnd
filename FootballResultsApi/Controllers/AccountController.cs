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
            var userDto = _accountService.Login(login);
            ;
            return Ok(userDto);
        }

        [HttpGet("all")]
        public ActionResult GetAll()
        {
            var users = _accountService.getAllUsers();
            return Ok(users.ToList());
        }
    }
}
