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
        }
    }
}
