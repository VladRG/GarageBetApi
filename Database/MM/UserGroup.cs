
using GarageBet.Api.Database.Tables;

namespace GarageBet.Api.Database
{
    public class LeaderboardUser
    {
        public long LeaderboardId { get; set; }

        public long UserId { get; set; }

        public Leaderboard Leaderboard { get; set; }

        public User User { get; set; }

        public bool Accepted { get; set; }
    }
}
