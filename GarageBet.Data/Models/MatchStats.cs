using GarageBet.Domain.Models;

namespace GarageBet.Data.Models
{
    public class MatchStats
    {
        public UserModel User { get; set; }

        public BetState BetState { get; set; }

        public int HomeScore { get; set; }

        public int AwayScore { get; set; }
    }
}
