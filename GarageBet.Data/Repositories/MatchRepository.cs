using GarageBet.Data.Interfaces;
using GarageBet.Domain.Tables;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System;
using GarageBet.Data.Models;
using GarageBet.Domain.Models;

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

        public IEnumerable<MatchBetModel> ListMatchBets(long userId)
        {
            return _context.Matches
                    .Select(row => new MatchBetModel
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
                        },
                        HomeScore = row.HomeScore,
                        AwayScore = row.AwayScore,
                        BetState = GetBetState(row, userId, row.Bets.SingleOrDefault(bet => bet.UserId == userId))
                    }).ToList();
        }

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
            return _context.Matches
               .Include(row => row.Championship)
               .Include(row => row.HomeTeam)
               .Include(row => row.AwayTeam)
               .Single(row => row.Id == id);
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
                 .Include(row => row.Championship)
                 .Include(row => row.HomeTeam)
                 .Include(row => row.AwayTeam)
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

        private BetState GetBetState(Match match, long userId, Bet bet)
        {

            if (match.DateTime > DateTime.Now && match.HomeScore < 0)
            {
                return BetState.CanBet;
            }

            if (bet == null)
            {
                return BetState.CanBet;
            }


            if (match.HomeScore == bet.HomeScore && match.AwayScore == bet.AwayScore)
            {
                return BetState.Won;
            }

            if (
              (match.HomeScore < match.AwayScore && bet.HomeScore < bet.AwayScore) ||
              (match.HomeScore > match.AwayScore && bet.HomeScore > bet.AwayScore) ||
              (match.HomeScore == match.AwayScore && bet.HomeScore == bet.AwayScore))
            {
                return BetState.Result;
            }

            return BetState.NotAvailable;
        }
    }
}
