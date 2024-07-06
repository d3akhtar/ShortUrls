using AutoMapper;
using ShortUrlService.DTO;
using ShortUrlService.Model;
using ShortUrlService.Profiles.Converters;

namespace ShortUrlService.Profiles
{
    public class ShortUrlProfiles : Profile
    {
        public ShortUrlProfiles()
        {
            CreateMap<ShortUrl, ShortUrlReadDTO>()
            .ForMember(dest => dest.PngQrCodeImage, opt => opt.ConvertUsing(new PngQrCodeConverter(), src => src.Code))
            .ForMember(dest => dest.SvgQrCodeImage, opt => opt.ConvertUsing(new SvgQrCodeConverter(), src => src.Code))
            .ForMember(dest => dest.AsciiQrCodeImage, opt => opt.ConvertUsing(new AsciiQrCodeConverter(), src => src.Code));
        }
    }
}