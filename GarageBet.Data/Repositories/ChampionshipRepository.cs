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
            if (championship.Teams == null)
                championship.Teams = new List<ChampionshipTeam>();

            ChampionshipTeam championshipTeam =
                Queryable.Where(championship.Teams as IQueryable<ChampionshipTeam>, row => row.TeamId == team.Id)
                .FirstOrDefault();
            if (championshipTeam != null)
            {
                throw new Exception("Exists");
            }

            championship.Teams.Add(
                new ChampionshipTeam
                {
                    TeamId = team.Id,
                    ChampionshipId = championship.Id
                }
            );
            _context.SaveChanges();
            return championship;
        }

        public Championship RemoveTeam(Championship championship, Team team)
        {
            ChampionshipTeam championshipTeam =
                    Queryable.Where(championship.Teams as IQueryable<ChampionshipTeam>, row => row.TeamId == team.Id)
                    .FirstOrDefault();

            championship.Teams.Remove(championshipTeam);
            Update(championship);

            return championship;
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
                .Include(row => row.Teams)
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
