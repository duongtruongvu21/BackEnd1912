using System.ComponentModel.DataAnnotations;

namespace DatingApp.API.DTOs
{
    public class AuthUserDto
    {
        [Required]
        public string Username {get; set;}
        [Required]
        public string Password {get; set;}
        [EmailAddress]
        public string Email {get; set;}
    }

    public class AuthUserTokenDTO
    {
        public string Username {get;set;}
        public string Token {get;set;}
    }

    public class UserPass
    {
        public string Username {get;set;}
        [Required]
        public string Password {get; set;}
    }
}