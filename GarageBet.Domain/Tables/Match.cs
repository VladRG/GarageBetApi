
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

        [ForeignKey("HomeTeamId")]
        private Team HomeTeamNavigationProperty { get; set; }

        [ForeignKey("AwayTeamId")]
        private Team AwayTeamNavigationProperty { get; set; }

        private Championship ChampionshipNavigationProperty { get; set; }

        public ICollection<Bet> Bets { get; set; }

        [NotMapped]
        public ChampionshipModel Championship => new ChampionshipModel
        {
            Id = ChampionshipNavigationProperty.Id,
            Name = ChampionshipNavigationProperty.Name
        };

        [NotMapped]
        public TeamModel HomeTeam => new TeamModel
        {
            Id = HomeTeamNavigationProperty.Id,
            Name = HomeTeamNavigationProperty.Name
        };

        [NotMapped]
        public TeamModel AwayTeam => new TeamModel
        {
            Id = AwayTeamNavigationProperty.Id,
            Name = AwayTeamNavigationProperty.Name
        };

    }
}
