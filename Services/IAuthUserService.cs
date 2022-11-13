using DatingApp.API.DTOs;

namespace DatingApp.API.Services
{
    public interface IAuthUserService
    {
        public string Login(UserPass authUserDto);
        public string Register(RegisterUserDto registerUserDto);
    }
}