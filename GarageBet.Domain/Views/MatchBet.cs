
using GarageBet.Domain.Tables;
using System;
using System.ComponentModel.DataAnnotations;

namespace Database.Views
{
    public class MatchBet
    {
        [Key]
        public long BetId { get; set; }

        public string HomeTeamName { get; set; }

        public string AwayTeamName { get; set; }

        public short HomeScore { get; set; }

        public short AwayScore { get; set; }

        public short HomeScoreBet { get; set; }

        public short AwayScoreBet { get; set; }

        public DateTime MatchDateTime { get; set; }

        public User User { get; set; }
    }
}
