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
        private readonly IFixtureService _fixtureService;

        public FavouriteService(
            IMapper mapper,
            FootballResultsDbContext context,
            IFixtureService fixtureService
        )
        {
            _mapper = mapper;
            _context = context;
            _fixtureService = fixtureService;
        }

        public void AddFavouriteTeam(FavouriteDto favouriteDto)
        {
            var user =
                _context.Users
                    .Include(t => t.Teams)
                    .FirstOrDefault(u => u.Id == int.Parse(favouriteDto.UserId))
                ?? throw new Exception("Nie ma usera z favService:27");

            var team =
                _context.Teams.FirstOrDefault(t => t.Id == int.Parse(favouriteDto.NewFavId))
                ?? throw new Exception("Nie ma teamu z favService:34");

            user.Teams.Add(team);
            _context.SaveChanges();
        }

        public void DeleteFavouriteTeam(FavouriteDto favouriteDto)
        {
            var user =
                _context.Users
                    .Include(t => t.Teams)
                    .FirstOrDefault(u => u.Id == int.Parse(favouriteDto.UserId))
                ?? throw new Exception("Nie ma usera z favService:27");

            var team =
                _context.Teams.FirstOrDefault(t => t.Id == int.Parse(favouriteDto.NewFavId))
                ?? throw new Exception("Nie ma teamu z favService:34");

            user.Teams.Remove(team);
            _context.SaveChanges();
        }

        public void AddFavouriteLeague(FavouriteDto favouriteDto)
        {
            var user =
                _context.Users
                    .Include(t => t.Leagues)
                    .FirstOrDefault(u => u.Id == int.Parse(favouriteDto.UserId))
                ?? throw new Exception("Nie ma usera z favService:27");

            var league =
                _context.Leagues.FirstOrDefault(t => t.Id == int.Parse(favouriteDto.NewFavId))
                ?? throw new Exception("Nie ma usera z favService:27");

            user.Leagues.Add(league);
            _context.SaveChanges();
        }

        public void DeleteFavouriteLeague(FavouriteDto favouriteDto)
        {
            var user =
                _context.Users
                    .Include(t => t.Leagues)
                    .FirstOrDefault(u => u.Id == int.Parse(favouriteDto.UserId))
                ?? throw new Exception("Nie ma usera z favService:27");

            var league =
                _context.Leagues.FirstOrDefault(t => t.Id == int.Parse(favouriteDto.NewFavId))
                ?? throw new Exception("Nie ma usera z favService:27");

            user.Leagues.Remove(league);
            _context.SaveChanges();
        }

        public List<TeamNFavMatches> GetFixtureListbyFavouriteTeams(int userId)
        {
            var user =
                _context.Users.Include(l => l.Teams).FirstOrDefault(i => i.Id == userId)
                ?? throw new Exception("Nie ma usera z favService:27");

            if (!user.Teams.Any())
            {
                return null;
            }

            var listOfTeams = user.Teams.Select(t => t.Id);
            var r = new List<TeamNFavMatches>();

            var fixtures = _context.Fixtures
                .Include(r => r.League)
                .ThenInclude(l => l.Country)
                .Include(m => m.MetaData)
                .Include(h => h.HomeTeam)
                .Include(h => h.AwayTeam)
                .Include(u => u.Users);

            foreach (var team in user.Teams)
            {
                var newDto = new TeamNFavMatches();
                newDto.Team = _mapper.Map<TeamDto>(team);
                var matches = fixtures
                    .Where(t => t.HomeTeamId == team.Id || t.AwayTeamId == team.Id)
                    .ToList();
                var mappedMatches = _mapper.Map<List<FixtureDto>>(matches);
                newDto.Fixtures = _fixtureService.GroupFixturesByLeague(mappedMatches);
                r.Add(newDto);
            }

            return r;
        }

        public List<List<FixtureDto>> GetFixtureListbyFavouriteLeagues(int userId, string date)
        {
            var user =
                _context.Users.Include(l => l.Leagues).FirstOrDefault(i => i.Id == userId)
                ?? throw new Exception("Nie ma usera z favService:27");

            if (!user.Leagues.Any())
            {
                return null;
            }
            if (DateTime.TryParse(date, out DateTime parsedDate))
            {
                DateOnly dateOnly = new DateOnly(parsedDate.Year, parsedDate.Month, parsedDate.Day);

                var listOfLeagues = user.Leagues.Select(l => l.Id);

                var fixtures = _context.Fixtures
                    .Include(r => r.League)
                    .ThenInclude(l => l.Country)
                    .Include(m => m.MetaData)
                    .Include(h => h.HomeTeam)
                    .Include(h => h.AwayTeam)
                    .Include(u => u.Users)
                    .Where(l => listOfLeagues.Contains(l.LeagueId))
                    .Where(f => f.MetaData.FixtureDate == dateOnly)
                    .ToList();

                var mappedFixtures = _mapper.Map<List<FixtureDto>>(fixtures);
                var result = _fixtureService.GroupFixturesByLeague(mappedFixtures);

                return result;
            }
            throw new Exception("GetFixtureDtosByDate Exception");
        }
    }
}
