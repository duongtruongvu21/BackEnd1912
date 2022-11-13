using System.ComponentModel.DataAnnotations;

namespace DatingApp.API.DTOs
{
    public class AuthUserDto
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        [EmailAddress]
        public string Email { get; set; }
    }

    public class AuthUserTokenDTO
    {
        public string Username { get; set; }
        public string Token { get; set; }
    }

    public class UserPass
    {
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }



    public class UserPassRegister
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public int Age { get; set; }
        public string KnownAs { get; set; }
        public string Gender { get; set; }
        public string Introdution { get; set; }
        public string City { get; set; }
        public string Avatar { get; set; }
    }
}