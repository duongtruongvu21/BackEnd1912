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

        public AuthController(DataContext context, ITokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }


        [HttpPost("register")]
        public IActionResult Register([FromBody] AuthUserDto authUserDto)
        {
            authUserDto.Username = authUserDto.Username.ToLower();
            if (_context.AppUsers.Any(u => u.Username == authUserDto.Username))
            {
                return BadRequest("Us is already exist");
            }

            using var hmac = new HMACSHA512();
            var passwordByte = Encoding.UTF8.GetBytes(authUserDto.Password);
            var newUs = new User()
            {
                Username = authUserDto.Username,
                PasswordSalt = hmac.Key,
                PasswordHashed = hmac.ComputeHash(passwordByte)
            };

            _context.AppUsers.Add(newUs);
            _context.SaveChanges();

            var token = _tokenService.CreateToken(newUs.Username);

            return Ok(token);
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] UserPass authUserDto)
        {
            authUserDto.Username = authUserDto.Username.ToLower();
            var currentUser = _context.AppUsers.FirstOrDefault(u => u.Username == authUserDto.Username);

            if (currentUser == null)
            {
                return Unauthorized("Không tồn tại tài khoản này.");
            }

            using (var hmac = new HMACSHA512(currentUser.PasswordSalt))
            {
                var passwordBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(authUserDto.Password));

                for (int i = 0; i < currentUser.PasswordHashed.Length; i++)
                {
                    if (currentUser.PasswordHashed[i] != passwordBytes[i])
                    {
                        return Unauthorized("Sai mật khẩu.");
                    }
                }

                var token = _tokenService.CreateToken(currentUser.Username);

                return Ok(token);
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