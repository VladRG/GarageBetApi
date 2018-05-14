
using System.ComponentModel.DataAnnotations;

namespace GarageBet.Api.Database.Tables
{
    public class Bet : EntityBase
    {
        [Range(-1, 20)]
        public short HomeScore { get; set; }

        [Range(-1, 20)]
        public short AwayScore { get; set; }

        public long UserId { get; set; }

        public long MatchId { get; set; }

        public User User { get; set; }

        public Match Match { get; set; }
    }
}
