using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DatingApp.API.Data.Entities;

namespace DatingApp.API.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;

        public UserRepository(DataContext context)
        {
            _context = context;
        }

        public void DeleteUser(User user)
        {
            _context.AppUsers.Remove(user);
        }

        public User GetUserById(int id)
        {
            return _context.AppUsers.FirstOrDefault(user => user.Id == id);
        }

        public User GetUserByUsername(string username)
        {
            return _context.AppUsers.FirstOrDefault(user => user.Username == username);
        }

        public List<User> GetUsers()
        {
            return _context.AppUsers.ToList();
        }

        public void InsertNewUser(User user)
        {
            _context.AppUsers.Add(user);
        }

        public bool IsSaveChange()
        {
            // trả về số lượng savechange, có số lượng đã lưu thì trả về true
            return _context.SaveChanges() > 0;
        }

        public void UpdateUser(User user)
        {
            _context.AppUsers.Update(user);
        }
    }
}