using GarageBet.Data.Interfaces;
using GarageBet.Domain;
using GarageBet.Domain.Tables;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System;

namespace GarageBet.Data.Repositories
{
    public class MatchRepository : IMatchRepository
    {
        DataContext _context;

        public MatchRepository(DataContext context)
        {
            _context = context;
        }

        #region IMatchRepository
        public IEnumerable<Match> ListByChampionshipId(long id)
        {
            return _context.Matches
                .Where(row => row.Championship.Id == id)
                .ToList();
        }

        public IEnumerable<Match> ListAvailable()
        {
            return _context.Matches
                .Where(match => match.DateTime < DateTime.Now)
                .ToList();
        }
        #endregion

        #region IRepository
        public Match Find(long id)
        {
            return _context.Matches.Find(id);
        }

        public async Task<Match> FindAsync(long id)
        {
            return await _context.Matches.FindAsync(id);
        }

        public Match Add(Match entity)
        {
            _context.Matches.Add(entity);
            _context.SaveChanges();
            return entity;
        }

        public async Task<Match> AddAsync(Match entity)
        {
            await _context.Matches.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public IEnumerable<Match> List()
        {
            return _context.Matches
                .Include("ChampionshipNavigationProperty")
                .Include("HomeTeamNavigationProperty")
                .Include("AwayTeamNavigationProperty")
                .ToList();
        }

        public async Task<List<Match>> ListAsync()
        {
            return await _context.Matches.ToListAsync();
        }

        public void Remove(Match entity)
        {
            _context.Remove(entity);
            _context.SaveChanges();
        }

        public async void RemoveAsync(Match entity)
        {
            _context.Matches.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public Match Update(Match entity)
        {
            _context.Matches.Remove(entity);
            _context.SaveChanges();
            return entity;
        }

        public async Task<Match> UpdateAsync(Match entity)
        {
            _context.Matches.Remove(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
        #endregion
    }
}
