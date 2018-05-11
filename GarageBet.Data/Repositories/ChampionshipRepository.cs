using GarageBet.Data.Interfaces;
using GarageBet.Domain.Tables;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Database.MM;

namespace GarageBet.Data.Repositories
{
    public class ChampionshipRepository : IChampionshipRepository
    {
        DataContext _context;

        public ChampionshipRepository(DataContext context)
        {
            _context = context;
        }

        #region IChampionshipRepository
        public Championship AddTeam(Championship championship, Team team)
        {
            return null;
        }

        public Championship RemoveTeam(Championship championship, Team team)
        {
            return null;
        }
        #endregion

        #region IRepository
        public Championship Find(long id)
        {
            Championship championship = _context.Championships.Find(id);
            return championship;
        }

        public async Task<Championship> FindAsync(long id)
        {
            return await _context.Championships.FindAsync(id);
        }

        public Championship Add(Championship entity)
        {
            _context.Championships.Add(entity);
            var teams = new List<ChampionshipTeam>();
            foreach (var team in entity.Teams)
            {
                teams.Add(new ChampionshipTeam { ChampionshipId = entity.Id, TeamId = team.Id });
            }
            _context.ChampionshipTeams.AddRange(teams);
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
            return _context.Championships
                .Include(row => row.ChampionshipTeams)
                .ToList();
        }

        public async Task<List<Championship>> ListAsync()
        {
            return await _context.Championships
                .Include(row => row.ChampionshipTeams)
                .ToListAsync();
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
            _context.Championships.Update(entity);
            var teams = _context.ChampionshipTeams.Where(e => e.ChampionshipId == entity.Id).ToList();
            _context.RemoveRange(teams);
            _context.SaveChanges();

            teams = new List<ChampionshipTeam>();
            foreach (var team in entity.Teams)
            {
                teams.Add(new ChampionshipTeam { ChampionshipId = entity.Id, TeamId = team.Id });
            }

            _context.AddRange(teams);
            _context.SaveChanges();

            return entity;
        }

        public async Task<Championship> UpdateAsync(Championship entity)
        {
            _context.Championships.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
        #endregion
    }
}
