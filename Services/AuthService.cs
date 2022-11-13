using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using DatingApp.API.Data.Entities;
using DatingApp.API.Data.Repositories;
using DatingApp.API.DTOs;

namespace DatingApp.API.Services
{
    public class AuthService : IAuthUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _token;

        public AuthService(IUserRepository userRepository, ITokenService token)
        {
            _userRepository = userRepository;
            _token = token;
        }

        public string Login(UserPass authUserDto)
        {
            authUserDto.Username = authUserDto.Username.ToUpper();
            var currentUser = _userRepository.GetUserByUsername(authUserDto.Username);

            if (currentUser == null)
            {
                throw new UnauthorizedAccessException("Username is invalid!");
            }



            using (var hmac = new HMACSHA512(currentUser.PasswordSalt))
            {
                var passwordBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(authUserDto.Password));

                for (int i = 0; i < currentUser.PasswordHashed.Length; i++)
                {
                    if (currentUser.PasswordHashed[i] != passwordBytes[i])
                    {
                        throw new UnauthorizedAccessException("Pass is invalid!");
                    }
                }

                var token = _token.CreateToken(currentUser.Username);

                return token;
            }
        }

        public string Register(RegisterUserDto registerUserDto)
        {
            registerUserDto.Username = registerUserDto.Username.ToLower();
            var currentUser = _userRepository.GetUserByUsername(registerUserDto.Username);
            if (currentUser != null)
            {
                throw new BadHttpRequestException("Username is already registed!");
            }

            using var hmac = new HMACSHA512();
            var passwordByte = Encoding.UTF8.GetBytes(registerUserDto.Password);
            var newUs = new User()
            {
                Username = registerUserDto.Username,
                PasswordSalt = hmac.Key,
                PasswordHashed = hmac.ComputeHash(passwordByte)
            };

            _userRepository.InsertNewUser(newUs);
            _userRepository.IsSaveChange();

            var token = _token.CreateToken(newUs.Username);

            return token;
        }
    }
}