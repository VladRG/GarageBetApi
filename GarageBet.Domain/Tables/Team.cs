using Database.MM;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GarageBet.Domain.Models;

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

        public ICollection<ChampionshipTeam> ChampionshipTeams { get; set; }

        public ICollection<Match> HomeMatches { get; set; }

        public ICollection<Match> AwayMatches { get; set; }
    }
}
