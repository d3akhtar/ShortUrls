using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using UserShortUrlService.DTO;
using UserShortUrlService.Model;
using UserShortUrlService.SyncDataServices.Http;

namespace UserShortUrlService.Data.Repository
{
    public class UserShortUrlCodeRepository : IUserShortUrlCodeRepository
    {
        private readonly AppDbContext _db;
        private readonly IShortUrlDataClient _shortUrlDataClient;

        public UserShortUrlCodeRepository(AppDbContext db, IShortUrlDataClient shortUrlDataClient)
        {
            _db = db;
            _shortUrlDataClient = shortUrlDataClient;
        }

        public void AddUser(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException();
            }

            _db.Users.Add(user);
        }

        public async Task<IEnumerable<UserShortUrl>> AddUserShortUrls(string userId, List<string> codes)
        {
            List<UserShortUrl> addedUserShortUrls = new List<UserShortUrl>();
            foreach (var code in codes){
                ShortUrlHttpResponseDTO shortUrlResponse = await _shortUrlDataClient.RequestForShortUrl(code);
                Console.WriteLine("shortUrlJson: " + shortUrlResponse.Message + " " + shortUrlResponse.ShortUrl);
                if (shortUrlResponse != null && _db.UserShortUrls.FirstOrDefault(u => u.ShortUrlCode == code && u.UserId == userId) == null){
                    // ShortUrlHttpResponseDTO shortUrlHttpResponseDTO = JsonSerializer.Deserialize<ShortUrlHttpResponseDTO>(shortUrlJson);
                    // Console.WriteLine("shortUrlHttpResponseDTO.Message: " + shortUrlHttpResponseDTO.Message + " shorturl is null: " + shortUrlHttpResponseDTO.ShortUrl == null);
                    UserShortUrl userShortUrl = new()
                    {
                        ShortUrlCode = code,
                        UserId = userId,
                        DestinationUrl = shortUrlResponse.ShortUrl.DestinationUrl
                    };
                    Console.WriteLine($"--> Adding UserShortUrlCode, code: {code}, userId: {userId}" );
                    _db.UserShortUrls.Add(userShortUrl);
                    addedUserShortUrls.Add(userShortUrl);
                }
                else{
                    Console.WriteLine($"--> ShortUrl with code: {code} not mapped or was added already" );
                }
            }
            return addedUserShortUrls;
        }

        public UserShortUrl DeleteUserShortUrl(string userId, string code)
        {
            UserShortUrl userShortUrl = _db.UserShortUrls.First(usrc => usrc.UserId == userId && usrc.ShortUrlCode == code);
            _db.UserShortUrls.Remove(userShortUrl);
            return userShortUrl;
        }

        public bool DoesUserShortUrlExist(string userId, string code)
        {
            return _db.UserShortUrls.FirstOrDefault(usrc => usrc.UserId == userId && usrc.ShortUrlCode == code) != null;
        }

        public bool DoesUserWithIdExist(string userId)
        {
            return _db.Users.FirstOrDefault(u => u.UserId == userId) != null;
        }

        public IEnumerable<User> GetAllUsers()
        {
            return _db.Users;
        }

        public IEnumerable<UserShortUrl> GetAllUserShortUrlCodes()
        {
            return _db.UserShortUrls.Include(usrc => usrc.User);
        }

        public IEnumerable<UserShortUrl> GetUserShortUrlCodes(string userId)
        {
            return _db.UserShortUrls.Include(usrc => usrc.User).Where(usrc => usrc.UserId == userId);
        }

        public bool SaveChanges()
        {
            return _db.SaveChanges() >= 0;
        }
    }
}