
using GarageBet.Domain.Models;

namespace GarageBet.Data.Models
{
    public class MatchBetModel
    {
        public int HomeScore { get; set; }

        public int AwayScore { get; set; }

        public MatchModel Match { get; set; }

        public ChampionshipModel Championship { get; set; }

        public BetState BetState { get; set; }
    }
}
