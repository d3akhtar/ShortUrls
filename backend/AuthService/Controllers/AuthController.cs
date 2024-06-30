using AuthService.DTO;
using AuthService.Model;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMapper _mapper;

        public AuthController(IMapper mapper)
        {
            _mapper = mapper;
        }

        [HttpPost]
        public ActionResult Test([FromBody]RegisterRequestDTO registerRequest){
            User user = _mapper.Map<User>(registerRequest);
            return Ok(user);
        }
    }
}