using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using DatingApp.API.Data;
using DatingApp.API.Data.Entities;
using DatingApp.API.DTOs;
using DatingApp.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.API.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly ITokenService _tokenService;
        private readonly IAuthUserService _authUserService;

        public AuthController(DataContext context, ITokenService tokenService, IAuthUserService authUserService)
        {
            _context = context;
            _tokenService = tokenService;
            _authUserService = authUserService;
        }


        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterUserDto authUserDto)
        {
            try
            {
                return Ok(_authUserService.Register(authUserDto));
            }
            catch (BadHttpRequestException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] UserPass authUserDto)
        {
            try
            {
                return Ok(_authUserService.Login(authUserDto));
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
        }

        [HttpGet("getAllUser")]
        // [Authorize] // dungf postman để test
        public IActionResult GetAllUser()
        {
            return Ok(_context.AppUsers.ToList());
        }


        [HttpGet("getUser")]
        [Authorize]
        public IActionResult GetUser()
        {
            string token = Request.Headers["Authorization"];
            token = token.Substring(7);
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var jsonToken = tokenHandler.ReadToken(token);
                var tokenS = jsonToken as JwtSecurityToken;
                var jti = tokenS.Claims.First(claim => claim.Type == JwtRegisteredClaimNames.NameId).Value;
                return Ok($"token: {jti}");
            }
            catch (Exception)
            {
                return Ok($"fails token: {token}");
            }
        }
    }
}