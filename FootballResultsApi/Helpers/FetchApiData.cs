using FootballResultsApi.Entities;
using FootballResultsApi.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Security.Cryptography.X509Certificates;

namespace FootballResultsApi.Helpers
{
    public class FetchApiData
    {
        private readonly HttpData _data;
        private readonly FootballResultsDbContext _context;

        public FetchApiData(HttpData data, FootballResultsDbContext context)
        {
            _data = data;
            _context = context;
        }

        public async Task StartApp()
        {
            checkDepricatedData();
            var currentDate = DateOnly.FromDateTime(DateTime.Now);
            int i = -7;
            while (i < 8)
            {
                await FeachFixtures(currentDate.AddDays(i));
                i++;
            }
        }

        public async Task FeachFixtures(DateOnly date)
        {
            if (!checkLastFetch(date))
                return;

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
                    saveResponse(responseBody, date);
                    Console.WriteLine("Finished");
                }
                else
                {
                    Console.WriteLine("Błąd w żądaniu HTTP. Status kod: " + response.StatusCode);
                }
            }
        }

        private void saveResponse(string responseString, DateOnly date)
        {
            FixtureResponse response = null;
            try
            {
                response = JsonConvert.DeserializeObject<FixtureResponse>(responseString);
            }
            catch (Exception ex)
            {
                //TODO dodac exception
                Console.WriteLine("Błąd podczas deserializacji JSONa: " + ex.Message);
            }
            var fixtures = new List<Fixture>();
            var teams = _context.Teams;
            var legues = _context.Leagues;
            var newTeams = new List<Entities.Team>();
            var newLeagues = new List<Entities.League>();
            var metaData = new MetaData() { LastRefresh = DateTime.Now, FixtureDate = date };
            _context.Add(metaData);
            _context.SaveChanges();
            foreach (var fixtureData in response.Response)
            {
                if (
                    legues.FirstOrDefault(i => i.Id == fixtureData.League.Id) == null
                    && newLeagues.FirstOrDefault(n => n.Id == fixtureData.League.Id) == null
                )
                {
                    var newLeague = new League()
                    {
                        Id = fixtureData.League.Id,
                        Name = fixtureData.League.Name,
                        Logo = fixtureData.League.Logo,
                        Country = fixtureData.League.Country
                    };
                    newLeagues.Add(newLeague);
                }

                if (
                    teams.FirstOrDefault(i => i.Id == fixtureData.Teams.Home.Id) == null
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
                    teams.FirstOrDefault(i => i.Id == fixtureData.Teams.Away.Id) == null
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
                    Date = fixtureData.Fixture.Date,
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
            _context.AddRange(newLeagues);
            _context.SaveChanges();

            _context.AddRange(newTeams);
            _context.SaveChanges();

            _context.AddRange(fixtures);
            _context.SaveChanges();
        }

        private bool checkLastFetch(DateOnly fixtureDate)
        {
            var metaData = _context.MetaDatas.FirstOrDefault(i => i.FixtureDate == fixtureDate);
            if (metaData == null)
            {
                return true;
            }

            var currentTime = DateTime.Now;
            var timeDifference = currentTime - metaData.LastRefresh;

            if (timeDifference.TotalHours > 24)
            {
                return true;
            }
            return false;
        }

        public void checkDepricatedData()
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
                    _context.Remove(data);
                    isChanged = true;
                }
            }
            if (isChanged)
            {
                _context.SaveChanges();
            }
        }
    }
}
