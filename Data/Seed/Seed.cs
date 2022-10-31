using System.Security.Cryptography;
using System.Text;
using DatingApp.API.Data.Entities;
using Newtonsoft.Json;

namespace DatingApp.API.Data.Seed
{
    public class Seed
    {
        public static void SeedUser(DataContext context)
        {
            if(context.AppUsers.Any()) return;
            
            var usersFile = System.IO.File.ReadAllText("Data/Seed/users.json");
            var users = JsonConvert.DeserializeObject<List<User>>((string)usersFile);

            if(users == null) return;
            foreach(var user in users)
            {
                user.CreateAt = DateTime.Now;

                using var hmac = new HMACSHA512();
                user.PasswordHashed = hmac.ComputeHash(Encoding.UTF8.GetBytes("1"));
                user.PasswordSalt = hmac.Key;

                context.AppUsers.Add(user);
            }

            context.SaveChanges();
        }
    }
}