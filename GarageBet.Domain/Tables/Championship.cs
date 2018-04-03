using Database.MM;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GarageBet.Domain.Tables
{
    public class Championship : EntityBase
    {
        [MaxLength(50)]
        [Required]
        public string Name { get; set; }

        [MaxLength(10)]
        [Required]
        public string CompetitiveYear { get; set; }

        public ICollection<Match> Matches { get; set; }

        public ICollection<ChampionshipTeam> Teams { get; set; }
    }
}
