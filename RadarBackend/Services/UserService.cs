using RadarBackend.Models;
using RadarBackend.Data;
using System.Collections.Generic;
using System.Linq;

namespace RadarBackend.Services
{
    public interface IUserService
    {
        User Authenticate(string email, string password);
        IEnumerable<User> GetAll();
    }

    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;

        public UserService(ApplicationDbContext context)
        {
            _context = context;
        }

        public User Authenticate(string email, string password)
        {
            var user = _context.Users.SingleOrDefault(x => x.Email == email && x.PasswordHash == password);
            if (user == null)
            {
                throw new Exception("User not found or incorrect password.");
            }
            return user;
        }


        public IEnumerable<User> GetAll()
        {
            return _context.Users;
        }
    }
}
