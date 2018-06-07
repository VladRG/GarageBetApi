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
            var match = _context.Matches.Single(row => row.Id == matchId);
            var result = new BetFormModel
            {

            };
            return result;
        }

        public BetFormModel GetModelForEdit(long betId)
        {
            var bet = _context.Bets.Single(row => row.Id == betId);

            var result = new BetFormModel
            {
                AwayBet = bet.AwayScore,
                AwayScore = bet.Match.AwayScore,
                AwayTeamName = bet.Match.AwayTeam.Name,
                BetId = bet.Id,
                HomeBet = bet.HomeScore,
                HomeScore = bet.Match.HomeScore,
                HomeTeamName = bet.Match.HomeTeam.Name,
                MatchId = bet.Match.Id,
                UserId = bet.User.Id
            };
            return result;
        }

        public IEnumerable<MatchBetModel> GetAvailable(long userId)
        {
            var matches = _context.Matches
                 .Where(row =>
                     row.Bets.Where(bet => bet.UserId == userId).Count() == 0 &&
                     row.DateTime > DateTime.Now
                  ).Include(row => row.Bets)
                  .Include(row => row.HomeTeam)
                  .Include(row => row.AwayTeam).ToList();

            var result = new List<MatchBetModel>();
            foreach (var match in matches)
            {
                var bet = match.Bets.FirstOrDefault(b => b.UserId == userId);
                result.Add(new MatchBetModel
                {
                    MatchId = match.Id,
                    BetId = bet.Id,
                    HomeTeamName = match.HomeTeam.Name,
                    AwayTeamName = match.AwayTeam.Name,
                    DateTime = match.DateTime,
                    HomeBet = bet.HomeScore,
                    AwayBet = bet.AwayScore,
                    HomeScore = match.HomeScore,
                    AwayScore = match.AwayScore,
                    ChampionshipName = match.Championship.Name,
                    CompetitiveYear = match.Championship.CompetitiveYear,
                    BetState = GetBetState(match, userId, bet),
                    Standing = match.Standing
                });
            }
            return result;
        }

        public IEnumerable<BetFormModel> GetActiveBets(long userId)
        {
            var matches = _context.Matches
                 .Where(row =>
                     row.Bets.Where(bet =>
                        bet.UserId == userId &&
                        row.HomeScore > -1
                     ).ToList().Count == 0
                  ).ToList();

            var result = new List<BetFormModel>();
            foreach (var match in matches)
            {
                var bet = match.Bets.Where(b => b.UserId == userId).First();
                result.Add(new BetFormModel
                {
                    AwayBet = bet.AwayScore,
                    AwayScore = match.AwayScore,
                    AwayTeamName = match.AwayTeam.Name,
                    HomeTeamName = match.HomeTeam.Name,
                    BetId = bet != null ? bet.Id : 0,
                    HomeBet = bet.HomeScore,
                    HomeScore = match.HomeScore,
                    MatchId = match.Id,
                    UserId = userId
                });
            }
            return result;
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

        public IEnumerable<UserStats> GetUserStats(int championshipId, int page, int pageSize)
        {
            List<UserStats> users = null;
            if (championshipId == 0)
            {
                users = _context.Users.Select(row => new UserStats
                {
                    User = new UserModel
                    {
                        Email = row.Email,
                        FirstName = row.FirstName,
                        LastName = row.LastName
                    },
                    Won = row.Bets.Where(
                        bet => bet.Match.HomeScore == bet.HomeScore &&
                        bet.Match.AwayScore == bet.AwayScore &&
                        bet.Match.HomeScore > -1 && bet.Match.AwayScore > -1
                        ).Count(),
                    Result = row.Bets.Where(bet =>
                         ((bet.Match.HomeScore < bet.Match.AwayScore && bet.HomeScore < bet.AwayScore) ||
                         (bet.Match.HomeScore > bet.Match.AwayScore && bet.HomeScore > bet.AwayScore) ||
                         (bet.Match.HomeScore == bet.Match.AwayScore && bet.HomeScore == bet.AwayScore)) &&
                         (bet.Match.HomeScore != bet.HomeScore || bet.Match.AwayScore != bet.AwayScore) &&
                         bet.Match.HomeScore > -1 && bet.Match.AwayScore > -1
                         ).Count(),
                    Count = row.Bets.Where(bet => bet.Match.HomeScore > -1 && bet.Match.AwayScore > -1).Count(),
                    Lost = 0
                }).OrderByDescending(r => ((r.Won * 3) + r.Result))
                .Skip(page * pageSize).Take(pageSize).ToList();
            }
            else
            {
                users = _context.Users.Select(row => new UserStats
                {
                    User = new UserModel
                    {
                        Email = row.Email,
                        FirstName = row.FirstName,
                        LastName = row.LastName
                    },
                    Won = row.Bets.Where(bet =>
                        bet.Match.HomeScore == bet.HomeScore &&
                        bet.Match.AwayScore == bet.AwayScore &&
                        bet.Match.Championship.Id == championshipId &&
                        bet.Match.HomeScore > -1 && bet.Match.AwayScore > -1
                    )
                        .Count(),
                    Result = row.Bets.Where(bet =>
                             ((bet.Match.HomeScore < bet.Match.AwayScore && bet.HomeScore < bet.AwayScore) ||
                             (bet.Match.HomeScore > bet.Match.AwayScore && bet.HomeScore > bet.AwayScore) ||
                             (bet.Match.HomeScore == bet.Match.AwayScore && bet.HomeScore == bet.AwayScore)) &&
                             bet.Match.Championship.Id == championshipId &&
                             bet.Match.HomeScore > -1 && bet.Match.AwayScore > -1
                         ).Count(),
                    Count = row.Bets.Where(bet =>
                        bet.Match.Championship.Id == championshipId &&
                        bet.Match.HomeScore > -1 && bet.Match.AwayScore > -1
                    ).Count(),
                    Lost = 0
                }).OrderByDescending(r => ((r.Won * 3) + r.Result))
                .ToList();
            }

            return users;
        }

        public int GetUserLeaderboardPosition(string email)
        {
            var users = _context.Users.Select(row => new UserStats
            {
                User = new UserModel
                {
                    Email = row.Email,
                    FirstName = row.FirstName,
                    LastName = row.LastName
                },
                Won = row.Bets.Where(bet =>
                    bet.Match.HomeScore == bet.HomeScore &&
                    bet.Match.AwayScore == bet.AwayScore &&
                    bet.Match.HomeScore > -1 && bet.Match.AwayScore > -1
                     )
                         .Count(),
                Result = row.Bets.Where(bet =>
                         ((bet.Match.HomeScore < bet.Match.AwayScore && bet.HomeScore < bet.AwayScore) ||
                         (bet.Match.HomeScore > bet.Match.AwayScore && bet.HomeScore > bet.AwayScore) ||
                         (bet.Match.HomeScore == bet.Match.AwayScore && bet.HomeScore == bet.AwayScore)) &&
                         bet.Match.HomeScore > -1 && bet.Match.AwayScore > -1
                          ).Count(),
                Count = row.Bets.Where(bet =>
                    bet.Match.HomeScore > -1 && bet.Match.AwayScore > -1
                     ).Count(),
                Lost = 0
            }).OrderByDescending(r => ((r.Won * 3) + r.Result))
                  .ToList();

            var user = users.Where(r => r.User.Email == email).FirstOrDefault();
            return users.IndexOf(user);
        }

        public int GetUserCount()
        {
            return _context.Users.Count();
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
            _context.Bets.Update(entity);
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

            if (bet == null && match.DateTime > DateTime.Now && match.HomeScore < 0)
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