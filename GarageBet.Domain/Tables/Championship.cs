using GarageBet.Domain.MM;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GarageBet.Domain.Tables
{
    [Table("Championships")]
    public class Championship : EntityBase
    {
        [MaxLength(50)]
        [Required(ErrorMessage = "Championship Name is required.")]
        public string Name { get; set; }

        [MaxLength(10)]
        public string CompetitiveYear { get; set; }

        public ICollection<ChampionshipMatch> Matches { get; set; }

        public ICollection<ChampionshipTeam> Teams { get; set; }
    }
}
