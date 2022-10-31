using AutoMapper;
using DatingApp.API.Data.Entities;
using DatingApp.API.DTOs;
using DatingApp.API.Extensions;

namespace DatingApp.API.Profilespace
{
    public class UserMapperProfile : Profile
    {
        public UserMapperProfile()
        {
            // CreateMap<User, MemberDto>()
            //     .ForMember(dest => dest.Age, 
            //     option => option.MapFrom(src => 
            //     src.DateOfBirth.HasValue? DateTime.Now.Year - src.DateOfBirth.Year : 0));

            CreateMap<User, MemberDto>()
                .ForMember(dest => dest.Age,
                option => option.MapFrom(src => src.DateOfBirth.CalAge()));
        }
    }
}