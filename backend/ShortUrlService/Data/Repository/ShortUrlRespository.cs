using ShortUrlService.AsyncDataServices;
using ShortUrlService.Data;
using ShortUrlService.Helper.Counter;
using ShortUrlService.Model;

namespace ShortUrlService.Data.Repository
{
    public class ShortUrlRepository : IShortUrlRepository
    {
        private readonly AppDbContext _db;
        private readonly ICounterRangeRpcClient _counterRangeRpcClient;

        public ShortUrlRepository(AppDbContext db, ICounterRangeRpcClient counterRangeRpcClient)
        {
            _db = db;
            _counterRangeRpcClient = counterRangeRpcClient;
        }
        public async Task<ShortUrl> AddShortUrl(string destinationUrl, string alias = "")
        {
            int currentCount = Counter.CurrentNumber;
            if (string.IsNullOrEmpty(alias)) await Counter.IncrementCounter(_counterRangeRpcClient);
            
            Console.WriteLine("Proceeding to Base62Encode number: " + currentCount);

            ShortUrl shortUrl = new()
            {
                Code = string.IsNullOrEmpty(alias) ? Base62Encode(currentCount):alias,
                DestinationUrl = destinationUrl,
                IsAlias = !string.IsNullOrEmpty(alias)
            };

            _db.ShortUrls.Add(shortUrl);
            return shortUrl;
        }

        public void DeleteShortUrl(string code)
        {
            _db.ShortUrls.Remove(_db.ShortUrls.First(u => u.Code == code));
        }

        public IEnumerable<ShortUrl> GetAllShortUrls(string searchQuery, int pageNumber, int pageSize)
        {
            return _db.ShortUrls.Where(u => 
                                        u.DestinationUrl.ToLower().Contains(searchQuery.ToLower()) || 
                                        u.Code.ToLower().Contains(searchQuery.ToLower()))
                                .Skip((pageNumber - 1) * pageSize)
                                .Take(pageSize);
        }

        public ShortUrl GetShortUrlWithCode(string code)
        {
            return _db.ShortUrls.FirstOrDefault(u => u.Code == code);
        }

        public ShortUrl GetShortUrlWithDestination(string destinationUrl)
        {
            return _db.ShortUrls.FirstOrDefault(u => u.DestinationUrl == destinationUrl && !u.IsAlias);
        }

        public string GetShortUrlDestination(string code)
        {
            return _db.ShortUrls.First(u => u.Code == code).DestinationUrl;
        }

        public void UpdateShortUrlDestinationLink(string code, string newDestinationUrl)
        {
            var shortUrl = _db.ShortUrls.First(u => u.Code == code);
            shortUrl.DestinationUrl  = newDestinationUrl;
            _db.ShortUrls.Update(shortUrl);
        }

        public bool SaveChanges()
        {
            return _db.SaveChanges() >= 0;
        }

        private string Base62Encode(int num)
        {
            string alphabet = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
            int basis = 62;
            string ret = "";
            while (num > 0)
            {
                ret = alphabet[num % basis] + ret;
                num = num / basis;
            }

            Random rnd = new Random();
            while (ret.Length < 7){
                ret += alphabet[rnd.Next(62)];
            }
            return ret;
        }

        public bool DoesAliasExist(string alias)
        {
            return _db.ShortUrls.FirstOrDefault(u => u.Code == alias && u.IsAlias) != null;
        }
    }
}