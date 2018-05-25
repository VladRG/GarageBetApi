
using System;

namespace GarageBet.Api.Models
{
    public class MatchBetModel
    {
        public long MatchId { get; set; }

        public long BetId { get; set; }

        public long ChampionshipId { get; set; }

        public int HomeBet { get; set; }

        public int AwayBet { get; set; }

        public int HomeScore { get; set; }

        public int AwayScore { get; set; }

        public string ChampionshipName { get; set; }

        public string CompetitiveYear { get; set; }

        public string HomeTeamName { get; set; }

        public string AwayTeamName { get; set; }

        public DateTime DateTime { get; set; }

        public BetState BetState { get; set; }

    }
}
