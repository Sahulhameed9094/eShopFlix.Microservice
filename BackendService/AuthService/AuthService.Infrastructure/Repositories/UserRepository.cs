using Authservice.Domain.Entites;
using AuthService.Application.Repositories;
using AuthService.Infrastructure.Persistent;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        AuthDbContext _db;

        public UserRepository(AuthDbContext db)
        {
            _db = db;
        }

        public IEnumerable<User> GetAll()
        {
            return _db.Users.ToList();
        }

        public User GetUserByEmail(string email)
        {
            return _db.Users.Include(x=> x.Roles).Where(x => x.Email == email).FirstOrDefault();
        }

        public bool RegisterUser(User user, string Role)
        {
            Role role = _db.Roles.Where(r => r.Name == Role).FirstOrDefault();
            if (role == null)
            {
                return false; // Role does not exist
            }
            user.Roles.Add(role);
            _db.Users.Add(user);
            _db.SaveChanges();
            return true; // User registered successfully
        }



    }
}
