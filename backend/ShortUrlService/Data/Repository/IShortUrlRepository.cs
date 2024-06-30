using ShortUrlService.Model;

namespace ShortUrlService.Data.Repository
{
    public interface IShortUrlRepository
    {
        IEnumerable<ShortUrl> GetAllShortUrls();
        string AddShortUrl(string destinationUrl, string alias = ""); // returns string so i can get the short url code back
        ShortUrl GetShortUrlWithCode(string code);
        ShortUrl GetShortUrlWithDestination(string destinationUrl);
        string GetShortUrlDestination(string code);
        void DeleteShortUrl(string code);
        void UpdateShortUrlDestinationLink(string code, string newDestinationUrl);
        bool SaveChanges();
    }
}