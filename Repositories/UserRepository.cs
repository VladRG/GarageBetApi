using GarageBet.Api.Repository.Interfaces;
using GarageBet.Api.Database.Tables;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System;
using GarageBet.Api.Database;
using GarageBet.Api.Models;

namespace GarageBet.Api.Repository.Repositories
{
    public class UserRepository : IUserRepository
    {
        private DataContext _context;
        public UserRepository(DataContext context)
        {
            _context = context;
        }

        #region IUserRepository
        public User FindByEmail(string email)
        {
            User user = null;
            try
            {
                user = _context.Users
                      .Include(row => row.Claims)
                      .Single(row => row.Email == email);
            }
            catch (Exception ex)
            {
                return null;
            }
            return user;
        }

        public List<UserModel> GetUsers()
        {
            return _context.Users.Select(user => new UserModel
            {
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName
            }).ToList();
        }
        #endregion

        #region IRepository
        public User Find(long id)
        {
            return _context.Users.Find(id);
        }

        public IEnumerable<User> List()
        {
            return _context.Users.ToList();
        }

        public User Add(User entity)
        {
            _context.Users.Add(entity);
            _context.SaveChanges();
            return entity;
        }

        public void Remove(User entity)
        {
            _context.Users.Remove(entity);
        }

        public User Update(User entity)
        {
            _context.Users.Update(entity);
            _context.SaveChanges();
            return entity;
        }

        public async Task<User> FindAsync(long id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<List<User>> ListAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User> AddAsync(User entity)
        {
            await _context.Users.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<User> UpdateAsync(User entity)
        {
            _context.Users.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async void RemoveAsync(User entity)
        {
            _context.Remove(entity);
            await _context.SaveChangesAsync();
        }
        #endregion
    }
}
