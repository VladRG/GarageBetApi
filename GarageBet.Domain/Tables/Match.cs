
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

        private Team HomeTeamNavigationProperty { get; set; }

        private Team AwayTeamNavigationProperty { get; set; }

        private Championship ChampionshipNavigationProperty { get; set; }

        public ICollection<Bet> Bets { get; set; }

        [NotMapped]
        public ChampionshipModel Championship { get; set; }

        [NotMapped]
        public TeamModel HomeTeam { get; set; }

        [NotMapped]
        public TeamModel AwayTeam { get; set; }

        public Match SetNavigationProperties()
        {
            Championship = new ChampionshipModel
            {
                Id = ChampionshipNavigationProperty.Id,
                Name = ChampionshipNavigationProperty.Name
            };

            HomeTeam = new TeamModel
            {
                Id = HomeTeamNavigationProperty.Id,
                Name = HomeTeamNavigationProperty.Name
            };

            AwayTeam = new TeamModel
            {
                Id = AwayTeamNavigationProperty.Id,
                Name = AwayTeamNavigationProperty.Name
            };
            return this;
        }
    }
}
