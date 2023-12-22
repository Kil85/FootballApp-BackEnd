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

        public void AddFavouriteLeague(FavouriteDto favouriteDto)
        {
            var user = _context.Users
                .Include(t => t.Leagues)
                .FirstOrDefault(u => u.Id == int.Parse(favouriteDto.UserId));
            if (user == null)
            {
                //TODO dodac wyjatek
                throw new Exception("Nie ma usera z favService:27");
            }

            var league = _context.Leagues.FirstOrDefault(
                t => t.Id == int.Parse(favouriteDto.NewFavId)
            );
            if (league == null)
            {
                //TODO dodac wyjatek
                throw new Exception("Nie ma teamu z favService:34");
            }

            user.Leagues.Add(league);
            _context.SaveChanges();
        }

        public void DeleteFavouriteLeague(FavouriteDto favouriteDto)
        {
            var user = _context.Users
                .Include(t => t.Leagues)
                .FirstOrDefault(u => u.Id == int.Parse(favouriteDto.UserId));
            if (user == null)
            {
                //TODO dodac wyjatek
                throw new Exception("Nie ma usera z favService:27");
            }

            var league = _context.Leagues.FirstOrDefault(
                t => t.Id == int.Parse(favouriteDto.NewFavId)
            );
            if (league == null)
            {
                //TODO dodac wyjatek
                throw new Exception("Nie ma teamu z favService:34");
            }

            user.Leagues.Remove(league);
            _context.SaveChanges();
        }

        public List<FixtureDto> GetFixtureListbyFavouriteTeams(int userId)
        {
            var user = _context.Users.Include(l => l.Teams).FirstOrDefault(i => i.Id == userId);

            if (user == null)
            {
                //TODO dodac wyjatek
                throw new Exception("Nie ma usera z favService:27");
            }

            if (!user.Teams.Any())
            {
                return null;
            }
            var listOfTeams = user.Teams.Select(t => t.Id);

            var fixtures = _context.Fixtures
                .Include(r => r.League)
                .ThenInclude(l => l.Country)
                .Include(m => m.MetaData)
                .Include(h => h.HomeTeam)
                .Include(h => h.AwayTeam)
                .Include(u => u.Users)
                .Where(
                    t => listOfTeams.Contains(t.HomeTeamId) || listOfTeams.Contains(t.AwayTeamId)
                )
                .ToList();

            var result = _mapper.Map<List<FixtureDto>>(fixtures);
            return result;
        }
    }
}
