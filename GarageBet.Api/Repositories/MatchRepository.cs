using GarageBet.Api.Repository.Interfaces;
using GarageBet.Api.Database.Tables;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System;
using GarageBet.Api.Models;
using GarageBet.Api.Database;

namespace GarageBet.Api.Repository.Repositories
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
                            DateTime = row.DateTime,
                            HomeTeam = new TeamModel
                            {
                                Id = row.HomeTeam.Id,
                                Name = row.HomeTeam.Name,
                                Country = row.HomeTeam.Country
                            },
                            AwayTeam = new TeamModel
                            {
                                Id = row.AwayTeam.Id,
                                Name = row.AwayTeam.Name,
                                Country = row.AwayTeam.Country
                            },
                            HomeScore = row.HomeScore,
                            AwayScore = row.AwayScore,
                        },
                        Championship = new ChampionshipModel
                        {
                            Id = row.Championship.Id,
                            CompetitiveYear = row.Championship.CompetitiveYear,
                            Name = row.Championship.Name
                        },

                        BetState = GetBetState(row, userId, row.Bets.FirstOrDefault(bet => bet.UserId == userId)),
                        Bet = new BetModel
                        {
                            HomeScore = row.Bets.FirstOrDefault(bet => bet.UserId == userId).HomeScore,
                            AwayScore = row.Bets.FirstOrDefault(bet => bet.UserId == userId).AwayScore
                        }
                    }).OrderBy(t => t.Match.DateTime).ToList();
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

        public IEnumerable<MatchStats> GetMatchStats(long matchId)
        {
            List<MatchStats> stats = new List<MatchStats>();
            var bets = _context.Bets
                .Include(row => row.Match)
                .Include(row => row.User)
                .Where(row => row.MatchId == matchId).ToList();

            foreach (var item in bets)
            {
                stats.Add(new MatchStats
                {
                    HomeScore = item.HomeScore,
                    AwayScore = item.AwayScore,
                    User = new UserModel
                    {
                        Email = item.User.Email,
                        FirstName = item.User.FirstName,
                        LastName = item.User.LastName
                    },
                    BetState = GetBetState(item.Match, item.User.Id, item)
                });
            };

            return stats;
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
            Match match = _context.Matches.Find(entity.Id);
            match.HomeScore = entity.HomeScore;
            match.AwayScore = entity.AwayScore;
            _context.Matches.Update(match);
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

        private BetModel GetBetModel(Match match, long userId)
        {
            Bet bet = match.Bets.FirstOrDefault(b => b.UserId == userId);
            if (bet == null)
            {
                return null;
            }
            return new BetModel
            {
                HomeScore = bet.HomeScore,
                AwayScore = bet.AwayScore
            };
        }

        private BetState GetBetState(Match match, long userId, Bet bet)
        {

            if (match.DateTime > DateTime.Now && match.HomeScore < 0 && bet == null)
            {
                return BetState.CanBet;
            }

            if (bet == null)
            {
                return BetState.NotAvailable;
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
            else
            {
                return BetState.Lost;
            }
        }
    }
}
