using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GarageBet.Api.Models
{
    public class MatchEditBetForm
    {
        public long MatchId { get; set; }

        public long BetId { get; set; }

        public int HomeBet { get; set; }

        public int AwayBet { get; set; }

        public string HomeTeamName { get; set; }

        public string AwayTeamName { get; set; }
    }
}
