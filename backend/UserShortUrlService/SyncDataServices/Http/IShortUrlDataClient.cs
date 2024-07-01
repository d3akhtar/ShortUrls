namespace UserShortUrlService.SyncDataServices.Http
{
    public interface IShortUrlDataClient
    {
        Task<bool> IsShortUrlMapped(string code);
    }
}