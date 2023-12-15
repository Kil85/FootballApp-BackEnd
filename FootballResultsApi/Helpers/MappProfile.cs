﻿using AutoMapper;
using FootballResultsApi.Entities;
using FootballResultsApi.Models;

namespace FootballResultsApi.Helpers
{
    public class MappProfile : Profile
    {
        public MappProfile()
        {
            CreateMap<LoginUserDto, User>();

            CreateMap<Entities.Team, TeamDto>();

            CreateMap<League, LeagueDto>();

            CreateMap<Fixture, FixtureDto>()
                .ForMember(dest => dest.League, opt => opt.MapFrom(src => src.League))
                .ForMember(dest => dest.HomeTeam, opt => opt.MapFrom(src => src.HomeTeam))
                .ForMember(dest => dest.AwayTeam, opt => opt.MapFrom(src => src.AwayTeam));
        }
    }
}
