using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserShortUrlService.Data.Repository;
using UserShortUrlService.DTO;
using UserShortUrlService.Model;
using UserShortUrlService.SyncDataServices.Http;

namespace UserShortUrlService.Controller
{
    [ApiController]
    [Route("/api/user")]
    public class UserShortUrlController : ControllerBase
    {
        private readonly IUserShortUrlCodeRepository _repo;
        private readonly IMapper _mapper;

        public UserShortUrlController(IUserShortUrlCodeRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        // for testing
        [Authorize]
        [HttpGet("users")]
        public ActionResult<IEnumerable<User>> GetAllUsers()
        {
            return Ok(_repo.GetAllUsers());
        }

        [HttpGet]
        public ActionResult<IEnumerable<UserShortUrl>> GetAllUserUrlCodes()
        {
            var allUserShortUrls = _repo.GetAllUserShortUrlCodes(); 
            return Ok(_mapper.Map<IEnumerable<UserShortUrlReadDTO>>(allUserShortUrls));
        }

        [HttpPost("{userId}")]
        public async Task<ActionResult> AddUrlCodeToUser(string userId, List<string> codes)
        {
            if (_repo.DoesUserWithIdExist(userId)){
                var addedUserShortUrlCodes = await _repo.AddUserShortUrls(userId, codes);
                _repo.SaveChanges();

                if (addedUserShortUrlCodes.Count() > 0){
                    return Ok(new 
                    { 
                        Message = $"Added short urls for user with id: {userId}", 
                        AddedShortUrls = _mapper.Map<IEnumerable<UserShortUrlReadDTO>>(addedUserShortUrlCodes)
                    });
                }
                else{
                    return BadRequest(new { Message = "No short urls were added for this user, maybe they were already added or they don't exist."});
                }
            }
            else{
                return NotFound(new { Message = "User not found."} );
            }
        }

        [HttpGet("{userId}")]
        public ActionResult<UserShortUrlReadDTO> GetShortUrlsForUser(string userId)
        {
            if (_repo.DoesUserWithIdExist(userId)){
                var userShortUrls = _repo.GetUserShortUrlCodes(userId); 
                return Ok(_mapper.Map<IEnumerable<UserShortUrlReadDTO>>(userShortUrls));
            }
            else{
                return NotFound(new { Message = "User not found."} );
            }
        }
    }
}