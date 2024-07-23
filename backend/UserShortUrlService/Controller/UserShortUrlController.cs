using System.IdentityModel.Tokens.Jwt;
using System.Text.Json;
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
    [Route("api/user")]
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
        [Authorize(Roles = "admin")]
        [HttpGet("users")]
        public ActionResult<IEnumerable<User>> GetAllUsers()
        {
            return Ok(_repo.GetAllUsers());
        }

        [Authorize(Roles = "admin")]
        [HttpGet]
        public ActionResult<IEnumerable<UserShortUrl>> GetAllUserShortUrls(string searchQuery = "", int pageNumber = 1, int pageSize = 10)
        {
            if (string.IsNullOrEmpty(searchQuery)) searchQuery = "";
            if (pageSize > 100) pageSize = 100;
            if (pageNumber < 1)
            {
                return BadRequest(new {Message = "Page number must be greater than 0."});
            }

            var allUserShortUrls = _repo.GetAllUserShortUrlCodes(searchQuery, pageNumber, pageSize); 
            return Ok(new 
                    {
                        CurrentPage = pageNumber, 
                        PageSize = pageSize,
                        UserShortUrls = _mapper.Map<IEnumerable<UserShortUrlReadDTO>>(allUserShortUrls)
                    });
        }

        [Authorize]
        [HttpPost("shorturls")]
        public async Task<ActionResult> AddUrlCodeToUser(List<string> codes)
        {
            try{
                var userId = GetUserIdFromHttpRequest(HttpContext.Request);
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
            catch(ArgumentException ex){
                Console.WriteLine(ex.Message);
                return BadRequest(new { Message = "Error while getting userId from token"});
            }
            catch(Exception ex){
                Console.WriteLine("Unexpected error occured: " + ex.Message);
                return BadRequest(new { Message = "Unexpected error occured."});
            }
        }

        [Authorize]
        [HttpGet("shorturls")]
        public ActionResult<UserShortUrlReadDTO> GetShortUrlsForUser(string searchQuery = "", int pageNumber = 1, int pageSize = 10)
        {
            if (string.IsNullOrEmpty(searchQuery)) searchQuery = "";
            if (pageSize > 100) pageSize = 100;
            if (pageNumber < 1)
            {
                return BadRequest(new {Message = "Page number must be greater than 0."});
            }

            try{
                var userId = GetUserIdFromHttpRequest(HttpContext.Request);
                if (_repo.DoesUserWithIdExist(userId)){
                    var userShortUrls = _repo.GetUserShortUrlCodes(userId, searchQuery, pageNumber, pageSize); 
                    return Ok(new 
                    {
                        CurrentPage = pageNumber, 
                        PageSize = pageSize,
                        UserShortUrls = _mapper.Map<IEnumerable<UserShortUrlReadDTO>>(userShortUrls) 
                    });
                }
                else{
                    return NotFound(new { Message = "User not found."} );
                }
            }
            catch(ArgumentException ex){
                Console.WriteLine(ex.Message);
                return BadRequest(new { Message = "Error while getting userId from token"});
            }
            catch(Exception ex){
                Console.WriteLine("Unexpected error occured: " + ex.Message);
                return BadRequest(new { Message = "Error while getting userId from token"});
            }
        }

        [Authorize]
        [HttpDelete("shorturls")]
        public ActionResult<UserShortUrlReadDTO> DeleteShortUrlsForUser([FromQuery]string code)
        {
            try{
                var userId = GetUserIdFromHttpRequest(HttpContext.Request);
                if (_repo.DoesUserWithIdExist(userId) && _repo.DoesUserShortUrlExist(userId, code)){
                    var userShortUrl = _repo.DeleteUserShortUrl(userId, code);
                    _repo.SaveChanges();
                    return Ok(new 
                    {
                        Message = "ShortUrl for user was successfully deleted!",
                        DeletedUserShortUrl = _mapper.Map<UserShortUrlReadDTO>(userShortUrl)
                    });
                }
                else{
                    return NotFound(new { Message = "User or ShortUrl not found."} );
                }
            }
            catch(ArgumentException ex){
                Console.WriteLine(ex.Message);
                return BadRequest(new { Message = "Error while getting userId from token"});
            }
            catch(Exception ex){
                Console.WriteLine("Unexpected error occured: " + ex.Message);
                return BadRequest(new { Message = "Error while getting userId from token"});
            }
        }

        private static int GetUserIdFromHttpRequest(HttpRequest request){
            var tokenString = GetJwtTokenFromHttpRequest(request);
            Console.WriteLine("tokenString: " + tokenString);
            if (tokenString == null) throw new ArgumentException("Error occured while decoding token.");
            var jwtPayload = GetJwtPayloadFromTokenString(tokenString);
            if (jwtPayload.TryGetValue("userId", out var userId)){
                return int.Parse(userId.ToString());
            }
            else{
                throw new ArgumentException("Error occured while decoding token.");
            }
        }
        private static string GetJwtTokenFromHttpRequest(HttpRequest request){
            if(request.Headers.TryGetValue("Authorization", out var headerAuth)){
                Console.WriteLine("headerAuth: " + headerAuth.ToString());
                return headerAuth.ToString().Split("Bearer ")[1];
            }
            else{
                return null;
            }
        }

        private static JwtPayload GetJwtPayloadFromTokenString(string tokenString){
            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(tokenString);
            
            Console.WriteLine("token payload being returned: " + token.Payload.SerializeToJson());
            return token.Payload;
        }
    }
}