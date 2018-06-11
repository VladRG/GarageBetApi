using GarageBet.Api.Models;
using GarageBet.Api.Database.Tables;
using System.Collections.Generic;

namespace GarageBet.Api.Repository.Interfaces
{
    public interface ILeaderboardRepository : IRepository<Leaderboard>
    {
        UserStats GetUserStat(long userId);

        IEnumerable<UserStats> GetUserStats(int page, int pageSize);

        int GetUserCount();

        int GetUserLeaderboardPosition(string email);

        IEnumerable<LeaderboardModel> GetUserLeaderboards(long userId, int page, int pageSize);

        void AcceptInvite(User user, long group);

        void DeclineInvite(User user, long group);
    }
}