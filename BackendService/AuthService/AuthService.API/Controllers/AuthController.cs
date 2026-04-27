using AuthService.Application.DTOs;
using AuthService.Application.Services.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.API.Controllers
{

    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        IUserAppService _userAppService;
        public AuthController(IUserAppService userAppService)
        {
            _userAppService = userAppService;
        }

        [HttpPost]
        public IActionResult Login([FromBody] LoginDTO loginDTO)
        {
            var user = _userAppService.LoginUser(loginDTO);
            if (user != null)
            {
                return Ok(user);
            }
            return Unauthorized("Invalid email or password.");
        }

        [HttpPost]
        public IActionResult SignUp([FromBody] SignUpDTO signUpDTO, string Role)
        {
            if (string.IsNullOrEmpty(Role))
            {
                return BadRequest("Role is required.");
            }
            bool isRegistered = _userAppService.SignUpUser(signUpDTO, Role);
            if (isRegistered)
            {
                return Ok("User registered successfully.");
            }
            return BadRequest("User registration failed. User may already exist or role does not exist.");
        }
    }

}
