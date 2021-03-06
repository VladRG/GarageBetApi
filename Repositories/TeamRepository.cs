﻿
using GarageBet.Api.Database;
using GarageBet.Api.Database.Tables;
using GarageBet.Api.Models;
using GarageBet.Api.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GarageBet.Api.Repository.Repositories
{
    public class TeamRepository : ITeamRepository
    {
        DataContext _context;

        public TeamRepository(DataContext context)
        {
            _context = context;
        }

        #region ITeamRepository
        public IEnumerable<Team> ListForChampionship(long id)
        {
            return _context.Teams;
        }

        public IEnumerable<TeamModel> ListModels()
        {
            return _context.Teams.Select(entity => new TeamModel
            {
                Id = entity.Id,
                Name = entity.Name,
                Country = entity.Country
            });
        }
        #endregion

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
            _context.Teams.Update(entity);
            _context.SaveChanges();
            return entity;
        }

        public async Task<Team> UpdateAsync(Team entity)
        {
            _context.Teams.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
        #endregion
    }
}
