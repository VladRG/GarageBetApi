using Database.MM;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GarageBet.Domain.Tables
{
    public class Team : EntityBase
    {
        [MaxLength(50)]
        [Required]
        public string Name { get; set; }

        public ICollection<ChampionshipTeam> Championships { get; set; }

        public ICollection<Match> HomeMatches { get; set; }

        public ICollection<Match> AwayMatches { get; set; }
    }
}
