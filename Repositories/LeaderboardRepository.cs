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

        public IEnumerable<UserStats> GetUserStats(int page, int pageSize, int group)
        {
            IEnumerable<UserStats> users = null;
            if (group != 0)
            {
                var leaderboard = _context.Leaderboards
                    .Include(row => row.Users)
                    .Include("Users.User")
                    .Include("Users.User.Bets")
                    .Include("Users.User.Bets.Match")
                    .Where(row => row.Id == group)
                    .First();

                users = leaderboard.Users.Select(row => new UserStats
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
            return users;
        }

        public IEnumerable<LeaderboardModel> GetUserLeaderboards(long userId, int page, int pageSize)
        {
            User user = _context.Users
                .Include(row => row.Leaderboards)
                .Include("Leaderboards.Leaderboard")
                .Include("Leaderboards.Leaderboard.Admin")
                .Where(row => row.Id == userId).First();

            List<LeaderboardModel> leaderboards = new List<LeaderboardModel>();
            foreach (var leaderboard in user.Leaderboards)
            {
                leaderboards.Add(new LeaderboardModel
                {
                    Admin = new UserModel
                    {
                        Email = leaderboard.Leaderboard.Admin.Email,
                        FirstName = leaderboard.Leaderboard.Admin.FirstName,
                        LastName = leaderboard.Leaderboard.Admin.LastName
                    },
                    Id = leaderboard.LeaderboardId,
                    Users = leaderboard.Leaderboard.Users.Select(row => new UserStats
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
                      .Skip(page * pageSize).Take(pageSize).ToList()
                });
            }
            return leaderboards;
        }

        public int GetUserLeaderboardPosition(string email, long group)
        {
            UserStats user = null;
            List<UserStats> users = new List<UserStats>();
            if (group != 0)
            {
                var leaderboard = _context.Leaderboards
                    .Include(row => row.Users)
                    .Include("Users.User")
                    .Include("Users.User.Bets")
                    .Include("Users.User.Bets.Match")
                    .Where(row => row.Id == group)
                    .First();
                users = leaderboard.Users.Select(row => new UserStats
                {
                    User = new UserModel
                    {
                        Email = row.User.Email,
                        FirstName = row.User.FirstName,
                        LastName = row.User.LastName
                    },
                    Won = row.User.Bets.Where(bet =>
                        bet.Match.HomeScore == bet.HomeScore &&
                        bet.Match.AwayScore == bet.AwayScore &&
                        bet.Match.HomeScore > -1 && bet.Match.AwayScore > -1
                       ).Count(),
                    Result = row.User.Bets.Where(bet =>
                         ((bet.Match.HomeScore < bet.Match.AwayScore && bet.HomeScore < bet.AwayScore) ||
                         (bet.Match.HomeScore == bet.Match.AwayScore && bet.HomeScore == bet.AwayScore)) &&
                         (bet.Match.HomeScore > bet.Match.AwayScore && bet.HomeScore > bet.AwayScore) ||
                         bet.Match.HomeScore > -1 && bet.Match.AwayScore > -1
                        ).Count(),
                    Count = row.User.Bets.Where(bet =>
                        bet.Match.HomeScore > -1 && bet.Match.AwayScore > -1
                    ).Count(),
                    Lost = 0
                }).OrderByDescending(r => ((r.Won * 3) + r.Result)).ToList();
                user = users.Where(r => r.User.Email == email).FirstOrDefault();
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
                }).OrderByDescending(r => ((r.Won * 3) + r.Result)).ToList();
                user = users.Where(r => r.User.Email == email).FirstOrDefault();
            }

            return users.IndexOf(user);
        }

        public int GetUserCount(long group)
        {
            if (group != 0)
            {
                return _context.Leaderboards.Find(group).Users.Count;
            }
            return _context.Users.Count();
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

        public Leaderboard Add(LeaderboardAddModel leaderboard)
        {
            Leaderboard addLeaderboard = new Leaderboard();
            addLeaderboard.AdminId = leaderboard.AdminId;
            addLeaderboard.Name = leaderboard.Name;
            addLeaderboard.Users = leaderboard.Users.Select(row => new LeaderboardUser
            {
                UserId = _context.Users.Where(user => row.Email == user.Email).First().Id,
            }).ToList();
            return Add(addLeaderboard);
        }

        public Leaderboard Update(LeaderboardAddModel entity)
        {
            var leaderboardUsers = _context.LeaderboardUsers
                .Where(e => e.LeaderboardId == entity.Id).ToList();
            _context.RemoveRange(leaderboardUsers);
            _context.SaveChanges();

            Leaderboard leaderboard = _context.Leaderboards.Find(entity.Id);
            leaderboard.Name = entity.Name;
            leaderboard.Users = entity.Users.Select(row => new LeaderboardUser
            {
                UserId = _context.Users.Where(user => row.Email == user.Email).First().Id,
            }).ToList();
            return Update(leaderboard);
        }

        public List<LeaderboardSummaryModel> GetLeaderboarSummaries(long userId)
        {
            User user = _context.Users
                .Include(row => row.Leaderboards)
                .Include("Leaderboards.Leaderboard")
                .Include("Leaderboards.Leaderboard.Admin")
                .Where(row => row.Id == userId).First();
            return user.Leaderboards.Select(row => new LeaderboardSummaryModel
            {
                Id = row.LeaderboardId,
                Name = row.Leaderboard.Name,
                Owner = new UserModel
                {
                    Email = row.Leaderboard.Admin.Email,
                    FirstName = row.Leaderboard.Admin.FirstName,
                    LastName = row.Leaderboard.Admin.LastName
                }
            }).ToList();
        }

        public LeaderboardAddModel GetLeaderboardForEdit(long group)
        {
            Leaderboard leaderboard = _context.Leaderboards
                .Include(row => row.Users)
                .Include("Users.User")
                .Where(row => row.Id == group).First();

            return new LeaderboardAddModel
            {
                AdminId = leaderboard.AdminId,
                Name = leaderboard.Name,
                Id = leaderboard.Id,
                Users = leaderboard.Users.Select(user => new UserModel
                {
                    Email = user.User.Email,
                    FirstName = user.User.FirstName,
                    LastName = user.User.LastName
                }).ToList()
            };
        }

        public void LeaveLeaderboard(long userId, long group)
        {
            var leaderboardUser = _context.LeaderboardUsers
                .Where(row => row.UserId == userId && row.LeaderboardId == group)
                .First();
            _context.LeaderboardUsers.Remove(leaderboardUser);
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
            var leaderboardUsers = new List<LeaderboardUser>();
            foreach (var user in entity.Users)
            {
                leaderboardUsers.Add(new LeaderboardUser { UserId = user.UserId, LeaderboardId = entity.Id });
            }
            entity.Users.Concat(leaderboardUsers);
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
            _context.SaveChanges();
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