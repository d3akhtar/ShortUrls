using ShortUrlService.Data;
using ShortUrlService.Helper.Counter;
using ShortUrlService.Model;

namespace ShortUrlService.Data.Repository
{
    public class ShortUrlRepository : IShortUrlRepository
    {
        private readonly AppDbContext _db;

        public ShortUrlRepository(AppDbContext db)
        {
            _db = db;
        }
        public string AddShortUrl(string destinationUrl, string alias = "")
        {
            int currentCount = Counter.CurrentNumber;
            if (string.IsNullOrEmpty(alias)) Counter.IncrementCounter();
            
            Console.WriteLine("Proceeding to Base62Encode number: " + currentCount);

            ShortUrl shortUrl = new()
            {
                Code = string.IsNullOrEmpty(alias) ? Base62Encode(currentCount):alias,
                DestinationUrl = destinationUrl,
                IsAlias = !string.IsNullOrEmpty(alias)
            };

            _db.ShortUrls.Add(shortUrl);
            return shortUrl.Code;
        }

        public void DeleteShortUrl(string code)
        {
            _db.ShortUrls.Remove(_db.ShortUrls.First(u => u.Code == code));
        }

        public IEnumerable<ShortUrl> GetAllShortUrls()
        {
            return _db.ShortUrls;
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
    }
}