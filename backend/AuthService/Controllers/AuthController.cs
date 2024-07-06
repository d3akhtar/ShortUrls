using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using AuthService.AsyncDataServices;
using AuthService.Data.Repository;
using AuthService.DTO;
using AuthService.ExternalAuthServices;
using AuthService.Model;
using AutoMapper;
using Microsoft.AspNetCore.Http.Extensions;
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
        private readonly IRabbitMqClient _rabbitMqClient;
        private readonly IGoogleAuth _googleAuth;

        public AuthController
        (
            IMapper mapper, 
            IUserManager userManager, 
            IConfiguration configuration,
            IRabbitMqClient rabbitMqClient,
            IGoogleAuth googleAuth
        )
        {
            _mapper = mapper;
            _userManager = userManager;
            _configuration = configuration;
            _rabbitMqClient = rabbitMqClient;
            _googleAuth = googleAuth;
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

            var tokenString = _userManager.GetTokenString(user);

            return Ok(
                new
                {
                    Message = "Login successful!",
                    Token = tokenString
                }
            );
        }

        [HttpGet("external-login")]
        public async Task<ActionResult> ExternalLogin([FromQuery]string thirdPartyName, [FromQuery]string accessToken)
        {
            switch(thirdPartyName.ToLower()){
                case "google":
                    var userInfo = await _googleAuth.GetUserInfo(accessToken);

                    User user = _userManager.FindUserWithEmail(userInfo.Email);

                    if (user == null)
                    {
                        user = _userManager.AddUser(_mapper.Map<User>(userInfo));
                        _userManager.SaveChanges();

                        _rabbitMqClient.PublishNewUser(_mapper.Map<UserPublishDTO>(user));
                    }

                    var tokenString = _userManager.GetTokenString(user);

                    return Ok(
                        new
                        {
                            Message = "Login successful!",
                            Token = tokenString
                        }
                    );
            }
            return Ok();
        }


        [HttpPost("register")]
        public ActionResult RegisterUser([FromBody]RegisterRequestDTO registerRequest){
            var email = new EmailAddressAttribute();
            if (ModelState.IsValid && email.IsValid(registerRequest.Email) && _userManager.FindUserWithEmail(registerRequest.Email) == null){
                try{
                    User user = _mapper.Map<User>(registerRequest);
                    
                    _userManager.AddUser(user);
                    _userManager.SaveChanges();
                    
                    _rabbitMqClient.PublishNewUser(_mapper.Map<UserPublishDTO>(user));
                    
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