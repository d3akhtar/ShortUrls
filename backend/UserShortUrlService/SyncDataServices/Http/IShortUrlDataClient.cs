using UserShortUrlService.DTO;
using UserShortUrlService.Model;

namespace UserShortUrlService.SyncDataServices.Http
{
    public interface IShortUrlDataClient
    {
        Task<ShortUrlHttpResponseDTO> RequestForShortUrl(string code);
    }
}