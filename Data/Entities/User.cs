using System.ComponentModel.DataAnnotations;

namespace DatingApp.API.Data.Entities
{
    public class User
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(32)]
        public string Username { get; set; }
        [MaxLength(64)]
        public string Email { get; set; }

        public byte[] PasswordHashed { get; set; }
        public byte[] PasswordSalt { get; set; }
    }
}