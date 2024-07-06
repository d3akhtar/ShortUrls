using AuthService;
using AutoMapper;
using ShortUrlService.Profiles.Converters;
using UserShortUrlService.DTO;
using UserShortUrlService.Model;

namespace UserShortUrlService.Profiles
{
    public class UserShortUrlProfiles : Profile
    {
        public UserShortUrlProfiles()
        {
            CreateMap<UserPublishedDTO, User>();
            CreateMap<User, UserReadDTO>();
            CreateMap<UserShortUrl, UserShortUrlReadDTO>()
            .ForMember(dest => dest.PngQrCodeImage, opt => opt.ConvertUsing(new PngQrCodeConverter(), src => src.ShortUrlCode))
            .ForMember(dest => dest.SvgQrCodeImage, opt => opt.ConvertUsing(new SvgQrCodeConverter(), src => src.ShortUrlCode))
            .ForMember(dest => dest.AsciiQrCodeImage, opt => opt.ConvertUsing(new AsciiQrCodeConverter(), src => src.ShortUrlCode));
            CreateMap<GrpcUserModel, User>();
        }
    }
}