using GarageBet.Data.Interfaces;
using GarageBet.Domain;
using GarageBet.Domain.Tables;
using System.Collections.Generic;
using System.Linq;

namespace GarageBet.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private DataContext _context;
        public UserRepository(DataContext context)
        {
            _context = context;
        }

        public ICollection<User> GetUsers()
        {
            return _context.Users.ToList();
        }
    }
}
