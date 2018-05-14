
using GarageBet.Api.Models;

namespace GarageBet.Api.Models
{
    public class MatchBetModel
    {

        public MatchModel Match { get; set; }

        public ChampionshipModel Championship { get; set; }

        public BetModel Bet { get; set; }

        public BetState BetState { get; set; }
    }
}
