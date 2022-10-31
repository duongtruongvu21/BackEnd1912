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
        public DateTime DateOfBirth {get;set;}
        [MaxLength(32)]
        public string KnownAs {get;set;}
        [MaxLength(8)]
        public string Gender {get;set;}
        [MaxLength(512)]
        public string Introdution {get;set;}
        public string City {get;set;}
        [MaxLength(256)]
        public string Avatar {get;set;}
        public DateTime? CreateAt {get;set;}
        public DateTime? UpdateAt {get;set;}
    }
}