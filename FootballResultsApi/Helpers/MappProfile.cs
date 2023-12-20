using AutoMapper;
using FootballResultsApi.Entities;
using FootballResultsApi.Models;

namespace FootballResultsApi.Helpers
{
    public class MappProfile : Profile
    {
        public MappProfile()
        {
            CreateMap<LoginUserDto, User>();
            CreateMap<Models.Country, Entities.Country>();

            CreateMap<Entities.Team, TeamDto>();

            CreateMap<League, LeagueDto>()
                .ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.Country));

            CreateMap<Entities.Country, CountryDto>();

            CreateMap<Fixture, FixtureDto>()
                .ForMember(dest => dest.League, opt => opt.MapFrom(src => src.League))
                .ForMember(dest => dest.HomeTeam, opt => opt.MapFrom(src => src.HomeTeam))
                .ForMember(dest => dest.AwayTeam, opt => opt.MapFrom(src => src.AwayTeam));

            CreateMap<User, UserDto>()
                .ForMember(
                    dest => dest.LeaguesIds,
                    opt => opt.MapFrom(src => src.Leagues.Select(l => l.Id))
                )
                .ForMember(
                    dest => dest.TeamsIds,
                    opt => opt.MapFrom(src => src.Teams.Select(t => t.Id))
                );
        }
    }
}
