using AutoMapper;
using FootballResultsApi.Entities;
using FootballResultsApi.Exceptions;
using FootballResultsApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FootballResultsApi.Services
{
    public class AccountService : IAccountService
    {
        private readonly FootballResultsDbContext _dbContext;
        private readonly IMapper _mapper;

        private readonly IPasswordHasher<User> _passwordHasher;

        ////private readonly AuthenticationSettings _authenticationSettings;
        private readonly ILogger<AccountService> _logger;

        //private readonly IAuthorizationService _authorizationService;

        //private readonly IUserContextService _userContextService;
        public AccountService(
            IMapper mapper,
            FootballResultsDbContext dbContext,
            IPasswordHasher<User> passwordHasher,
            //    //AuthenticationSettings authenticationSettings,
            //    IAuthorizationService authorizationService,
            ILogger<AccountService> logger
        ////IUserContextService userContextService
        )
        {
            _mapper = mapper;
            _dbContext = dbContext;
            _passwordHasher = passwordHasher;
            //    ////_authenticationSettings = authenticationSettings;
            //    //_authorizationService = authorizationService;
            _logger = logger;
            //    //_userContextService = userContextService;
        }

        public int Register(LoginUserDto userDto)
        {
            var user = _mapper.Map<User>(userDto);
            var hashedPassword = _passwordHasher.HashPassword(user, userDto.Password);
            user.HashedPassword = hashedPassword;

            _dbContext.Add(user);
            _dbContext.SaveChanges();
            return user.Id;
        }

        public string Login(LoginUserDto loginDto)
        {
            var user = _dbContext.Users.FirstOrDefault(u => u.Email == loginDto.Email);

            if (user == null)
            {
                throw new LoginFailedException("Email or Password incorrect");
            }

            var isPasswordOk = _passwordHasher.VerifyHashedPassword(
                user,
                user.HashedPassword,
                loginDto.Password
            );

            if (isPasswordOk == PasswordVerificationResult.Failed)
            {
                throw new LoginFailedException("Email or Password incorrect");
            }

            //var claims = new List<Claim>()
            //{
            //    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            //    new Claim(ClaimTypes.Role, user.Role.Name)
            //};

            //var key = new SymmetricSecurityKey(
            //    Encoding.UTF8.GetBytes(_authenticationSettings.JwtKey)
            //);

            //var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            //var expires = DateTime.Now.AddDays(_authenticationSettings.JwtExpiredDays);

            //var token = new JwtSecurityToken(
            //    _authenticationSettings.JwtIssuer,
            //    _authenticationSettings.JwtIssuer,
            //    claims,
            //    expires: expires,
            //    signingCredentials: cred
            //);

            //var tokenHandler = new JwtSecurityTokenHandler();
            //return tokenHandler.WriteToken(token);
            return "OK";
        }
    }
}
