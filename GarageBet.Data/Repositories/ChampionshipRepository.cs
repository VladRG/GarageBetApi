using GarageBet.Data.Interfaces;
using GarageBet.Domain;
using GarageBet.Domain.Tables;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace GarageBet.Data.Repositories
{
    public class ChampionshipRepository : IChampionshipRepository
    {
        DataContext _context;

        public ChampionshipRepository(DataContext context)
        {
            _context = context;
        }

        #region IRepository
        public Championship Find(long id)
        {
            return _context.Championships.Find(id);
        }

        public async Task<Championship> FindAsync(long id)
        {
            return await _context.Championships.FindAsync(id);
        }

        public Championship Add(Championship entity)
        {
            _context.Championships.Add(entity);
            _context.SaveChanges();
            return entity;
        }

        public async Task<Championship> AddAsync(Championship entity)
        {
            await _context.Championships.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public IEnumerable<Championship> List()
        {
            return _context.Championships.ToList();
        }

        public async Task<List<Championship>> ListAsync()
        {
            return await _context.Championships.ToListAsync();
        }

        public void Remove(Championship entity)
        {
            _context.Remove(entity);
            _context.SaveChanges();
        }

        public async void RemoveAsync(Championship entity)
        {
            _context.Championships.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public Championship Update(Championship entity)
        {
            _context.Championships.Remove(entity);
            _context.SaveChanges();
            return entity;
        }

        public async Task<Championship> UpdateAsync(Championship entity)
        {
            _context.Championships.Remove(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
        #endregion
    }
}
