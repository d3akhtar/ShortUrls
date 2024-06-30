using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using AuthService.Data.Repository;
using AuthService.DTO;
using AuthService.Model;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace AuthService.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUserManager _userManager;
        private readonly IConfiguration _configuration;

        public AuthController(IMapper mapper, IUserManager userManager, IConfiguration configuration)
        {
            _mapper = mapper;
            _userManager = userManager;
            _configuration = configuration;
        }

        // for testing
        [HttpGet]
        public ActionResult GetAllUsers()
        {
            var users = _userManager.GetAllUsers();
            return Ok(_mapper.Map<IEnumerable<UserReadDTO>>(users));
        }

        [HttpPost("login")]
        public ActionResult Login([FromBody]LoginRequestDTO loginRequest)
        {
            var user = _userManager.FindUserWithEmail(loginRequest.Email);
            if (user == null || !_userManager.IsPasswordValid(user, loginRequest.Password)) return BadRequest("Invalid login");

            string secretKey = _configuration["SecretKey"];
            JwtSecurityTokenHandler tokenHandler = new();
            byte[] key = Encoding.ASCII.GetBytes(secretKey);

            SecurityTokenDescriptor descriptor = new()
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim("id", user.UserId),
                    new Claim("email", user.Email),
                    new Claim("username", user.Username),
                    new Claim("role", user.Role),
                }),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            SecurityToken token = tokenHandler.CreateToken(descriptor);
            string tokenString = tokenHandler.WriteToken(token);
            return Ok(
                new
                {
                    Message = "Login successful!",
                    Token = tokenString
                }
            );
        }

        [HttpPost("register")]
        public ActionResult RegisterUser([FromBody]RegisterRequestDTO registerRequest){
            var email = new EmailAddressAttribute();
            if (ModelState.IsValid && email.IsValid(registerRequest.Email) && _userManager.FindUserWithEmail(registerRequest.Email) == null){
                try{
                    User user = _mapper.Map<User>(registerRequest);
                    
                    _userManager.AddUser(user);
                    _userManager.SaveChanges();
                    
                    return Ok(new { Message = "Registration complete!", User = _mapper.Map<UserReadDTO>(user) });
                }
                catch(Exception ex){
                    Console.WriteLine("--> Error while registering user: " + ex.Message);
                    return StatusCode(500, new { Message = "Something went wrong"} );
                }
            }
            else{
                return BadRequest(new {Message = "Register request was invalid, please check your inputs!"});
            }
        }
    }
}