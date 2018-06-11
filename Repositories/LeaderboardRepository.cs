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

    public class LeaderboardRepository : ILeaderboardRepository
    {
        DataContext _context;

        public LeaderboardRepository(DataContext context)
        {
            _context = context;
        }

        public IEnumerable<UserStats> GetUserStats(int page, int pageSize)
        {
            IEnumerable<UserStats> users = null;
            users = this.GetUserStatsFromQueryable(_context.Users, page, pageSize);
            return users;
        }

        public IEnumerable<LeaderboardModel> GetUserLeaderboards(long userId, int page, int pageSize)
        {
            User user = _context.Users.Find(userId);
            List<LeaderboardModel> leaderboards = new List<LeaderboardModel>();
            foreach (var leaderboard in user.Leaderboards)
            {
                leaderboards.Add(new LeaderboardModel
                {
                    Admin = new UserModel
                    {
                        Email = user.Email,
                        FirstName = user.FirstName,
                        LastName = user.LastName
                    },
                    Id = leaderboard.LeaderboardId,
                    Users = this.GetUserStatsFromQueryable(leaderboard.Leaderboard.Users, page, pageSize).ToList()
                });
            }
            return leaderboards;
        }

        private IEnumerable<UserStats> GetUserStatsFromQueryable(IEnumerable<User> users, int page, int pageSize)
        {
            return users.Select(row => new UserStats
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

        private IEnumerable<UserStats> GetUserStatsFromQueryable(ICollection<LeaderboardUser> users, int page, int pageSize)
        {
            return users.Select(row => new UserStats
            {
                User = new UserModel
                {
                    Email = row.User.Email,
                    FirstName = row.User.FirstName,
                    LastName = row.User.LastName
                },
                Won = row.User.Bets.Where(
                    bet => bet.Match.HomeScore == bet.HomeScore &&
                    bet.Match.AwayScore == bet.AwayScore &&
                    bet.Match.HomeScore > -1 && bet.Match.AwayScore > -1
                    ).Count(),
                Result = row.User.Bets.Where(bet =>
                     ((bet.Match.HomeScore < bet.Match.AwayScore && bet.HomeScore < bet.AwayScore) ||
                     (bet.Match.HomeScore > bet.Match.AwayScore && bet.HomeScore > bet.AwayScore) ||
                     (bet.Match.HomeScore == bet.Match.AwayScore && bet.HomeScore == bet.AwayScore)) &&
                     (bet.Match.HomeScore != bet.HomeScore || bet.Match.AwayScore != bet.AwayScore) &&
                     bet.Match.HomeScore > -1 && bet.Match.AwayScore > -1
                     ).Count(),
                Count = row.User.Bets.Where(bet => bet.Match.HomeScore > -1 && bet.Match.AwayScore > -1).Count(),
                Lost = 0
            }).OrderByDescending(r => ((r.Won * 3) + r.Result))
            .Skip(page * pageSize).Take(pageSize).ToList();
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

        public UserStats GetUserStat(long userId)
        {
            throw new NotImplementedException();
        }

        public void AcceptInvite(User user, long groupId)
        {
            var leaderboard = _context.Leaderboards.Find(groupId);
            var leaderboardUser = leaderboard.Users.Where(u => u.User.Id == user.Id).First();
            leaderboardUser.Accepted = true;
            _context.Leaderboards.Update(leaderboard);
            _context.SaveChanges();
        }

        public void DeclineInvite(User user, long groupId)
        {
            var leaderboard = _context.Leaderboards.Find(groupId);
            var leaderboardUser = leaderboard.Users.Where(u => u.User.Id == user.Id).First();
            leaderboardUser.Accepted = false;
            _context.Leaderboards.Update(leaderboard);
            _context.SaveChanges();
        }


        #region IRepository
        public Leaderboard Find(long id)
        {
            return _context.Leaderboards.Find(id);
        }

        public IEnumerable<Leaderboard> List()
        {
            return _context.Leaderboards.ToList();
        }

        public Leaderboard Add(Leaderboard entity)
        {
            _context.Leaderboards.Add(entity);
            _context.SaveChanges();
            return entity;
        }

        public Leaderboard Update(Leaderboard entity)
        {
            _context.Leaderboards.Update(entity);
            _context.SaveChanges();
            return entity;
        }

        public void Remove(Leaderboard entity)
        {
            _context.Leaderboards.Remove(entity);
        }

        public Task<Leaderboard> FindAsync(long id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Leaderboard>> ListAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Leaderboard> AddAsync(Leaderboard entity)
        {
            throw new NotImplementedException();
        }

        public Task<Leaderboard> UpdateAsync(Leaderboard entity)
        {
            throw new NotImplementedException();
        }

        public void RemoveAsync(Leaderboard entity)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}