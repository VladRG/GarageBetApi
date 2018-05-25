using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GarageBet.Api.Models
{
    public class BetFormModel
    {
        public long BetId { get; set; }

        public long MatchId { get; set; }

        public long UserId { get; set; }

        public int HomeScore { get; set; }

        public int AwayScore { get; set; }

        public string HomeTeamName { get; set; }

        public string AwayTeamName { get; set; }

        public int HomeBet { get; set; }

        public int AwayBet { get; set; }

    }
}
