using GarageBet.Data.Interfaces;
using GarageBet.Domain.Tables;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System;
using GarageBet.Data.Models;

namespace GarageBet.Data.Repositories
{
    public class BetRepository : IBetRepository
    {
        DataContext _context;

        public BetRepository(DataContext context)
        {
            _context = context;
        }

        #region IBetRepository
        public BetModel GetModelForMatch(long matchId)
        {
            return _context.Matches
                .Include(row => row.HomeTeam)
                .Include(row => row.AwayTeam)
                .Include(row => row.Championship)
                .Select(row => new BetModel
                {
                    Match = new MatchModel
                    {
                        Id = row.Id,
                        HomeTeam = new TeamModel
                        {
                            Id = row.HomeTeam.Id,
                            Name = row.HomeTeam.Name
                        },
                        AwayTeam = new TeamModel
                        {
                            Id = row.AwayTeam.Id,
                            Name = row.AwayTeam.Name
                        }
                    },
                    Championship = new ChampionshipModel
                    {
                        Id = row.Championship.Id,
                        CompetitiveYear = row.Championship.CompetitiveYear,
                        Name = row.Championship.Name
                    }
                })
                .Single(row => row.Match.Id == matchId);
        }

        public BetModel GetModelForEdit(long betId)
        {
            return _context.Bets
                .Include(row => row.Match)
                .Select(row => new BetModel
                {
                    Match = new MatchModel
                    {
                        Id = row.Id,
                        HomeTeam = new TeamModel
                        {
                            Id = row.Match.HomeTeam.Id,
                            Name = row.Match.HomeTeam.Name
                        },
                        AwayTeam = new TeamModel
                        {
                            Id = row.Match.AwayTeam.Id,
                            Name = row.Match.AwayTeam.Name
                        }
                    },
                    Championship = new ChampionshipModel
                    {
                        Id = row.Match.Championship.Id,
                        Name = row.Match.Championship.Name,
                        CompetitiveYear = row.Match.Championship.CompetitiveYear
                    },
                    HomeScore = row.HomeScore,
                    AwayScore = row.AwayScore
                }).Single(row => row.Id == betId);
        }

        public IEnumerable<BetModel> GetAvailable(long userId)
        {
            return _context.Matches
                 .Include(row => row.HomeTeam)
                 .Include(row => row.AwayTeam)
                 .Include(row => row.Championship)
                 .Include(row => row.Bets)
                 .Where(
                     row => row.DateTime > DateTime.Now &&
                     row.Bets.Where(bet => bet.UserId == userId).Count() == 0
                  )
                 .Select(row => new BetModel
                 {
                     Match = new MatchModel
                     {
                         Id = row.Id,
                         HomeTeam = new TeamModel
                         {
                             Id = row.HomeTeam.Id,
                             Name = row.HomeTeam.Name
                         },
                         AwayTeam = new TeamModel
                         {
                             Id = row.AwayTeam.Id,
                             Name = row.AwayTeam.Name
                         }
                     },
                     Championship = new ChampionshipModel
                     {
                         Id = row.Championship.Id,
                         CompetitiveYear = row.Championship.CompetitiveYear,
                         Name = row.Championship.Name
                     }
                 }).ToList();
        }

        public IEnumerable<BetModel> GetActiveBets(long userId)
        {
            return _context.Matches
                 .Include(row => row.HomeTeam)
                 .Include(row => row.AwayTeam)
                 .Include(row => row.Championship)
                 .Include(row => row.Bets)
                 .Where(row =>
                     row.Bets.Where(bet =>
                        bet.UserId == userId &&
                        row.HomeScore > -1
                     ).ToList().Count == 0
                  )
                 .Select(row => new BetModel
                 {
                     Match = new MatchModel
                     {
                         Id = row.Id,
                         HomeTeam = new TeamModel
                         {
                             Id = row.HomeTeam.Id,
                             Name = row.HomeTeam.Name
                         },
                         AwayTeam = new TeamModel
                         {
                             Id = row.AwayTeam.Id,
                             Name = row.AwayTeam.Name
                         }
                     },
                     Championship = new ChampionshipModel
                     {
                         Id = row.Championship.Id,
                         CompetitiveYear = row.Championship.CompetitiveYear,
                         Name = row.Championship.Name
                     }
                 });
        }
        #endregion

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
