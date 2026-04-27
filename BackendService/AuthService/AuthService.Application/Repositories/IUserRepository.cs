using Authservice.Domain.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Application.Repositories
{
    // only implemented for infrastructure layer, not used in application layer, so no need to add methods here
    //step 2: create the user repository interface with the required methods for user registration and retrieval
    public interface IUserRepository
    {
        bool RegisterUser(User user, string Role);
        User GetUserByEmail(string email);
        IEnumerable<User> GetAll();
    }
}
