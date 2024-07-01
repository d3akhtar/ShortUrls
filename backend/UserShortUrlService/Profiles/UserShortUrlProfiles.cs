using AutoMapper;
using UserShortUrlService.DTO;
using UserShortUrlService.Model;

namespace UserShortUrlService.Profiles
{
    public class UserShortUrlProfiles : Profile
    {
        public UserShortUrlProfiles()
        {
            CreateMap<UserPublishedDTO, User>();
        }
    }
}