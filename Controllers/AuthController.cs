using System.Security.Cryptography;
using System.Text;
using DatingApp.API.Data;
using DatingApp.API.Data.Entities;
using DatingApp.API.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.API.Controllers
{
    public class AuthController : BaseController
    {
        private readonly DataContext _context;

        public AuthController(DataContext context)
        {
            _context = context;
        }


        [HttpPost("register")]
        public IActionResult Register([FromBody] AuthUserDto authUserDto)
        {
            authUserDto.Username = authUserDto.Username.ToLower();
            if(_context.AppUsers.Any(u => u.Username == authUserDto.Username))
            {
                return BadRequest("Us is already exist");
            }

            using var hmac = new HMACSHA512();
            var passwordByte = Encoding.UTF8.GetBytes(authUserDto.Password);
            var newUs = new User() {
                Username = authUserDto.Username,
                PasswordSalt = hmac.Key,
                PasswordHashed = hmac.ComputeHash(passwordByte)
            };

            _context.AppUsers.Add(newUs);
            _context.SaveChanges();
            return Ok($"Oke account {newUs.Username}");
        }


        [HttpPost("login")]
        public void Login([FromBody] string value)
        {
        }
    }
}