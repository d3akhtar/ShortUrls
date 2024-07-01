using Microsoft.AspNetCore.Mvc;
using UserShortUrlService.Data.Repository;
using UserShortUrlService.Model;

namespace UserShortUrlService.Controller
{
    [ApiController]
    [Route("/api/user")]
    public class UserShortUrlController : ControllerBase
    {
        private readonly IUserShortUrlCodeRepository _repo;

        public UserShortUrlController(IUserShortUrlCodeRepository repo)
        {
            _repo = repo;
        }

        // for testing
        [HttpGet("user")]
        public ActionResult<IEnumerable<User>> GetAllUsers()
        {
            return Ok(_repo.GetAllUsers());
        }

        [HttpGet]
        public ActionResult<IEnumerable<UserShortUrlCode>> GetAllUserUrlCodes()
        {
            return Ok(_repo.GetAllUserShortUrlCodes());
        }

        [HttpPost("{userId}")]
        public ActionResult AddUrlCodeToUser(string userId, List<string> codes)
        {
            return Ok("in progress");
        }
    }
}