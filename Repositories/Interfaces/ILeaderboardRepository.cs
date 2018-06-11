using GarageBet.Api.Models;
using GarageBet.Api.Database.Tables;
using System.Collections.Generic;

namespace GarageBet.Api.Repository.Interfaces
{
    public interface ILeaderboardRepository : IRepository<Leaderboard>
    {
        UserStats GetUserStat(long userId);

        IEnumerable<UserStats> GetUserStats(int page, int pageSize, int group);

        int GetUserCount(long group);

        int GetUserLeaderboardPosition(string email, long group);

        IEnumerable<LeaderboardModel> GetUserLeaderboards(long userId, int page, int pageSize);

        void AcceptInvite(User user, long group);

        void DeclineInvite(User user, long group);

        Leaderboard Add(LeaderboardAddModel leaderboard);

        List<LeaderboardSummaryModel> GetLeaderboarSummaries(long userId);

        LeaderboardAddModel GetLeaderboardForEdit(long group);

        void LeaveLeaderboard(long userId, long group);

        Leaderboard Update(LeaderboardAddModel entity);
    }
}