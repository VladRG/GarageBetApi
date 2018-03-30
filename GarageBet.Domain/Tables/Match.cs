
using GarageBet.Domain.MM;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace GarageBet.Domain.Tables
{
    [Table("Matches")]
    public class Match : EntityBase
    {
        public short HomeScore { get; set; }

        public short AwayScore { get; set; }

        public DateTime DateTime { get; set; }

        public Team HomeTeam { get; set; }

        public Team AwayTeam { get; set; }

        public ICollection<ChampionshipMatch> Championships { get; set; }

        public ICollection<Bet> Bets { get; set; }
    }
}
