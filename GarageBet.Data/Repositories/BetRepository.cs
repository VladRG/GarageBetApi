using GarageBet.Data.Interfaces;
using GarageBet.Domain.Tables;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace GarageBet.Data.Repositories
{
    public class BetRepository : IBetRepository
    {
        DataContext _context;

        public BetRepository(DataContext context)
        {
            _context = context;
        }

        #region IRepository
        public Bet Find(long id)
        {
            return _context.Bets.Find(id);
        }

        public async Task<Bet> FindAsync(long id)
        {
            return await _context.Bets.FindAsync(id);
        }

        public Bet Add(Bet entity)
        {
            _context.Bets.Add(entity);
            _context.SaveChanges();
            return entity;
        }

        public async Task<Bet> AddAsync(Bet entity)
        {
            await _context.Bets.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public IEnumerable<Bet> List()
        {
            return _context.Bets.ToList();
        }

        public async Task<List<Bet>> ListAsync()
        {
            return await _context.Bets.ToListAsync();
        }

        public void Remove(Bet entity)
        {
            _context.Remove(entity);
            _context.SaveChanges();
        }

        public async void RemoveAsync(Bet entity)
        {
            _context.Bets.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public Bet Update(Bet entity)
        {
            _context.Bets.Remove(entity);
            _context.SaveChanges();
            return entity;
        }

        public async Task<Bet> UpdateAsync(Bet entity)
        {
            _context.Bets.Remove(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
        #endregion
    }
}
