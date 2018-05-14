using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GarageBet.Api.Models;

namespace GarageBet.Api.Database.Tables
{
    public class Team : EntityBase
    {
        [MaxLength(50)]
        [Required]
        public string Name { get; set; }

        [Required]
        [MaxLength(50)]
        public string Country { get; set; }

        public ICollection<ChampionshipTeam> ChampionshipTeams { get; set; }

        public ICollection<Match> HomeMatches { get; set; }

        public ICollection<Match> AwayMatches { get; set; }
    }
}
