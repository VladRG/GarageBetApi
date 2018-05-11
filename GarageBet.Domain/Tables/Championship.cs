using Database.MM;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GarageBet.Domain.Models;

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

        public ICollection<ChampionshipTeam> ChampionshipTeams { get; set; }

        public List<Team> Teams { get; set; }

    }
}
