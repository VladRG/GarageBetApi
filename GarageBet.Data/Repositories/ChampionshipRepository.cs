using GarageBet.Data.Interfaces;
using GarageBet.Domain;
using GarageBet.Domain.Tables;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Database.MM;
using System;

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
            return _context.Championships
                .Include("ChampionshipTeams.Team")
                .ToList();
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
            _context.Championships.Update(entity);
            _context.SaveChanges();
            entity.ChampionshipTeams.Clear();
            var championshipTeams = new List<ChampionshipTeam>();
            foreach (var team in entity.Teams)
            {
                championshipTeams.Add(new ChampionshipTeam
                {
                    Team = team,
                    Championship = entity
                });
            }
            _context.AddRange(championshipTeams);
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
