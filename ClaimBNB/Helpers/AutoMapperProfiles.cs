using AutoMapper;
using ClaimBNB.Dtos;
using Data.Models;

namespace ClaimBNB.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<UserForRegisterDto, User>().ReverseMap();
            CreateMap<UserForListDto, User>().ReverseMap();
            CreateMap<UserForLoginDto, User>().ReverseMap();
        }
    }
}
