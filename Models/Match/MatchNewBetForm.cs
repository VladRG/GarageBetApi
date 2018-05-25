using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GarageBet.Api.Models
{
    public class MatchNewBetForm
    {
        public long MatchId { get; set; }

        public string HomeTeamName { get; set; }

        public string AwayTeamName { get; set; }
    }
}
