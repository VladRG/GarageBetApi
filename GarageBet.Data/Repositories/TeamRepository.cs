using GarageBet.Data.Interfaces;
using GarageBet.Domain;
using GarageBet.Domain.Tables;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace GarageBet.Data.Repositories
{
    public class TeamRepository : ITeamRepository
    {
        DataContext _context;

        public TeamRepository(DataContext context)
        {
            _context = context;
        }

        #region IRepository
        public Team Find(long id)
        {
            return _context.Teams.Find(id);
        }

        public async Task<Team> FindAsync(long id)
        {
            return await _context.Teams.FindAsync(id);
        }

        public Team Add(Team entity)
        {
            _context.Teams.Add(entity);
            _context.SaveChanges();
            return entity;
        }

        public async Task<Team> AddAsync(Team entity)
        {
            await _context.Teams.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public IEnumerable<Team> List()
        {
            return _context.Teams.ToList();
        }

        public async Task<List<Team>> ListAsync()
        {
            return await _context.Teams.ToListAsync();
        }

        public void Remove(Team entity)
        {
            _context.Remove(entity);
            _context.SaveChanges();
        }

        public async void RemoveAsync(Team entity)
        {
            _context.Teams.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public Team Update(Team entity)
        {
            _context.Teams.Remove(entity);
            _context.SaveChanges();
            return entity;
        }

        public async Task<Team> UpdateAsync(Team entity)
        {
            _context.Teams.Remove(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
        #endregion
    }
}
