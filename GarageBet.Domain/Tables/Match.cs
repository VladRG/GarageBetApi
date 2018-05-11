
using GarageBet.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GarageBet.Domain.Tables
{
    public class Match : EntityBase
    {
        [Range(-1, 20)]
        public short HomeScore { get; set; }

        [Range(-1, 20)]
        public short AwayScore { get; set; }

        public DateTime DateTime { get; set; }

        public long? HomeTeamId { get; set; }

        public long? AwayTeamId { get; set; }

        public long? ChampionshipId { get; set; }

        public List<Bet> Bets { get; set; }

        public Championship Championship { get; set; }

        public Team HomeTeam { get; set; }

        public Team AwayTeam { get; set; }
        
    }
}
