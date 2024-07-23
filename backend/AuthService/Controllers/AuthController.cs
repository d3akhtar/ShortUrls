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
using AuthService.SyncDataServices.Smtp;
using AuthServiceq.DTO;
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
        private readonly IEmailService _emailService;

        public AuthController
        (
            IMapper mapper, 
            IUserManager userManager, 
            IConfiguration configuration,
            IRabbitMqClient rabbitMqClient,
            IGoogleAuth googleAuth,
            IEmailService emailService
        )
        {
            _mapper = mapper;
            _userManager = userManager;
            _configuration = configuration;
            _rabbitMqClient = rabbitMqClient;
            _googleAuth = googleAuth;
            _emailService = emailService;
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

            if (user.VerifiedAt != null)
            {
                return Ok(
                    new
                    {
                        Message = "Login successful!",
                        Token = tokenString
                    }
                );
            }
            else return BadRequest("User must be verified first.");
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
                        // For external logins, notify user that an account for this website was created using their email
                        user.VerifiedAt = DateTime.Now;
                        user = _userManager.AddUser(_mapper.Map<User>(userInfo));
                        _userManager.SaveChanges();

                        _rabbitMqClient.PublishNewUser(_mapper.Map<UserPublishDTO>(user));

                        _emailService.SendEmail(
                            subject: "ShortUrls Account Registration",
                            body: 
                            $@" <h3>ShortUrls Account Registration</h3>
                                <p>This email was recently used create an account for https://shorturls.danyalakt.com</p>
                            ",
                            receiverEmail: user.Email
                        );
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
                    
                    var verificationUrl = _configuration["Jwt:Issuer"] + "/api/auth/verify?token=" + user.VerificationToken;

                    _emailService.SendEmail(
                        subject: "Account Verification",
                        body: 
                        $@" <h3>ShortUrls Account Verification</h3>
                            <p>Click on the given link to verify your newly registered account.</p>
                            <form action={verificationUrl} method={"POST"}>
                            <button type={"submit"}
                            style={
                                @"
                                    border: none;
                                    outline: none;
                                    background: none;
                                    cursor: pointer;
                                    color: #0000EE;
                                    padding: 0;
                                    text-decoration: underline;
                                    font-family: inherit;
                                    font-size: inherit;
                                "
                                }
                            >
                                    Verify
                                </button>
                            </form>    
                        ",
                        receiverEmail: user.Email
                    );
                    
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

        [HttpPost("verify")]
        public ActionResult Verify(string token)
        {
            var user = _userManager.FindUserWithVerificationToken(token);
            if (user == null) return BadRequest(new { Message = "The verification token was invalid." });
            if (user.VerifiedAt != null) return Ok(new {Message = "User has already been verified. Try logging in."});

            _userManager.VerifyUser(user);
            _userManager.SaveChanges();

            return new RedirectResult(_configuration["FrontEnd:LoginPage"]);
        }

        [HttpGet("send-verification-email")]
        public ActionResult SendVerificationEmail(string email)
        {
            var user = _userManager.FindUserWithEmail(email);
            if (user == null) return BadRequest(new {Message = "Cannot find an account registered under this email."});
            if (user.VerifiedAt != null) return BadRequest(new {Message = "Account is already verified, try logging in."});

            var verificationUrl = _configuration["Jwt:Issuer"] + "/api/auth/verify?token=" + user.VerificationToken;
            _emailService.SendEmail(
                subject: "Account Verification",
                body: 
                $@" <h3>ShortUrls Account Verification</h3>
                    <p>Click on the given link to verify your account.</p>
                    <form action={verificationUrl} method={"POST"}>
                        <button type={"submit"}
                        style={
                            @"
                                border: none;
                                outline: none;
                                background: none;
                                cursor: pointer;
                                color: #0000EE;
                                padding: 0;
                                text-decoration: underline;
                                font-family: inherit;
                                font-size: inherit;
                            "
                            }
                        >
                            Verify
                        </button>
                    </form>    
                ",
                receiverEmail: user.Email
            );

            return Ok(new {Message = "Verification email was sent successfully."});
        }

        [HttpPost("forgot-password")]
        public ActionResult ForgotPassword(string email)
        {
            var user = _userManager.FindUserWithEmail(email);
            if (user == null) return BadRequest(new {Message = "Cannot find an account registered under this email."});

            string token = _userManager.SetPasswordResetToken(user);
            _userManager.SaveChanges();

            var passwordResetUrl = _configuration["FrontEnd:PasswordResetUrl"] + token;
            _emailService.SendEmail(
                subject: "Account Password Reset",
                body: 
                $@" <h3>ShortUrls Account Password Reset</h3>
                    <p>Click on the given link to reset your password. If this wasn't you, ignore this email</p>
                    <form action={passwordResetUrl} method={"GET"}>
                        <button type={"submit"}
                        style={
                            @"
                                border: none;
                                outline: none;
                                background: none;
                                cursor: pointer;
                                color: #0000EE;
                                padding: 0;
                                text-decoration: underline;
                                font-family: inherit;
                                font-size: inherit;
                            "
                            }
                        >
                            Reset Password
                        </button>
                    </form>    
                ",
                receiverEmail: user.Email
            );

            return Ok(new {Message = "Password reset link was sent."});
        }

        [HttpPost("reset-password")]
        public ActionResult ResetPassword(PasswordResetRequestDTO passwordResetRequestDTO)
        {
            var user = _userManager.FindUserWithPasswordResetToken(passwordResetRequestDTO.Token);
            if (user == null) return BadRequest (new {Message = "Password reset token was invalid"});
            if (DateTime.Now > user.ResetTokenExpires) return BadRequest (new {Message = "Password reset token has expired"});

            _userManager.SetUserPassword(user, passwordResetRequestDTO.Password);
            _userManager.SaveChanges();

            return Ok(new {Message = "Password was reset."});
        }
    }
}