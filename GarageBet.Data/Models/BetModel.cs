using System;

namespace GarageBet.Data.Models
{
    public class BetModel
    {
        public int HomeScore { get; set; }

        public int AwayScore { get; set; }

    }

    public class BetFormModel
    {
        public long Id { get; set; }

        public long UserId { get; set; }

        public int HomeScore { get; set; }

        public int AwayScore { get; set; }

        public MatchModel Match { get; set; }

        public ChampionshipModel Championship { get; set; }
    }
}
