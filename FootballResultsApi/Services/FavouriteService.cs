using AutoMapper;
using FootballResultsApi.Entities;
using FootballResultsApi.Interfaces;
using FootballResultsApi.Models;
using Microsoft.EntityFrameworkCore;

namespace FootballResultsApi.Services
{
    public class FavouriteService : IFavouriteService
    {
        private readonly IMapper _mapper;
        private readonly FootballResultsDbContext _context;

        public FavouriteService(IMapper mapper, FootballResultsDbContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public void AddFavouriteTeam(FavouriteDto favouriteDto)
        {
            var user = _context.Users
                .Include(t => t.Teams)
                .FirstOrDefault(u => u.Id == int.Parse(favouriteDto.UserId));
            if (user == null)
            {
                //TODO dodac wyjatek
                throw new Exception("Nie ma usera z favService:27");
            }

            var team = _context.Teams.FirstOrDefault(t => t.Id == int.Parse(favouriteDto.NewFavId));
            if (team == null)
            {
                //TODO dodac wyjatek
                throw new Exception("Nie ma teamu z favService:34");
            }

            user.Teams.Add(team);
            _context.SaveChanges();
        }

        public void DeleteFavouriteTeam(FavouriteDto favouriteDto)
        {
            var user = _context.Users
                .Include(t => t.Teams)
                .FirstOrDefault(u => u.Id == int.Parse(favouriteDto.UserId));
            if (user == null)
            {
                //TODO dodac wyjatek
                throw new Exception("Nie ma usera z favService:27");
            }

            var team = _context.Teams.FirstOrDefault(t => t.Id == int.Parse(favouriteDto.NewFavId));
            if (team == null)
            {
                //TODO dodac wyjatek
                throw new Exception("Nie ma teamu z favService:34");
            }

            user.Teams.Remove(team);
            _context.SaveChanges();
        }
    }
}
