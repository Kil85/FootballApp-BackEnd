using AutoMapper;
using FootballResultsApi.Entities;
using FootballResultsApi.Helpers;
using FootballResultsApi.Interfaces;
using FootballResultsApi.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FootballResultsApi.Services
{
    public class FixtureSevice : IFixtureService
    {
        private readonly HttpData _data;
        private readonly FootballResultsDbContext _context;
        private readonly IMapper _mapper;
        private readonly object _lockObj = new object();

        public FixtureSevice(HttpData data, FootballResultsDbContext context, IMapper mapper)
        {
            _data = data;
            _context = context;
            _mapper = mapper;
        }

        public async Task StartApp()
        {
            await fetchCountries();

            var currentDate = DateOnly.FromDateTime(DateTime.Now);
            int i = -7;
            while (i < 8)
            {
                await FeachFixtures(currentDate.AddDays(i));
                i++;
            }
        }

        public List<List<FixtureDto>> GetFixtureDtosByDate(string date)
        {
            if (DateTime.TryParse(date, out DateTime parsedDate))
            {
                DateOnly dateOnly = new DateOnly(parsedDate.Year, parsedDate.Month, parsedDate.Day);
                var fixtures = _context.Fixtures
                    .Include(r => r.League)
                    .ThenInclude(l => l.Country)
                    .Include(m => m.MetaData)
                    .Include(h => h.HomeTeam)
                    .Include(h => h.AwayTeam)
                    .ToList();

                var fixturesByDate = fixtures
                    .Where(f => f.MetaData.FixtureDate == dateOnly)
                    .ToList();

                var mappedFixtures = _mapper.Map<List<FixtureDto>>(fixturesByDate);

                var result = GroupFixturesByLeague(mappedFixtures);

                return result;
            }
            //TODO dodac wyjatek
            throw new Exception("GetFixtureDtosByDate Exception");
        }

        private async Task<List<Fixture>> FeachFixtures(DateOnly date)
        {
            var doesExist = await checkIfFixtureExist(date);
            if (!doesExist)
                return null;

            using (var client = new HttpClient())
            {
                var uriBuilder = new UriBuilder(_data.BaseLink + "/fixtures");
                var query = System.Web.HttpUtility.ParseQueryString(uriBuilder.Query);
                string formattedDate = date.ToString("yyyy-MM-dd");

                query["date"] = formattedDate;
                query["season"] = date.Year.ToString();
                uriBuilder.Query = query.ToString();
                string urlWithParams = uriBuilder.ToString();

                client.DefaultRequestHeaders.Add(_data.ApiKeyPropName, _data.API_KEY);
                client.DefaultRequestHeaders.Add(_data.HostPropName, _data.Host);
                HttpResponseMessage response = await client.GetAsync(urlWithParams);

                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();

                    var fixtures = await saveResponse(responseBody, date);
                    Console.Out.WriteLine("finished!");
                    return fixtures;
                }
                throw new Exception("FeachFixtures Exception");
            }
        }

        private async Task<List<Fixture>> saveResponse(string responseString, DateOnly date)
        {
            FixtureResponse response = null;
            try
            {
                lock (_lockObj)
                {
                    response = JsonConvert.DeserializeObject<FixtureResponse>(responseString);
                }
            }
            catch (Exception ex)
            {
                //TODO dodac exception
                throw new Exception("SaveResponse Exception" + ex.Message);
            }
            var fixtures = new List<Fixture>();
            var teams = _context.Teams;
            var legues = _context.Leagues;
            var countries = _context.Countries;
            var newTeams = new List<Entities.Team>();
            var newLeagues = new List<Entities.League>();
            var metaData = new MetaData() { LastRefresh = DateTime.Now, FixtureDate = date };
            await _context.AddAsync(metaData);
            await _context.SaveChangesAsync();
            foreach (var fixtureData in response.Response)
            {
                if (
                    await legues.FirstOrDefaultAsync(i => i.Id == fixtureData.League.Id) == null
                    && newLeagues.FirstOrDefault(n => n.Id == fixtureData.League.Id) == null
                )
                {
                    var country = await countries.FirstOrDefaultAsync(
                        c => c.Name == fixtureData.League.Country
                    );
                    var newLeague = new League()
                    {
                        Id = fixtureData.League.Id,
                        Name = fixtureData.League.Name,
                        Logo = fixtureData.League.Logo,
                        CountryId = country.Id,
                    };
                    newLeagues.Add(newLeague);
                }

                if (
                    await teams.FirstOrDefaultAsync(i => i.Id == fixtureData.Teams.Home.Id) == null
                    && newTeams.FirstOrDefault(n => n.Id == fixtureData.Teams.Home.Id) == null
                )
                {
                    var newTeam = new Entities.Team()
                    {
                        Id = fixtureData.Teams.Home.Id,
                        Name = fixtureData.Teams.Home.Name,
                        Logo = fixtureData.Teams.Home.Logo,
                        LeagueId = fixtureData.League.Id,
                    };
                    newTeams.Add(newTeam);
                }
                if (
                    await teams.FirstOrDefaultAsync(i => i.Id == fixtureData.Teams.Away.Id) == null
                    && newTeams.FirstOrDefault(n => n.Id == fixtureData.Teams.Away.Id) == null
                )
                {
                    var newTeam = new Entities.Team()
                    {
                        Id = fixtureData.Teams.Away.Id,
                        Name = fixtureData.Teams.Away.Name,
                        Logo = fixtureData.Teams.Away.Logo,
                        LeagueId = fixtureData.League.Id,
                    };
                    newTeams.Add(newTeam);
                }
                var fixture = new Fixture
                {
                    Id = fixtureData.Fixture.Id,
                    Referee = fixtureData.Fixture.Referee,
                    TimeZone = fixtureData.Fixture.TimeZone,
                    Date = fixtureData.Fixture.Date.ToString("d"),
                    Time = fixtureData.Fixture.Date.TimeOfDay.ToString(@"hh\:mm"),
                    Status = fixtureData.Fixture.Status.Long,
                    HomeTeamGoals = fixtureData.Goals.Home,
                    AwayTeamGoals = fixtureData.Goals.Away,
                    LeagueId = fixtureData.League.Id,
                    HomeTeamId = fixtureData.Teams.Home.Id,
                    AwayTeamId = fixtureData.Teams.Away.Id,
                    MetaDataId = metaData.Id,
                };
                fixtures.Add(fixture);
            }
            await _context.AddRangeAsync(newLeagues);
            await _context.SaveChangesAsync();

            await _context.AddRangeAsync(newTeams);
            await _context.SaveChangesAsync();

            await _context.AddRangeAsync(fixtures);
            await _context.SaveChangesAsync();
            return fixtures;
        }

        private async Task<bool> checkIfFixtureExist(DateOnly fixtureDate)
        {
            var metaData = _context.MetaDatas.FirstOrDefault(i => i.FixtureDate == fixtureDate);
            if (metaData == null)
            {
                return true;
            }
            return false;
        }

        public async Task checkDepricatedData()
        {
            var metaData = _context.MetaDatas;
            var currentTime = DateTime.Now;
            var isChanged = false;

            foreach (var data in metaData)
            {
                var fixtureDate = data.FixtureDate.ToDateTime(new TimeOnly(0));
                var timeDifference = (currentTime - fixtureDate).Days;
                if (Math.Abs(timeDifference) > 7)
                {
                    _context.MetaDatas.Remove(data);
                    isChanged = true;
                }
                else
                {
                    var lastRefresh = DateOnly.FromDateTime(data.LastRefresh);

                    var timeDif = currentTime - data.LastRefresh;

                    if (timeDif.TotalHours > 24)
                    {
                        _context.MetaDatas.Remove(data);
                        isChanged = true;
                    }
                }
            }
            if (isChanged)
            {
                _context.SaveChanges();
            }
        }

        public List<List<FixtureDto>> GroupFixturesByLeague(List<FixtureDto> fixtures)
        {
            // Grupowanie FixtureDto na podstawie LeagueDto
            var groupedFixtures = fixtures
                .GroupBy(fixture => fixture.League.Id)
                .Select(group => group.ToList())
                .ToList();

            int[] specificLeagueIds = { 1, 2, 3, 4, 15, 848, 39, 140, 135, 78, 61 }; // Określone ID ligi, które chcesz umieścić na początku

            // Wyodrębnienie grup o konkretnych ID ligi i ich usunięcie z listy
            var specificGroups = groupedFixtures
                .Where(list => specificLeagueIds.Contains(list.FirstOrDefault()?.League.Id ?? -1))
                .ToList();
            groupedFixtures.RemoveAll(
                list => specificLeagueIds.Contains(list.FirstOrDefault()?.League.Id ?? -1)
            );

            // Sortowanie listy groupedFixtures
            groupedFixtures = specificGroups.Concat(groupedFixtures).ToList();

            return groupedFixtures;
        }

        private async Task fetchCountries()
        {
            var countries = _context.Countries;
            if (countries.Any())
            {
                return;
            }

            using (var client = new HttpClient())
            {
                var uriBuilder = new UriBuilder(_data.BaseLink + "/countries");

                string urlString = uriBuilder.ToString();

                client.DefaultRequestHeaders.Add(_data.ApiKeyPropName, _data.API_KEY);
                client.DefaultRequestHeaders.Add(_data.HostPropName, _data.Host);
                HttpResponseMessage response = await client.GetAsync(urlString);

                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    saveCountriesResponse(responseBody);
                    return;
                }
                throw new Exception("FeachFixtures Exception");
            }
        }

        private void saveCountriesResponse(string responseString)
        {
            CountriesResponse response = null;
            try
            {
                response = JsonConvert.DeserializeObject<CountriesResponse>(responseString);
            }
            catch (Exception ex)
            {
                //TODO dodac exception
                throw new Exception("SaveResponse Exception" + ex.Message);
            }
            var mappedCountries = _mapper.Map<List<Entities.Country>>(response.Response);
            Console.WriteLine();
            _context.AddRange(mappedCountries);
            _context.SaveChanges();
        }

        private string fromFile()
        {
            string filePath =
                "C:\\Users\\kil85\\source\\repos\\FootballResultsApi\\FootballResultsApi\\Helpers\\json.txt";
            string text = "";

            try
            {
                text = File.ReadAllText(filePath);
            }
            catch (IOException e)
            {
                Console.WriteLine("Błąd odczytu pliku: " + e.Message);
            }

            return text;
        }
    }
}
