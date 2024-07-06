using System.Reflection.Metadata.Ecma335;
using AuthService.DTO;
using AuthService.Model;
using AutoMapper;
using AutoMapper.Features;
using Microsoft.Data.SqlClient;

namespace AuthService.Profiles
{
    public class AuthProfiles : Profile
    {
        public AuthProfiles()
        {
            CreateMap<User, UserReadDTO>();
            CreateMap<User, UserPublishDTO>();
            CreateMap<User, GrpcUserModel>();
            CreateMap<RegisterRequestDTO, User>()
            .ForMember(dest => dest.HashedPassword, opt => opt.ConvertUsing(new PasswordConverter(), src => src.Password));
            CreateMap<ExternalUserProfileDTO, User>()
            .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Name));
        }
    }
}