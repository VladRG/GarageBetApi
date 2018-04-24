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

        private ICollection<ChampionshipTeam> ChampionshipTeams { get; } = new List<ChampionshipTeam>();

        [NotMapped]
        public IEnumerable<TeamModel> Teams => ChampionshipTeams.Select(e => new TeamModel
        {
            Id = e.Team.Id,
            Name = e.Team.Name
        });
    }
}
