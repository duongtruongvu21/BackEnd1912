using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatingApp.API.DTOs
{
    public class MemberDto
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public int Age {get;set;}
        public string KnownAs {get;set;}
        public string Gender {get;set;}
        public string Introdution {get;set;}
        public string City {get;set;}
        public string Avatar {get;set;}
        
    }
}