using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GarageBet.Api.Database.Tables
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

        public ICollection<ChampionshipTeam> ChampionshipTeams { get; set; }

        [NotMapped]
        public List<Team> Teams { get; set; }

    }
}
