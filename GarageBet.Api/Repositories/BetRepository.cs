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
    public class BetRepository : IBetRepository
    {
        DataContext _context;

        public BetRepository(DataContext context)
        {
            _context = context;
        }

        #region IBetRepository
        public BetFormModel GetModelForMatch(long matchId)
        {
            return _context.Matches
                .Select(row => new BetFormModel
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

        public BetFormModel GetModelForEdit(long betId)
        {
            return _context.Bets
                .Select(row => new BetFormModel
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
                    AwayScore = row.AwayScore,
                }).Single(row => row.Id == betId);
        }

        public IEnumerable<MatchBetModel> GetAvailable(long userId)
        {
            return _context.Matches
                 .Where(row =>
                     row.Bets.Where(bet => bet.UserId == userId).Count() == 0 &&
                     row.DateTime > DateTime.Now
                  )
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
                         },
                         DateTime = row.DateTime,
                     },
                     BetState = GetBetState(row, userId, row.Bets.FirstOrDefault(bet => bet.UserId == userId)),
                     Bet = new BetModel
                     {
                         HomeScore = row.Bets.FirstOrDefault(bet => bet.UserId == userId).HomeScore,
                         AwayScore = row.Bets.FirstOrDefault(bet => bet.UserId == userId).AwayScore
                     },
                     Championship = new ChampionshipModel
                     {
                         Id = row.Championship.Id,
                         CompetitiveYear = row.Championship.CompetitiveYear,
                         Name = row.Championship.Name
                     }
                 }).ToList();
        }

        public IEnumerable<BetFormModel> GetActiveBets(long userId)
        {
            return _context.Matches
                 .Where(row =>
                     row.Bets.Where(bet =>
                        bet.UserId == userId &&
                        row.HomeScore > -1
                     ).ToList().Count == 0
                  )
                 .Select(row => new BetFormModel
                 {
                     Match = new MatchModel
                     {
                         Id = row.Id,
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

        public UserStats GetUserStat(long userId)
        {
            User user = _context.Users.Find(userId);
            List<Bet> bets = _context.Bets.Include(row => row.Match)
                    .Where(row => row.UserId == userId).ToList();

            UserStats stats = new UserStats();
            foreach (var bet in bets)
            {
                switch (GetBetState(bet.Match, userId, bet))
                {
                    case BetState.Won:
                        stats.Won++;
                        break;
                    case BetState.Result:
                        stats.Result++;
                        break;
                    case BetState.Lost:
                        stats.Lost++;
                        break;
                }
            }
            stats.User = new UserModel
            {
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName
            };
            return stats;
        }

        public IEnumerable<UserStats> GetUserStats()
        {
            List<UserStats> stats = new List<UserStats>();
            foreach (var user in _context.Users.ToList())
            {
                stats.Add(GetUserStat(user.Id));
            }
            stats.OrderBy(row => (row.Won * 3) + row.Lost);
            return stats;
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

            if ((match.DateTime < DateTime.Now || match.HomeScore > -1) && bet == null)
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