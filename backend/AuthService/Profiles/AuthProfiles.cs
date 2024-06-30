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
            CreateMap<RegisterRequestDTO, User>()
            .ForMember(dest => dest.HashedPassword, opt => opt.ConvertUsing(new PasswordConverter(), src => src.Password));
        }
    }
}