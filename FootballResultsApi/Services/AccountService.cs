﻿using AutoMapper;
using FootballResultsApi.Authentication;
using FootballResultsApi.Entities;
using FootballResultsApi.Exceptions;
using FootballResultsApi.Interfaces;
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

        private readonly AuthenticationSettings _authenticationSettings;
        private readonly ILogger<AccountService> _logger;

        public AccountService(
            IMapper mapper,
            FootballResultsDbContext dbContext,
            IPasswordHasher<User> passwordHasher,
            AuthenticationSettings authenticationSettings,
            ILogger<AccountService> logger
        )
        {
            _mapper = mapper;
            _dbContext = dbContext;
            _passwordHasher = passwordHasher;
            _authenticationSettings = authenticationSettings;
            _logger = logger;
        }

        public int Register(LoginUserDto userDto)
        {
            var user = _mapper.Map<User>(userDto);
            var hashedPassword = _passwordHasher.HashPassword(user, userDto.Password);
            user.HashedPassword = hashedPassword;
            user.Role = new Role();

            _dbContext.Add(user);
            _dbContext.SaveChanges();
            return user.Id;
        }

        public string Login(LoginUserDto loginDto)
        {
            var user = _dbContext.Users
                .Include(r => r.Role)
                .FirstOrDefault(u => u.Email == loginDto.Email);

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

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.Role.Name.ToString()),
                new Claim(ClaimTypes.Email, user.Email.ToString())
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_authenticationSettings.JwtKey)
            );

            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(_authenticationSettings.JwtExpiredDays);

            var token = new JwtSecurityToken(
                _authenticationSettings.JwtIssuer,
                _authenticationSettings.JwtIssuer,
                claims,
                expires: expires,
                signingCredentials: cred
            );

            var tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.WriteToken(token);
        }

        public IEnumerable<User> getAllUsers()
        {
            return _dbContext.Users.Include(r => r.Role);
        }
    }
}
