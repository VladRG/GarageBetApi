using Database.MM;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GarageBet.Domain.Tables
{
    public class Team : EntityBase
    {
        [MaxLength(50)]
        [Required]
        public string Name { get; set; }

        [Required]
        [MaxLength(50)]
        public string Country { get; set; }

        public ICollection<ChampionshipTeam> ChampionshipTeams { get; } = new List<ChampionshipTeam>();

        [NotMapped]
        public ICollection<Championship> Championships = new List<Championship>();

        public ICollection<Match> HomeMatches { get; set; }

        public ICollection<Match> AwayMatches { get; set; }
    }
}
